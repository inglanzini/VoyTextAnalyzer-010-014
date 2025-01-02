using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class PackGrammarAndWrite
    {


        public class ChunksGrammar
        {
            public List<Dictionary<string, EValueOcc>> slots_table = new List<Dictionary<string, EValueOcc>>();  // analoga a string[][] slots_table in ParseVoynich

            public List<Dictionary<string, SlotTransition>> transitions_table = new List<Dictionary<string, SlotTransition>>();
        }


        public class SlotTransition
        {
            public string preceding_chunk;
            public string following_chunk;

            public long occurrences;
        }


        public static mdError calculate_chunkified_grammar(ParseVoynich.ParsingResults Voynich_parsing_results, ref ChunksGrammar chunks_grammar)
        {
            mdError error = new mdError();

            if (Voynich_parsing_results.grammar == null)
            {
                error.root("", "Parse a Voynich file first");
                error.Display_and_Clear();
                return error;            
            }


            chunks_grammar = new ChunksGrammar();

            int num_chunks_slots = Form1.Voynich_current_max_loop_repeats;

            for (int i = 0; i < Form1.Voynich_current_max_loop_repeats; i++)
            {   // Init slots_table
                Dictionary<string, EValueOcc> new_chunk_slot = new Dictionary<string, EValueOcc>();
                chunks_grammar.slots_table.Add(new_chunk_slot);
            }
            // Init transitions table (serve un elemento in più di prima, per la transizione -->END!!)
            for (int i = 0; i < Form1.Voynich_current_max_loop_repeats + 1; i++)
            {  
                Dictionary<string, SlotTransition> new_transition_slot = new Dictionary<string, SlotTransition>();
                chunks_grammar.transitions_table.Add(new_transition_slot);
            }


            // Compilazione della grammatica

            // Per ogni parola
            foreach (ParseVoynich.WordParseData word_data in Voynich_parsing_results.data)
            {
                if (word_data.word_found == false)
                {
                    continue;
                }


                string preceding_chunk = "START";

                // Per ogni chunk che compone la parola (ognuno in un new_chunk_slot differente!!)

                // ATTENZIONE al <= !!! Il for viene eseguito una volta in più, per gestire anche la END, che viene testata subito per evitare accessi fuori range
                for (int chunk_slot_counter = 0; chunk_slot_counter <= word_data.parsed_word.Count; chunk_slot_counter++)
                {
                    SlotTransition transition = new SlotTransition();
                    List<EValue> atomized_chunk = new List<EValue>();
                    String chunk = "";

                    if (chunk_slot_counter == word_data.parsed_word.Count)
                    {
                        atomized_chunk = word_data.parsed_word[chunk_slot_counter - 1]; // accedi all'ultimo chunk
                        chunk = ParseVoynich.compose_chunk(atomized_chunk);

                        transition.preceding_chunk = preceding_chunk;
                        transition.following_chunk = "END";
                        transition.occurrences = word_data.occurrences;
                    }
                    else
                    {
                        atomized_chunk = word_data.parsed_word[chunk_slot_counter];
                        chunk = ParseVoynich.compose_chunk(atomized_chunk);

                        transition.preceding_chunk = preceding_chunk;
                        transition.following_chunk = chunk;
                        transition.occurrences = word_data.occurrences;
                    }

                    // Vediamo se esiste nel chunk_slot_counter-esimo dictionary di slots_table, sennò inseriamolo
                    //   Non va fatto all'ultima iterazione del for, il chunk è già stato inserito al giro precedente (e non c'e' abbastanza spazio
                    //   nella slots_table). Il for allungato di 1 serve solo per la transitions table più avanti (dove invece lo spazio c'è, è più lunga di 1)
                    if (chunk_slot_counter != word_data.parsed_word.Count)
                    {
                        EValueOcc old_chunk;
                        if (chunks_grammar.slots_table[chunk_slot_counter].TryGetValue(chunk, out old_chunk) == false)
                        {
                            EValueOcc new_chunk = new EValueOcc();
                            new_chunk.element = chunk;
                            new_chunk.value = word_data.occurrences;
                            chunks_grammar.slots_table[chunk_slot_counter].Add(chunk, new_chunk);
                        }
                        else
                        {
                            // aggiorna le occorrenze
                            old_chunk.value = old_chunk.value + word_data.occurrences;
                            chunks_grammar.slots_table[chunk_slot_counter].Remove(chunk);
                            chunks_grammar.slots_table[chunk_slot_counter].Add(chunk, old_chunk);
                        }
                    }


                    // Vediamo se esiste nel chunk_slot_counter-esimo dictionary di transitions_table, sennò inseriamolo

                    string transition_key = transition.preceding_chunk + "-" + transition.following_chunk;
                    preceding_chunk = chunk; // per prossima iterazione !!


                    SlotTransition old_transition;
                    if (chunks_grammar.transitions_table[chunk_slot_counter].TryGetValue(transition_key, out old_transition) == false)
                    {
                        chunks_grammar.transitions_table[chunk_slot_counter].Add(transition_key, transition);
                    }
                    else
                    {
                        // aggiorna le occorrenze
                        transition.occurrences = old_transition.occurrences + word_data.occurrences;
                        chunks_grammar.transitions_table[chunk_slot_counter].Remove(transition_key);
                        chunks_grammar.transitions_table[chunk_slot_counter].Add(transition_key, transition);
                    }

                }

            }


            return error;
        }



        public class CompareSlotTransitionByValue : IComparer<SlotTransition>
        {
            public int Compare(SlotTransition x, SlotTransition y)
            {
                if (x.occurrences == y.occurrences) return (0);

                if (x.occurrences < y.occurrences)
                {
                    return (1);
                }
                return (-1);
            }
        }



        public static string Markov_asemic_writer(List<Dictionary<string, SlotTransition>> transitions_table)
        {
            string out_text = "";

            // Qui è un bel casino... devo convertire la transitions_table in una forma tale da poter scegliere un elemento basandomi su un numero
            //    casuale.
            
            //   Per il primo 'slot' della transitions_table è facile: basterebbe una lista, in cui inserisco il numero di occorrenze CUMULATIVO delle varie transizioni

            //   Ma per gli altri è più complicato: devo creare un dictionary che ha come chiave il chunk 'preceding', e come records l'elenco dei possibili chunks 'following'
            //          con le loro occorrenze cumulative [a questo punto uso un dicitonary anche per il primo slot, ha un solo record con preceding = "START"

            //  Lista di dizionari di liste.... e avrebbe anche potuto essere una lista di dizionari di dizionari... ma vabbè
            List<Dictionary<string, List<EValueOcc>>> cumulative_transitions_table_list = new List<Dictionary<string, List<EValueOcc>>>();
            List<long> total_number_of_transitions_list = new List<long>(); 
            foreach (Dictionary<string, SlotTransition> source_slot_dictionary in transitions_table)
            {
                cumulative_transitions_table_list.Add(new Dictionary<string, List<EValueOcc>>());
                int last_element = cumulative_transitions_table_list.Count - 1;  // solo per accorciare le istruzioni, sennò sono incomprensibili

                // E' meglio partire da dati ordinati: non è necessaro per l'algoritmo che poi genera il testo asemico, ma facilita comunque il debugging
                //   Ergo... trasferimento in una lista...
                List<SlotTransition> source_slot_ordered_list = new List<SlotTransition>();
                foreach(SlotTransition source_transition in source_slot_dictionary.Values)
                {
                    SlotTransition transition = new SlotTransition();
                    transition.preceding_chunk = source_transition.preceding_chunk;
                    transition.following_chunk = source_transition.following_chunk;
                    transition.occurrences = source_transition.occurrences;

                    source_slot_ordered_list.Add(transition);
                }               
                IComparer<SlotTransition> comparer = new CompareSlotTransitionByValue();
                source_slot_ordered_list.Sort(comparer);

                foreach (SlotTransition source_transition in source_slot_ordered_list)
                {
                    List<EValueOcc> old_value = new List<EValueOcc>();
                    if (cumulative_transitions_table_list[last_element].TryGetValue(source_transition.preceding_chunk, out old_value) == false)
                    {
                        EValueOcc scratch_value = new EValueOcc();
                        List<EValueOcc> scratch_list = new List<EValueOcc>();
                        scratch_value.element = source_transition.following_chunk;
                        scratch_value.value = source_transition.occurrences;
                        scratch_list.Add(scratch_value);
                        cumulative_transitions_table_list[last_element].Add(source_transition.preceding_chunk, scratch_list);
                    }
                    else
                    {
                        EValueOcc scratch_value = new EValueOcc();
                        List<EValueOcc> scratch_list = new List<EValueOcc>();
                        foreach (var item in old_value)
                        {
                            scratch_list.Add(item);
                        }

                        scratch_value.element = source_transition.following_chunk;

                        scratch_value.value = source_transition.occurrences + old_value[old_value.Count - 1].value; // Cumulative occurrences!
                        scratch_list.Add(scratch_value);                                                           

                        cumulative_transitions_table_list[last_element].Remove(source_transition.preceding_chunk);
                        cumulative_transitions_table_list[last_element].Add(source_transition.preceding_chunk, scratch_list);
                    }
                }
            }



            Random random_generator = new Random(Form1.Voynich_asemic_random_seed);

            long added_words_with_rarechars = 0;  // Verranno inserite alla fine!
            foreach (ParseVoynich.WordParseData word_data in Form1.Voynich_parsing_results.data)
            {
                if (word_data.word_not_found_but_has_rare_characters == true)
                {
                    added_words_with_rarechars += word_data.occurrences;
                }
            }

            // per avere lo stesso numero di parole del Voynich originale una volta aggiunte le parore con RARECHARS
            int total_words = (int)(Form1.text_analyzer.analysis_results.total_number_of_words_in_the_text - added_words_with_rarechars);

            out_text += "%c% " + Form1.program_identifier.name + " " + Form1.program_identifier.version + " " + DateTime.Now + "\r\n\r\n";

            out_text += "%c% Asemic random pseudo-text, grammar name: " + Form1.Voynich_parsing_results.grammar.name + "\r\n";

            out_text += "%c% Generated " + total_words + " word tokens." + " Random seed: " + Form1.Voynich_asemic_random_seed + "\r\n";            

            out_text += "%c% Moreover, " + added_words_with_rarechars + " word tokens with 'rare' characters have been copied from the original text to get comparable statistics and added at the end\r\n\r\n";
            
            // Generazione del testo
            for (int i = 0; i < total_words; i++)
            {
                string word_token = "";
                string current_chunk = "START";

                for (int transition_slot_number = 0; transition_slot_number < cumulative_transitions_table_list.Count; transition_slot_number++)
                {

                    if (current_chunk == "END") break;  // in effetti si esce da qua

                    List<EValueOcc> destinations_list = new List<EValueOcc>();
                    if (cumulative_transitions_table_list[transition_slot_number].TryGetValue(current_chunk, out destinations_list) == false)
                    {
                        mdError error = new mdError();
                        error.root("Software error", "Cannot find starting position in transitions table");
                        out_text += "SOFTWARE ERROR";
                        return out_text;
                    }
                    else
                    {
                        long total_occurrences = destinations_list[destinations_list.Count - 1].value;
                        // Eh, mi tocca castare a int... e vabbè xD 
                        int random_throw = random_generator.Next(1, (int)total_occurrences);

                        // Cerchiamo nella list il chunk estratto
                        bool found = false;
                        foreach (EValueOcc next_step in destinations_list)
                        {
                            if (random_throw <= next_step.value)
                            {
                                current_chunk = next_step.element;
                                found = true;
                                if (next_step.element != "END")
                                {
                                    word_token += next_step.element;
                                }

                                break;
                            }
                        }
                        if (found == false)
                        {
                            mdError error = new mdError();
                            error.root("Software error", "Cannot find starting chunk in destinations list");
                            out_text += "SOFTWARE ERROR";
                            return out_text;
                        }
                    }
                }

                out_text += word_token + " ";
            }

            // Mò aggiungiamo tutte le parole RARECHARS del Voynich originale (la grammatica non le può generare), così non alterano le statistiche
            foreach (ParseVoynich.WordParseData word_data in Form1.Voynich_parsing_results.data)
            {
                if (word_data.word_not_found_but_has_rare_characters == true)
                {
                    for (int i = 0; i < word_data.occurrences; i++)
                    {
                        out_text += word_data.word + " ";
                    }
                }
            }


            return out_text;
        }






    }
}
