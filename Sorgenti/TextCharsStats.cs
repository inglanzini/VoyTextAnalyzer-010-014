using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class TextCharsStats
    {

        public static mdError get_monograms_stats(string source_text, string charset, ref TextAnalysis_results analysis_results)
        {
            mdError error = new mdError();
        
            // Costruiamo l'elenco completo dei caratteri (la chiave del dizionario) col loro numero di occorrenze
            Dictionary<string, EValueOcc> monograms = new Dictionary<string, EValueOcc>(); // Dictionary per velocità di elaborazione (ma non è sortabile)

            for (int cursor = 0; cursor < source_text.Length; cursor++)
            {
                EValueOcc old_value = new EValueOcc();
                if (monograms.TryGetValue(source_text[cursor].ToString(), out old_value) == true)
                {
                    old_value.value += 1;
                    monograms.Remove(source_text[cursor].ToString());
                    monograms.Add(source_text[cursor].ToString(), old_value);
                }
                else
                {
                    EValueOcc new_value = new EValueOcc(); // impossibile riciclare old_value qua... provare per credere
                    new_value.element = source_text[cursor].ToString();
                    new_value.value = 1;
                    monograms.Add(source_text[cursor].ToString(), new_value);
                }
            }

            // Dal dizionario compiliamo (e ordiniamo) tutte le statistiche derivate dai monogrammi
            get_and_format_all_monograms_based_stats(monograms, ref analysis_results);

            return error;        
        }

        public static void get_and_format_all_monograms_based_stats(Dictionary<string, EValueOcc> monograms, ref TextAnalysis_results analysis_results)
        {
            // trasferiamo il dictionary in una lista, che è più comoda per elaborazioni successive (p.es. il dizionario non supporta il sorting)
            foreach (EValueOcc item in monograms.Values)
            {
                EValueOcc new_value = new EValueOcc();
                new_value.element = item.element;
                new_value.value = item.value;
                analysis_results.monograms_distribution.Add(new_value);
            }
            IComparer<EValueOcc> comparer_by_value_occ = new CompareEValueOccByValue();
            analysis_results.monograms_distribution.Sort(comparer_by_value_occ);

            // dalla lista precedente ricaviamo quella senza il carattere SPACE
            foreach (EValueOcc item in analysis_results.monograms_distribution)
            {
                if (item.element != " ")
                {
                    EValueOcc new_value = new EValueOcc();
                    new_value.element = item.element;
                    new_value.value = item.value;
                    analysis_results.monograms_distribution_excluding_spaces.Add(new_value);
                }
            }
            analysis_results.monograms_distribution_excluding_spaces.Sort(comparer_by_value_occ);


            // Adesso calcoliamo la distribuzione teorica delle FREQUENZE dei bigrammi, basandoci solo sulle occorrenze dei monogrammi, che servirà come base di riferimento per
            //  statistiche successive (direi abbia poco senso calcolarne anche una versione senza spaces)
            // Perchè FREQUENZE e non OCCORRENZE? Perchè le occorrenze sono integer e si perde risoluzione nei calcoli, per esempio se un bigramma
            //   ha una probabilità teorica bassa, e il testo ha non troppi caratteri, potrebbe avere meno di una occorrenza stimata, che verrebbe troncata a zero
            // Vengono calcolate contemporaneamente sia la distribuzione in forma tabellare che quella lineare
            long total_occurrences = 0;
            foreach (EValueOcc item in analysis_results.monograms_distribution)
            {
                total_occurrences += item.value;
            }
            foreach (EValueOcc EValue_1st in analysis_results.monograms_distribution)
            {
                List<EValue> scratch_list = new List<EValue>();
                foreach (EValueOcc EValue_2nd in analysis_results.monograms_distribution)
                {
                    EValue new_value = new EValue();
                    new_value.element = EValue_1st.element + EValue_2nd.element;
                    if (EValue_1st.element == " " && EValue_2nd.element == " ")
                    {
                        new_value.value = 0;    // SPACE-SPACE viene messo a zero (come è giusto che sia, è sicuramente una combinazione proibita)
                    }
                    else
                    {
                        float frequency_1st = (float)((double)EValue_1st.value / (double)total_occurrences);
                        float frequency_2nd = (float)((double)EValue_2nd.value / (double)total_occurrences);
                        new_value.value = frequency_1st * frequency_2nd;
                    }
                    scratch_list.Add(new_value);
                    analysis_results.bigrams_theoretical_distribution.Add(new_value);
                }
                analysis_results.bigrams_theoretical_distribution_table.Add(scratch_list);
            }
            IComparer<EValue> comparer_by_value = new CompareEValueByValue();
            analysis_results.bigrams_theoretical_distribution.Sort(comparer_by_value);
        }








        public static mdError get_bigrams_stats(string source_text, string charset, ref TextAnalysis_results analysis_results)
        {
            mdError error = new mdError();

            // Uso la stessa tecnica già usata per i monogrammi: prima creo un dictionary contenente tutti i bigrammi, poi inizio la scansione del testo
            //  e aggiorno il dizionario

            // E' IMPORTANTE CHE I BIGRAMMI SIANO INSERITI NEL DIZIONARIO NELLO STESSO ORDINE CON CUI SONO STATI INSERITI NELLE TABELLE DELLE DISTRIBUZIONI TEORICHE,
            //   CIOE' SEGUENDO L'ORDINE DI FREQUENZA DEI MONOGRAMMI!

            Dictionary<string, EValueOcc> bigrams = new Dictionary<string, EValueOcc>();

            string bigram_key = "";
            foreach (EValueOcc EValue_1st in analysis_results.monograms_distribution)
            {
                foreach (EValueOcc EValue_2nd in analysis_results.monograms_distribution)
                {
                    EValueOcc old_value = new EValueOcc();
                    bigram_key = EValue_1st.element + EValue_2nd.element;
                    if (bigrams.TryGetValue(bigram_key, out old_value) == true)
                    {
                        string message = "SOFTWARE ERROR: duplicated bigram while creating basic dictionary structure in get_bigrams_stats";
                        Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    }
                    else
                    {
                        EValueOcc new_value = new EValueOcc(); // impossibile riciclare old_value qua... provare per credere
                        new_value.element = bigram_key;
                        new_value.value = 0;
                        bigrams.Add(bigram_key, new_value);
                    }
                }
            }

            // Scansione del testo con inserimento delle occorrenze dei bigrammi nel dizionario
            if (source_text.Length < 2)
            {
                string message = "ERROR: input text is only one character long, cannot calculate bigrams statistics";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                return error;
            }
            char first_char = source_text[0];
            char second_char;
            for (int cursor = 0; cursor < source_text.Length - 1; cursor++)
            {
                second_char = source_text[cursor + 1];
                string this_bigram = first_char.ToString() + second_char.ToString();

                EValueOcc old_value = new EValueOcc();
                if (bigrams.TryGetValue(this_bigram, out old_value) == true)
                {
                    old_value.value += 1;
                    bigrams.Remove(this_bigram);
                    bigrams.Add(this_bigram, old_value);
                }
                else
                {
                    string message = "SOFTWARE ERROR: could not find a bigram in dictionary in get_bigrams_stats";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    break;
                }
                first_char = second_char;
            }

            // Dal dizionario compiliamo (e ordiniamo) tutte le statistiche derivate dai bigrammi
            get_and_format_all_bigrams_based_stats(bigrams, ref analysis_results);


            return error;
        }

        public static void get_and_format_all_bigrams_based_stats(Dictionary<string, EValueOcc> bigrams, ref TextAnalysis_results analysis_results)
        {

            // trasferiamo il dictionary nella lista, che è più comoda per elaborazioni successive (p.es. il dizionario non supporta il sorting)
            foreach (EValueOcc item in bigrams.Values)
            {
                EValueOcc new_value = new EValueOcc();
                new_value.element = item.element;
                new_value.value = item.value;
                analysis_results.bigrams_distribution.Add(new_value);
            }

            // ATTENZIONE!!!!! LA LISTA TABELLARE VA GENERATA PRIMA DEL SORTING PER NON SCASINARE COMPLETAMENTE LE STRINGHE!!!
            // E adesso creiamo la lista in forma di tabella. AL SOLITO, ATTENZIONE A NON SCAMBIAR RIGHE CON COLONNE..
            // I bigrammi vengono separati in blocchi lunghi quanto il numero di MONOGRAMMI trovati (se tutto è andato bene la lista lineare sarà lunga #monogrammi al quadrato)
            int chunk_index = 0;
            List<EValueOcc> scratch_list = new List<EValueOcc>();
            foreach (EValueOcc item in analysis_results.bigrams_distribution)
            {
                if (chunk_index == 0)
                {
                    scratch_list = new List<EValueOcc>();
                }

                EValueOcc new_value = new EValueOcc();
                new_value.element = item.element;
                new_value.value = item.value;
                scratch_list.Add(new_value);

                chunk_index++;
                //  MONOgrams_distribuition.Count è giusto!!
                if (chunk_index == analysis_results.monograms_distribution.Count)
                {
                    analysis_results.bigrams_distribution_table.Add(scratch_list);
                    chunk_index = 0;
                }
            }




            // Adesso calcoliamo la distribuzione dei bigrammi divisi per la distribuzione teorica (suppression/enhanchement dei bigrammi)

            // Caso lineare
            //  Qua c'era un bug rognoso nella versione precedente: usavo un for (come più sotto nella versione tabellare) ipotizzando che le due liste
            //    avessero gli element (le stringhe dei bigrammi) ordinate allo stesso modo, ma non è così perchè entrambe le liste sono state ordinate per value,
            //    cosa che mette gli elements fuori ordine! Vabbè, riordino entrambe le liste alfabeticamente e così vado sul sicuro
            IComparer<EValueOcc> comparer_alphabetic_occ = new CompareEValueOccByElement_AlphabeticOrder();
            analysis_results.bigrams_distribution.Sort(comparer_alphabetic_occ);
            IComparer<EValue> comparer_alphabetic = new CompareEValueByElement_AlphabeticOrder();
            analysis_results.bigrams_theoretical_distribution.Sort(comparer_alphabetic);

            // La distribuzione dei bigrammi reale è in OCCORRENZE, mentre i bigrammi teorici sono già FREQUENZE, devo trasformare in frequenze anche i bigrammi reali per il calcolo
            long total_occurrences = 0;
            foreach (EValueOcc item in analysis_results.bigrams_distribution)
            {
                total_occurrences += item.value;
            }
            for (int i = 0; i < analysis_results.bigrams_distribution.Count; i++)
            {
                EValue new_value = new EValue();
                if (analysis_results.bigrams_theoretical_distribution[i].value != 0)
                {
                    float bigram_frequency = (float)analysis_results.bigrams_distribution[i].value / (float)total_occurrences;
                    new_value.value = bigram_frequency / analysis_results.bigrams_theoretical_distribution[i].value;
                }
                else
                {
                    new_value.value = 0;
                }
                new_value.element = analysis_results.bigrams_distribution[i].element;
                analysis_results.bigrams_distribution_delta.Add(new_value);
            }

            //  E adesso riordiniamo le liste per valori
            IComparer<EValueOcc> comparer_by_value_occ = new CompareEValueOccByValue();
            analysis_results.bigrams_distribution.Sort(comparer_by_value_occ);
            IComparer<EValue> comparer_by_value = new CompareEValueByValue();
            analysis_results.bigrams_theoretical_distribution.Sort(comparer_by_value);
            analysis_results.bigrams_distribution_delta.Sort(comparer_by_value);


            // Caso tabellare
            for (int i = 0; i < analysis_results.bigrams_distribution_table.Count; i++)
            {
                List<EValue> scratch_list_EV = new List<EValue>();
                for (int j = 0; j < analysis_results.bigrams_distribution_table[i].Count; j++)
                {
                    EValue new_value = new EValue();
                    if (analysis_results.bigrams_theoretical_distribution_table[i][j].value != 0)
                    {
                        float bigram_frequency = (float)analysis_results.bigrams_distribution_table[i][j].value / (float)total_occurrences;
                        new_value.value = bigram_frequency / analysis_results.bigrams_theoretical_distribution_table[i][j].value;
                    }
                    else
                    {
                        new_value.value = 0;
                    }
                    new_value.element = analysis_results.bigrams_distribution_table[i][j].element;
                    scratch_list_EV.Add(new_value);
                }
                analysis_results.bigrams_distribution_delta_table.Add(scratch_list_EV);
            }
        }



        public static mdError get_bigrams_cXc_table(ref TextAnalysis_results analysis_results)
        {
            mdError error = new mdError();

            // Le tabelle cX e Xc meritano qualche spiegazione

            // La prima tabella, la 'Xc', dà, per ogni carattere X, la probabilità che sia seguito da un altro certo altro carattere c. Questa probabilità non
            //  considera la frequenza globale del carattere, ma solo che probabilità ha di essere seguito da qualcosa d'altro. Per esempio: due caratteri (anche rarissimi),
            //  per esempio 'ç' e 'õ', potrebbero comparire solo come 'çõ', col che la ç avrebbe probabilità 1 di essere seguita da õ (il che significa anche che in
            //  questa la somma di tutte le righe dà 1)

            // La seconda tabella, la 'cX', è al contrario: considera la probabilità che il carattere X sia preceduto, invece che seguito, dal carattere c. In generale i numeri
            // sono completamente diversi da quelli della tabella precedente: p.es. in Italiano 'q' è seguita da 'u' il 100% delle volte, ma è preceduta quasi sempre da SPACE
            //  (e a volte da altre lettere, come in 'acqua'). Ma anche qua la somma di tutte le righe (o colonne, dipende da come si gira la tabella) dà 1.


            if (analysis_results.bigrams_distribution_table.Count == 0) // Trappa casi sfigati (p.es. testi lunghi solo 1 carattere e senza bigrammi)
            {
                return error;
            }



            // Tabella Xc: partendo dalla distribuzione dei digrammi, estraiamo i dati riga per riga, poi normalizziamo ogni riga in modo che la somma finale sia 1
            // Al solito usiamo il numero dei MONOgrammi per il termine dei loops

            // NOTARE che in tutte queste routines si parte da liste di EValueOcc (monograms_distribution, bigrams_distribution) e si esce con liste di EValue
            //      nel codice non lo si vede perchè si usano solo cicli for (col foreach si vedrebbe)
            for (int i = 0; i < analysis_results.monograms_distribution.Count; i++)
            {

                List<EValue> scratch_list = new List<EValue>();
                float total_sum = 0;
                for (int j = 0; j < analysis_results.monograms_distribution.Count; j++) // lettura di una riga
                {
                    EValue _value = new EValue();
                    _value.element = analysis_results.bigrams_distribution_table[i][j].element;
                    _value.value = analysis_results.bigrams_distribution_table[i][j].value;
                    total_sum += _value.value;
                    scratch_list.Add(_value);
                }
                // rinormalizzazione della riga
                List<EValue> normalized_list = new List<EValue>();
                foreach (EValue _value in scratch_list)
                {
                    EValue normalized_value = new EValue();
                    normalized_value.element = _value.element;
                    normalized_value.value = _value.value * ( 1/total_sum);
                    normalized_list.Add(normalized_value);
                }
                analysis_results.following_character_distribution.Add(normalized_list);
            }


            // Tabella cX: partendo dalla distribuzione dei digrammi, estraiamo i dati colonna per colonna, poi normalizziamo ogni colonna in modo che la somma finale sia 1
            //  Occhio che,ovviamente, gli indici sono scambiati di ruolo, e che la label del dato (es. "ab") va rovesciata ("ba")
            //  QUA CI SAREBBE VOLUTA UN FUNCTION, DUPLICA IL CODICE PRECEDENTE
            for (int i = 0; i < analysis_results.monograms_distribution.Count; i++)
            {

                List<EValue> scratch_list = new List<EValue>();
                float total_sum = 0;
                for (int j = 0; j < analysis_results.monograms_distribution.Count; j++) // lettura di una colonna
                {
                    EValue _value = new EValue();
                    string header = analysis_results.bigrams_distribution_table[j][i].element;
                    _value.element = header[1].ToString() + header[0].ToString();
                    _value.value = analysis_results.bigrams_distribution_table[j][i].value;
                    total_sum += _value.value;
                    scratch_list.Add(_value);
                }
                // rinormalizzazione della riga
                List<EValue> normalized_list = new List<EValue>();
                foreach (EValue _value in scratch_list)
                {
                    EValue normalized_value = new EValue();
                    normalized_value.element = _value.element;
                    normalized_value.value = _value.value * (1 / total_sum);
                    normalized_list.Add(normalized_value);
                }
                analysis_results.previous_character_distribution.Add(normalized_list);
            }



            // Trovate le tabelle cXc, calcoliamo quelle delle distanze
            // per ogni riga calcoliamo la distanza geometrica (radice quadrata della somma dei quadrati delle differenze di ogni cella) da ogni altra riga
            for (int row_index = 0; row_index < analysis_results.monograms_distribution.Count; row_index++) // per ogni riga
            {
                // get la riga
                List<EValue> row_list = new List<EValue>();
                foreach (EValue value in analysis_results.following_character_distribution[row_index])
                {
                    EValue evalue = new EValue();
                    evalue.element = value.element;
                    evalue.value = value.value;
                    row_list.Add(evalue);
                }
                // calcola le distanze da ogni altra riga
                List<EValue> distances = new List<EValue>();
                for (int scan_index = 0; scan_index < analysis_results.monograms_distribution.Count; scan_index++)
                {
                    float distance = 0;
                    for (int column_index = 0; column_index < analysis_results.monograms_distribution.Count; column_index++)
                    {
                        float delta = row_list[column_index].value - analysis_results.following_character_distribution[scan_index][column_index].value;
                        distance += delta * delta;
                    }
                    distance = (float)Math.Sqrt((double)distance);
                    EValue evalue = new EValue();
                    evalue.value = distance;
                    // troppo lungo spiegare perchè l'element (che qua alla fin fine serve solo per labellare rigeh e colonne) va calcolato così
                    string source_element = row_list[0].element;
                    string destination_element = analysis_results.following_character_distribution[scan_index][scan_index].element;
                    evalue.element = source_element[0].ToString() + destination_element[1].ToString();
                    distances.Add(evalue);
                }
                // inserisci le distanze nella tabella destinazione
                analysis_results.monograms_distances_according_to_following_character.Add(distances);
            }
            // OKAY, AVREI DOVUTO DEFINIRE UNA FUNCTION... DUPLICA IL CODICE DELLA TABELLA PRECDENTE
            for (int row_index = 0; row_index < analysis_results.monograms_distribution.Count; row_index++) // per ogni riga
            {
                // get la riga
                List<EValue> row_list = new List<EValue>();
                foreach (EValue value in analysis_results.previous_character_distribution[row_index])
                {
                    EValue evalue = new EValue();
                    evalue.element = value.element;
                    evalue.value = value.value;
                    row_list.Add(evalue);
                }
                // calcola le distanze da ogni altra riga
                List<EValue> distances = new List<EValue>();
                for (int scan_index = 0; scan_index < analysis_results.monograms_distribution.Count; scan_index++)
                {
                    float distance = 0;
                    for (int column_index = 0; column_index < analysis_results.monograms_distribution.Count; column_index++)
                    {
                        float delta = row_list[column_index].value - analysis_results.previous_character_distribution[scan_index][column_index].value;
                        distance += delta * delta;
                    }
                    distance = (float)Math.Sqrt((double)distance);
                    EValue evalue = new EValue();
                    evalue.value = distance;
                    // troppo lungo spiegare perchè l'element (che qua alla fin fine serve solo per labellare rigeh e colonne) va calcolato così
                    string source_element = row_list[0].element;
                    string destination_element = analysis_results.previous_character_distribution[scan_index][scan_index].element;
                    evalue.element = source_element[0].ToString() + destination_element[1].ToString();
                    distances.Add(evalue);
                }
                // inserisci le distanze nella tabella destinazione
                analysis_results.monograms_distances_according_to_previous_character.Add(distances);
            }



            return error;
        }

        


        public class CompareEValueByValue : IComparer<EValue>
        {
            public int Compare(EValue x, EValue y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (1);
                }
                return (-1);
            }
        }

        public class CompareEValueByValue_reverse : IComparer<EValue>
        {
            public int Compare(EValue x, EValue y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (-1);
                }
                return (1);
            }
        }

        public class CompareEValueByElement_AlphabeticOrder : IComparer<EValue>
        {
            public int Compare(EValue x, EValue y)
            {
                if (String.Compare(x.element, y.element) == 0) return (0);

                if (String.Compare(x.element, y.element) > 0)
                {                        
                    return (1);
                }
                return (-1);
            }
        }


        public class CompareEValueByElement_NumericOrder : IComparer<EValue>
        {
            public int Compare(EValue x, EValue y)
            {
                int x_number = Int32.Parse(x.element); // converte una stringa che rappresenta un numero in un numero
                int y_number = Int32.Parse(y.element);
                if (x_number == y_number) return (0);

                if (x_number > y_number)   // qua l'ordine del sort cambia ripestto a CompareEValueByValue, on effetti ci vorrebbero due routines per tipo
                {                          //  (sort in una direzione o nell'altra)
                    return (1);
                }
                return (-1);
            }           
        }



        public class CompareEValueOccByValue : IComparer<EValueOcc>
        {
            public int Compare(EValueOcc x, EValueOcc y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (1);
                }
                return (-1);
            }
        }

        public class CompareEValueOccByElement_AlphabeticOrder : IComparer<EValueOcc>
        {
            public int Compare(EValueOcc x, EValueOcc y)
            {
                if (String.Compare(x.element, y.element) == 0) return (0);

                if (String.Compare(x.element, y.element) > 0)
                {
                    return (1);
                }
                return (-1);
            }
        }


        public class CompareEValueOccByElement_NumericOrder : IComparer<EValueOcc>
        {
            public int Compare(EValueOcc x, EValueOcc y)
            {
                int x_number = Int32.Parse(x.element); // converte una stringa che rappresenta un numero in un numero
                int y_number = Int32.Parse(y.element);
                if (x_number == y_number) return (0);

                if (x_number > y_number)   // qua l'ordine del sort cambia ripestto a CompareEValueByValue, on effetti ci vorrebbero due routines per tipo
                {                          //  (sort in una direzione o nell'altra)
                    return (1);
                }
                return (-1);
            }
        }

        // Comparer SPECIALIZZATO per i bigrammi, non usarlo per altro!!! Ordina solo elementi che hanno la forma "AB"
        public class CompareEValueOcc_FollowList_monograms : IComparer<EValueOcc>
        {
            public int Compare(EValueOcc x, EValueOcc y)
            {
                // trova la posizione dell'elemento x nella lista
                int x_position = 0;
                foreach (EValueOcc list_value_row in Form1.text_analyzer.analysis_results.monograms_distribution)
                {
                    if (x.element[0].ToString() == list_value_row.element)
                    {
                        break;
                    }
                    x_position++;
                }

                // trova la posizione dell'elemento y nella lista
                int y_position = 0;
                foreach (EValueOcc list_value_row in Form1.text_analyzer.analysis_results.monograms_distribution)
                {
                    if (y.element[0].ToString() == list_value_row.element)
                    {
                        break;
                    }
                    y_position++;
                }

                if (x_position == y_position) // stessa posizione di riga
                {
                    // ordinamento secondario sul secondo carattere dell'elemento "AB"
                    x_position = 0;
                    foreach (EValueOcc list_value_row in Form1.text_analyzer.analysis_results.monograms_distribution)
                    {
                        if (x.element[1].ToString() == list_value_row.element)
                        {
                            break;
                        }
                        x_position++;
                    }

                    // trova la posizione dell'elemento y nella lista
                    y_position = 0;
                    foreach (EValueOcc list_value_row in Form1.text_analyzer.analysis_results.monograms_distribution)
                    {
                        if (y.element[1].ToString() == list_value_row.element)
                        {
                            break;
                        }
                        y_position++;
                    }

                    if (x_position == y_position) return (0);

                    if (x_position < y_position)
                    {
                        return (-1);
                    }
                    return (1);
                }

                // else x_pos != y_pos
                if (x_position < y_position)
                {
                    return (-1);
                }
                return (1);
            }
        }


        public class CompareEValue_2dByValue_reverse : IComparer<EValue_2d>
        {
            public int Compare(EValue_2d x, EValue_2d y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (-1);
                }
                return (1);
            }
        }

        public class CompareEValue_2dFollowList_row : IComparer<EValue_2d>
        {
            public int Compare(EValue_2d x, EValue_2d y)
            {
                // trova la posizione dell'elemento x nella lista
                int x_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (x.element_row == element)
                    {
                        break;
                    }
                    x_position++;
                }

                // trova la posizione dell'elemento y nella lista
                int y_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (y.element_row == element)
                    {
                        break;
                    }
                    y_position++;
                }

                if (x_position== y_position) return (0);

                if (x_position < y_position)
                {
                    return (-1);
                }
                return (1);
            }
        }


        public class CompareEValue_2dFollowList_column : IComparer<EValue_2d>
        {
            public int Compare(EValue_2d x, EValue_2d y)
            {
                // trova la posizione dell'elemento x nella lista
                int x_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (x.element_column == element)
                    {
                        break;
                    }
                    x_position++;
                }

                // trova la posizione dell'elemento y nella lista
                int y_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (y.element_column == element)
                    {
                        break;
                    }
                    y_position++;
                }

                if (x_position == y_position) return (0);

                if (x_position < y_position)
                {
                    return (-1);
                }
                return (1);
            }
        }


        public class CompareListEValue_2dFollowList_row : IComparer<List<EValue_2d>>
        {
            public int Compare(List<EValue_2d> x, List<EValue_2d> y)
            {
                // trova la posizione dell'elemento x nella lista
                int x_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (x[0].element_row == element)
                    {
                        break;
                    }
                    x_position++;
                }

                // trova la posizione dell'elemento y nella lista
                int y_position = 0;
                foreach (string element in FormCompare.order_1st_row)
                {
                    if (y[0].element_row == element)
                    {
                        break;
                    }
                    y_position++;
                }

                if (x_position == y_position) return (0);

                if (x_position < y_position)
                {
                    return (-1);
                }
                return (1);
            }
        }


        public class CompareEValue_extendedByValue : IComparer<EValue_extended>
        {
            public int Compare(EValue_extended x, EValue_extended y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (1);
                }
                return (-1);
            }
        }

        public class CompareEValueOcc_extendedByValue : IComparer<EValueOcc_extended>
        {
            public int Compare(EValueOcc_extended x, EValueOcc_extended y)
            {
                if (x.value == y.value) return (0);

                if (x.value < y.value)
                {
                    return (1);
                }
                return (-1);
            }
        }


        public class CompareEValue_extendedByElement_AlphabeticOrder : IComparer<EValue_extended>
        {
            public int Compare(EValue_extended x, EValue_extended y)
            {
                if (String.Compare(x.element, y.element) == 0) return (0);

                if (String.Compare(x.element, y.element) > 0)
                {
                    return (1);
                }
                return (-1);
            }
        }

    }
}
