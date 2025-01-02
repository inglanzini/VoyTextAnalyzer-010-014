using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class SyllablesStats
    {

        // La gestione delle sillabe è stata presa da Words Analyzer 010 014, con modifiche perchè era una gestione troppo specializzata per l'Italiano
        //   RICORDO CHE SI TRATTA DI SILLABE "ORTOGRAFICHE" !!! NON E' DETTO CHE SEGUANO LE REGOLE DI SILLABAZIONE SPECIFICHE DI UN LINGUAGGIO, PER CUI
        //   SI TROVERANNO SEMPRE SILLABE 'SBAGLIATE'

        // Riguardo alle sillabe, c'è da dire che richiedono MOLTA informazione sul linguaggio: in primis quali caratteri sono le vocali, ma questo è un
        //   problema che almeno in teoria si può risolvere con un algoritmo di identificazione automatica delle vocali (è spiegato su www.voynichese.nu, è un Hidden Markov Model)

        //   Invece non vedo come si potrebbe risolvere il problema della scala di sonorità delle consonanti... e anche fosse un problema risolvibile mi sa che sarebbe un gran casino farlo...
        //     [forse si potrebbero ricavare informazioni dagli inizi/fine delle parole?? p.es. da 'STRano' o 'niCHT'??? ]

        //      Direi di sbattermene le palle e considerare che le consonanti siano tutte uguali. Cè solo da decidere cosa fare per casi come paSTRano, lo 'sillabo'
        //              paSTR-aN-o oppure pa-STRa-No?? In questo caso ovviamente è meglio la seconda (una sillaba termina sulla vocale, tutte le altre consonanti vanno alla successiva)
        //              ma mi sa tanto che ci saranno innumerevoli casi in cui questa regola 'sbaglia'.



        // Anche i nuclei, comunque, hanno le loro rogne: nessun problema se c'è solo una vocale contornata da consonanti, ma se ci sono due (o più) vocali di
        //  seguito in alcune lingue (come l'Inglese) sono quasi sempre dittonghi e formano un nucleo unico, ma in altre lingue, come il rognoso Italiano,
        //  sono o nuclei separati (aereo = a-e-re-o) oppure casi in cui le vocali 'i', 'u' sono in realtà semivocali e vanno nello stesso nucleo (aiuto = a-IU-to)

        // Dato che qua sono interessato solo a sillabe ORTOGRAFICHE e voglio che il programma sia il più possibile ignorante delle caratteristiche di ogni singola lingua
        //   direi che convenga solo calcolare le sillabe in due modi: tutte le vocali adiacenti in uno stesso nucleo, oppure tutte le vocali in nuclei separati




        public static mdError get_syllables(ref TextAnalysis_results analysis_results)
        {
            mdError error = new mdError();

            Dictionary<string, EValueOcc> syllables_dictionary_single_vowels = new Dictionary<string, EValueOcc>();
            Dictionary<string, EValueOcc> syllables_dictionary_multiple_vowels = new Dictionary<string, EValueOcc>();

            foreach (EValueOcc item in analysis_results.vocabulary_words_distribution)    // x ogni parola nel vocabolario
            {
                // La parola viene sillabata anche se include un apostrofo !!! Notare che il test viene sempre passato!!
                //  (ma se dovesse servire, ci sono già tutte le istruzioni per scartare le parole con l'apostrofo, basta togliere || item.element.Contains("'") == true  ) 
                if (item.element.Contains("'") == false  || item.element.Contains("'") == true)
                {
                    // Separazione in sillabe
                    List<string> syllables_single_vowels = new List<string>();
                    List<string> syllables_multiple_vowels = new List<string>();
                    syllabify(item.element, ref syllables_single_vowels, ref syllables_multiple_vowels);

                    // inserimento delle sillabe single_vowel nel loro dizionario con calcolo occorrenze
                    foreach (string syllab in syllables_single_vowels)
                    {
                        EValueOcc old_value = new EValueOcc();
                        if (syllables_dictionary_single_vowels.TryGetValue(syllab, out old_value) == true)
                        {
                            old_value.value += 1;
                            syllables_dictionary_single_vowels.Remove(syllab);
                            syllables_dictionary_single_vowels.Add(syllab, old_value);
                        }
                        else
                        {
                            EValueOcc new_value = new EValueOcc(); // impossibile riciclare old_value qua... provare per credere
                            new_value.element = syllab;
                            new_value.value = 1;
                            syllables_dictionary_single_vowels.Add(syllab, new_value);
                        }
                    }
                    // inserimento delle sillabe multiple_vowels nel loro dizionario con calcolo occorrenze (SI, CI VOLEVA UNA FUNCTION...)
                    foreach (string syllab in syllables_multiple_vowels)
                    {
                        EValueOcc old_value = new EValueOcc();
                        if (syllables_dictionary_multiple_vowels.TryGetValue(syllab, out old_value) == true)
                        {
                            old_value.value += 1;
                            syllables_dictionary_multiple_vowels.Remove(syllab);
                            syllables_dictionary_multiple_vowels.Add(syllab, old_value);
                        }
                        else
                        {
                            EValueOcc new_value = new EValueOcc(); // impossibile riciclare old_value qua... provare per credere
                            new_value.element = syllab;
                            new_value.value = 1;
                            syllables_dictionary_multiple_vowels.Add(syllab, new_value);
                        }
                    }
                }
                else
                {
                    analysis_results.words_with_apostrophe_not_syllabified += 1;
                }
            }

            // Al solito, trasferiamo i dictionaries in liste, che sono più comode per elaborazioni successive (p.es. il dizionario non supporta il sorting)
            format_syllables_stats(syllables_dictionary_single_vowels, syllables_dictionary_multiple_vowels, ref analysis_results);


            return error;
        }

        public static void format_syllables_stats(Dictionary<string, EValueOcc> syllables_dictionary_single_vowels, Dictionary<string, EValueOcc> syllables_dictionary_multiple_vowels, ref TextAnalysis_results analysis_results)
        {
            foreach (EValueOcc EValue in syllables_dictionary_single_vowels.Values)
            {
                EValueOcc new_value = new EValueOcc();
                new_value.element = EValue.element;
                new_value.value = EValue.value;
                analysis_results.syllables_distribution_single_vowels.Add(new_value);
            }
            IComparer<EValueOcc> comparer = new TextCharsStats.CompareEValueOccByValue();
            analysis_results.syllables_distribution_single_vowels.Sort(comparer);

            foreach (EValueOcc EValue in syllables_dictionary_multiple_vowels.Values)
            {
                EValueOcc new_value = new EValueOcc();
                new_value.element = EValue.element;
                new_value.value = EValue.value;
                analysis_results.syllables_distribution_multiple_vowels.Add(new_value);
            }
            analysis_results.syllables_distribution_multiple_vowels.Sort(comparer);
        }



        public static void syllabify (string word, ref List<string> out_syllables_single_vowel, ref List<string> out_syllables_multiple_vowel)
        {
            
            string vowels_set = "aeẹoọiuàêéèĕēôòŏōìùįıịụĭŭAEẸOỌIUÀÊÉÈĔĒÔÒŎŌÌÙĮIỊỤĬŬαυηεωοιΑΥΗΕΩΟΙÄËÖÜÏØäëöüïø";

            // La regola è: le consonanti inziali sono un prefisso per il primo nucleo
            //              le consonanti fra due nuclei vanno in parte nel primo nucleo e in parte nel secondo:
            //                  se c'è una sola consonante va nel secondo nucleo
            //                  se ce ne sono due la prima è un suffisso del primo nucleo, la seconda un prefisso del secondo nucleo
            //                  e così via: con tre consonanti la prima va col primo nucleo, le altre due col secondo: con quattro: due per nucleo etc. etc.

            // Sillabe con nuclei a vocale singola
            string current_syllable = "";
            int pointer = 0;
            bool nucleus_found = false;
            while (pointer < word.Length)
            {
                char carattere = word[pointer];
                if (vowels_set.Contains(carattere) == false)
                {
                    // consonant
                    current_syllable += carattere;
                    pointer++;
                }
                else
                {
                    nucleus_found = true;
                    // vocale: termina il nucleo, ma alla sillaba vanno aggiunte metà meno uno delle eventuali consonanti che seguono
                    current_syllable += carattere;
                    int sub_pointer_initial_value = pointer + 1;
                    int sub_pointer = sub_pointer_initial_value;
                    int following_consonants_number = 0;
                    while (sub_pointer < word.Length)
                    {
                        if (vowels_set.Contains(word[sub_pointer]) == false)
                        {
                            // consonante
                            following_consonants_number++;
                        }
                        else
                        {
                            // siamo arrivati al nucleo della sillaba successiva, completa la sillaba precedente con metà delle consonanti trovate
                            //  dato che la divisione per due è integer, se ci sono 1, 3, 5 consonanti nel suffissio del primo nucleo
                            //   ce ne vanno 0, 1, 2, come deve essere
                            int inter_nucleus_consonants = sub_pointer - sub_pointer_initial_value;
                            int first_nucleus_suffix_consonants = inter_nucleus_consonants / 2;
                            for (int i = 0; i < first_nucleus_suffix_consonants; i++)
                            {
                                current_syllable += word[sub_pointer_initial_value + i];
                            }
                            out_syllables_single_vowel.Add(current_syllable);
                            current_syllable = "";
                            // e posizioniamoci sulla prima consonante (o la prima vocale, se non ci sono consonanti) del nucleo successivo
                            pointer = sub_pointer_initial_value + first_nucleus_suffix_consonants;
                            break;
                        }
                        sub_pointer++;
                    }
                    if (sub_pointer == word.Length)
                    {
                        // Capitiamo qua al termine di una parola, con due casi diversi:
                        //    la parola termina su una vocale
                        //    la parola termina su una consonante
                        if (vowels_set.Contains(word[word.Length - 1]))
                        {
                            // parola termina su una vocale: aggiungi la sillaba corrente
                            out_syllables_single_vowel.Add(current_syllable);
                            pointer++;
                            // arrivati qua pointer == word.Length, per cui si esce automaticamente anche dal while() più esterno
                        }
                        else
                        {
                            // parola termina su una consonante: aggiungi al nuleo precedente le consonanti che mancano
                            pointer = sub_pointer_initial_value;
                            while (pointer < word.Length)
                            {
                                current_syllable += word[pointer];
                                pointer++;
                            }
                            out_syllables_single_vowel.Add(current_syllable);
                            // arrivati qua pointer == word.Length, per cui si esce automaticamente anche dal while() più esterno                            
                        }
                    }
                }
            }
            if (nucleus_found == false)
            {
                // Parola che non contiene nemmeno una vocale, va inserita qua
                out_syllables_single_vowel.Add(word);
            }


            // Sillabe con nuclei a vocale multipla
            current_syllable = "";
            pointer = 0;
            nucleus_found = false;
            while (pointer < word.Length)
            {
                char carattere = word[pointer];
                if (vowels_set.Contains(carattere) == false)
                {
                    // consonant
                    current_syllable += carattere;
                    pointer++;
                }
                else
                {
                    nucleus_found = true;
                    // vocale: loop che inserisce tutte le vocali che trova nel nucleo
                    while (pointer < word.Length)
                    {
                        carattere = word[pointer];
                        if (vowels_set.Contains(carattere) == true)
                        {
                            // Vocale, aggiungila al nucleo
                            current_syllable += carattere;
                            pointer++;
                        }
                        else
                        {
                            // Consonante: il nucleo è terminato
                            pointer--; // serve... rimette a posto il puntatore per la fase successiva
                            break;
                        }
                    }
                    if (pointer == word.Length)
                    {
                        // la parola termina qua (alla fine di un nucleo): inseriamo la sillaba appena trovata e usciamo dal while più esterno
                        out_syllables_multiple_vowel.Add(current_syllable);
                        break;
                    }


                    // Else abbiamo trovato il nucleo e la parola prosegue, vanno aggiunte metà meno uno delle eventuali consonanti che seguono
                    //   (da qua in poi la gestione è identica al caso 'sillabe con nuclei di una sola vocale'
                    int sub_pointer_initial_value = pointer + 1;
                    int sub_pointer = sub_pointer_initial_value;
                    int following_consonants_number = 0;
                    while (sub_pointer < word.Length)
                    {
                        if (vowels_set.Contains(word[sub_pointer]) == false)
                        {
                            // consonante
                            following_consonants_number++;
                        }
                        else
                        {
                            // siamo arrivati al nucleo della sillaba successiva: completa la sillaba precedente con metà delle consonanti trovate
                            //  dato che la divisione per due è integer, se ci sono 1, 3, 5 consonanti nel suffissio del primo nucleo
                            //   ce ne vanno 0, 1, 2, come deve essere
                            int inter_nucleus_consonants = sub_pointer - sub_pointer_initial_value;
                            int first_nucleus_suffix_consonants = inter_nucleus_consonants / 2;
                            for (int i = 0; i < first_nucleus_suffix_consonants; i++)
                            {
                                current_syllable += word[sub_pointer_initial_value + i];
                            }
                            out_syllables_multiple_vowel.Add(current_syllable);
                            current_syllable = "";
                            // e posizioniamoci sulla prima consonante (o la prima vocale, se non ci sono consonanti) del nucleo successivo
                            pointer = sub_pointer_initial_value + first_nucleus_suffix_consonants;
                            break;
                        }
                        sub_pointer++;
                    }
                    if (sub_pointer == word.Length)
                    {
                        // Capitiamo qua al termine di una parola, con due casi diversi:
                        //    la parola termina su una vocale
                        //    la parola termina su una consonante
                        if (vowels_set.Contains(word[word.Length - 1]))
                        {
                            // parola termina su una vocale: aggiungi la sillaba corrente
                            out_syllables_multiple_vowel.Add(current_syllable);
                            pointer++;
                            // arrivati qua pointer == word.Length, per cui si esce automaticamente anche dal while() più esterno
                        }
                        else
                        {
                            // parola termina su una consonante: aggiungi al nuleo precedente le consonanti che mancano
                            pointer = sub_pointer_initial_value;
                            while (pointer < word.Length)
                            {
                                current_syllable += word[pointer];
                                pointer++;
                            }
                            out_syllables_multiple_vowel.Add(current_syllable);
                            // arrivati qua pointer == word.Length, per cui si esce automaticamente anche dal while() più esterno                            
                        }
                    }
                }
            }
            if (nucleus_found == false)
            {
                // Parola che non contiene nemmeno una vocale, va inserita qua
                out_syllables_multiple_vowel.Add(word);
            }

            // DEBUG INSTRUCTIONS
            //string message = word + " = ";
            //foreach (string syllable in out_syllables_multiple_vowel)
            //{
            //  message += syllable + "-";
            //}
            //Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            return;
        }



    }
}
