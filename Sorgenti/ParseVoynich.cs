using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{




    public class ParseVoynich
    {


        public class ParsingGrammar
        {
            public string name;
            public string notes;

            public string[][] slots_table;

            public int default_loop_repeats;
            public int max_loop_repeats;

            public char[] rare_characters;
            public string[] rare_chars_groups;


        }





        // PARSER DI UN TESTO USANDO UNO 'SLOT MODEL'

        // SI FERMA ALLA PRIMA SCOMPOSIZIONE CHE TROVA (MA SUPPORTEREBBE UNA RICERCA COMPLETA)

        public static mdError parse_words(TextAnalysis_results analysis_results, ref ParsingResults Voynich_parsing_results)
        {

            mdError error = new mdError();


            // La grammatica da usare è già in Voynich_parsing_results.grammar (dalla gestione della combobox)
            // Ma devo cancellare tutto il resto dei dati, che potrebbero essere rimasti da un'elaboraizone precedente con un diverso # di ripetizioni
            Voynich_parsing_results.data = new List<WordParseData>();
            Voynich_parsing_results.total_wordtypes_found = 0;
            Voynich_parsing_results.total_wordstypes_not_found_but_with_rare_characters = 0;

            // Calcolo Ngrammarsize
            Voynich_parsing_results.total_grammar_wordtypes = 1;
            for (int i = 0; i < Voynich_parsing_results.grammar.slots_table.GetLength(0); i++)
            {
                Voynich_parsing_results.total_grammar_wordtypes *= (Voynich_parsing_results.grammar.slots_table[i].Length + 1);
            }
            Voynich_parsing_results.total_grammar_wordtypes = (float)Math.Pow(Voynich_parsing_results.total_grammar_wordtypes, Form1.Voynich_current_max_loop_repeats);


            // Per ogni word type
            foreach (EValueOcc source_word_evalue in analysis_results.vocabulary_words_distribution)
            {

                int min_occurrences = 1;
                if (source_word_evalue.value < min_occurrences)
                {
                    continue;
                }

                string source_word = source_word_evalue.element;

                bool word_found = false;
                bool word_not_found_but_has_rare_characters = false;

                int number_chunks_in_the_word = 0;  // dummy, deve essere inizializzato perchè è passato come ref (vabbè, potrebbe anche essere passato come out....)

                List<List<EValue>> parsed_word = new List<List<EValue>>();

                // qua sarebbe meglio passare la struttura WordParseData (spostando in alto la sua creazione) per evitare una quantità di parametri....
                string reconstructed_word = check_word(source_word, Voynich_parsing_results.grammar, ref word_found, ref word_not_found_but_has_rare_characters, ref number_chunks_in_the_word, ref parsed_word);

                if (word_found == true)
                {
                    // sanity check!!!!
                    if (reconstructed_word != source_word)
                    {
                        error.root("Software error", "Sanity check failed, source: '" + source_word + "', reconstructed: '" + reconstructed_word + "'");
                        error.Display_and_Clear();
                        return error;
                    }
                    Voynich_parsing_results.total_wordtypes_found++;
                }
                else
                {
                    if (word_not_found_but_has_rare_characters == true)
                    {
                        Voynich_parsing_results.total_wordstypes_not_found_but_with_rare_characters++;
                    }
                }

                WordParseData word_data = new WordParseData();
                word_data.word = source_word;
                word_data.occurrences = source_word_evalue.value;
                word_data.word_found = word_found;
                word_data.word_not_found_but_has_rare_characters = word_not_found_but_has_rare_characters;
                word_data.number_of_words_chunks = number_chunks_in_the_word;
                word_data.parsed_word = parsed_word;
                Voynich_parsing_results.data.Add(word_data);
            }

            return error;
        }



        private static string check_word(string source_word, ParsingGrammar grammar, ref bool word_found, ref bool word_not_found_but_has_rare_characters, ref int number_of_chunks_in_the_word, ref List<List<EValue>> parsed_word)
        {

            // Abbiamo:

            //      word_pointer, che scorre lungo la parola
            //      slot_pointer e string_pointer, che scorrono la tabella degli slots

            int word_pointer = 0;

            int slot_pointer = 0;
            int string_pointer = 0;

            string reconstructed_word = "";

            int current_chunk = 0;
            parsed_word = new List<List<EValue>>();

            int current_word_chunk = -1;    // -1 significa 'a START'


            // Dobbiamo attraversare un albero di percorsi nella tabella per cercare un parsing valido. Quindi mi serve uno stack.
            List<StackRecord> stack = new List<StackRecord>();

            while (true)
            {

                string current_slot_string = grammar.slots_table[slot_pointer][string_pointer];

                // Slot vero e proprio
                if (string_matches(source_word, word_pointer, current_slot_string) == true)
                {

                    // Salva nello stack la posizione attuale!
                    StackRecord stack_record = new StackRecord();
                    stack_record.word_pointer = word_pointer;
                    stack_record.slot_pointer = slot_pointer;
                    stack_record.string_pointer = string_pointer;
                    stack_record.reconstructed_word = reconstructed_word;
                    stack_record.current_word_chunk = current_chunk;
                    stack_record.number_of_word_chunks = current_word_chunk;
                    stack_record.parsed_word = copy_parsed_word(parsed_word);  // Ok.. era meglio un constructor....
                    stack.Add(stack_record);

                    // Gestione chunks per LOOP grammars!
                    if (current_chunk != current_word_chunk)
                    {
                        // Siamo entrati in un nuovo chunk                       
                        List<EValue> chunk = new List<EValue>();
                        EValue value = new EValue();
                        value.element = current_slot_string;
                        value.value = slot_pointer;
                        chunk.Add(value);
                        parsed_word.Add(chunk);

                        current_word_chunk = current_chunk;
                    }
                    else
                    {
                        List<EValue> chunk = new List<EValue>();
                        EValue value = new EValue();
                        value.element = current_slot_string;
                        value.value = slot_pointer;

                        parsed_word[parsed_word.Count - 1].Add(value);
                    }
                    // Fine gestione word chunks

                    reconstructed_word = reconstructed_word + current_slot_string;

                    word_pointer = word_pointer + current_slot_string.Length;
                    // abbiamo trovato la parola completa?
                    if (word_pointer == source_word.Length)
                    {
                        // si!
                        word_found = true;
                        number_of_chunks_in_the_word = current_word_chunk + 1;
                        return reconstructed_word;
                    }

                    // ok.. è uno schifoso trucco... quello che voglio dopo un match è che si passi allo slot successivo, ma proseguendo con la routine
                    //   viene invece incrementato il puntatore di stringa. Settando string_pointer a Lenght-1 di forza l'incremento di slot_pointer quando si prosegue
                    string_pointer = grammar.slots_table[slot_pointer].Length - 1;
                }

                // else proseguiamo con la stringa/slot successivo
                string_pointer++;
                if (string_pointer == grammar.slots_table[slot_pointer].Length)
                {
                    string_pointer = 0;
                    slot_pointer++;

                    if (slot_pointer == grammar.slots_table.GetLength(0))
                    {
                        // Siamo arrivati alla fine di un chunk
                        current_chunk++;
                        slot_pointer = 0;
                    }
                }

                if (current_chunk == Form1.Voynich_current_max_loop_repeats)
                {
                    // Siamo arrivati alla fine delle ripetizioni degli slots e la parola non è stata trovata
                    while (true)
                    {
                        if (stack.Count == 0)
                        {
                            // Stack vuoto, la parola non è realizzabile dal modello!
                            word_found = false;

                            if (contains_rare_characters(source_word, grammar.rare_characters, grammar.rare_chars_groups) == true)
                            {
                                word_not_found_but_has_rare_characters = true;
                            }

                            parsed_word = new List<List<EValue>>();         // questi dati sono tutti non validi

                            return source_word; // Mentre qua viene ritornata la parola originale per poterla poi visualizzare
                        }

                        // Recuperiamo l'ultima posizione dallo stack, poi incrementiamo di 1 i puntatori alla tabella e proseguiamo da lì
                        StackRecord old_position = stack[stack.Count - 1];
                        word_pointer = old_position.word_pointer;
                        reconstructed_word = old_position.reconstructed_word;
                        slot_pointer = old_position.slot_pointer;
                        string_pointer = old_position.string_pointer;
                        current_chunk = old_position.current_word_chunk;
                        current_word_chunk = old_position.number_of_word_chunks;
                        parsed_word = old_position.parsed_word;
                        stack.RemoveAt(stack.Count - 1);

                        string_pointer++;
                        if (string_pointer == grammar.slots_table[slot_pointer].Length)
                        {
                            string_pointer = 0;
                            slot_pointer++;

                            if (slot_pointer == grammar.slots_table.GetLength(0))
                            {
                                // Siamo arrivati alla fine di un chunk
                                current_chunk++;
                                slot_pointer = 0;
                            }
                        }

                        if (current_chunk == Form1.Voynich_current_max_loop_repeats)
                        {
                            // siamo alla fine della grammatica: recupera un altro valore dallo stack
                            continue;
                        }
                        else
                        {
                            // proseguiamo da questa posizione
                            break;
                        }

                    }

                }


            }


        }




        private static bool string_matches(string source_word, int word_pointer, string current_slot_string)
        {
            bool string_matches = true;
            // caso particolare da trappare subito per non incasinare il loop successivo: la stringa non matcha perchè è più lunga di quanto rimane della parola
            if (word_pointer + current_slot_string.Length > source_word.Length)
            {
                string_matches = false;
                return string_matches;
            }
            // check i caratteri della stringa
            for (int slot_string_pointer = 0; slot_string_pointer < current_slot_string.Length; slot_string_pointer++)
            {
                if (source_word[word_pointer] != current_slot_string[slot_string_pointer])
                {
                    string_matches = false;
                    break;
                }
                word_pointer++;
            }
            return string_matches;
        }


        public static bool contains_rare_characters(string word, char[] rare_characters_list, string[] groups_list)
        {

            bool contains_rare_characters = false;

            string source_word = "";


            // la prima cosa da fare è eliminare eventuali gruppi 'ch', 'sh', 'ckh', 'cth' , 'cph', 'cfh'
            //    a questo ci pensa la groups_list (vedi anche commenti nelle definizioni delle grammatiche)

            // search
            int word_pointer = 0;
            while (word_pointer < word.Length)
            {
                bool match_found = false;
                foreach (string group in groups_list)
                {
                    if (string_matches(word, word_pointer, group) == true)
                    {
                        // replace
                        for (int i = 0; i < group.Length; i++)
                        {
                            source_word += "X";
                            word_pointer++;
                        }
                        match_found = true;
                        break;
                    }
                }
                if (match_found == false)
                {
                    source_word += word[word_pointer];
                    word_pointer++;
                }
            }


            // e adesso possimo cercare i caratteri rari singoli in source_word
            foreach (char rare_char in rare_characters_list)
            {
                foreach (char source_char in source_word)
                {
                    if (source_char == rare_char)
                    {
                        contains_rare_characters = true;
                        return contains_rare_characters;
                    }
                }
            }

            return contains_rare_characters;
        }


        public class StackRecord
        {
            public int word_pointer = 0;

            public int slot_pointer = 0;
            public int string_pointer = 0;

            public string reconstructed_word = "";

            public int number_of_word_chunks;
            public int current_word_chunk;
            public List<List<EValue>> parsed_word;
        }


        private static List<List<EValue>> copy_parsed_word(List<List<EValue>> source_parsed_word) // Ok.. eram meglio un copy constructor... non ho voglia di pensare xD
        {
            List<List<EValue>> new_parsed_word = new List<List<EValue>>();

            foreach (List<EValue> list in source_parsed_word)
            {
                List<EValue> new_list = new List<EValue>();
                foreach (EValue value in list)
                {
                    new_list.Add(value);
                }
                new_parsed_word.Add(new_list);
            }

            return new_parsed_word;
        }





        public static void get_chunks_data(List<ParseVoynich.WordParseData> word_data_list, ref ParsingResults Voynich_parsing_results)
        {

            // Qua troviamo sia il chunks_dictionary (che è l'output fondamentale) che chunks_categories + chunks_vs_categories (che sono
            //     degli accessori, visualizzati ma non ulteriormente sviluppati)

            Voynich_parsing_results.chunks_dictionary = new Dictionary<string, ParseVoynich.Chunk>();

            Voynich_parsing_results.chunks_categories = new Dictionary<string, EValueOcc_extended>();
            Voynich_parsing_results.chunks_vs_categories = new Dictionary<string, Dictionary<string, EValueOcc_extended>>();


            // Per ogni parola
            foreach (ParseVoynich.WordParseData word_data in word_data_list)
            {

                if (word_data.parsed_word != null) // si sa mai...
                {

                    // Per ogni chunk
                    for (int i = 0; i < word_data.parsed_word.Count; i++)
                    {

                        // La 'keystring' è formata da "_" se lo slot non è stato usato, else da "X"
                        string keystring = "";
                        int keystring_pointer = 0;
                        int chunk_slot_pointer = 0;

                        int number_of_slots_in_a_chunk = Voynich_parsing_results.grammar.slots_table.GetLength(0);

                        // Per ogni slot
                        for (int j = 0; j < number_of_slots_in_a_chunk; j++)
                        {
                            if (chunk_slot_pointer < word_data.parsed_word[i].Count)
                            {
                                // La divisione in modulo serve perchè i numeri degli slots sono sempre progressivi, non tornano a zero quando si passa di chunk
                                if (word_data.parsed_word[i][chunk_slot_pointer].value % number_of_slots_in_a_chunk == keystring_pointer)
                                {
                                    keystring += "X";
                                    chunk_slot_pointer++;
                                }
                                else
                                {
                                    keystring += "_";
                                }
                            }
                            else
                            {
                                keystring += "_";
                            }
                            keystring_pointer++;
                        }


                        // Trovata la keystring, aggiorniamo il vocabolario chunk_types (tutto molto simile p.es. a TextCharStats)
                        EValueOcc_extended old_value = new EValueOcc_extended();
                        if (Voynich_parsing_results.chunks_categories.TryGetValue(keystring, out old_value) == true)
                        {
                            old_value.value += 1;
                            Voynich_parsing_results.chunks_categories.Remove(keystring);
                            Voynich_parsing_results.chunks_categories.Add(keystring, old_value);
                        }
                        else
                        {
                            EValueOcc_extended new_value = new EValueOcc_extended(); // impossibile riciclare old_value qua... provare per credere
                            new_value.element = keystring;
                            new_value.element_additional = word_data.word;
                            new_value.value = 1;
                            Voynich_parsing_results.chunks_categories.Add(keystring, new_value);
                        }

                        // E adesso aggiorniamo anche il dictionary of dictionaries..
                        update_chunks_list(keystring, word_data.parsed_word[i], ref Voynich_parsing_results.chunks_vs_categories);

                        // E il dizionario fondamentale, chunks_dictionary (è alla fine per motivi storici)
                        string this_chunk = compose_chunk(word_data.parsed_word[i]);
                        Chunk old_chunk = new Chunk();
                        if (Voynich_parsing_results.chunks_dictionary.TryGetValue(this_chunk, out old_chunk) == true)
                        {
                            Chunk new_chunk = new Chunk();
                            new_chunk.chunk = this_chunk;
                            new_chunk.number_of_times_used = old_chunk.number_of_times_used + word_data.occurrences;
                            new_chunk.Huffman_code = "";
                            Voynich_parsing_results.chunks_dictionary.Remove(this_chunk);
                            Voynich_parsing_results.chunks_dictionary.Add(this_chunk, new_chunk);
                        }
                        else
                        {
                            Chunk new_chunk = new Chunk();
                            new_chunk.chunk = this_chunk;
                            new_chunk.number_of_times_used = word_data.occurrences;
                            new_chunk.Huffman_code = "";
                            Voynich_parsing_results.chunks_dictionary.Add(this_chunk, new_chunk);
                        }

                    }

                }
            }

            Voynich_parsing_results.total_number_of_chunktypes = Voynich_parsing_results.chunks_dictionary.Count;

        }



        private static void update_chunks_list(string keystring, List<EValue> parsed_chunk, ref Dictionary<string, Dictionary<string, EValueOcc_extended>> type_vs_chunks_list)
        {

            string this_chunk = compose_chunk(parsed_chunk);

            // Vediamo se esite la keystring nel dizionario esterno
            Dictionary<string, EValueOcc_extended> old_chunks_list = new Dictionary<string, EValueOcc_extended>();
            if (type_vs_chunks_list.TryGetValue(keystring, out old_chunks_list) == true)
            {
                // Vediamo se il chunk esiste già nel dizionario interno
                EValueOcc_extended old_value = new EValueOcc_extended();
                if (old_chunks_list.TryGetValue(this_chunk, out old_value) == true)
                {
                    old_value.value += 1;
                    old_chunks_list.Remove(this_chunk);
                    old_chunks_list.Add(this_chunk, old_value);
                }
                else
                {
                    EValueOcc_extended new_value = new EValueOcc_extended(); // impossibile riciclare old_value qua... provare per credere
                    new_value.element = this_chunk;
                    new_value.element_additional = "";
                    new_value.value = 1;
                    old_chunks_list.Add(this_chunk, new_value);
                }

                // Emmò aggiorniamo il dizionario esterno
                type_vs_chunks_list.Remove(keystring);
                type_vs_chunks_list.Add(keystring, old_chunks_list);
            }
            else
            {
                Dictionary<string, EValueOcc_extended> new_chunks_list = new Dictionary<string, EValueOcc_extended>();
                EValueOcc_extended new_value = new EValueOcc_extended(); // impossibile riciclare old_value qua... provare per credere
                new_value.element = this_chunk;
                new_value.element_additional = "";
                new_value.value = 1;
                new_chunks_list.Add(this_chunk, new_value);
                type_vs_chunks_list.Add(keystring, new_chunks_list);
            }

        }

        public static string compose_chunk(List<EValue> parsed_chunk)
        {
            string out_string = "";
            foreach (EValue value in parsed_chunk)
            {
                out_string += value.element;
            }
            return out_string;
        }









        public class ParsingResults
        {
            public ParsingGrammar grammar;

            public int total_wordtypes_found;                                   // Nhits
            public int total_wordstypes_not_found_but_with_rare_characters;
            public float total_grammar_wordtypes;                               // Ngrammarsize

            public int total_number_of_chunktypes;    // Nchunktypes

            public List<WordParseData> data = new List<WordParseData>();   // Chunkified word types

            public Dictionary<string, Chunk> chunks_dictionary = new Dictionary<string, Chunk>();

            // Variabili con le 'categorie' dei chunks (idea non sviulppata ulteriormente, le due variabili contengono +- gli stessi dati e potrebbero essere riuniti in un'unica variabile)
            public Dictionary<string, EValueOcc_extended> chunks_categories = new Dictionary<string, EValueOcc_extended>(); // Lista delle categorie con # utilizzi e parola di esempio
            // Come sopra, ma la 'categoria' è la chiave del dizionario esterno, e quelli interni contengono l'elenco dei chunks in ogni categoria col loro numero di utilizzi
            public Dictionary<string, Dictionary<string, EValueOcc_extended>> chunks_vs_categories = new Dictionary<string, Dictionary<string, EValueOcc_extended>>();
        }

        public class WordParseData
        {
            public string word;
            public long occurrences;

            public bool word_found;
            public bool word_not_found_but_has_rare_characters;

            public int number_of_words_chunks;

            public List<List<EValue>> parsed_word = new List<List<EValue>>(); // contiene i chunks che formano la word, suddivisi slot per slot
        }

        public class Chunk
        {
            public string chunk;
            public long number_of_times_used;

            // Codifica Huffman
            public string Huffman_code = "";    // Risultato          
        }


        public class CompareChunk_by_times_used : IComparer<Chunk>
        {
            public int Compare(Chunk x, Chunk y)
            {
                if (x.number_of_times_used == y.number_of_times_used) return (0);

                if (x.number_of_times_used < y.number_of_times_used)
                {
                    return (1);
                }
                return (-1);
            }
        }


    }
}
