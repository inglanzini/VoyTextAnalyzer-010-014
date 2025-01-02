using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template
{
    public partial class FormCompare : Form
    {

        public static FormCompare mainFormCompare;

        public static CompareResults compare_results = new CompareResults();   // classe primaria di questa Form


        public static readonly int rare_characters_min_cutoff = 50;
        public static readonly int rare_characters_default_cutoff = 250;       // serve a findBestMatches ! (in Form1)
        public static readonly int rare_characters_max_cutoff = 2000;
        private static int rare_characters_cutoff;


        private static int graphs_limit_1d;                                   // Simile a quello di Form1
        private static List<int> current_graphs_limit_2d = new List<int>();   // Simile a quello di Form1, ma qua è un array per dare la possibilità di limitare ogni
                                                                              //      grafico ad un diverso valore (p.es. per escludere i 'caratteri rari' lingua per lingua
                                                                              
        private static List<int> base_graphs_limit_2d = new List<int>();      // Limiti base (quelli che visualizzano tutti i caratteri per ogni lingua, calcolato in inizializzazione)
        private static List<int> rare_chars_removed_graphs_limit_2d = new List<int>();   // Limiti ai quali ridurre i grafici con i 'rare chars' removed. Calcolato in inizializzazione
                                                                                         //   ed ogni volta che si modifica il valore del rare_charcaters_cutoff   



        private static int words_to_examine_min = 50;                         // Parametri per additional report vocabulary
        private static int words_to_examine_default = 200;                  
        private static float amplification_threshold_min = 4;
        private static float amplification_threshold_default = 10;
        private static int words_to_examine;
        private static float amplification_threshold;

        private static float accepted_threshold_unblinded = 0.042f;
        private static float rejected_threshold_unblinded = 0.070f;

        private static float accepted_threshold_blind = 0.059f;
        private static float rejected_threshold_blind = 0.078f;





        // Dati del report finale del Compare
        private static Dictionary<string, EValue> cluster_unblinded_accepted = new Dictionary<string, EValue>();
        private static Dictionary<string, EValue> cluster_unblinded_dubious = new Dictionary<string, EValue>();
        private static Dictionary<string, EValue> cluster_blind_accepted = new Dictionary<string, EValue>();
        private static Dictionary<string, EValue> cluster_blind_dubious = new Dictionary<string, EValue>();

        private static List<EValue> unclustered = new List<EValue>();                  // contiene nome + distanza unblinded
        private static List<float> unclustered_blind_distances = new List<float>();    // contiene la corrispondente distanza blinded


        // Dati dell'additional report vocabolario
        List<EValue_extended> unusual_words_list = new List<EValue_extended>();



        // Questa variabile viene usata da CompareEValue_2dFollowList_row e CompareEValue_2dFollowList_column E' brutto, ma purtroppo non ho trovato un modo semplice
        //     per poter passare un parametro ad un comparer
        public static List<string> order_1st_row = new List<string>();



        public enum BlindFactor2d
        {
            _wholly_blinded = 0,                    // nessuna ipotesi su cosa rappresentino i caratteri: tabelle comparate cosi' come sono
            _wholly_blinded_but_try_reshuffle = 1,  //   come sopra, ma si cerca di ridisporre le righe/colonne per minimizzare la distanza calcolata
            _unblinded = 2,                         // si ipotizza che uno stesso carattere rappresnti la stessa cosa nei due testi: le tabelle vengono riordinate in
                                                    //   modo che abbiano la stessa sequenza dei caratteri
        };


        public enum BlindFactor1d
        {
            _wholly_blinded = 0,                    // nessuna ipotesi su cosa rappresentino i caratteri: vettori comparati cosi' come sono
            _unblinded = 1,                         // si ipotizza che uno stesso carattere rappresenti la stessa cosa nei due testi: i vettori vengono riordinati in
                                                    //   modo che le parole uguali vengano comparate fra di loro
        };


        public FormCompare()
        {
            InitializeComponent();

            mainFormCompare = this;  // Variabile che torna utile in vari casi per accedere alla Form!

            CompareResults compare_results = new CompareResults();

            initialize_controls();
            display_controls();


            // ATTENZIONE: bisognerebbe aggiungere una thread, altrimenti si può andare in timeout (vedere se basta mettere i calcoli sotto thread)
            calculate_and_display_data(Form1._display_all);
        }


        private void initialize_controls()
        {

            graphs_limit_1d = Form1.default_linear_graph_length;
            trackBar_Compare_linear_size.Minimum = Form1.min_linear_graph_length;
            trackBar_Compare_linear_size.Maximum = Form1.max_linear_graph_length;
            trackBar_Compare_linear_size.ValueChanged -= new System.EventHandler(trackBar_Compare_linear_size_ValueChanged);
            trackBar_Compare_linear_size.Value = graphs_limit_1d;
            trackBar_Compare_linear_size.ValueChanged += new System.EventHandler(trackBar_Compare_linear_size_ValueChanged);

            // Calcola i base limits dei grafici 2d, e preset i limiti attuali a quelli base
            base_graphs_limit_2d = new List<int>();
            current_graphs_limit_2d = new List<int>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                base_graphs_limit_2d.Add(analysis.analysis_results.monograms_distribution.Count);  // Setta ogni grafico 2D al suo limite massimo
                current_graphs_limit_2d.Add(analysis.analysis_results.monograms_distribution.Count);
            }

            // calcola i limiti col rare_characters_cutoff
            rare_characters_cutoff = rare_characters_default_cutoff;
            textBox_Compare_rare_characters_cutoff.TextChanged -= new System.EventHandler(textBox_Compare_rare_characters_cutoff_TextChanged);
            textBox_Compare_rare_characters_cutoff.Text = rare_characters_cutoff.ToString();
            textBox_Compare_rare_characters_cutoff.TextChanged += new System.EventHandler(textBox_Compare_rare_characters_cutoff_TextChanged);
            rare_chars_removed_graphs_limit_2d = get_2d_limits(rare_characters_cutoff);

            // Se il cutoff è attivo modifica i limiti attuali
            if (checkBox_Compare_remove_rare_characters.Checked == true)
            {
                current_graphs_limit_2d = get_2d_limits(rare_characters_cutoff);
            }



            // Parametri additional report vocabulary
            words_to_examine = words_to_examine_default;
            textBox_Compare_additional_report_words_limit.TextChanged -= new System.EventHandler(textBox_Compare_additional_report_words_limit_TextChanged);
            textBox_Compare_additional_report_words_limit.Text = words_to_examine.ToString();
            textBox_Compare_additional_report_words_limit.TextChanged += new System.EventHandler(textBox_Compare_additional_report_words_limit_TextChanged);

            amplification_threshold = amplification_threshold_default;
            textBox_Compare_additional_report_min_amplification.TextChanged -= new System.EventHandler(textBox_Compare_additional_report_min_amplification_TextChanged);
            textBox_Compare_additional_report_min_amplification.Text = amplification_threshold.ToString();
            textBox_Compare_additional_report_min_amplification.TextChanged += new System.EventHandler(textBox_Compare_additional_report_min_amplification_TextChanged);

            enforce_controls_coherency();

            // settature splitters

        }
        private void display_controls()
        {
            textBox_Compare_linear_size.TextChanged -= new System.EventHandler(textBox_Compare_linear_size_TextChanged);
            textBox_Compare_linear_size.Text = graphs_limit_1d.ToString();
            textBox_Compare_linear_size.TextChanged += new System.EventHandler(textBox_Compare_linear_size_TextChanged);
        }

        private void enforce_controls_coherency()
        {        
        }

        private static void set_splitter_to_middle_horizontal(SplitContainer container)
        {
            container.SplitterDistance = container.Width / 2;
        }
        private static void set_splitter_to_middle_vertical(SplitContainer container)
        {
            container.SplitterDistance = container.Height / 2;
        }



        public static List<int> get_2d_limits(int rare_characters_cutoff)
        {
            List<int> limits = new List<int>();

            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                // Ci servono le frequenze dei monogrammi per sapere quanti caratteri scartare: convertiamo da EValueOcc (occorrenze) ad EValue (frequenze)
                List<EValue> monograms_frequencies = Form1.EVconvert(analysis.analysis_results.monograms_distribution);

                // calcoliamo il limite
                int current_limit = 1;
                foreach (EValue value in monograms_frequencies)
                {
                    if (value.value < (1/(float)rare_characters_cutoff))
                    {
                        break;
                    }
                    current_limit++;
                }
                // nel caso in cui tutti i caratteri del testo abbiano una frequenza superiore al cutoff il foreach viene completato e il risultato
                //    è che current_limit vale 1 in più del numero reale dei caratteri, bisogna rimetterlo a posto
                if (current_limit > monograms_frequencies.Count) current_limit = monograms_frequencies.Count;
                limits.Add(current_limit);
            }

            return limits;
        }


        private static void update_graphs_limits_display()
        {
            // megacasino se non si disabilitano gli eventi, perchè altrimenti viene richiamato l'evento TextChanged sulle textBoxes
            mainFormCompare.textBox_Compare_linear_size.TextChanged -= new System.EventHandler(mainFormCompare.textBox_Compare_linear_size_TextChanged);
            mainFormCompare.textBox_Compare_linear_size.Text = graphs_limit_1d.ToString();
            mainFormCompare.textBox_Compare_linear_size.TextChanged += new System.EventHandler(mainFormCompare.textBox_Compare_linear_size_TextChanged);
            mainFormCompare.trackBar_Compare_linear_size.ValueChanged -= new System.EventHandler(mainFormCompare.trackBar_Compare_linear_size_ValueChanged);
            mainFormCompare.trackBar_Compare_linear_size.Value = graphs_limit_1d;
            mainFormCompare.trackBar_Compare_linear_size.ValueChanged += new System.EventHandler(mainFormCompare.trackBar_Compare_linear_size_ValueChanged);
        }


        private void calculate_2d_data()
        {
            Form1.newline_to_mainStatusWindow("\tBigrams...");
            calculate_bigrams_comparisons();
            Form1.newline_to_mainStatusWindow("\tFollowing character...");
            calculate_following_char_comparisons();
            Form1.newline_to_mainStatusWindow("\tPrevious character...");
            calculate_previous_char_comparisons();
            Form1.newline_to_mainStatusWindow("\tCharacters distance following...");
            calculate_following_distances_comparisons();
            Form1.newline_to_mainStatusWindow("\tCharacters distance previous...");
            calculate_previous_distances_comparisons();
        }

        private void calculate_1d_data()
        {
            Form1.newline_to_mainStatusWindow("\tSingle characters...");
            calculate_monograms_distances_comparisons();
            Form1.newline_to_mainStatusWindow("\tWords distribution...");
            calculate_vocabulary_distances_comparisons();
            Form1.newline_to_mainStatusWindow("\tWords length distributions...");
            calculate_wordslength_distances_comparisons();
            Form1.newline_to_mainStatusWindow("\tPseudo-syllables...");
            calculate_syllables_distances_comparisons();
        }

        private void calculate_bigrams_comparisons()
        {
            compare_results.bigrams_distribution = new List<List<List<EValueOcc>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValueOcc>> single_analysis = new List<List<EValueOcc>>();
                foreach (List<EValueOcc> row in analysis.analysis_results.bigrams_distribution_table)
                {
                    List<EValueOcc> single_row = new List<EValueOcc>();
                    foreach (EValueOcc single_value in row)
                    {
                        single_row.Add(single_value); // Uhhh occhio... avrei dovuto creare un EvalueOcc nuovo!!! E non solo qua.... anche in tutti gli omologhi di questa parte
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.bigrams_distribution.Add(single_analysis);
            }
            compare_results.bigrams_distances_from_first_blind = calculate_distances_from_first_2d(Form1.EVconvert(compare_results.bigrams_distribution), BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.bigrams_distances_from_first_unblinded = calculate_distances_from_first_2d(Form1.EVconvert(compare_results.bigrams_distribution), BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.bigrams_all_distances_blind = calculate_all_distances_2d(Form1.EVconvert(compare_results.bigrams_distribution), BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.bigrams_all_distances_unblinded = calculate_all_distances_2d(Form1.EVconvert(compare_results.bigrams_distribution), BlindFactor2d._unblinded, current_graphs_limit_2d);


            compare_results.bigrams_vs_theoric_distribution = new List<List<List<EValue>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.bigrams_distribution_delta_table)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        single_row.Add(single_value);
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.bigrams_vs_theoric_distribution.Add(single_analysis);
            }
            compare_results.bigrams_vs_theoric_distances_from_first_blind = calculate_distances_from_first_2d(compare_results.bigrams_vs_theoric_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.bigrams_vs_theoric_distances_from_first_unblinded = calculate_distances_from_first_2d(compare_results.bigrams_vs_theoric_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.bigrams_vs_theoric_all_distances_blind = calculate_all_distances_2d(compare_results.bigrams_vs_theoric_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.bigrams_vs_theoric_all_distances_unblinded = calculate_all_distances_2d(compare_results.bigrams_vs_theoric_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
        }

        private void calculate_following_char_comparisons()
        {
            compare_results.following_character_distribution = new List<List<List<EValue>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.following_character_distribution)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        single_row.Add(single_value);
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.following_character_distribution.Add(single_analysis);
            }
            compare_results.following_character_distances_from_first_blind = calculate_distances_from_first_2d(compare_results.following_character_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.following_character_distances_from_first_unblinded = calculate_distances_from_first_2d(compare_results.following_character_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.following_character_all_distances_blind = calculate_all_distances_2d(compare_results.following_character_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.following_character_all_distances_unblinded = calculate_all_distances_2d(compare_results.following_character_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
        }

        private void calculate_previous_char_comparisons()
        {
            compare_results.previous_character_distribution = new List<List<List<EValue>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.previous_character_distribution)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        single_row.Add(single_value);
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.previous_character_distribution.Add(single_analysis);
            }
            compare_results.previous_character_distances_from_first_blind = calculate_distances_from_first_2d(compare_results.previous_character_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.previous_character_distances_from_first_unblinded = calculate_distances_from_first_2d(compare_results.previous_character_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.previous_character_all_distances_blind = calculate_all_distances_2d(compare_results.previous_character_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.previous_character_all_distances_unblinded = calculate_all_distances_2d(compare_results.previous_character_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
        }

        private void calculate_following_distances_comparisons()
        {
            compare_results.following_chardistances_distribution = new List<List<List<EValue>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.monograms_distances_according_to_following_character)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        single_row.Add(single_value);
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.following_chardistances_distribution.Add(single_analysis);
            }
            compare_results.following_chardistances_distances_from_first_blind = calculate_distances_from_first_2d(compare_results.following_chardistances_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.following_chardistances_distances_from_first_unblinded = calculate_distances_from_first_2d(compare_results.following_chardistances_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.following_chardistances_all_distances_blind = calculate_all_distances_2d(compare_results.following_chardistances_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.following_chardistances_all_distances_unblinded = calculate_all_distances_2d(compare_results.following_chardistances_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
        }

        private void calculate_previous_distances_comparisons()
        {
            compare_results.previous_chardistances_distribution = new List<List<List<EValue>>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.monograms_distances_according_to_previous_character)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        single_row.Add(single_value);
                    }
                    single_analysis.Add(single_row);
                }
                compare_results.previous_chardistances_distribution.Add(single_analysis);
            }
            compare_results.previous_chardistances_distances_from_first_blind = calculate_distances_from_first_2d(compare_results.previous_chardistances_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.previous_chardistances_distances_from_first_unblinded = calculate_distances_from_first_2d(compare_results.previous_chardistances_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
            compare_results.previous_chardistances_all_distances_blind = calculate_all_distances_2d(compare_results.previous_chardistances_distribution, BlindFactor2d._wholly_blinded, current_graphs_limit_2d);
            compare_results.previous_chardistances_all_distances_unblinded = calculate_all_distances_2d(compare_results.previous_chardistances_distribution, BlindFactor2d._unblinded, current_graphs_limit_2d);
        }

        private void calculate_vocabulary_distances_comparisons()
        {
            compare_results.vocabulary_words_distribution = new List<List<EValueOcc>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc value in analysis.analysis_results.vocabulary_words_distribution)
                {
                    single_analysis.Add(value);
                }
                compare_results.vocabulary_words_distribution.Add(single_analysis);
            }
            compare_results.vocabulary_words_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.vocabulary_words_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.vocabulary_words_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.vocabulary_words_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
        }

        private void calculate_wordslength_distances_comparisons()
        {
            compare_results.wordslength_text_distribution = new List<List<EValueOcc_extended>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc_extended> single_analysis = new List<EValueOcc_extended>();
                foreach (EValueOcc_extended value in analysis.analysis_results.words_length_distribution_in_text)
                {
                    single_analysis.Add(value);
                }
                compare_results.wordslength_text_distribution.Add(single_analysis);
            }
            compare_results.wordslength_text_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.wordslength_text_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.wordslength_text_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.wordslength_text_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), BlindFactor1d._unblinded, graphs_limit_1d);


            compare_results.wordslength_vocabulary_distribution = new List<List<EValue>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValue> single_analysis = Form1.EVconvert(analysis.analysis_results.words_length_distribution_in_vocabulary);
                compare_results.wordslength_vocabulary_distribution.Add(single_analysis);
            }
            compare_results.wordslength_vocabulary_distances_from_first_blind = calculate_distances_from_first_1d(compare_results.wordslength_vocabulary_distribution, BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.wordslength_vocabulary_distances_from_first_unblinded = calculate_distances_from_first_1d(compare_results.wordslength_vocabulary_distribution, BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.wordslength_vocabulary_all_distances_blind = calculate_all_distances_1d(compare_results.wordslength_vocabulary_distribution, BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.wordslength_vocabulary_all_distances_unblinded = calculate_all_distances_1d(compare_results.wordslength_vocabulary_distribution, BlindFactor1d._unblinded, graphs_limit_1d);
        }

        private void calculate_syllables_distances_comparisons()
        {
            compare_results.syllables_singlevowel_distribution = new List<List<EValueOcc>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc value in analysis.analysis_results.syllables_distribution_single_vowels)
                {
                    single_analysis.Add(value);
                }
                compare_results.syllables_singlevowel_distribution.Add(single_analysis);
            }
            compare_results.syllables_singlevowel_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.syllables_singlevowel_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.syllables_singlevowel_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.syllables_singlevowel_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), BlindFactor1d._unblinded, graphs_limit_1d);


            compare_results.syllables_multiplevowel_distribution = new List<List<EValueOcc>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc value in analysis.analysis_results.syllables_distribution_multiple_vowels)
                {
                    single_analysis.Add(value);
                }
                compare_results.syllables_multiplevowel_distribution.Add(single_analysis);
            }
            compare_results.syllables_multiplevowel_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.syllables_multiplevowel_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.syllables_multiplevowel_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.syllables_multiplevowel_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
        }

        private void calculate_monograms_distances_comparisons()
        {
            compare_results.monograms_no_spaces_distribution = new List<List<EValueOcc>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc value in analysis.analysis_results.monograms_distribution_excluding_spaces)
                {
                    single_analysis.Add(value);
                }
                compare_results.monograms_no_spaces_distribution.Add(single_analysis);
            }
            compare_results.monograms_no_spaces_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_no_spaces_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.monograms_no_spaces_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_no_spaces_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.monograms_no_spaces_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_no_spaces_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.monograms_no_spaces_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_no_spaces_distribution), BlindFactor1d._unblinded, graphs_limit_1d);


            compare_results.monograms_distribution = new List<List<EValueOcc>>();
            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc value in analysis.analysis_results.monograms_distribution)
                {
                    single_analysis.Add(value);
                }
                compare_results.monograms_distribution.Add(single_analysis);
            }
            compare_results.monograms_distances_from_first_blind = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.monograms_distances_from_first_unblinded = calculate_distances_from_first_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
            compare_results.monograms_all_distances_blind = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_distribution), BlindFactor1d._wholly_blinded, graphs_limit_1d);
            compare_results.monograms_all_distances_unblinded = calculate_all_distances_1d(Form1.EVconvert_1d_multipli(compare_results.monograms_distribution), BlindFactor1d._unblinded, graphs_limit_1d);
        }




        private void calculate_and_display_data(bool display_all_flag)
        {
            enforce_controls_coherency();
            display_controls();
            update_graphs_limits_display();

            Form1.newline_to_mainStatusWindow("Calculating comparisons...");

            calculate_and_display_1d_data(display_all_flag);
            calculate_and_display_2d_data(display_all_flag);

            Form1.newline_to_mainStatusWindow("\tReport...");
            calculate_and_display_report(display_all_flag);

            Form1.newline_to_mainStatusWindow("\tAdditional report...");
            calculate_and_display_additional_report(compare_results.bigrams_distances_from_first_unblinded, compare_results.bigrams_distances_from_first_blind);

            Form1.newline_to_mainStatusWindow("Comparisons completed");
        }

        private void calculate_and_display_2d_data(bool display_all)
        {
            calculate_2d_data();
            display_2d_data(display_all);
        }

        private void calculate_and_display_1d_data(bool display_all)
        {
            calculate_1d_data();
            display_1d_data(display_all);
        }



        private void display_2d_data(bool display_all)
        {
            display_bigrams_comparisons(display_all);
            display_following_char_comparisons(display_all);
            display_previous_char_comparisons(display_all);
            display_following_distances_comparisons(display_all);
            display_previous_distances_comparisons(display_all);
        }
        private void display_1d_data(bool display_all)
        {
            display_monograms_distances_comparisons(display_all);
            display_vocabulary_distances_comparisons(display_all);
            display_wordslength_distances_comparisons(display_all);
            display_syllables_distances_comparisons(display_all);
        }



        private void display_data (bool display_all)
        {
            display_1d_data(display_all);
            display_2d_data(display_all);
            calculate_and_display_report(display_all);
            calculate_and_display_additional_report(compare_results.bigrams_distances_from_first_unblinded, compare_results.bigrams_distances_from_first_blind);
        }

        private void display_bigrams_comparisons(bool display_all)
        {

            PictureBox picturebox_main_graph = pictureBox_Compare_bigrams;
            TextBox textbox_distances = textBox_Compare_bigrams;
            PictureBox picturebox_distances = pictureBox_Compare_bigrams_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_bigrams_table;
            CheckBox checkbox_blind = checkBox_Compare_bigrams_blind;

            string title_addendum_limits = add_rare_characters_info("characters");
            if (checkBox_Compare_bigrams_delta.Checked == false)
            {
                // Grafico principale
                display_2d_graphs("Bigrams frequencies", Form1._zero_evidenced_graph, Form1.EVconvert(compare_results.bigrams_distribution), Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to bigram frequencies:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.bigrams_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.bigrams_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to bigram frequencies:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.bigrams_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.bigrams_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to bigram frequencies:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.bigrams_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.bigrams_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to bigram frequencies:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.bigrams_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.bigrams_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
            else
            {
                // Grafico principale
                display_2d_graphs(Form1.add_symmetrization_info("Bigrams frequencies vs. theoric frequencies"), Form1._extremes_evidenced_graph, Form1.Symmetrize(compare_results.bigrams_vs_theoric_distribution), Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";
                    string warning = "WARNING: these distances are VERY sensitive to the statistics of rare characters!";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to bigram frequencies vs. theoric frequencies:\r\n" + title_addendum + title_addendum_limits + warning;
                            display_linear_comparisons(title, compare_results.bigrams_vs_theoric_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.bigrams_vs_theoric_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to bigram frequencies vs. theoric frequencies:\r\n" + title_addendum + title_addendum_limits + warning;
                            display_table_comparisons(title, compare_results.bigrams_vs_theoric_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.bigrams_vs_theoric_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to bigram frequencies vs. theoric frequencies:\r\n" + title_addendum + title_addendum_limits + warning;
                            display_linear_comparisons(title, compare_results.bigrams_vs_theoric_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.bigrams_vs_theoric_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to bigram frequencies vs. theoric frequencies:\r\n" + title_addendum + title_addendum_limits + warning;
                            display_table_comparisons(title, compare_results.bigrams_vs_theoric_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.bigrams_vs_theoric_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
        }

        private void display_following_char_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_follchar;
            TextBox textbox_distances = textBox_Compare_follchar;
            PictureBox picturebox_distances = pictureBox_Compare_follchar_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_follchar_table;
            CheckBox checkbox_blind = checkBox_Compare_follchar_blind;

            string title_addendum_limits = add_rare_characters_info("characters");


            // Grafico principale
            display_2d_graphs("Probability of character 'row' to be followed by character 'column'", Form1._zero_evidenced_graph, compare_results.following_character_distribution, Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

            // Sottopagina distanze
            if (display_all == Form1._display_all)
            {
                string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";
                if (checkbox_blind.Checked == false)
                {
                    title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";

                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the probability of a character to be followed by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.following_character_distances_from_first_unblinded, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.following_character_distances_from_first_unblinded, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the probability of a character to be followed by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.following_character_all_distances_unblinded, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.following_character_all_distances_unblinded, picturebox_distances);
                    }
                }
                else
                {

                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the probability of a character to be followed by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.following_character_distances_from_first_blind, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.following_character_distances_from_first_blind, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the probability of a character to be followed by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.following_character_all_distances_blind, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.following_character_all_distances_blind, picturebox_distances);
                    }
                }
            }
        }

        private void display_previous_char_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_prevchar;
            TextBox textbox_distances = textBox_Compare_prevchar;
            PictureBox picturebox_distances = pictureBox_Compare_prevchar_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_prevchar_table;
            CheckBox checkbox_blind = checkBox_Compare_prevchar_blind;

            string title_addendum_limits = add_rare_characters_info("characters");


            // Grafico principale
            display_2d_graphs("Probability of character 'row' to be preceded by character 'column'", Form1._zero_evidenced_graph, compare_results.previous_character_distribution, Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

            // Sottopagina distanze
            if (display_all == Form1._display_all)
            {
                string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";
                if (checkbox_blind.Checked == false)
                {
                    title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";

                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the probability of a character to be preceded by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.previous_character_distances_from_first_unblinded, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.previous_character_distances_from_first_unblinded, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the probability of a character to be preceded by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.previous_character_all_distances_unblinded, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.previous_character_all_distances_unblinded, picturebox_distances);
                    }
                }
                else
                {
                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the probability of a character to be preceded by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.previous_character_distances_from_first_blind, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.previous_character_distances_from_first_blind, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the probability of a character to be preceded by another character:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.previous_character_all_distances_blind, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.previous_character_all_distances_blind, picturebox_distances);
                    }
                }
            }
        }

        private void display_following_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_folldist;
            TextBox textbox_distances = textBox_Compare_folldist;
            PictureBox picturebox_distances = pictureBox_Compare_folldist_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_folldist_table;
            CheckBox checkbox_blind = checkBox_Compare_folldist_blind;

            string title_addendum_limits = add_rare_characters_info("characters");


            // Grafico principale
            display_2d_graphs("Characters distances according to the probability of the following character", Form1._zero_evidenced_graph, compare_results.following_chardistances_distribution, Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

            // Sottopagina distanze
            if (display_all == Form1._display_all)
            {
                string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";
                if (checkbox_blind.Checked == false)
                {
                    title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";

                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the following-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.following_chardistances_distances_from_first_unblinded, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.following_chardistances_distances_from_first_unblinded, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the following-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.following_chardistances_all_distances_unblinded, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.following_chardistances_all_distances_unblinded, picturebox_distances);
                    }
                }
                else
                {
                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the following-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.following_chardistances_distances_from_first_blind, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.following_chardistances_distances_from_first_blind, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the following-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.following_chardistances_all_distances_blind, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.following_chardistances_all_distances_blind, picturebox_distances);
                    }
                }
            }
        }

        private void display_previous_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_prevdist;
            TextBox textbox_distances = textBox_Compare_prevdist;
            PictureBox picturebox_distances = pictureBox_Compare_prevdist_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_prevdist_table;
            CheckBox checkbox_blind = checkBox_Compare_prevdist_blind;

            string title_addendum_limits = add_rare_characters_info("characters");


            // Grafico principale
            display_2d_graphs("Characters distances according to the probability of the previous character", Form1._zero_evidenced_graph, compare_results.previous_chardistances_distribution, Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

            // Sottopagina distanze
            if (display_all == Form1._display_all)
            {
                string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (tables are compared as they are)\r\n";

                if (checkbox_blind.Checked == false)
                {
                    title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (tables are re-ordered, comparing each character in the first text with the corresponding character in the second text)\r\n";
                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the previous-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.previous_character_distances_from_first_unblinded, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.previous_character_distances_from_first_unblinded, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the previous-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.previous_character_all_distances_unblinded, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.previous_character_all_distances_unblinded, picturebox_distances);
                    }
                }
                else
                {
                    if (checkbox_table.Checked == false)
                    {
                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to the previous-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.previous_character_distances_from_first_blind, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.previous_character_distances_from_first_blind, picturebox_distances);
                    }
                    else
                    {
                        string title = "Distances of all texts according to the previous-character distances:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.previous_character_all_distances_blind, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.previous_character_all_distances_blind, picturebox_distances);
                    }
                }
            }
        }



        private void display_vocabulary_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_vocabulary;
            TextBox textbox_distances = textBox_Compare_vocabulary;
            PictureBox picturebox_distances = pictureBox_Compare_vocabulary_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_vocabulary_table;
            CheckBox checkbox_blind = checkBox_Compare_vocabulary_blind;


            // Grafico principale
            string title_addendum_limits = set_linear_limits_info("Calculated on the first", "words", Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), graphs_limit_1d);
            display_1d_graphs("Distributions\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.vocabulary_words_distribution), Form1.analysis_filenames, graphs_limit_1d, picturebox_main_graph);

            // Sottopagina distanze
            if (display_all == Form1._display_all)
            {
                string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                if (checkbox_blind.Checked == false)
                {
                    title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                    if (checkbox_table.Checked == false)
                    {

                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to words distribution:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.vocabulary_words_distances_from_first_unblinded, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.vocabulary_words_distances_from_first_unblinded, picturebox_distances);
                    }
                    else
                    {

                        string title = "Distances of all texts according to words distribution:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.vocabulary_words_all_distances_unblinded, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.vocabulary_words_all_distances_unblinded, picturebox_distances);
                    }
                }
                else
                {
                    if (checkbox_table.Checked == false)
                    {

                        string title = "Distances from " + Form1.analysis_filenames[0] + " according to words distribution:\r\n" + title_addendum + title_addendum_limits;
                        display_linear_comparisons(title, compare_results.vocabulary_words_distances_from_first_blind, textbox_distances);
                        display_linear_comparisons_graph(title, compare_results.vocabulary_words_distances_from_first_blind, picturebox_distances);
                    }
                    else
                    {

                        string title = "Distances of all texts according to words distribution:\r\n" + title_addendum + title_addendum_limits;
                        display_table_comparisons(title, compare_results.vocabulary_words_all_distances_blind, textbox_distances);
                        display_table_comparisons_graph(title, compare_results.vocabulary_words_all_distances_blind, picturebox_distances);
                    }
                }
            }
        }

        private void display_wordslength_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_wordslength;
            TextBox textbox_distances = textBox_Compare_wordslength;
            PictureBox picturebox_distances = pictureBox_Compare_wordslength_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_wordslength_table;
            CheckBox checkbox_blind = checkBox_Compare_wordslength_blind;

            if (checkBox_Compare_wordslength_vocabulary.Checked == true)
            {
                // Grafico principale
                string title_addendum_limits = set_linear_limits_info("Calculated on the words up to a length of", "characters", compare_results.wordslength_vocabulary_distribution, graphs_limit_1d);
                display_1d_graphs("Distributions of words lengths in the vocabularies\r\n" + title_addendum_limits, compare_results.wordslength_vocabulary_distribution, Form1.analysis_filenames, graphs_limit_1d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to words lengths distribution in the vocabularies:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.wordslength_vocabulary_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.wordslength_vocabulary_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to words lengths distribution in the vocabularies:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.wordslength_vocabulary_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.wordslength_vocabulary_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to words lengths distribution in the vocabularies:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.wordslength_vocabulary_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.wordslength_vocabulary_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to words lengths distribution in the vocabularies:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.wordslength_vocabulary_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.wordslength_vocabulary_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
            else 
            {
                // Grafico principale
                string title_addendum_limits = set_linear_limits_info("Calculated on the words up to a length of", "characters", Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), graphs_limit_1d);
                display_1d_graphs("Distributions of words lengths in the texts\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.wordslength_text_distribution), Form1.analysis_filenames, graphs_limit_1d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to words lengths distribution in the texts:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.wordslength_text_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.wordslength_text_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to words lengths distribution in the texts:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.wordslength_text_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.wordslength_text_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to words lengths distribution in the texts:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.wordslength_text_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.wordslength_text_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to words lengths distribution in the texts:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.wordslength_text_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.wordslength_text_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
        }

        private void display_syllables_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_syllables;
            TextBox textbox_distances = textBox_Compare_syllables;
            PictureBox picturebox_distances = pictureBox_Compare_syllables_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_syllables_table;
            CheckBox checkbox_blind = checkBox_Compare_syllables_blind;

            if (checkBox_Compare_syllables_singlevowelnuclei.Checked == true)
            {
                // Grafico principale
                string title_addendum_limits = set_linear_limits_info("Calculated on the first", "syllables", Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), graphs_limit_1d);
                display_1d_graphs("Distributions of pseudo-syllables (each vowel forms a different nucleus)\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.syllables_singlevowel_distribution), Form1.analysis_filenames, graphs_limit_1d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to pseudo-syllables distribution (each vowel forms a different nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.syllables_singlevowel_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.syllables_singlevowel_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to pseudo-syllables distribution (each vowel forms a different nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.syllables_singlevowel_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.syllables_singlevowel_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to pseudo-syllables distribution (each vowel forms a different nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.syllables_singlevowel_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.syllables_singlevowel_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to pseudo-syllables distribution (each vowel forms a different nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.syllables_singlevowel_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.syllables_singlevowel_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
            else
            {
                // Grafico principale
                string title_addendum_limits = set_linear_limits_info("Calculated on the first", "syllables", Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), graphs_limit_1d);
                display_1d_graphs("Distributions of pseudo-syllables (multiple adjacent vowels form a single nucleus)\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.syllables_multiplevowel_distribution), Form1.analysis_filenames, graphs_limit_1d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to pseudo-syllables distribution (multiple adjacent vowels form a single nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.syllables_multiplevowel_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.syllables_multiplevowel_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to pseudo-syllables distribution (multiple adjacent vowels form a single nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.syllables_multiplevowel_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.syllables_multiplevowel_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to pseudo-syllables distribution (multiple adjacent vowels form a single nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.syllables_multiplevowel_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.syllables_multiplevowel_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to pseudo-syllables distribution (multiple adjacent vowels form a single nucleus):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.syllables_multiplevowel_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.syllables_multiplevowel_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
        }


        // display_monograms_distances è speciale, perchè non usa il limite dei grafici lineari ma usa invece il limite dei caratteri rari,
        //   tramite overloads delle funzioni display_1d_graphs, calculate_distances_list_1d, calculate_distances_table_1d
        private void display_monograms_distances_comparisons(bool display_all)
        {
            PictureBox picturebox_main_graph = pictureBox_Compare_singlechars;
            TextBox textbox_distances = textBox_Compare_singlechars;
            PictureBox picturebox_distances = pictureBox_Compare_singlechars_distances_graph;

            CheckBox checkbox_table = checkBox_Compare_singlechars_table;
            CheckBox checkbox_blind = checkBox_Compare_singlechars_blind;

            if (checkBox_Compare_singlechars_nospaces.Checked == true)
            {
                // Grafico principale
                string title_addendum_limits = add_rare_characters_info("characters");
                display_1d_graphs("Distributions of monograms ('space' excluded)\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.monograms_no_spaces_distribution), Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to monograms distributions ('space' excluded):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.monograms_no_spaces_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.monograms_no_spaces_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to monograms distributions ('space' exluded):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.monograms_no_spaces_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.monograms_no_spaces_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to monograms distributions ('space' excluded):\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.monograms_no_spaces_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.monograms_no_spaces_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to monograms distributions ('space' exluded):\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.monograms_no_spaces_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.monograms_no_spaces_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
            else
            {
                // Grafico principale
                string title_addendum_limits = add_rare_characters_info("characters");
                display_1d_graphs("Distributions of  of monograms\r\n" + title_addendum_limits, Form1.EVconvert_1d_multipli(compare_results.monograms_distribution), Form1.analysis_filenames, current_graphs_limit_2d, picturebox_main_graph);

                // Sottopagina distanze
                if (display_all == Form1._display_all)
                {
                    string title_addendum = "BLINDED comparison: no hypothesis on what each character represents in the texts (data are compared as they are)\r\n";
                    if (checkbox_blind.Checked == false)
                    {
                        title_addendum = "UNBLINDED comparison: it's assumed each character in the texts represents the same 'symbol' (data are re-ordered, comparing each word in the first text with the corresponding word in the second text, if it exists)\r\n";

                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to monograms distributions:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.monograms_distances_from_first_unblinded, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.monograms_distances_from_first_unblinded, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to monograms distributions:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.monograms_all_distances_unblinded, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.monograms_all_distances_unblinded, picturebox_distances);
                        }
                    }
                    else
                    {
                        if (checkbox_table.Checked == false)
                        {
                            string title = "Distances from " + Form1.analysis_filenames[0] + " according to monograms distributions:\r\n" + title_addendum + title_addendum_limits;
                            display_linear_comparisons(title, compare_results.monograms_distances_from_first_blind, textbox_distances);
                            display_linear_comparisons_graph(title, compare_results.monograms_distances_from_first_blind, picturebox_distances);
                        }
                        else
                        {
                            string title = "Distances of all texts according to monograms distributions:\r\n" + title_addendum + title_addendum_limits;
                            display_table_comparisons(title, compare_results.monograms_all_distances_blind, textbox_distances);
                            display_table_comparisons_graph(title, compare_results.monograms_all_distances_blind, picturebox_distances);
                        }
                    }
                }
            }
        }




        private void calculate_and_display_report(bool display_all)
        {

            List<EValue> statistic_results_unblinded = compare_results.bigrams_distances_from_first_unblinded;
            List<EValue> statistic_results_blind = compare_results.bigrams_distances_from_first_blind;

            string report_title = "Clustering report based on bigrams frequencies distances\r\n";
            string graph_unblinded_title = "Distances of bigrams frequencies (unblinded)";
            string graph_blind_title = "Distances of bigrams frequencies (blind)";

            string report = report_title + add_rare_characters_info("characters") + "\r\n";

            // Inseriamo nei dictionaries cluster_unblinded tutti i files con distanza unblinded inferiore a accepted_threshold, o a rejected_threshold
            //   Usando un dicitonary è poi più facile fare la ricerca
            cluster_unblinded_accepted = new Dictionary<string, EValue>();
            cluster_unblinded_dubious = new Dictionary<string, EValue>();
            foreach (EValue value in statistic_results_unblinded)
            {
                if (value.element == statistic_results_unblinded[0].element) continue; // è la distanza con se stesso

                if (value.value < accepted_threshold_unblinded)
                {
                    cluster_unblinded_accepted.Add(value.element, value);
                }
                else
                {
                    if (value.value < rejected_threshold_unblinded)
                    {
                        cluster_unblinded_dubious.Add(value.element, value);
                    }
                }
            }


            // Adesso inseriamo nel dictionary cluster_blind_accepted tutti i files con distanza unblinded inferiore a accepted_threshold che NON compaiono
            //    in cluster_unblinded_accepetd: se ce ne sono è probabile si tratti di testi cifrati con un codice a sostituzione semplice,
            //    e inseriamo nel dictionary cluster_blind_dubious i files che non compaiono nè in cluster_unblinded_accepted nè in cluster_unblinded_dubious
            cluster_blind_accepted = new Dictionary<string, EValue>();
            cluster_blind_dubious = new Dictionary<string, EValue>();
            foreach (EValue value in statistic_results_blind)
            {
                if (value.element == statistic_results_blind[0].element) continue; // è la distanza con se stesso

                if (value.value < accepted_threshold_blind)
                {
                    EValue old_value = new EValue();
                    if (cluster_unblinded_accepted.TryGetValue(value.element, out old_value) == true) { }
                    else
                    {
                        cluster_blind_accepted.Add(value.element, value);
                    }
                }
                else
                {
                    if (value.value < rejected_threshold_blind)
                    {
                        EValue old_value = new EValue();
                        if (cluster_unblinded_accepted.TryGetValue(value.element, out old_value) == true || cluster_unblinded_dubious.TryGetValue(value.element, out old_value) == true) { }
                        else
                        {
                            cluster_blind_dubious.Add(value.element, value);
                        }
                    }
                }
            }

           

            // Ci manca solo la lista dei files rifiutati sia come unblinded che come blinded
            //  Inoltre voglio poter scrivere entrambe le distanze unblinded e blinded per ogni file, MA QUA BISOGNA FARE ATTENZIONE
            //      perchè la lista delle distanze blinded non è detto sia ordinata allo stesso modo di quella unblinded
            //      ergo mi tocca trasferire i un dictionary la statistics_results_blind in modo tale da poterla poi cercare per nome
            Dictionary<string, EValue> statistics_results_blind_dictionary = new Dictionary<string, EValue>();
            foreach (EValue value in statistic_results_blind)
            {
                EValue dummy_value = new EValue();
                // Il test serve perchè si potrebbe avere un file .txa caricato, e compararlo con se stesso, col che il nome comparirebbe
                //     due volte in statistic_results (e se si cerca di inserire due volte la stessa chiave nel dictionary viene generata un'eccezione)
                if (statistics_results_blind_dictionary.TryGetValue(value.element, out dummy_value) == false)
                {
                    statistics_results_blind_dictionary.Add(value.element, value);
                }
            }


            unclustered = new List<EValue>();          // conterrà nome+distanza unblinded
            unclustered_blind_distances = new List<float>();
            foreach (EValue value in statistic_results_unblinded)   // per ogni file
            {
                if (value.element == statistic_results_blind[0].element) continue; // è la distanza con se stesso

                EValue old_value = new EValue();
                if (cluster_unblinded_accepted.TryGetValue(value.element, out old_value) == true || cluster_unblinded_dubious.TryGetValue(value.element, out old_value) == true ||
                    cluster_blind_accepted.TryGetValue(value.element, out old_value) == true || cluster_blind_dubious.TryGetValue(value.element, out old_value) == true) { }
                else
                {
                    unclustered.Add(value);

                    // adesso cerchiamo la distanza nelle statistiche blinded
                    EValue blind_distance = new EValue();
                    statistics_results_blind_dictionary.TryGetValue(value.element, out blind_distance);
                    unclustered_blind_distances.Add(blind_distance.value);
                }
            }





            // Visualizzazione!

            // 1) Grafici

            // Grafico unblinded
            XPlotGraphs1d graph_unblinded = new XPlotGraphs1d();
            graph_unblinded.title = graph_unblinded_title + "\r\n";
            graph_unblinded.title += add_rare_characters_info("characters");
            graph_unblinded.y_units = "";
            graph_unblinded.x_units = "";
            graph_unblinded.histogram = true;
            graph_unblinded.plot_color_series_0 = Color.Orange;

            List<string> unblinded_labels = new List<string>();
            List<float> unblinded_values = new List<float>();
            foreach (EValue value in statistic_results_unblinded)
            {
                if (value.element == statistic_results_unblinded[0].element) continue; // è la distanza con se stesso

                unblinded_labels.Add(get_reference_number(value.element, statistic_results_unblinded) + ")");
                unblinded_values.Add(value.value);
            }
            Size graph_size = new Size(pictureBox_Compare_report_graph_unblinded.Width, pictureBox_Compare_report_graph_unblinded.Height);
            Bitmap image = XPlotGraphs1d.DisplayGraph(graph_size, unblinded_values, unblinded_labels, graph_unblinded);
            pictureBox_Compare_report_graph_unblinded.Image = image;

            // Grafico blind, NOTARE CHE LA get_reference_number assicura che le labels siano ordinate allo stesso modo del grafico unblinded
            XPlotGraphs1d graph_blind = new XPlotGraphs1d();
            graph_blind.title = graph_blind_title + "\r\n";
            graph_blind.title += add_rare_characters_info("characters");
            graph_blind.y_units = "";
            graph_blind.x_units = "";
            graph_blind.histogram = true;
            graph_blind.plot_color_series_0 = Color.Green;

            List<string> blind_labels = new List<string>();
            List<float> blind_values = new List<float>();
            foreach (EValue value in statistic_results_blind)
            {
                if (value.element == statistic_results_blind[0].element) continue; // è la distanza con se stesso

                blind_labels.Add(get_reference_number(value.element, statistic_results_blind) + ")"); // unblinded in get_reference_number è giusto!!
                blind_values.Add(value.value);
            }
            graph_size = new Size(pictureBox_Compare_report_graph_blind.Width, pictureBox_Compare_report_graph_blind.Height);
            image = XPlotGraphs1d.DisplayGraph(graph_size, blind_values, blind_labels, graph_blind);
            pictureBox_Compare_report_graph_blind.Image = image;


            // 2) Report testuale
            report += statistic_results_unblinded[0].element + " clusters with (unblinded, threshold max = " + accepted_threshold_unblinded + "):";
            int report_number = 1;
            if (cluster_unblinded_accepted.Count != 0)
            {
                report += "\r\n\r\n";
                foreach (EValue value in cluster_unblinded_accepted.Values)
                {
                    report += "\t" + get_reference_number(value.element, statistic_results_unblinded) + ") " + value.element + "\t" + value.value + "\r\n";
                    report_number++;
                }
            }
            else
            {
                report += " no other text\r\n";
            }
            report += "\r\n";


            if (cluster_unblinded_dubious.Count != 0)
            {
                report += statistic_results_unblinded[0].element + " clusters with (unblinded dubious, threshold max = " + rejected_threshold_unblinded + "):\r\n\r\n";
                foreach (EValue value in cluster_unblinded_dubious.Values)
                {
                    report += "\t" + get_reference_number(value.element, statistic_results_unblinded) + ") " + value.element + "\t" + value.value + "\r\n";
                }
            }
            report += "\r\n";


            if (cluster_blind_accepted.Count != 0)
            {
                report += statistic_results_unblinded[0].element + " is possibly a simple-substitution cipher, clusters with (blind, threshold max = " + accepted_threshold_blind + "):\r\n\r\n";
                foreach (EValue value in cluster_blind_accepted.Values)
                {
                    report += "\t" + get_reference_number(value.element, statistic_results_unblinded) + ") " + value.element + "\t" + value.value + "\r\n";
                }
            }
            report += "\r\n";

            if (cluster_blind_dubious.Count != 0)
            {
                report += statistic_results_unblinded[0].element + " is possibly a simple-substitution cipher, clusters with (blind dubious, threshold max = " + rejected_threshold_blind + "):\r\n\r\n";
                foreach (EValue value in cluster_blind_dubious.Values)
                {
                    report += "\t" + get_reference_number(value.element, statistic_results_unblinded) + ") " + value.element + "\t" + value.value + "\r\n";
                }
            }
            report += "\r\n";



            if (unclustered.Count != 0)
            {
                report += statistic_results_unblinded[0].element + " does not cluster with any of the following (unblinded, blinded distances):\r\n\r\n";
                int counter = 0;
                foreach (EValue value in unclustered)
                {
                    report += "\t" + get_reference_number(value.element, statistic_results_unblinded) + ") " + value.element + "\t" + value.value + "\t" + unclustered_blind_distances[counter] + "\r\n";
                    counter++;
                }
            }

            textBox_Compare_Report.Text = report;

        }

        private int get_reference_number(string string_to_find, List<EValue> statistic_list)
        {
            bool found = false;
            int counter = 1;
            foreach (EValue value in statistic_list)
            {
                if (value.element == statistic_list[0].element) continue; // è la distanza con se stesso

                if (value.element == string_to_find) { found = true; break; }
                counter++;
            }
            if (found == false) counter = 99999;  // segnalazione di errore
            return counter;
        }



        private void calculate_and_display_additional_report(List<EValue> statistic_unblinded, List<EValue> statistic_blind)
        {
            string report = "";


            if (cluster_unblinded_accepted.Count != 0)
            {
                // Additional report: se la lista dei clusters unblinded non è vuota, vengono ricavate le parole 'comuni' del vocabolario del testo
                //   sotto esame ma che non sono comuni nelgi altri testi

                // Questa funzione è prevista per comparare un testo and un singolo .txalysis di riferimento (tipicamente, il corpus del linguaggio)
                //   e avrebbe poco senso farla sull'intera lista dei comparandi: è per questo che viene fatta solo comparando il primo elemento
                //   della lista dei clusters (che è il testo di riferimento) col secondo (che dovrebbe essere il corpus)

                // Limitandosi a comparare con un solo file, inoltre, si evitano un mucchio di menate software (come la necessità
                //      di usare una List<List>> invece che una List<> semplice per unusual_words_list) e l'output nella text
                //      box mantiene una lunghezza ragionevole

                // ATTENZIONE!!! SE C'E' PIU' DI UN FILE NEL CLUSTER NON E' PIU' DETTO CHE statistic_unblinded (USATA PER SCRIVERE I NOMI DEI FILES)
                //  ABBIA LO STESSO ORDINE DI compare_results.vocabulary_words_distribution , CHE INVECE E' USATA PER I CALCOLI
                //  SISTEMARE QUESTO PROBLEMA NON E' SEMPLICE, ERGO DECIDO DI NON FARE LA COMPARAZIONE DEL TUTTO IN QUESTO CASO
                if (Form1.analysis_to_compare.Count > 2) // il test è su 2 perchè c'è l'analisi base + almeno un'altra analisi con la quale compararla
                {
                    report = "This function is meant to compare one text against one corpus, choose only one file for the comparison to enable it.";
                    textBox_Compare_additional_report.Text = report;
                    return;
                }

                report = "Words which are common in " + statistic_unblinded[0].element + ", but are uncommon in " + statistic_unblinded[1].element;
                report += "\r\n\r\n";

                if (words_to_examine < compare_results.vocabulary_words_distribution[0].Count)
                {
                    report += "The first " + words_to_examine + " most frequent ";
                }
                else
                {
                    report += "All the " + compare_results.vocabulary_words_distribution[0].Count;
                }
                report += " words of " + statistic_unblinded[0].element + " have been considered, with a minimum required amplification factor of " + amplification_threshold;


                report += ". Take care: this function works best if the comparison is made with a large corpus, comparing against a small corpus or a single text will produce degraded results\r\n\r\n";

                report += get_unusual_words(words_to_examine, amplification_threshold);
            }
            else
            {
                // Else: se la lista dei clusters blind non è vuota (il testo è un possibile cifrario a sostituzione semplice), vengono ricavi i comandi
                // Regex per la sua decrittazione
                if (cluster_blind_accepted.Count != 0)
                {
                    report = "Future expansion (automatic decryption)";
                }
                else
                {
                    report = "No additional informations can be added, no language clusters with the one under analysis";
                }
            }

            textBox_Compare_additional_report.Text = report;
        }

        private string get_unusual_words(int words_to_examine, float amplification_threshold)
        {
            string output = "";

            unusual_words_list = new List<EValue_extended>();

            // calcolo occorrenze totali nel vocabolario reference (serve dopo x la conversione in frequenze)
            long total_reference_words_occurrences = 0;
            foreach (EValueOcc reference_vocabulary_word in compare_results.vocabulary_words_distribution[0])
            {
                total_reference_words_occurrences += reference_vocabulary_word.value;
            }

            // estrazione del sottoinsieme di parole da comparare
            List<EValueOcc> reduced_reference_vocabulary = new List<EValueOcc>();
            int num_words = 0;
            foreach (EValueOcc reference_vocabulary_word in compare_results.vocabulary_words_distribution[0])
            {
                reduced_reference_vocabulary.Add(reference_vocabulary_word);

                num_words++;
                if (num_words == words_to_examine) break;
            }




            // Compilazione di un dictionary dalla lista del vocabolario target, per le elaborazioni successive
            long total_target_words_occurrences = 0;
            Dictionary<string, EValueOcc> target_vocabulary_dictionary = new Dictionary<string, EValueOcc>();
            foreach (EValueOcc target_word in compare_results.vocabulary_words_distribution[1])
            {
                total_target_words_occurrences += target_word.value;
                target_vocabulary_dictionary.Add(target_word.element, target_word);
            }



            // Adesso cerchiamo ogni parola del reduced_reference_vocabulary nel target_vocabulary
            foreach (EValueOcc reference_vocabulary_word in reduced_reference_vocabulary)
            {
                EValueOcc target_word = new EValueOcc();
                if (target_vocabulary_dictionary.TryGetValue(reference_vocabulary_word.element, out target_word) == true)
                {
                    float ref_freq = (float)reference_vocabulary_word.value / (float)total_reference_words_occurrences;
                    float tar_freq = (float)target_word.value / (float)total_target_words_occurrences;
                    if (ref_freq / tar_freq >= amplification_threshold)
                    {
                        EValue_extended new_value = new EValue_extended();
                        new_value.element = reference_vocabulary_word.element;
                        new_value.value = ref_freq / tar_freq;
                        new_value.element_additional = reference_vocabulary_word.value.ToString(); // # occorrenze (verrà poi visualizzato)
                        unusual_words_list.Add(new_value);
                    }
                }
                else
                {
                    EValue_extended new_value = new EValue_extended();
                    new_value.element = reference_vocabulary_word.element;
                    new_value.value = float.MaxValue; // segnala trovata nuova parola...
                    new_value.element_additional = reference_vocabulary_word.value.ToString(); // # occorrenze (verrà poi visualizzato)
                    unusual_words_list.Add(new_value);
                }
            }

            // ORDINAMENTO LISTA UNUSUAL WORDS           
            IComparer<EValue_extended> comparer_by_value = new TextCharsStats.CompareEValue_extendedByValue();
            unusual_words_list.Sort(comparer_by_value);


            // scrittura stringa di output
            output += unusual_words_list.Count + " unusual words found:\r\n\r\n";

            foreach (EValue_extended value in unusual_words_list)
            {
                if (value.value != float.MaxValue)  // segnala hapax legomena...
                {
                    output += value.element + "\t\t amplification factor = \t" + value.value;
                }
                else
                {
                    output += value.element + "\t\t never found-before word";
                }
                output += "\t\toccurrences in text = \t" + value.element_additional;

                output += "\r\n";
            }

            return output;
        }









        // Queste routines sono, in pratica, degli overloads di quelle in Form1. La differenza è che accettano in ingresso serie multiple di dati.
        private static void display_2d_graphs(string title, bool graph_type, List<List<List<EValue>>> multiple_data, List<string> series_names_list, List<int> graphs_limits, PictureBox picture_box)
        {

            XPlotGraphs2d graph = new XPlotGraphs2d();
            graph.title = title;

            if (graph_type == true)
            {
                graph.put_extremes_in_evidence = true;
                graph.high_extreme = Form1.max_symmetric_value;
                graph.low_extreme = Form1.min_symmetric_value;
            }
            else
            {
                graph.put_zero_in_evidence = true;
            }

            if (graphs_limits.Count != multiple_data.Count) // safety test!
            {
                Bitmap error_image = new Bitmap(picture_box.Width, picture_box.Height);
                picture_box.Image = error_image;
                return;
            }


            List<string> names_list = new List<string>();
            List<List<List<float>>> multiple_graphs_data = new List<List<List<float>>>();
            List<List<string>> multiple_x_labels = new List<List<string>>();
            int current_table = 0;
            foreach (List<List<EValue>> table_data in multiple_data)
            {
                // Avviso dati troncati: viene modificato il nome di ogni singolo grafico
                if (graphs_limits[current_table] < base_graphs_limit_2d[current_table])
                {
                    names_list.Add(series_names_list[current_table] + " (first " + graphs_limits[current_table] + " chars)");
                }
                else
                {
                    names_list.Add(series_names_list[current_table] + " (all " + graphs_limits[current_table] + " chars)");
                }


                List<string> x_labels = new List<string>();
                List<List<float>> graphs_data = new List<List<float>>();
                foreach (List<EValue> row in table_data.Take<List<EValue>>(graphs_limits[current_table]))
                {
                    List<float> scratch_list = new List<float>();
                    foreach (EValue cell in row.Take<EValue>(graphs_limits[current_table]))
                    {
                        scratch_list.Add(cell.value);
                    }
                    graphs_data.Add(scratch_list);
                    if (row.Count == 0) x_labels.Add("ERROR");
                    else
                    {
                        // QUI LA GESTIONE E' CONFUSA: l'idea originale era di scrivere TUTTO l'element come label per avere la routine il più
                        //  generica possibile, ma poi c'è il problema che nell'uso effettivo quello che voglio davvero scrivere è solo il
                        //  secondo carattere dell'element (p.es.: gli elements sono "aa" "ab "ac", voglio scrivere come intestazioni 
                        //  riga/colonna delle tabelle "a", "b", "c"). Ergo ho dovuto correggere. C'è una cosa analoga in Form1.display_2d_graphs
                        if (row[0].element.Length < 1) x_labels.Add("ERROR1");
                        else
                        {
                            x_labels.Add("" + row[0].element[0]);
                        }
                    }
                }

                multiple_graphs_data.Add(graphs_data);
                multiple_x_labels.Add(x_labels);

                current_table++;
            }


            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs2d.DisplayGraph(graph_size, multiple_graphs_data, names_list, multiple_x_labels, graph);

            picture_box.Image = image;

            return;
        }


        private static void display_1d_graphs(string title, List<List<EValue>> multiple_linear_data, List<string> series_names_list, int graphs_limit, PictureBox picture_box)
        {
            XPlotGraphs1d graph = new XPlotGraphs1d();
            graph.title = title;
            graph.y_units = "";
            graph.x_units = "";
            graph.join_points = true;
            //graph.histogram = true;   Meglio evitarlo perchè rende questi grafici difficilmente leggibili (cmq funziona :)

           
            List<string> x_labels = new List<string>();
            List<List<float>> graphs_data = new List<List<float>>();
            foreach (List<EValue> single_linear_data in multiple_linear_data)
            {
                // Qua non ha senso passare le labels orizzontali, dato che in generale cambiano fra una serie e l'altra (p.es.: grafico 'vocabolario',
                //   le labels della prima serie potrebbero essere in italiano "e" "il" "che", ma quelle della seconda serie in inglese: "the" "and" ... )
                x_labels.Add("");
                List<float> single_graphs_data = new List<float>();
                foreach (EValue item in single_linear_data.Take<EValue>(graphs_limit))
                {
                    single_graphs_data.Add(item.value);
                }
                graphs_data.Add(single_graphs_data);
            }

            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs1d.DisplayGraph(graph_size, graphs_data, series_names_list, x_labels, graph);
            picture_box.Image = image;
        }

        // Overload di display_1d_graphs che usa i limiti 'rare characters' invece del limi8te dei dati lineari (per visualizzazione statistiche monogrammi)
        private static void display_1d_graphs(string title, List<List<EValue>> multiple_linear_data, List<string> series_names_list, List<int> graphs_limit, PictureBox picture_box)
        {
            XPlotGraphs1d graph = new XPlotGraphs1d();
            graph.title = title;
            graph.y_units = "";
            graph.x_units = "";
            graph.join_points = true;
            //graph.histogram = true;   Meglio evitarlo perchè rende questi grafici difficilmente leggibili (cmq funziona :)


            List<string> x_labels = new List<string>();
            List<List<float>> graphs_data = new List<List<float>>();
            int current_table = 0;
            foreach (List<EValue> single_linear_data in multiple_linear_data)
            {
                // Qua non ha senso passare le labels orizzontali, dato che in generale cambiano fra una serie e l'altra (p.es.: grafico 'vocabolario',
                //   le labels della prima serie potrebbero essere in italiano "e" "il" "che", ma quelle della seconda serie in inglese: "the" "and" ... )
                x_labels.Add("");
                List<float> single_graphs_data = new List<float>();
                foreach (EValue item in single_linear_data.Take<EValue>(graphs_limit[current_table]))
                {
                    single_graphs_data.Add(item.value);
                }
                graphs_data.Add(single_graphs_data);

                current_table++;
            }

            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs1d.DisplayGraph(graph_size, graphs_data, series_names_list, x_labels, graph);
            picture_box.Image = image;
        }



        private string add_rare_characters_info(string input_addendum) // input_addendum è sempre "characters" ...
        {
            string title_addendum = ""; ;
            if (checkBox_Compare_remove_rare_characters.Checked == true)
            {
                title_addendum = "Calculated excluding " + input_addendum + " occurring less than once every " + textBox_Compare_rare_characters_cutoff.Text + " " + input_addendum + "\r\n";
            }
            else
            {
                title_addendum = "\r\n";
            }
            return title_addendum;
        }

        private string set_linear_limits_info(string first_addendum, string second_addendum, List<List<EValue>> multiple_linear_data, int graph_limit)
        {
            string title_addendum = "";
            int linear_data_max_length = 0;
            foreach (List<EValue> single_linear_data in multiple_linear_data)
            {
                if (single_linear_data.Count > linear_data_max_length) linear_data_max_length = single_linear_data.Count;
            }
            if (graph_limit < linear_data_max_length)
            {
                title_addendum = first_addendum + " " + graph_limit + " " + second_addendum + "\r\n";
            }
            else
            {
                title_addendum = "\r\n";
            }
            return title_addendum;
        }


        private void display_linear_comparisons_graph(string title, List<EValue> distances, PictureBox picture_box)
        {
            XPlotGraphs1d graph = new XPlotGraphs1d();
            graph.title = title;
            graph.y_units = "";
            graph.x_units = "";
            graph.histogram = true;
            graph.join_points = true;

            graph.plot_color_series_0 = Color.Green;

            List<string> x_labels = new List<string>();
            List<float> graphs_data = new List<float>();
            //   Le labels vengono convertite ad un semplice numero progressivo (i nomi sono sempre troppo lunghi per cercare di scriverli)
            int label_number_counter = 0;
            foreach (EValue item in distances)
            {
                x_labels.Add((label_number_counter+1).ToString());
                label_number_counter++;
                graphs_data.Add(item.value);
            }
            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs1d.DisplayGraph(graph_size, graphs_data, x_labels, graph);
            picture_box.Image = image;
        }


        private void display_table_comparisons_graph(string title, List<List<EValue_2d>> distances, PictureBox picture_box)
        {
            XPlotGraphs2d graph = new XPlotGraphs2d();
            graph.title = title;

            graph.graph_color_high_extreme = Color.Red;
            graph.graph_color_center = Color.Yellow;
            graph.graph_color_low_extreme = Color.Green;

            graph.ver_labels_maximum_length = 100;

            graph.put_zero_in_evidence = true;


            // conversione dei dati per passarli a DisplayGraph
            //   Le labels vengono convertite ad un semplice numero progressivo (i nomi sono sempre troppo lunghi per cercare di scriverli)
            List<List<float>> graphs_data = new List<List<float>>();
            List<string> x_labels = new List<string>();
            int label_number_counter = 0;
            foreach (List<EValue_2d> row in distances)
            {
                List<float> row_values = new List<float>();
                foreach (EValue_2d value in row)
                {
                    row_values.Add(value.value);
                }
                graphs_data.Add(row_values);
                x_labels.Add(row[0].element_row);
                label_number_counter++;
            }


            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs2d.DisplayGraph(graph_size, graphs_data, x_labels, graph);

            picture_box.Image = image;
        }










        // Calcolo delle distanze fra tabelle 2d
        public static List<EValue> calculate_distances_from_first_2d(List<List<List<EValue>>> source_tables, BlindFactor2d blind_factor, List<int> graphs_limits)
        {
            // Calcola le distanze dalla prima tabella a tutte le altre e inseriscile nella lista

            List<List<EValue>> reduced_reference_table = new List<List<EValue>>();
            foreach (List<EValue> row in source_tables[0].Take<List<EValue>>(graphs_limits[0]))
            {
                List<EValue> reduced_row = new List<EValue>();
                foreach (EValue value in row.Take<EValue>(graphs_limits[0]))
                {
                    EValue new_value = new EValue();
                    new_value.element = value.element;
                    new_value.value = value.value;
                    reduced_row.Add(new_value);
                }
                reduced_reference_table.Add(reduced_row);
            }



            List<EValue> result = new List<EValue>();
            int table_counter = 0;
            foreach (List<List<EValue>> table_data in source_tables)
            {
                List<List<EValue>> reduced_current_table_data = new List<List<EValue>>();
                foreach (List<EValue> row in table_data.Take<List<EValue>>(graphs_limits[table_counter]))
                {
                    List<EValue> reduced_row = new List<EValue>();
                    foreach (EValue value in row.Take<EValue>(graphs_limits[table_counter]))
                    {
                        EValue new__value = new EValue();
                        new__value.element = value.element;
                        new__value.value = value.value;
                        reduced_row.Add(new__value);
                    }
                    reduced_current_table_data.Add(reduced_row);
                }


                EValue new_value = new EValue();
                new_value.element = Form1.analysis_filenames[table_counter];
                new_value.value = calculate_single_distance_2d(reduced_reference_table, reduced_current_table_data, blind_factor);
                result.Add(new_value);

                table_counter++;
            }

            // ordinamento per distanze crescenti
            IComparer<EValue> comparer_by_value = new TextCharsStats.CompareEValueByValue_reverse();
            result.Sort(comparer_by_value);

            return result;
        }


        private static List<List<EValue_2d>> calculate_all_distances_2d(List<List<List<EValue>>> source_tables, BlindFactor2d blind_factor, List<int> graphs_limits)
        {
            // Calcola le distanze di ogni tabella da ogni altra
            // Se tutto è andato bene series_names_list è lunga quanto ognuna delle dimensioni di distances!
            List<List<float>> result_table = new List<List<float>>();

            int analysis_number = 0; // serve per l'accesso a graphs_limits
            foreach (List<List<EValue>> reference_table_data in source_tables)
            {
                // Per ogni lingua

                List<List<EValue>> reduced_reference_table = new List<List<EValue>>();
                foreach (List<EValue> row in reference_table_data.Take<List<EValue>>(graphs_limits[analysis_number]))
                {
                    List<EValue> reduced_row = new List<EValue>();
                    foreach (EValue value in row.Take<EValue>(graphs_limits[analysis_number]))
                    {
                        EValue new_value = new EValue();
                        new_value.element = value.element;
                        new_value.value = value.value;
                        reduced_row.Add(new_value);
                    }
                    reduced_reference_table.Add(reduced_row);
                }


                List<float> result_list = new List<float>();
                int current_table_counter = 0;
                foreach (List<List<EValue>> current_table_data in source_tables)
                {

                    List<List<EValue>> reduced_current_table_data = new List<List<EValue>>();
                    foreach (List<EValue> row in current_table_data.Take<List<EValue>>(graphs_limits[current_table_counter]))
                    {
                        List<EValue> reduced_row = new List<EValue>();
                        foreach (EValue value in row.Take<EValue>(graphs_limits[current_table_counter]))
                        {
                            EValue new_value = new EValue();
                            new_value.element = value.element;
                            new_value.value = value.value;
                            reduced_row.Add(new_value);
                        }
                        reduced_current_table_data.Add(reduced_row);
                    }

                    result_list.Add(calculate_single_distance_2d(reduced_reference_table, reduced_current_table_data, blind_factor));
                    current_table_counter++;
                }
                result_table.Add(result_list);
                analysis_number++;
            }

            // Riordinamento!
            List<List<EValue_2d>> out_list  = reorder_and_prepare_tables(result_table);

            return out_list;
        }


        private static List<List<EValue_2d>> reorder_and_prepare_tables(List<List<float>> result_table)
        {
            List<List<EValue_2d>> out_list = new List<List<EValue_2d>>();
            if (mainFormCompare.checkBox_Compare_show_clusters.Checked == false)
            {
                out_list = reorder_table(result_table);

                // Aggiunta di un numero progressivo (in ordine di distanze) al nome del file
                int counter = 1;
                foreach (var row_list in out_list)
                {
                    row_list[0].element_row = counter.ToString() + ") " + row_list[0].element_row;
                    counter++;
                }

            }
            else
            {
                // Quello che voglio qua è che i numeri assegnati ai files siano per ordine di distanza (come nel caso precedente), non di clustering (cosa
                //     che causerebbe un mucchio di confusione). Come prima cosa determino la lista dei numeri secondo l'ordinamento per distanza
                //  Mi serve una copia della prima riga per trovare l'ordinamento
                List<EValue_2d> scratch_list = new List<EValue_2d>();
                int counter = 0;
                foreach (var distance in result_table[0])
                {
                    EValue_2d new_value = new EValue_2d();
                    new_value.element_row = Form1.analysis_filenames[counter];
                    new_value.value = distance;
                    scratch_list.Add(new_value);
                    counter++;
                }
                // Ordinamento per distanza
                IComparer<EValue_2d> comparer_by_value = new TextCharsStats.CompareEValue_2dByValue_reverse();
                scratch_list.Sort(comparer_by_value);

                // Riordinamento della tabella col clusetring
                out_list = reorder_table_clustering(result_table);


                // Aggiunta del numero ai nomi, seguendo lo stesso ordine di scratch_list
                foreach (var row_list in out_list)
                {
                    for (int i = 0; i < scratch_list.Count; i++)
                    {
                        if (scratch_list[i].element_row == row_list[0].element_row)
                        {
                            row_list[0].element_row = (i + 1).ToString() + ") " + row_list[0].element_row;
                            break;
                        }
                    }
                }
            }

            return out_list;
        }



        private static List<List<EValue_2d>> reorder_table(List<List<float>> result_table)
        {
            // Inseriamo tutto in una List<List<EValues_2d>> per poi poter fare l'ordinamento della tabella
            List<List<EValue_2d>> out_list = new List<List<EValue_2d>>();
            int row_counter = 0;
            foreach (List<float> row in result_table)
            {
                int column_counter = 0;
                List<EValue_2d> row_list = new List<EValue_2d>();
                foreach (float value in row)
                {
                    EValue_2d value_2d = new EValue_2d();
                    value_2d.element_row = Form1.analysis_filenames[row_counter];
                    value_2d.element_column = Form1.analysis_filenames[column_counter];
                    value_2d.value = value;
                    row_list.Add(value_2d);

                    column_counter++;
                }
                out_list.Add(row_list);

                row_counter++;
            }


            // Riordinamento:
            //   1) Riordino la prima riga in funzione della distanza dal primo elemento, e salvo come è stato effettuato  l'ordinamento
            //   2) Riordino tutte le altre righe seguendo lo stesso ordinamento

            //  3) Riordino le righe fra di loro seguendo lo stesso ordinamento

            // Riordino la prima riga e salvo l'ordinamento nella variabile che verrà poi usata dal comparer
            IComparer<EValue_2d> comparer_by_value = new TextCharsStats.CompareEValue_2dByValue_reverse();
            out_list[0].Sort(comparer_by_value);
            order_1st_row = new List<string>();
            foreach (EValue_2d value in out_list[0])
            {
                order_1st_row.Add(value.element_column);
            }


            // Riordino tutte le righe con lo stesso ordinamento della prima
            IComparer<EValue_2d> comparer_following_list = new TextCharsStats.CompareEValue_2dFollowList_column();
            foreach (List<EValue_2d> row in out_list)
            {
                row.Sort(comparer_following_list);
            }

            // Riordino le righe fra di loro seguendo l'ordinamento della prima riga
            IComparer<List<EValue_2d>> list_comparer_following_list = new TextCharsStats.CompareListEValue_2dFollowList_row();
            out_list.Sort(list_comparer_following_list);

            return out_list;
        }


        private static List<List<EValue_2d>> reorder_table_clustering(List<List<float>> result_table)
        {
            // Inseriamo tutto in una List<List<EValues_2d>> per poi poter fare l'ordinamento della tabella
            List<List<EValue_2d>> out_list = new List<List<EValue_2d>>();
            List<List<EValue_2d>> scratch_list = new List<List<EValue_2d>>();  // per questa routine me ne serva anche una copia!
            int row_counter = 0;
            foreach (List<float> row in result_table)
            {
                int column_counter = 0;
                List<EValue_2d> row_list = new List<EValue_2d>();
                List<EValue_2d> scratch_row_list = new List<EValue_2d>();
                foreach (float value in row)
                {
                    EValue_2d value_2d = new EValue_2d();
                    value_2d.element_row = Form1.analysis_filenames[row_counter];
                    value_2d.element_column = Form1.analysis_filenames[column_counter];
                    value_2d.value = value;
                    row_list.Add(value_2d);

                    EValue_2d scratch_value_2d = new EValue_2d();
                    scratch_value_2d.element_row = Form1.analysis_filenames[row_counter];
                    scratch_value_2d.element_column = Form1.analysis_filenames[column_counter];
                    scratch_value_2d.value = value;
                    scratch_row_list.Add(scratch_value_2d);

                    column_counter++;
                }
                out_list.Add(row_list);
                scratch_list.Add(scratch_row_list);

                row_counter++;
            }


            // Riordinamento che tiene assieme tutti i clusters di lingue (anche se non garantisce più che le distanze dal primo elemento siano
            //    strettamente crescenti nell prima riga/colonna della tabella).

            // Come prima cosa determiniamo l'ordine della tabella finale, creando order_1st_row
            order_1st_row = new List<string>();

            // Aggiungiamo tutti i files, cercando sempre quello più vicino al precedente _fra tutti quelli che non sono già stati inseriti_
            int current_column = 0;
            for (int i = 0; i < scratch_list[0].Count; i++)
            {
                // Inserisci il file in lista
                order_1st_row.Add(scratch_list[0][current_column].element_column);

                // Trova il successivo
                int following_column = find_nearest(current_column, scratch_list);

                // Marca che il file appena inserito è stato usato (mettendo a MaxValue le sue distanze)
                set_as_used(current_column, ref scratch_list);
                current_column = following_column;
            }



            // Adesso l'algoritmo procede in modo simile a reorder_table
            //   1) + 2) Riordino tutte le righe (inclusa la prima) in funzione di order_1st_row

            //    3) Riordino le righe fra di loro seguendo lo stesso ordinamento

  
            // Riordino tutte le righe con lo stesso ordinamento della prima
            IComparer<EValue_2d> comparer_following_list = new TextCharsStats.CompareEValue_2dFollowList_column();
            foreach (List<EValue_2d> row in out_list)
            {
                row.Sort(comparer_following_list);
            }

            // Riordino le righe fra di loro seguendo l'ordinamento della prima riga
            IComparer<List<EValue_2d>> list_comparer_following_list = new TextCharsStats.CompareListEValue_2dFollowList_row();
            out_list.Sort(list_comparer_following_list);

            return out_list;
        }

        private static int find_nearest(int column, List<List<EValue_2d>> scratch_list)
        {

            // cerchiamo quale colonna è quella con distanza minima da 'column'
            int nearest = 0;
            float current_distance = float.MaxValue;
            for (int i = 0; i < scratch_list[column].Count; i++)
            {
                if (i == column)
                {
                    // distanza da se stesso
                    continue;
                }
                if (scratch_list[column][i].value < current_distance)
                {
                    nearest = i;
                    current_distance = scratch_list[column][i].value;
                }
            }

            return nearest;
        }




        private static void set_as_used(int column, ref List<List<EValue_2d>> scratch_list)
        {
            // Mette a MaxValue sia la colonna che la riga puntate da 'column'
            for (int i = 0; i < scratch_list[column].Count; i++)
            {
                scratch_list[column][i].value = float.MaxValue;
                scratch_list[i][column].value = float.MaxValue;
            }
        }


        public static float calculate_single_distance_2d(List<List<EValue>> reference_table, List<List<EValue>> table_to_compare, BlindFactor2d blind_factor)
        {
            float distance = 0;

            switch (blind_factor)
            {
                case BlindFactor2d._wholly_blinded:
                    distance = calculate_single_distance_2d_wholly_blinded(reference_table, table_to_compare);
                    break;
                case BlindFactor2d._wholly_blinded_but_try_reshuffle:
                    // MODALITA' NON ANCORA IMPLEMENTATA!!!!!!!!!!!!! DEFAULTA A WHOLLY_BLINDED
                    distance = calculate_single_distance_2d_wholly_blinded(reference_table, table_to_compare);
                    break;
                case BlindFactor2d._unblinded:
                    distance = calculate_single_distance_2d_unblinded(reference_table, table_to_compare);
                    break;
                default:
                    break;
            }

            return (distance);
        }


        private static float calculate_single_distance_2d_wholly_blinded(List<List<EValue>> reference_list, List<List<EValue>> list_to_compare)
        {
            float distance = 0;
            // Calcolo 100% blind, cioè senza fare alcuna ipotesi su cosa rappresentino effettivamente i caratteri nei due testi
            //  Calcolo brutale cella per cella. L'unica cosa a cui fare attenzione è che le due tabelle possono avere dimensioni diverse

            // E' stata impostata con gli arrays e la lascio così, anche perchè sugli array ho la funzione shuffle_rowcolumns che potrebbe tornarmi utile
            //    in futuro (p.es. modalità blinded ma con reshuffle colonne) MA E' UNA GRAN CAZZATA... BASTAVA USARE LE LISTE COME FOSSERO ARRAYS...

            EValue[,] reference_table = convert_to_array(reference_list);
            EValue[,] table_to_compare = convert_to_array(list_to_compare);

            int max_size = reference_table.GetLength(0);
            if (table_to_compare.GetLength(0) > max_size) max_size = table_to_compare.GetLength(0);

            for (int i = 0; i < max_size; i++)
            {
                for (int j = 0; j < max_size; j++)
                {
                    float reference_value = 0;
                    float value_to_compare = 0;
                    if (i < reference_table.GetLength(0) && j < reference_table.GetLength(0)) reference_value = reference_table[i, j].value;
                    if (i < table_to_compare.GetLength(0) && j < table_to_compare.GetLength(0)) value_to_compare = table_to_compare[i, j].value;

                    distance += (reference_value - value_to_compare) * (reference_value - value_to_compare);
                }
            }
            distance = (float)Math.Sqrt((double)distance);
            return (distance);
        }

        private static float calculate_single_distance_2d_unblinded(List<List<EValue>> reference_table, List<List<EValue>> table_to_compare)
        {

            List<List<EValue>> sorted_table_to_compare = sort_target_table_as_reference(reference_table, table_to_compare);

            return calculate_single_distance_2d_wholly_blinded(reference_table, sorted_table_to_compare);
        }

        private static List<List<EValue>> sort_target_table_as_reference(List<List<EValue>> reference_table, List<List<EValue>> table_to_compare)
        {
            // Algoritmo simile a quelli che creano le tabelle in TextCharsStats etc.

            // Come prima cosa creo la lista complessiva dei caratteri, unendo assieme quelli della reference_table e della table_to_compare
            //   Da questa creo la sorted_table inserendo tutti gli elementi calcolati dalla lista complessiva
            //   Adesso scandisco la sorted_table, e cerco ogni elemento nella table_to_compare
            //      Se lo trovo inserisco il value, altrimenti inserisco zero
            //  Complicazione per risparmiare tempo di esecuzione: la table_to_compare va trasferita in un Dictionary per velocizzare la ricerca dei bigrammi al suo interno


            // Lista complessiva di tutti i caratteri
            List<string> reference_character_set = get_character_set(reference_table);
            List<string> to_compare_character_set = get_character_set(table_to_compare);

            List<string> combined_character_set = get_character_set(reference_table); // preset a 'tutti i caratteri della reference table'
            foreach (string this_char in to_compare_character_set)
            {
                // controlla se c'è già nel reference_character_set
                bool found = false;
                foreach (string ref_char in reference_character_set)
                {
                    if (this_char == ref_char)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    combined_character_set.Add(this_char);
                }
            }

            // Creazione della sorted_table vuota, solo con gli elements
            List<List<EValue>> sorted_table = new List<List<EValue>>();
            foreach (string char_1st in combined_character_set)
            {
                List<EValue> sorted_row = new List<EValue>();
                foreach (string char_2nd in combined_character_set)
                {
                    EValue new_value = new EValue();
                    new_value.element = char_1st + char_2nd;
                    sorted_row.Add(new_value);
                }
                sorted_table.Add(sorted_row);
            }

            // Trasferimento della table_to_compare in un Dictionary
            Dictionary<string, EValue> to_compare_dictionary = new Dictionary<string, EValue>();
            foreach (List<EValue> row in table_to_compare)
            {
                foreach (EValue value in row)
                {
                    to_compare_dictionary.Add(value.element, value);
                }
            }

            // Scansione della sorted table: ogni bigramma viene ricercato nel to_compare_dictionary e, se esiste, si aggiorna il value (altrimenti resta a zero)
            for (int i = 0; i < sorted_table.Count; i++)
            {
                for (int j = 0; j < sorted_table.Count; j++)
                {
                    EValue found_value = new EValue();
                    if (to_compare_dictionary.TryGetValue(sorted_table[i][j].element, out found_value) == true)
                    {
                        sorted_table[i][j].value = found_value.value;
                    }
                }
            }

            return sorted_table;
        }


        private static List<string> get_character_set(List<List<EValue>> table)
        {
            List<string> out_list = new List<string>();

            foreach (EValue value in table[0])
            {
                // Scandisco una row, il carattere è nella seconda posizione del bigarmma
                string out_string = "" + value.element[1];
                out_list.Add(out_string);
            }
            return out_list;
        }





        //  Routines che lavorano su ARRAYS

        // Routine che scambia fra loro le righe/colonne di index_1st e index_2nd. Usa un ref per evitare di dover copiare ogni volta la tabella
        private static void shuffle_rowcolumns(ref EValue[,] table, int index_1st, int index_2nd)
        {
            if (index_1st == index_2nd)
            {
                return;
            }

            // pos_1 è sempre l'indice più basso, pos_2 il più alto
            int pos_1 = index_1st;
            int pos_2 = index_2nd;
            if (index_1st > index_2nd)
            {
                pos_1 = index_2nd;
                pos_2 = index_1st;
            }

            // Più facile del previsto... basta scambiare fra di loro le due colonne (o le due righe, fa lo stesso)

            EValue[] column_1st = new EValue[table.GetLength(0)];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                EValue new_value = new EValue();
                new_value.element = table[index_1st,i].element;
                new_value.value = table[index_1st, i].value;
                column_1st[i] = new_value;
            }

            EValue[] column_2nd = new EValue[table.GetLength(0)];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                EValue new_value = new EValue();
                new_value.element = table[index_2nd, i].element;
                new_value.value = table[index_2nd, i].value;
                column_2nd[i] = new_value;
            }

            for (int i = 0; i < table.GetLength(0); i++)
            {
                table[index_2nd, i].element = column_1st[i].element;
                table[index_2nd, i].value = column_1st[i].value;
            }
            for (int i = 0; i < table.GetLength(0); i++)
            {
                table[index_1st, i].element = column_2nd[i].element;
                table[index_1st, i].value = column_2nd[i].value;
            }

            return;
        }


        private static EValue[,] convert_to_array(List<List<EValue>> source_table)
        {
            EValue[,] out_array = new EValue[source_table.Count, source_table.Count];

            for (int i = 0; i < source_table.Count; i++)
            {
                for (int j = 0; j < source_table.Count; j++)
                {
                    EValue new_value = new EValue();
                    new_value.element = source_table[i][j].element;
                    new_value.value = source_table[i][j].value;
                    out_array[i, j] = new_value;
                }
            }
            return (out_array);
        }





        private void display_linear_comparisons(string title, List<EValue> distances, TextBox text_box)
        {
            string out_text = title + "\r\n";

            int counter = 0;
            foreach (EValue value in distances)
            {
                //     scrivo prima il numero e poi la stringa per evitare problemi di impaginazione
                //        aggiungo un numero progressivo alla stringa come riferimento al grafico
                out_text += value.value.ToString("F8") + "\t" + (counter+1).ToString() + ") " + value.element + "\r\n";
                counter++;
            }

            text_box.Text = out_text;
        }

        private void display_table_comparisons(string title, List<List<EValue_2d>> distances_list, TextBox text_box)
        {
            string out_text = title + "\r\n";

            // Intestazione orizzontale
            out_text += "\t";
            foreach (EValue_2d value in distances_list[0])
            {
                out_text += value.element_column + "\t";
            }
            out_text += "\r\n";

            // Righe della tabella
            int row_index = 0;
            foreach (List<EValue_2d> row in distances_list)
            {
                // Intestazione di riga
                out_text += "\r\n" + distances_list[row_index][0].element_row + "\t";
                row_index++;

                // Dati
                foreach (EValue_2d value in row)
                {
                    out_text += value.value.ToString() + "\t";
                }
            }

            text_box.Text = out_text;
        }






        // Calcolo delle distanze su tabelle 1d
        private List<EValue> calculate_distances_from_first_1d(List<List<EValue>> source_lists, BlindFactor1d blind_factor, int graph_limit)
        {
            // Calcola le distanze dal primo vettore a tutti gli altri e inseriscile nella lista

            // Qua serve il Take: questa funzione tratta dati come il vocabolario, le pseudo-sillabe...
            //    ma per qualche strano motivo ho dovuto scrivermi un loop, Take<> non ne voleva sapere di funzionare
            List<EValue> reduced_reference_list = new List<EValue>();
            int counter = 0;
            foreach (EValue value in source_lists[0])
            {
                EValue new_value = new EValue();
                new_value.element = value.element;
                new_value.value = value.value;
                reduced_reference_list.Add(new_value);
                counter++;
                if (counter == graph_limit) break;
            }


            // Se tutto è andato bene analysis_filenames e distances hanno la stessa dimensione!
            List<EValue> result = new List<EValue>();
            int table_counter = 0;
            foreach (List<EValue> list_data in source_lists)
            {
                // E serve un Take anche qua...
                List<EValue> reduced_list_data = new List<EValue>();
                counter = 0;
                foreach (EValue value in list_data)
                {
                    EValue new__value = new EValue();
                    new__value.element = value.element;
                    new__value.value = value.value;
                    reduced_list_data.Add(new__value);
                    counter++;
                    if (counter == graph_limit) break;
                }

                EValue new_value = new EValue();
                new_value.element = Form1.analysis_filenames[table_counter];
                new_value.value = calculate_single_distance_1d(reduced_reference_list, reduced_list_data, blind_factor);
                result.Add(new_value);

                table_counter++;
            }

            // ordinamento per distanze crescenti
            IComparer<EValue> comparer_by_value = new TextCharsStats.CompareEValueByValue_reverse();
            result.Sort(comparer_by_value);

            return result;
        }

        // Overload di calculate_distances_list_1d che usa i limiti 'rare characters' invece del limite dei dati lineari (per visualizzazione statistiche monogrammi)
        private List<EValue> calculate_distances_from_first_1d(List<List<EValue>> source_lists, BlindFactor1d blind_factor, List<int> graph_limit)
        {
            // Calcola le distanze dal primo vettore a tutti gli altri e inseriscile nella lista

            // Qua serve il Take: questa funzione tratta dati come il vocabolario, le pseudo-sillabe...
            //    ma per qualche strano motivo ho dovuto scrivermi un loop, Take<> non ne voleva sapere di funzionare
            List<EValue> reduced_reference_list = new List<EValue>();
            int counter = 0;
            foreach (EValue value in source_lists[0])
            {
                EValue new_value = new EValue();
                new_value.element = value.element;
                new_value.value = value.value;
                reduced_reference_list.Add(new_value);
                counter++;
                if (counter == graph_limit[0]) break;
            }


            // Se tutto è andato bene series_names_list e distances hanno la stessa dimensione!
            List<EValue> result = new List<EValue>();
            int table_counter = 0;
            foreach (List<EValue> list_data in source_lists)
            {
                // E serve un Take anche qua...
                List<EValue> reduced_list_data = new List<EValue>();
                counter = 0;
                foreach (EValue value in list_data)
                {
                    EValue new__value = new EValue();
                    new__value.element = value.element;
                    new__value.value = value.value;
                    reduced_list_data.Add(new__value);
                    counter++;
                    if (counter == graph_limit[table_counter]) break;
                }

                EValue new_value = new EValue();
                new_value.element = Form1.analysis_filenames[table_counter];
                new_value.value = calculate_single_distance_1d(reduced_reference_list, reduced_list_data, blind_factor);
                result.Add(new_value);

                table_counter++;
            }

            // ordinamento per distanze crescenti
            IComparer<EValue> comparer_by_value = new TextCharsStats.CompareEValueByValue_reverse();
            result.Sort(comparer_by_value);

            return result;
        }


        private static List<List<EValue_2d>> calculate_all_distances_1d(List<List<EValue>> source_lists, BlindFactor1d blind_factor, int graph_limit)
        {
            // Calcola le distanze di ogni vettore da ogni altro
            // Se tutto è andato bene series_names_list è lunga quanto ognuna delle dimensioni di distances!
            List<List<float>> result_table = new List<List<float>>();

            // Qua serve il Take, ma per qualche strano motivo ho dovuto scrivermi un loop, Take<> non ne voleva sapere di funzionare
            foreach (List<EValue> reference_list_data in source_lists)
            {
                List<EValue> reduced_reference_list = new List<EValue>();
                int counter = 0;
                foreach (EValue value in reference_list_data)
                {
                    EValue new_value = new EValue();
                    new_value.element = value.element;
                    new_value.value = value.value;
                    reduced_reference_list.Add(new_value);
                    counter++;
                    if (counter == graph_limit) break;
                }

                List<float> result_list = new List<float>();
                foreach (List<EValue> current_list_data in source_lists)
                {

                    List<EValue> reduced_current_list_data = new List<EValue>();
                    counter = 0;
                    foreach (EValue value in current_list_data)
                    {
                        EValue new_value = new EValue();
                        new_value.element = value.element;
                        new_value.value = value.value;
                        reduced_current_list_data.Add(new_value);
                        counter++;
                        if (counter == graph_limit) break;
                    }
                    result_list.Add(calculate_single_distance_1d(reduced_reference_list, reduced_current_list_data, blind_factor));
                }
                result_table.Add(result_list);

            }

            // Riordinamento!
            List<List<EValue_2d>> out_list = reorder_and_prepare_tables(result_table);

            return out_list;
        }

        // Overload di calculate_distances_table_1d che usa i limiti 'rare characters' invece del limite dei dati lineari (per visualizzazione statistiche monogrammi)
        private static List<List<EValue_2d>> calculate_all_distances_1d(List<List<EValue>> source_lists, BlindFactor1d blind_factor, List<int> graph_limit)
        {
            // Calcola le distanze di ogni vettore da ogni altro
            // Se tutto è andato bene series_names_list è lunga quanto ognuna delle dimensioni di distances!
            List<List<float>> result_table = new List<List<float>>();

            // Qua serve il Take, ma per qualche strano motivo ho dovuto scrivermi un loop, Take<> non ne voleva sapere di funzionare
            int reference_counter = 0;
            foreach (List<EValue> reference_list_data in source_lists)
            {
                List<EValue> reduced_reference_list = new List<EValue>();
                int counter = 0;
                foreach (EValue value in reference_list_data)
                {
                    EValue new_value = new EValue();
                    new_value.element = value.element;
                    new_value.value = value.value;
                    reduced_reference_list.Add(new_value);
                    counter++;
                    if (counter == graph_limit[reference_counter]) break;
                }
                reference_counter++;


                List<float> result_list = new List<float>();
                int current_list_counter = 0;
                foreach (List<EValue> current_list_data in source_lists)
                {

                    List<EValue> reduced_current_list_data = new List<EValue>();
                    counter = 0;
                    foreach (EValue value in current_list_data)
                    {
                        EValue new_value = new EValue();
                        new_value.element = value.element;
                        new_value.value = value.value;
                        reduced_current_list_data.Add(new_value);
                        counter++;
                        if (counter == graph_limit[current_list_counter]) break;
                    }
                    current_list_counter++;
                    result_list.Add(calculate_single_distance_1d(reduced_reference_list, reduced_current_list_data, blind_factor));
                }
                result_table.Add(result_list);

            }

            // Riordinamento!
            List<List<EValue_2d>> out_list = reorder_and_prepare_tables(result_table);

            return out_list;
        }



        public static float calculate_single_distance_1d(List<EValue> reference_list, List<EValue> list_to_compare, BlindFactor1d blind_factor)
        {
            float distance = 0;
            switch (blind_factor)
            {
                case BlindFactor1d._wholly_blinded:
                    distance = calculate_single_distance_1d_wholly_blinded(reference_list, list_to_compare);
                    break;
                case BlindFactor1d._unblinded:
                    distance = calculate_single_distance_1d_unblinded(reference_list, list_to_compare);
                    break;
                default:
                    break;
            }
            return (distance);
        }

        private static float calculate_single_distance_1d_wholly_blinded(List<EValue> reference_list, List<EValue> list_to_compare)
        {
            float distance = 0;
            // Calcolo 100% blind, cioè senza fare alcuna ipotesi su cosa rappresentino effettivamente i caratteri nei due testi
            //  Calcolo brutale cella per cella. L'unica cosa a cui fare attenzione è che i due vettori possono avere dimensioni diverse

            int max_size = reference_list.Count;
            if (list_to_compare.Count > max_size) max_size = list_to_compare.Count;

            for (int i = 0; i < max_size; i++)
            {
                float reference_value = 0;
                float value_to_compare = 0;
                if (i < reference_list.Count) reference_value = reference_list[i].value;
                if (i < list_to_compare.Count) value_to_compare = list_to_compare[i].value;

                distance += (reference_value - value_to_compare) * (reference_value - value_to_compare);
            }
            distance = (float)Math.Sqrt((double)distance);
            return (distance);
        }

        private static float calculate_single_distance_1d_unblinded(List<EValue> reference_list, List<EValue> list_to_compare)
        {

            List<EValue> sorted_list_to_compare = sort_target_list_as_reference(reference_list, list_to_compare);

            return calculate_single_distance_1d_wholly_blinded(reference_list, sorted_list_to_compare);
        }

        private static List<EValue> sort_target_list_as_reference(List<EValue> reference_list, List<EValue> list_to_compare)
        {

            // Algoritmo simile a quello di sort_target_table_as_reference

            // Come prima cosa creo la lista complessiva delle parole, unendo assieme quelle della reference_list e della list_to_compare
            //   Da questa creo la sorted_table inserendo tutti gli elementi calcolati dalla lista complessiva
            //   Adesso scandisco la sorted_table, e cerco ogni elemento nella table_to_compare
            //      Se lo trovo inserisco il value, altrimenti inserisco zero
            //  Complicazione per risparmiare tempo di esecuzione: la list_to_compare va trasferita in un Dictionary per velocizzare la ricerca delle parole al suo interno

            // Lista complessiva di tutte le parole
            List<string> reference_words_set = get_words_set(reference_list);
            List<string> to_compare_words_set = get_words_set(list_to_compare);

            List<string> combined_words_set = get_words_set(reference_list); // preset a 'tutte le parole caratteri della reference list'
            foreach (string this_word in to_compare_words_set)
            {
                // controlla se c'è già nel reference_words_set
                bool found = false;
                foreach (string ref_word in reference_words_set)
                {
                    if (this_word == ref_word)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    combined_words_set.Add(this_word);
                }
            }

            // Creazione della sorted_list vuota, solo con gli elements
            //   E' identica a combined_words_set, ma tengo il trasferimento per uniformità con la gestione di sort_target_table_as_reference
            List<EValue> sorted_list = new List<EValue>();
            foreach (string word in combined_words_set)
            {
                EValue new_value = new EValue();
                new_value.element = word;
                sorted_list.Add(new_value);
            }

            // Trasferimento della list_to_compare in un Dictionary
            Dictionary<string, EValue> to_compare_dictionary = new Dictionary<string, EValue>();
            foreach (EValue value in list_to_compare)
            {
                to_compare_dictionary.Add(value.element, value);
            }


            // Scansione della sorted table: ogni word viene ricercata nel to_compare_dictionary e, se esiste, si aggiorna il value (altrimenti resta a zero)
            for (int i = 0; i < sorted_list.Count; i++)
            {
                EValue found_value = new EValue();
                if (to_compare_dictionary.TryGetValue(sorted_list[i].element, out found_value) == true)
                {
                   sorted_list[i].value = found_value.value;
                }
            }

            return sorted_list;
        }


        private static List<string> get_words_set(List<EValue> input_list)
        {
            List<string> words = new List<string>();
            foreach (EValue value in input_list)
            {
                words.Add(value.element);
            }
            return words;
        }



        private void FormCompare_Resize(object sender, EventArgs e)
        {
            display_data(Form1._display_all);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            display_data(Form1._display_all);
        }

        private void checkBox_Compare_bigrams_delta_CheckedChanged(object sender, EventArgs e)
        {
            display_bigrams_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_bigrams_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_bigrams_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_bigrams_table_CheckedChanged(object sender, EventArgs e)
        {
            display_bigrams_comparisons(Form1._display_all);
        }

        private void button_Compare_bigrams_select_all_Click(object sender, EventArgs e)
        {
            textBox_Compare_bigrams.Select();
        }

        private void checkBox_Compare_follchar_table_CheckedChanged(object sender, EventArgs e)
        {
            display_following_char_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_follchar_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_following_char_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_prevchar_table_CheckedChanged(object sender, EventArgs e)
        {
            display_previous_char_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_prevchar_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_previous_char_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_folldist_table_CheckedChanged(object sender, EventArgs e)
        {
            display_following_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_folldist_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_following_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_prevdist_table_CheckedChanged(object sender, EventArgs e)
        {
            display_previous_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_prevdist_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_previous_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_remove_rare_characters_CheckedChanged(object sender, EventArgs e)
        {
            current_graphs_limit_2d = new List<int>();
            if (checkBox_Compare_remove_rare_characters.Checked == true)
            {
                foreach (int limit_value in rare_chars_removed_graphs_limit_2d)
                {
                    current_graphs_limit_2d.Add(limit_value);
                }
            }
            else
            {
                foreach (int limit_value in base_graphs_limit_2d)
                {
                    current_graphs_limit_2d.Add(limit_value);
                }
            }
            calculate_and_display_data(Form1._display_all); // serve la display_data completa e non solo quella 2d perchè questo limite agisce anche sui grafici dei monogrammi (che sono 1d)
        }

        private void textBox_Compare_rare_characters_cutoff_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Compare_rare_characters_cutoff.Text, out value) == true)
            {
                if (value >= rare_characters_min_cutoff && value <= rare_characters_max_cutoff)
                {
                    rare_characters_cutoff = value;
                    // Aggiorniamo i limiti al nuovo cutoff!
                    rare_chars_removed_graphs_limit_2d = get_2d_limits(rare_characters_cutoff);
                    current_graphs_limit_2d = get_2d_limits(rare_characters_cutoff);
                }
            }

            calculate_and_display_data(Form1._display_all); // serve la display_data completa e non solo quella 2d perchè questo limite agisce anche sui grafici dei monogrammi (che sono 1d)
        }

        private void textBox_Compare_rare_characters_cutoff_Leave(object sender, EventArgs e)
        {
            textBox_Compare_rare_characters_cutoff_TextChanged(sender, e);
        }

        private void textBox_Compare_rare_characters_cutoff_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_Compare_rare_characters_cutoff_TextChanged(sender, e);
        }



        // GESTIONE DI TRACKBARS ASSIEME A CASELLE DI TESTO (fatta come in Form1)
        private void trackBar_Compare_linear_size_ValueChanged(object sender, EventArgs e)
        {
            graphs_limit_1d = trackBar_Compare_linear_size.Value;
            this.textBox_Compare_linear_size.TextChanged -= new System.EventHandler(this.textBox_Compare_linear_size_TextChanged);
            textBox_Compare_linear_size.Text = graphs_limit_1d.ToString();
            this.textBox_Compare_linear_size.TextChanged += new System.EventHandler(this.textBox_Compare_linear_size_TextChanged);

            calculate_and_display_1d_data(Form1._display_all);
        }

        private void textBox_Compare_linear_size_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Compare_linear_size.Text, out value) == true)
            {
                process_linear_size(value);
            }
            calculate_and_display_1d_data(Form1._display_all);
        }

        private void textBox_Compare_linear_size_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Compare_linear_size.Text, out value) == true)
            {
                process_linear_size(value);
            }
            calculate_and_display_1d_data(Form1._display_all);
        }

        private void textBox_Compare_linear_size_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_Compare_linear_size_Leave(sender, e);
        }

        private void process_linear_size(int value)
        {
            if (int.TryParse(textBox_Compare_linear_size.Text, out value) == true)
            {
                if (value >= Form1.min_linear_graph_length && value <= Form1.max_linear_graph_length)
                {
                    graphs_limit_1d = value;
                    this.trackBar_Compare_linear_size.ValueChanged -= new System.EventHandler(this.trackBar_Compare_linear_size_ValueChanged);
                    trackBar_Compare_linear_size.Value = graphs_limit_1d;
                    this.trackBar_Compare_linear_size.ValueChanged += new System.EventHandler(this.trackBar_Compare_linear_size_ValueChanged);
                }
                else
                {
                    this.trackBar_Compare_linear_size.ValueChanged -= new System.EventHandler(this.trackBar_Compare_linear_size_ValueChanged);
                    if (value < Form1.min_linear_graph_length && trackBar_Compare_linear_size.Value > Form1.min_linear_graph_length) trackBar_Compare_linear_size.Value = Form1.min_linear_graph_length;
                    if (value > Form1.max_linear_graph_length && trackBar_Compare_linear_size.Value < Form1.max_linear_graph_length) trackBar_Compare_linear_size.Value = Form1.max_linear_graph_length;
                    this.trackBar_Compare_linear_size.ValueChanged += new System.EventHandler(this.trackBar_Compare_linear_size_ValueChanged);
                }
            }
        }
        // FINE GESTIONE TRACKBAR + CASELLA DI TESTO

        private void checkBox_Compare_vocabulary_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_vocabulary_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_vocabulary_table_CheckedChanged(object sender, EventArgs e)
        {
            display_vocabulary_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_wordslength_table_CheckedChanged(object sender, EventArgs e)
        {
            display_wordslength_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_wordslength_vocabulary_CheckedChanged(object sender, EventArgs e)
        {
            display_wordslength_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_wordslength_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_wordslength_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_syllables_table_CheckedChanged(object sender, EventArgs e)
        {
            display_syllables_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_syllables_singlevowelnuclei_CheckedChanged(object sender, EventArgs e)
        {
            display_syllables_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_syllables_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_syllables_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_singlechars_table_CheckedChanged(object sender, EventArgs e)
        {
            display_monograms_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_singlechars_nospaces_CheckedChanged(object sender, EventArgs e)
        {
            display_monograms_distances_comparisons(Form1._display_all);
        }

        private void checkBox_Compare_singlechars_blind_CheckedChanged(object sender, EventArgs e)
        {
            display_monograms_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_bigrams_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_bigrams_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_bigrams_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_bigrams_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_follchar_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_following_char_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_follchar_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_following_char_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_folldist_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_following_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_folldist_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_following_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_prevchar_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_previous_char_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_prevchar_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_previous_char_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_prevdist_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_previous_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_prevdist_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_previous_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_singlechars_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_monograms_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_singlechars_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_monograms_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_syllables_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_syllables_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_syllables_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_syllables_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_vocabulary_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_vocabulary_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_vocabulary_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_vocabulary_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_wordslength_distances_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_wordslength_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_wordslength_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_wordslength_distances_comparisons(Form1._display_all);
        }

        private void splitContainer_Compare_report_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            calculate_and_display_report(Form1._display_all);
        }

        private void splitContainer_Compare_report_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            calculate_and_display_report(Form1._display_all);
        }

        private void textBox_Compare_additional_report_words_limit_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Compare_additional_report_words_limit.Text, out value) == true)
            {
                if (value >= words_to_examine_min) // Limite inferiore proprio per sfizio
                {
                    words_to_examine = value;
                }
            }
            calculate_and_display_additional_report(compare_results.bigrams_distances_from_first_unblinded, compare_results.bigrams_distances_from_first_blind);
        }

        private void textBox_Compare_additional_report_words_limit_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_Compare_additional_report_words_limit_TextChanged(sender,e);
        }

        private void textBox_Compare_additional_report_words_limit_Leave(object sender, EventArgs e)
        {
            textBox_Compare_additional_report_words_limit_TextChanged(sender, e);
        }

        private void textBox_Compare_additional_report_min_amplification_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_Compare_additional_report_min_amplification_TextChanged(sender, e);
        }

        private void textBox_Compare_additional_report_min_amplification_Leave(object sender, EventArgs e)
        {
            textBox_Compare_additional_report_min_amplification_TextChanged(sender, e);
        }

        private void textBox_Compare_additional_report_min_amplification_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Compare_additional_report_min_amplification.Text, out value) == true)
            {
                if (value >= amplification_threshold_min) // Limite inferiore proprio per sfizio
                {
                    amplification_threshold = value;
                }
            }
            calculate_and_display_additional_report(compare_results.bigrams_distances_from_first_unblinded, compare_results.bigrams_distances_from_first_blind);
        }

        private void checkBox_Compare_show_clusters_CheckedChanged(object sender, EventArgs e)
        {
            calculate_and_display_data(Form1._display_all);  // serve la display_data completa!
        }
    }



}
