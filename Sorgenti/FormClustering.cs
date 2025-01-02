using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Template
{
    public partial class FormClustering : Form
    {

        public static FormClustering mainFormClustering;

        private Thread calculate_and_display_dataThread;
        private static bool a_threadIsStarted = false;

        // Delegates per routines che vanno richiamate dall'interno di una thread
        private delegate void delegate_calculate_and_display_data();
        private delegate void delegate_disable_controls_while_threads_are_running();
        private delegate void delegate_enable_controls_when_thread_stops();
        private delegate void delegate_update_textBox_Clustering_Text(string report);
        private delegate void delegate_update_pictureBox_Clustering(Bitmap image);
        // Fine delegates per routines che vanno richiamate dall'interno di una thread

        // Variabili gestione MouseHover sul grafico
        private static System.Timers.Timer mousehover_Timer = new System.Timers.Timer();
        private static Point mouse_position = new Point(0, 0); // coordinate correnti del mouse (da mouseHover event)





        public static List<ClusteringResults> clustering_results = new List<ClusteringResults>();   // classe primaria di questa Form
        public static List<ClusterNew> clustering_resultsNew = new List<ClusterNew>();   // classe primaria di questa Form

        private static List<RelativeDistance> distances_list = new List<RelativeDistance>();  // distanze fra i testi in forma lineare
        private static List<List<float>> distances_table = new List<List<float>>();           // distanze fra i testi in forma tabellare



        private static List<int> graphs_limit_2d = new List<int>();    // simile a FormCompare

        // E' necessario definirlo globale, perchè bisogna potervi accedere con gli eventi MouseHover
        private static XPlotClusters graph;



        public FormClustering()
        {
            InitializeComponent();

            mainFormClustering = this;  // Variabile che torna utile in vari casi per accedere alla Form!

            CompareResults compare_results = new CompareResults();

            initialize_controls();
            display_controls();


            disable_controls_while_threads_are_running();

            calculate_and_display_dataThread = new Thread(calculate_and_display_data);
            calculate_and_display_dataThread.Start();
        }


        private static void initialize_controls()
        {

            // Calcola i limits dei grafici 2d
            graphs_limit_2d = new List<int>();
            if (Form1.clustering_choices_remove_rare_characters == false)
            {
                // Setta ogni grafico 2D al suo limite massimo
                foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
                {
                    graphs_limit_2d.Add(analysis.analysis_results.monograms_distribution.Count);  
                }
            }
            else
            {
                graphs_limit_2d = FormCompare.get_2d_limits(Form1.clustering_choices_rare_characters_cutoff);
            }


            initialize_mouse_hover();

        }

        private static void initialize_mouse_hover()
        {
            // Timer che evita di lanciare la gestione del MouseHover ogni volta che si verifica un evento

            // Il timer è un monostabile che viene lanciato ad ogni evento mousehover, se arriva un altro mousehover
            //   prima che il timer scada il timer riparte e ritarda la gestione finchè il punto di hovering è 'stabile'            
            mousehover_Timer.Interval = 50;    // tempo in ms
            mousehover_Timer.Enabled = true;
            mousehover_Timer.AutoReset = false;
            mousehover_Timer.Stop();
            mousehover_Timer.Elapsed += new System.Timers.ElapsedEventHandler(mousehover_Timer_Elapsed);
        }



        private static void display_controls()
        {
        }

        private static void enforce_controls_coherency()
        {        
        }

        private static void disable_controls_while_threads_are_running()
        {
            a_threadIsStarted = true;

        }

        private static void enable_controls_when_thread_stops()
        {

            a_threadIsStarted = false;
        }

        private void calculate_and_display_data()
        {
            enforce_controls_coherency();
            display_controls();

            string message = "Starting clustering calculations";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            calculate_data();

            message = "Clustering calculations completed, formatting data for display...";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            display_data();

            message = "Clustering completed";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            // Riabilitazione dei controlli
            mainFormClustering.Invoke((delegate_enable_controls_when_thread_stops)enable_controls_when_thread_stops, new object[] { });

        }

        private static void calculate_data()
        {
            string message = "Calculating distances...";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            switch (Form1.clustering_choices_status)
            {
                case VClusteringChoices.combo_clustering_choices.bigrams:
                    calculate_bigrams_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.monograms:
                    calculate_monograms_distances(ref distances_list, ref distances_table); // ricordare che i monogrammi usano i graphs_limits_2d, non quello lineare! unico caso
                    break;
                case VClusteringChoices.combo_clustering_choices.following_char:
                    calculate_following_char_probability_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_char:
                    calculate_previous_char_probability_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.following_distance:
                    calculate_following_distance_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_distance:
                    calculate_previous_distance_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.vocabulary:
                    calculate_vocabulary_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_text:
                    calculate_wordslength_text_distances(ref distances_list, ref distances_table);
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_vocabulary:
                    calculate_wordslength_vocabulary_distances(ref distances_list, ref distances_table);
                    break;
                default:
                    message = "SOFTWARE ERROR: cannot find clustering_choices_status = " + Form1.clustering_choices_status + "in FormClustering.calculate_data";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    break;
            }

            message = "Calculating clusters...";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            clustering_results = calculate_clusters();

            clustering_resultsNew = calculate_clustersNew();
            
        }


        private static void calculate_bigrams_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<List<EValueOcc>>> source_data = new List<List<List<EValueOcc>>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValueOcc>> single_analysis = new List<List<EValueOcc>>();
                foreach (List<EValueOcc> row in analysis.analysis_results.bigrams_distribution_table)
                {
                    List<EValueOcc> single_row = new List<EValueOcc>();
                    foreach (EValueOcc single_value in row)
                    {
                        EValueOcc new_value = new EValueOcc();
                        new_value.value = single_value.value;
                        new_value.element = single_value.element;
                        single_row.Add(new_value);
                    }
                    single_analysis.Add(single_row);
                }
                source_data.Add(single_analysis);
            }

            calculate_2d_distances_list_and_table(Form1.EVconvert(source_data), graphs_limit_2d, ref distances_list, ref distances_table);
        }

        private static void calculate_following_char_probability_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<List<EValue>>> source_data = new List<List<List<EValue>>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.following_character_distribution)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        EValue new_value = new EValue();
                        new_value.value = single_value.value;
                        new_value.element = single_value.element;
                        single_row.Add(new_value);
                    }
                    single_analysis.Add(single_row);
                }
                source_data.Add(single_analysis);
            }

            calculate_2d_distances_list_and_table(source_data, graphs_limit_2d, ref distances_list, ref distances_table);
        }
        private static void calculate_previous_char_probability_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<List<EValue>>> source_data = new List<List<List<EValue>>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.previous_character_distribution)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        EValue new_value = new EValue();
                        new_value.value = single_value.value;
                        new_value.element = single_value.element;
                        single_row.Add(new_value);
                    }
                    single_analysis.Add(single_row);
                }
                source_data.Add(single_analysis);
            }

            calculate_2d_distances_list_and_table(source_data, graphs_limit_2d, ref distances_list, ref distances_table);
        }

        private static void calculate_following_distance_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<List<EValue>>> source_data = new List<List<List<EValue>>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.monograms_distances_according_to_following_character)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        EValue new_value = new EValue();
                        new_value.value = single_value.value;
                        new_value.element = single_value.element;
                        single_row.Add(new_value);
                    }
                    single_analysis.Add(single_row);
                }
                source_data.Add(single_analysis);
            }

            calculate_2d_distances_list_and_table(source_data, graphs_limit_2d, ref distances_list, ref distances_table);
        }

        private static void calculate_previous_distance_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<List<EValue>>> source_data = new List<List<List<EValue>>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<List<EValue>> single_analysis = new List<List<EValue>>();
                foreach (List<EValue> row in analysis.analysis_results.monograms_distances_according_to_previous_character)
                {
                    List<EValue> single_row = new List<EValue>();
                    foreach (EValue single_value in row)
                    {
                        EValue new_value = new EValue();
                        new_value.value = single_value.value;
                        new_value.element = single_value.element;
                        single_row.Add(new_value);
                    }
                    single_analysis.Add(single_row);
                }
                source_data.Add(single_analysis);
            }

            calculate_2d_distances_list_and_table(source_data, graphs_limit_2d, ref distances_list, ref distances_table);
        }




        private static void calculate_vocabulary_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<EValueOcc>> source_data = new List<List<EValueOcc>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc single_value in analysis.analysis_results.vocabulary_words_distribution)
                {
                    EValueOcc new_value = new EValueOcc();
                    new_value.value = single_value.value;
                    new_value.element = single_value.element;
                    single_analysis.Add(new_value);
                }
                source_data.Add(single_analysis);
            }

            calculate_1d_distances_list_and_table(Form1.EVconvert_1d_multipli(source_data), ref distances_list, ref distances_table);
        }
        private static void calculate_wordslength_text_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<EValueOcc_extended>> source_data = new List<List<EValueOcc_extended>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc_extended> single_analysis = new List<EValueOcc_extended>();
                foreach (EValueOcc_extended single_value in analysis.analysis_results.words_length_distribution_in_text)
                {
                    EValueOcc_extended new_value = new EValueOcc_extended();
                    new_value.value = single_value.value;
                    new_value.element = single_value.element;
                    new_value.element_additional = single_value.element_additional;
                    single_analysis.Add(new_value);
                }
                source_data.Add(single_analysis);
            }

            calculate_1d_distances_list_and_table(Form1.EVconvert_1d_multipli(source_data), ref distances_list, ref distances_table);
        }

        private static void calculate_wordslength_vocabulary_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<EValueOcc_extended>> source_data = new List<List<EValueOcc_extended>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc_extended> single_analysis = new List<EValueOcc_extended>();
                foreach (EValueOcc_extended single_value in analysis.analysis_results.words_length_distribution_in_vocabulary)
                {
                    EValueOcc_extended new_value = new EValueOcc_extended();
                    new_value.value = single_value.value;
                    new_value.element = single_value.element;
                    new_value.element_additional = single_value.element_additional;
                    single_analysis.Add(new_value);
                }
                source_data.Add(single_analysis);
            }

            calculate_1d_distances_list_and_table(Form1.EVconvert_1d_multipli(source_data), ref distances_list, ref distances_table);
        }

        private static void calculate_monograms_distances(ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {

            // Estrazione dei dati da analysis_to_compare
            //   E' impossibile mettere questa parte in una routine perchè compare esplicitamente il nome del campo da copiare
            //    (parte molto simile a quella omologa in FormCompare.calculate_bigrams_comparisons
            List<List<EValueOcc>> source_data = new List<List<EValueOcc>>();
            foreach (TextAnalyzerClass analysis in Form1.analysis_to_compare)
            {
                List<EValueOcc> single_analysis = new List<EValueOcc>();
                foreach (EValueOcc single_value in analysis.analysis_results.monograms_distribution)
                {
                    EValueOcc new_value = new EValueOcc();
                    new_value.value = single_value.value;
                    new_value.element = single_value.element;
                    single_analysis.Add(new_value);
                }
                source_data.Add(single_analysis);
            }

            // I monogrammi usano un overload di calculate_1d_distances che gestisce i graphs_limits_2d
            calculate_1d_distances_list_and_table(Form1.EVconvert_1d_multipli(source_data), graphs_limit_2d, ref distances_list, ref distances_table);
        }




        private static void calculate_2d_distances_list_and_table(List<List<List<EValue>>> source_data, List<int> graphs_limits, ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {
            // PREPARAZIONE DEI DATI APPLICANDO I GRAPHS_LIMIT 2d
            List<List<List<EValue>>> source_data_after_applying_limits = new List<List<List<EValue>>>();

            int analysis_number = 0; // serve per l'accesso a graphs_limits
            foreach (List<List<EValue>> reference_table_data in source_data)
            {
                List<List<EValue>> reduced_table = new List<List<EValue>>();
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
                    reduced_table.Add(reduced_row);
                }
                source_data_after_applying_limits.Add(reduced_table);

                analysis_number++;
            }


            // CALCOLO DELLE DISTANZE

            // Contrariamente a quello che accade in FormCompare (in calculate_all_distances_2d):

            //      1) Le distanze vengono inserite in una lista, non in una tabella
            //      2) Non vengono calcolate N*N distanze ma solo (N*N-N)/2 (le distanze sono simmetriche, comparirebbero due volte ognuna nella lista, oltre allo spreco di tempo)
            //              (pensare ad una tabella triangolare diove manca anche la diagonale principale (che sono tutte distanze zero))
            //      3) Ogni distanza viene marcata col numero delle due analisi, usando quella che in effetti è una variante di EValues_2d: RelativeDistances, dove
            //              gli 'elments' sono nuemri interi e non stringhe
            //      4) Si calcola solo la distanza unblinded (è di sicuro l'unica che serve per questo tipo di analisi!)

            //      5) Viene generata anche una versione tabellare delle distanze (che serve nelle elaborazioni successive).

            distances_list = new List<RelativeDistance>();
            distances_table = initialize_distances_table(source_data_after_applying_limits.Count);  // Prepara una tabella di N*N celle, tutte a zero

            for (int reference_analysis_counter = 0; reference_analysis_counter < source_data_after_applying_limits.Count; reference_analysis_counter++)
            {
                // tabella diagonale (e senza la diagonale principale)
                for (int target_analysis_counter = (reference_analysis_counter + 1); target_analysis_counter < source_data_after_applying_limits.Count; target_analysis_counter++)
                {
                    // Inserimento nella lista
                    RelativeDistance new_distance = new RelativeDistance();
                    new_distance.distance = FormCompare.calculate_single_distance_2d(source_data_after_applying_limits[reference_analysis_counter], source_data_after_applying_limits[target_analysis_counter], FormCompare.BlindFactor2d._unblinded);
                    new_distance.text_1st = reference_analysis_counter;
                    new_distance.text_2nd = target_analysis_counter;
                    distances_list.Add(new_distance);

                    // Inserimento distanza nella tabella
                    distances_table[reference_analysis_counter][target_analysis_counter] = new_distance.distance;
                }
            }

            // ordinamento della lista per distanze crescenti
            IComparer<RelativeDistance> comparer = new CompareRelativeDistance_ByValue_reverse();
            distances_list.Sort(comparer);


            // completamento della tabella simmetrica (la diagonale principale è già a zero)
            for (int i = 0; i < source_data_after_applying_limits.Count; i++)
            {
                for (int j = i; j < source_data_after_applying_limits.Count; j++)
                {
                    distances_table[j][i] = distances_table[i][j];
                }
            }

        }


        private static void calculate_1d_distances_list_and_table(List<List<EValue>> source_data, ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {
            // PREPARAZIONE DEI DATI APPLICANDO I GRAPHS_LIMIT 1D
            List<List<EValue>> source_data_after_applying_limits = new List<List<EValue>>();

            foreach (List<EValue> reference_linear_data in source_data)
            {
                List<EValue> reduced_list = new List<EValue>();
                foreach (EValue value in reference_linear_data.Take<EValue>(Form1.clustering_choices_linear_limit))
                {
                    EValue new_value = new EValue();
                    new_value.element = value.element;
                    new_value.value = value.value;
                    reduced_list.Add(new_value);
                }
                source_data_after_applying_limits.Add(reduced_list);
            }


            // CALCOLO DELLE DISTANZE

            // Contrariamente a quello che accade in FormCompare (in calculate_all_distances_2d):

            //      1) Le distanze vengono inserite in una lista, non in una tabella
            //      2) Non vengono calcolate N*N distanze ma solo (N*N-N)/2 (le distanze sono simmetriche, comparirebbero due volte ognuna nella lista, oltre allo spreco di tempo)
            //              (pensare ad una tabella triangolare diove manca anche la diagonale principale (che sono tutte distanze zero))
            //      3) Ogni distanza viene marcata col numero delle due analisi, usando quella che in effetti è una variante di EValues_2d: RelativeDistances, dove
            //              gli 'elments' sono nuemri interi e non stringhe
            //      4) Si calcola solo la distanza unblinded (è di sicuro l'unica che serve per questo tipo di analisi!)

            //      5) Viene generata anche una versione tabellare delle distanze (che serve nelle elaborazioni successive).

            distances_list = new List<RelativeDistance>();
            distances_table = initialize_distances_table(source_data_after_applying_limits.Count);  // Prepara una tabella di N*N celle, tutte a zero

            for (int reference_analysis_counter = 0; reference_analysis_counter < source_data_after_applying_limits.Count; reference_analysis_counter++)
            {
                // tabella diagonale (e senza la diagonale principale)
                for (int target_analysis_counter = (reference_analysis_counter + 1); target_analysis_counter < source_data_after_applying_limits.Count; target_analysis_counter++)
                {
                    // Inserimento nella lista
                    RelativeDistance new_distance = new RelativeDistance();
                    new_distance.distance = FormCompare.calculate_single_distance_1d(source_data_after_applying_limits[reference_analysis_counter], source_data_after_applying_limits[target_analysis_counter], FormCompare.BlindFactor1d._unblinded);
                    new_distance.text_1st = reference_analysis_counter;
                    new_distance.text_2nd = target_analysis_counter;
                    distances_list.Add(new_distance);

                    // Inserimento distanza nella tabella
                    distances_table[reference_analysis_counter][target_analysis_counter] = new_distance.distance;
                }
            }

            // ordinamento della lista per distanze crescenti
            IComparer<RelativeDistance> comparer = new CompareRelativeDistance_ByValue_reverse();
            distances_list.Sort(comparer);


            // completamento della tabella simmetrica (la diagonale principale è già a zero)
            for (int i = 0; i < source_data_after_applying_limits.Count; i++)
            {
                for (int j = i; j < source_data_after_applying_limits.Count; j++)
                {
                    distances_table[j][i] = distances_table[i][j];
                }
            }

        }

        // Versione specializzata per i monogrammi: applica i graphs_limits_2d
        private static void calculate_1d_distances_list_and_table(List<List<EValue>> source_data, List<int> graphs_limits, ref List<RelativeDistance> distances_list, ref List<List<float>> distances_table)
        {
            // PREPARAZIONE DEI DATI APPLICANDO I GRAPHS_LIMIT
            List<List<EValue>> source_data_after_applying_limits = new List<List<EValue>>();

            int analysis_number = 0; // serve per l'accesso a graphs_limits
            foreach (List<EValue> reference_linear_data in source_data)
            {
                List<EValue> reduced_list = new List<EValue>();
                foreach (EValue value in reference_linear_data.Take<EValue>(graphs_limits[analysis_number]))
                {
                    EValue new_value = new EValue();
                    new_value.element = value.element;
                    new_value.value = value.value;
                    reduced_list.Add(new_value);
                }
                source_data_after_applying_limits.Add(reduced_list);

                analysis_number++;
            }


            // CALCOLO DELLE DISTANZE

            // Contrariamente a quello che accade in FormCompare (in calculate_all_distances_2d):

            //      1) Le distanze vengono inserite in una lista, non in una tabella
            //      2) Non vengono calcolate N*N distanze ma solo (N*N-N)/2 (le distanze sono simmetriche, comparirebbero due volte ognuna nella lista, oltre allo spreco di tempo)
            //              (pensare ad una tabella triangolare diove manca anche la diagonale principale (che sono tutte distanze zero))
            //      3) Ogni distanza viene marcata col numero delle due analisi, usando quella che in effetti è una variante di EValues_2d: RelativeDistances, dove
            //              gli 'elments' sono nuemri interi e non stringhe
            //      4) Si calcola solo la distanza unblinded (è di sicuro l'unica che serve per questo tipo di analisi!)

            //      5) Viene generata anche una versione tabellare delle distanze (che serve nelle elaborazioni successive).

            distances_list = new List<RelativeDistance>();
            distances_table = initialize_distances_table(source_data_after_applying_limits.Count);  // Prepara una tabella di N*N celle, tutte a zero

            for (int reference_analysis_counter = 0; reference_analysis_counter < source_data_after_applying_limits.Count; reference_analysis_counter++)
            {
                // tabella diagonale (e senza la diagonale principale)
                for (int target_analysis_counter = (reference_analysis_counter + 1); target_analysis_counter < source_data_after_applying_limits.Count; target_analysis_counter++)
                {
                    // Inserimento nella lista
                    RelativeDistance new_distance = new RelativeDistance();
                    new_distance.distance = FormCompare.calculate_single_distance_1d(source_data_after_applying_limits[reference_analysis_counter], source_data_after_applying_limits[target_analysis_counter], FormCompare.BlindFactor1d._unblinded);
                    new_distance.text_1st = reference_analysis_counter;
                    new_distance.text_2nd = target_analysis_counter;
                    distances_list.Add(new_distance);

                    // Inserimento distanza nella tabella
                    distances_table[reference_analysis_counter][target_analysis_counter] = new_distance.distance;
                }
            }

            // ordinamento della lista per distanze crescenti
            IComparer<RelativeDistance> comparer = new CompareRelativeDistance_ByValue_reverse();
            distances_list.Sort(comparer);


            // completamento della tabella simmetrica (la diagonale principale è già a zero)
            for (int i = 0; i < source_data_after_applying_limits.Count; i++)
            {
                for (int j = i; j < source_data_after_applying_limits.Count; j++)
                {
                    distances_table[j][i] = distances_table[i][j];
                }
            }

        }

        private static List<List<float>> initialize_distances_table(int size)
        {
            // Inizializza una tabella di N*N celle, tutte a zero
            List<List<float>> result = new List<List<float>>();
            for (int row_counter = 0; row_counter < size; row_counter++)
            {
                List<float> distances_row = new List<float>();
                for (int column_counter = 0; column_counter < size; column_counter++)
                {
                    distances_row.Add(0f);
                }
                result.Add(distances_row);
            }
            return result;
        }

        private class CompareRelativeDistance_ByValue_reverse : IComparer<RelativeDistance>
        {
            public int Compare(RelativeDistance x, RelativeDistance y)
            {
                if (x.distance == y.distance) return (0);

                if (x.distance < y.distance)
                {
                    return (-1);
                }
                return (1);
            }
        }







        private static List<ClusteringResults> calculate_clusters()
        {
            List<ClusteringResults> result = new List<ClusteringResults>();

            // Tutto l'algoritmo si basa su una sequenza di "distanze limite", che variano dalla distanza minima fra due testi (che porta a generare un unico gruppo
            //    (salvo casi eccezionali) contenente i due testi in questione) alla distanza massima

            float min_distance = distances_list[0].distance;
            float max_distance = distances_list[distances_list.Count - 1].distance;

            int distances_layers = 12;    // numero di 'fette' in cui viene suddiviso il segmento da min_distance a max_min_distance

            float distances_ratio = max_distance / min_distance;
            // devo calcolare la radice distances_layers-esima di distances_ratio (come le note su un pianoforte)
            double multiplicative_factor = Math.Pow((double)distances_ratio, 1/(double)distances_layers);


            double current_limit_distance = min_distance;
            for (int i = 0; i < distances_layers; i++)
            {
                ClusteringResults this_result = get_clusters((float)current_limit_distance);
                this_result.limit_distance = (float)current_limit_distance;
                result.Add(this_result);

                current_limit_distance *= multiplicative_factor;
                if (i == distances_layers - 2)
                {
                    current_limit_distance = max_distance;  // Necessario per evitare problemi di arrotondamenti
                }
            }

            return result;
        }


        private static ClusteringResults get_clusters(float limit_distance)
        {
            ClusteringResults result = new ClusteringResults();

            foreach (RelativeDistance distance in distances_list)
            {
                if (distance.distance > limit_distance) break;  // la lista è ordinata per distanza, posso breakkare alla prima oltre al limite!

                // Vediamo se esistono già dei gruppi a cui aggiungere i due testi.
                //
                //  Entrambi i testi sono già inseriti in un gruppo. Questo accade p.es. se ho gestito le distanze {0, 1} e creato il gruppo [0,1]
                //      poi trovo la distanza {0, 2} e da questa aggiungo 2 al gruppo: [0,1,2], poi trovo la distanza {1, 2} con sia 1 che 2 già inseriti.
                //          In questo caso devo solo aggiornare la max_intra_cluster_distance (che potrebbe cambiare)

                //  Uno dei due testi è già inserito in un gruppo
                //      Se la distanza dell'altro testo da tutti gli elementi del gruppo è minore del limite aggiungi l'altro testo al gruppo

                // Nessuno dei due testi è già inserito in un gruppo
                //      Se la distanza di entrambi i testi da tutti gli elementi del gruppo è minore del limite aggiungi entrambi i testi al gruppo

                // Alla fine, se i testi non sono stati inseriti, crea un nuovo gruppo per loro

                bool text_inserted = false;
                List<int> overlapping_groups_list = new List<int>();
                int cluster_counter = 0;
                foreach (ClusterGroup cluster in result.clusters)
                {
                    // Test per vedere se entrambi i testi sono già inseriti
                    if (cluster.group_members.ContainsKey(distance.text_1st) == true && cluster.group_members.ContainsKey(distance.text_2nd) == true)
                    {
                        if (distance.distance > cluster.max_intra_cluster_distance) cluster.max_intra_cluster_distance = distance.distance;

                        text_inserted = true;
                        break;
                    }

                    float max_this_text_intra_cluster_distance = 0; //dummy value
                    if (cluster.group_members.ContainsKey(distance.text_1st) == true)
                    {
                        // se tutti gli elementi del gruppo sono a meno di limit_distance dal secondo testo: inseriscilo nel gruppo
                        if (all_distances_from_cluster_members_are_inside_limit_distance(distance.text_2nd, limit_distance, cluster, ref max_this_text_intra_cluster_distance) == true)
                        {
                            // Inserimento del testo nel gruppo
                            result.clusters[cluster_counter].group_members.Add(distance.text_2nd, distance.text_2nd);
                            if (max_this_text_intra_cluster_distance > result.clusters[cluster_counter].max_intra_cluster_distance)
                            {
                                result.clusters[cluster_counter].max_intra_cluster_distance = max_this_text_intra_cluster_distance;
                            }

                            update_text_assignements(distance.text_2nd, cluster_counter, ref result.texts_assignements);

                            text_inserted = true;
                            break;
                        }

                        // se arriviamo qua il testo non è stato inserito e c'è un overlap fra gruppi
                        overlapping_groups_list.Add(cluster_counter);
                    }

                    if (cluster.group_members.ContainsKey(distance.text_2nd) == true)
                    {
                        // se tutti gli elementi del gruppo sono a meno di limit_distance dal primo testo: inseriscilo nel gruppo
                        if (all_distances_from_cluster_members_are_inside_limit_distance(distance.text_1st, limit_distance, cluster, ref max_this_text_intra_cluster_distance) == true)
                        {
                            // Inserimento del testo nel gruppo
                            result.clusters[cluster_counter].group_members.Add(distance.text_1st, distance.text_1st);
                            if (max_this_text_intra_cluster_distance > result.clusters[cluster_counter].max_intra_cluster_distance)
                            {
                                result.clusters[cluster_counter].max_intra_cluster_distance = max_this_text_intra_cluster_distance;
                            }

                            update_text_assignements(distance.text_1st, cluster_counter, ref result.texts_assignements);

                            text_inserted = true;
                            break;
                        }

                        // se arriviamo qua il testo non è stato inserito e c'è un overlap fra gruppi
                        overlapping_groups_list.Add(cluster_counter);
                    }

                    // test per vedere se entrambi i testi possono essere inseriti nel gruppo
                    if (cluster.group_members.ContainsKey(distance.text_1st) == false && cluster.group_members.ContainsKey(distance.text_2nd) == false)
                    {
                        // Il primo testo può essere inserito?
                        if (all_distances_from_cluster_members_are_inside_limit_distance(distance.text_1st, limit_distance, cluster, ref max_this_text_intra_cluster_distance) == false)
                        {
                            cluster_counter++;
                            continue;
                        }
                        float first_max_distance = max_this_text_intra_cluster_distance;

                        // Il secondo testo può essere inserito?
                        if (all_distances_from_cluster_members_are_inside_limit_distance(distance.text_2nd, limit_distance, cluster, ref max_this_text_intra_cluster_distance) == false)
                        {
                            cluster_counter++;
                            continue;
                        }

                        // Inserimento nel gruppo di entrambi i testi!
                        result.clusters[cluster_counter].group_members.Add(distance.text_1st, distance.text_1st);
                        result.clusters[cluster_counter].group_members.Add(distance.text_2nd, distance.text_2nd);

                        if (first_max_distance > max_this_text_intra_cluster_distance) max_this_text_intra_cluster_distance = first_max_distance;
                        if (max_this_text_intra_cluster_distance > result.clusters[cluster_counter].max_intra_cluster_distance)
                        {
                            result.clusters[cluster_counter].max_intra_cluster_distance = max_this_text_intra_cluster_distance;
                        }

                        update_text_assignements(distance.text_1st, result.clusters.Count - 1, ref result.texts_assignements);
                        update_text_assignements(distance.text_2nd, result.clusters.Count - 1, ref result.texts_assignements);

                        text_inserted = true;
                        break;
                    }

                    cluster_counter++;

                }

                if (text_inserted == false)
                {
                    // Nuovo cluster
                    ClusterGroup new_cluster = new ClusterGroup();
                    new_cluster.group_members = new Dictionary<int, int>();

                    new_cluster.group_members.Add((int)distance.text_1st, (int)distance.text_1st);
                    new_cluster.group_members.Add((int)distance.text_2nd, (int)distance.text_2nd);

                    new_cluster.max_intra_cluster_distance = distance.distance;

                    new_cluster.overlapping_clusters = overlapping_groups_list;

                    result.clusters.Add(new_cluster);

                    update_text_assignements(distance.text_1st, result.clusters.Count - 1, ref result.texts_assignements);
                    update_text_assignements(distance.text_2nd, result.clusters.Count - 1, ref result.texts_assignements);

                }

            }

            // Ordinamento dei clusters per distanza
            IComparer<ClusterGroup> comparer = new CompareClusterGroup_Bydistance_ascending();
            result.clusters.Sort(comparer);

            return result;
        }


        private static bool all_distances_from_cluster_members_are_inside_limit_distance(int text_number, float limit_distance, ClusterGroup cluster, ref float max_intra_cluster_distance)
        {
            bool result = true;

            max_intra_cluster_distance = float.MinValue;
            foreach (int target_text in cluster.group_members.Values)
            {
                float this_distance = distances_table[text_number][target_text];
                if (this_distance > max_intra_cluster_distance) max_intra_cluster_distance = this_distance;
                if (this_distance > limit_distance)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }


        private static void update_text_assignements(int text, int cluster_number, ref Dictionary<int, AssignedText> texts_assignements)
        {
            if (texts_assignements.ContainsKey(text) == false)
            {
                AssignedText new_assigned_text = new AssignedText();
                new_assigned_text.assigned_clusters_list = new List<int>();
                new_assigned_text.assigned_clusters_list.Add(cluster_number);

                texts_assignements.Add(text, new_assigned_text);
            }
            else
            {
                AssignedText old_assigned_text = new AssignedText();
                texts_assignements.TryGetValue(text, out old_assigned_text);

                AssignedText new_assigned_text = new AssignedText();
                new_assigned_text.assigned_clusters_list = old_assigned_text.assigned_clusters_list;
                new_assigned_text.assigned_clusters_list.Add(cluster_number);

                texts_assignements.Remove(text);
                texts_assignements.Add(text, new_assigned_text);
            }        
        }


        public class CompareClusterGroup_Bydistance_ascending : IComparer<ClusterGroup>
        {
            public int Compare(ClusterGroup x, ClusterGroup y)
            {
                if (x.max_intra_cluster_distance == y.max_intra_cluster_distance) return (0);

                if (x.max_intra_cluster_distance < y.max_intra_cluster_distance)
                {
                    return (-11);
                }
                return (1);
            }
        }








        
        private static List<ClusterNew> calculate_clustersNew()
        {
            List<ClusterNew> result = new List<ClusterNew>();

            foreach (RelativeDistance distance in distances_list)
            {
                // Se nessuno dei due testi (numeri) di questa distance è già stato inserito in un cluster precedente: creane uno nuovo
                // Se entrambi i numeri sono già stati inseriti, ma in due clusters diversi: ????????? QUESTO E' QUELLO CHE COLLEGA I DUE CLUSTERS?? HMMMMMM
                //    ANDRA' BENE CREARE UN PADRE COI DUE CHILD ALLA LORO MAX DISTANZA?
                // Se uno dei due numeri è già stato inserito in un cluster: crea un nuovo cluster in cui quello precedente è un child, e a cui viene aggiunto il numero
                //     mancante (e la distanza intra_cluster viene ricalcolata includendo anche il numero mancante)
                // Se entrambi i numeri sono già stati inseriti nello stesso cluster: è un overlap, scarta questa distanza
                int cluster_counter = 0;
                bool discard_distance = false;
                int? number_1st_already_assigned_to = null;
                int? number_2nd_already_assigned_to = null;
                foreach (ClusterNew cluster in result)
                {
                    if (cluster.assigned_numbers.ContainsKey(distance.text_1st) == true && cluster.assigned_numbers.ContainsKey(distance.text_2nd) == true)
                    {
                        //Overlap: viene scartato
                        discard_distance = true;
                        break;
                    }

                    if (cluster.assigned_numbers.ContainsKey(distance.text_1st) == true)
                    {
                        number_1st_already_assigned_to = cluster_counter;
                    }
                    if (cluster.assigned_numbers.ContainsKey(distance.text_2nd) == true)
                    {
                        number_2nd_already_assigned_to = cluster_counter;                 
                    }
                    if (number_1st_already_assigned_to != null && number_2nd_already_assigned_to != null)
                    {
                        break;
                    }


                    cluster_counter++;
                }

                if (discard_distance == true) continue;




                ClusterNew new_cluster = new ClusterNew();
                if (number_1st_already_assigned_to == null && number_2nd_already_assigned_to == null)
                {
                    // Nuovo cluster
                    new_cluster.max_intra_cluster_distance = distance.distance;

                    new_cluster.numbers_list = new List<int>();
                    new_cluster.numbers_list.Add(distance.text_1st);
                    new_cluster.numbers_list.Add(distance.text_2nd);

                    new_cluster.assigned_numbers = new Dictionary<int, int>();
                    new_cluster.assigned_numbers.Add(distance.text_1st, distance.text_1st);
                    new_cluster.assigned_numbers.Add(distance.text_2nd, distance.text_2nd);

                    new_cluster.child_clusters = new List<ClusterNew>();

                    // Info aggiuntive
                    new_cluster.link_number_1st = distance.text_1st;
                    new_cluster.link_number_2nd = distance.text_2nd;
                    new_cluster.link_distance = distance.distance;

                    result.Add(new_cluster);
                }
                else
                {
                    if (number_1st_already_assigned_to != null && number_2nd_already_assigned_to != null)
                    {
                        // Creazione di un cluster che ha come childrens i due clusters collegati 
                        new_cluster.max_intra_cluster_distance = get_intra_cluster_distance(result[(int)number_1st_already_assigned_to], result[(int)number_2nd_already_assigned_to]);

                        new_cluster.numbers_list = new List<int>(); // Lista vuota in queso caso

                        new_cluster.assigned_numbers = new Dictionary<int, int>();
                        foreach (int old_cluster_number in result[(int)number_1st_already_assigned_to].assigned_numbers.Values)
                        {
                            new_cluster.assigned_numbers.Add(old_cluster_number, old_cluster_number);
                        }
                        foreach (int old_cluster_number in result[(int)number_2nd_already_assigned_to].assigned_numbers.Values)
                        {
                            new_cluster.assigned_numbers.Add(old_cluster_number, old_cluster_number);
                        }

                        new_cluster.child_clusters = new List<ClusterNew>();
                        new_cluster.child_clusters.Add(result[(int)number_1st_already_assigned_to]);  // Setta i due clusters come children
                        new_cluster.child_clusters.Add(result[(int)number_2nd_already_assigned_to]);

                        // Info aggiuntive
                        new_cluster.link_number_1st = distance.text_1st;
                        new_cluster.link_number_2nd = distance.text_2nd;
                        new_cluster.link_distance = distance.distance;


                        // Rimozione dei due clusters inseriti come children: attenzione perchè per primo va eliminato quello con l'indice più alto!
                        if (number_1st_already_assigned_to > number_2nd_already_assigned_to)
                        {
                            result.RemoveAt((int)number_1st_already_assigned_to);
                            result.RemoveAt((int)number_2nd_already_assigned_to);
                        }
                        else
                        {
                            result.RemoveAt((int)number_2nd_already_assigned_to);
                            result.RemoveAt((int)number_1st_already_assigned_to);
                        }

                        result.Add(new_cluster);
                    }
                    else
                    {
                        // Estensione del cluster precedente
                        int number_to_add = 0; // dummy
                        int cluster_to_add_as_child_index = 0; // dummy
                        if (number_1st_already_assigned_to != null)
                        {
                            number_to_add = distance.text_2nd;
                            cluster_to_add_as_child_index = (int)number_1st_already_assigned_to;
                        }
                        else
                        {
                            number_to_add = distance.text_1st;
                            cluster_to_add_as_child_index = (int)number_2nd_already_assigned_to;
                        }


                        new_cluster.max_intra_cluster_distance = get_intra_cluster_distance(number_to_add, result[cluster_to_add_as_child_index]);

                        new_cluster.numbers_list = new List<int>();
                        new_cluster.numbers_list.Add(number_to_add);       // Numero aggiunto  

                        new_cluster.assigned_numbers = new Dictionary<int, int>();
                        new_cluster.assigned_numbers.Add(number_to_add, number_to_add);
                        foreach (int old_cluster_number in result[cluster_to_add_as_child_index].assigned_numbers.Values)
                        {
                            new_cluster.assigned_numbers.Add(old_cluster_number, old_cluster_number);
                        }

                        new_cluster.child_clusters = new List<ClusterNew>();
                        new_cluster.child_clusters.Add(result[cluster_to_add_as_child_index]);  // Setta il cluster precedente come child
                        result.RemoveAt(cluster_to_add_as_child_index);                         //   e rimuovilo dalla lista dei risultati

                        // Info aggiuntive
                        new_cluster.link_number_1st = distance.text_1st;
                        new_cluster.link_number_2nd = distance.text_2nd;
                        new_cluster.link_distance = distance.distance;

                        result.Add(new_cluster);
                    }
                }

                // Usciamo quando tutti i numeri sono stati inseriti in un unico cluster (che avrà vari childrens, ovviamente)
                if (result.Count == 1 && result[0].assigned_numbers.Count == Form1.analysis_filenames.Count) break;

            }



            return result;
        }

        private static float get_intra_cluster_distance(ClusterNew cluster_to_add_as_child_1st, ClusterNew cluster_to_add_as_child_2nd)
        {
            float result = (float)Math.Max(cluster_to_add_as_child_1st.max_intra_cluster_distance, cluster_to_add_as_child_2nd.max_intra_cluster_distance);

            foreach (int old_cluster_1st_number in cluster_to_add_as_child_1st.assigned_numbers.Values)
            {
                foreach (int old_cluster_2nd_number in cluster_to_add_as_child_2nd.assigned_numbers.Values)
                {
                    if (distances_table[old_cluster_1st_number][old_cluster_2nd_number] > result)
                    {
                        result = distances_table[old_cluster_1st_number][old_cluster_2nd_number];
                    }
                }
            }
            return result;
        }
        private static float get_intra_cluster_distance(int number_to_add, ClusterNew cluster_to_add_as_child)
        {
            float result = cluster_to_add_as_child.max_intra_cluster_distance;

            foreach (int old_cluster_number in cluster_to_add_as_child.assigned_numbers.Values)
            {
                if (distances_table[number_to_add][old_cluster_number] > result)
                {
                    result = distances_table[number_to_add][old_cluster_number];
                }
            }
            return result;
        }










        private static void display_data()
        {
            //display_textual_data();
            display_textual_dataNew();

            display_graph_data();
        }

        private static void display_textual_data()
        {
            string message = "";

            string report = "Clustering report according to " + XComboBox.get_string((int)Form1.clustering_choices_status, VClusteringChoices.combo_clustering_choices_structure) + " \r\n";

            bool limits_are_2d = true;
            switch (Form1.clustering_choices_status)
            {
                case VClusteringChoices.combo_clustering_choices.bigrams:
                    break;
                case VClusteringChoices.combo_clustering_choices.monograms:
                    break;
                case VClusteringChoices.combo_clustering_choices.following_char:
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_char:
                    break;
                case VClusteringChoices.combo_clustering_choices.following_distance:
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_distance:
                    break;
                case VClusteringChoices.combo_clustering_choices.vocabulary:
                    limits_are_2d = false;
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_text:
                    limits_are_2d = false;
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_vocabulary:
                    limits_are_2d = false;
                    break;
                default:
                    report += "SOFTWARE ERROR: cannot find clustering_choices_status = " + Form1.clustering_choices_status + " in FormClustering.display_data";
                    break;
            }

            if (limits_are_2d == true)
            {
                if (Form1.clustering_choices_remove_rare_characters == true)
                {
                    report += "\tCalculated removing all characters appearing less than once every " + Form1.clustering_choices_rare_characters_cutoff + " characters";
                }
                else
                {
                    report += "\tCalculated considering all characters";
                }
            }
            else
            {
                report += "\tCalculated using the first " + Form1.clustering_choices_linear_limit + " records";
            }
            report += "\r\n\r\n";


            int layer_display_counter = 1;
            foreach (ClusteringResults layer in clustering_results)
            {
                message = "Layer " + layer_display_counter.ToString() + "...";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

                report += "Layer " + layer_display_counter.ToString() +", limit_distance = " + layer.limit_distance.ToString() + ", " + layer.clusters.Count
                            + " total clusters, " + layer.texts_assignements.Count + "/" + Form1.analysis_filenames.Count + " total assigned texts" + "\r\n\r\n";

                int group_display_counter = 1;
                foreach (ClusterGroup cluster_group in layer.clusters)
                {
                    report += "\tCluster " + group_display_counter.ToString() +", max intra-cluster distance = " + cluster_group.max_intra_cluster_distance.ToString() + "\r\n";

                    if (cluster_group.overlapping_clusters.Count != 0)
                    {
                        report += "\tOverlaps: ";
                        foreach (int overlap in cluster_group.overlapping_clusters)
                        {
                            report += (overlap + 1).ToString() + ", ";
                        }
                        report += "\r\n";
                    }


                    // Spostiamo il dictionaty dei numeri in una lista, per poterla ordinare
                    List<int> ordered_numbers = new List<int>();
                    foreach (int text in cluster_group.group_members.Values)
                    {
                        ordered_numbers.Add(text);                           ;
                    }
                    ordered_numbers.Sort();

                    foreach (int number in ordered_numbers)
                    {
                        report += "\t\t" + (number + 1).ToString() + ") " + Form1.analysis_filenames[number] + "\r\n";
                    }

                    group_display_counter++;
                    report += "\r\n\r\n";
                }

                layer_display_counter++;
                report += "\r\n\r\n";
            }

            message = "Writing data to screen...";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            mainFormClustering.Invoke((delegate_update_textBox_Clustering_Text)update_textBox_Clustering_Text, new object[] { report });
        }

        private static void update_textBox_Clustering_Text(string report)
        {
            mainFormClustering.textBox_Clustering.Text = report;
        }

        private static void display_textual_dataNew()
        {

            string report = "Clustering report according to " + XComboBox.get_string((int)Form1.clustering_choices_status, VClusteringChoices.combo_clustering_choices_structure) + " \r\n";

            bool limits_are_2d = true;
            switch (Form1.clustering_choices_status)
            {
                case VClusteringChoices.combo_clustering_choices.bigrams:
                    break;
                case VClusteringChoices.combo_clustering_choices.monograms:
                    break;
                case VClusteringChoices.combo_clustering_choices.following_char:
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_char:
                    break;
                case VClusteringChoices.combo_clustering_choices.following_distance:
                    break;
                case VClusteringChoices.combo_clustering_choices.previous_distance:
                    break;
                case VClusteringChoices.combo_clustering_choices.vocabulary:
                    limits_are_2d = false;
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_text:
                    limits_are_2d = false;
                    break;
                case VClusteringChoices.combo_clustering_choices.wordslength_vocabulary:
                    limits_are_2d = false;
                    break;
                default:
                    report += "SOFTWARE ERROR: cannot find clustering_choices_status = " + Form1.clustering_choices_status + " in FormClustering.display_data";
                    break;
            }

            if (limits_are_2d == true)
            {
                if (Form1.clustering_choices_remove_rare_characters == true)
                {
                    report += "\tCalculated removing all characters appearing less than once every " + Form1.clustering_choices_rare_characters_cutoff + " characters";
                }
                else
                {
                    report += "\tCalculated considering all characters";
                }
            }
            else
            {
                report += "\tCalculated using the first " + Form1.clustering_choices_linear_limit + " records";
            }
            report += "\r\n\r\n";


            report += "Texts list (" + Form1.analysis_filenames.Count + " texts):\r\n";
            int text_counter = 0;
            foreach (string text in Form1.analysis_filenames)
            {
                report += "\t" + (text_counter + 1) + ") " + text +"\r\n";
                text_counter++;
            }
            report += "\r\n";

            int indentation_level = 0;
            foreach (ClusterNew child_cluster in clustering_resultsNew)
            {
                display_textualclusterNew(child_cluster, ref indentation_level, ref report);
            }

            mainFormClustering.Invoke((delegate_update_textBox_Clustering_Text)update_textBox_Clustering_Text, new object[] { report });
        }

        private static void display_textualclusterNew(ClusterNew cluster, ref int indentation_level, ref string report)
        {
            string indentation = get_indentation(indentation_level);

            report += indentation + "Distance = " + cluster.max_intra_cluster_distance + "\r\n";

            report += indentation + "All included numbers: ";
            foreach (int number in cluster.assigned_numbers.Values)
            {
                report += (number + 1) + ", ";
            }
            report += "\r\n";

            report += indentation + "Single numbers: ";
            foreach (int number in cluster.numbers_list)
            {
                report += (number + 1) + ", ";
            }
            report += "\r\n";

            report += indentation + "Number of childrens: " + cluster.child_clusters.Count + "\r\n\r\n";

            indentation_level += 1;
            foreach (ClusterNew child_cluster in cluster.child_clusters)
            {
                display_textualclusterNew(child_cluster, ref indentation_level, ref report);
            }
            indentation_level -= 1;
        }

        private static string get_indentation(int indentation_level)
        {
            string result = "";
            for (int i = 0; i < indentation_level; i++)
            {
                result += "\t";
            }
            return result;
        }





        private static void display_graph_data()
        {
            graph = new XPlotClusters();

            if (mainFormClustering.checkBox_Clustering_optimize_graph_area.Checked == true)
            {
                graph.optimize_graph_area = true;
            }
            else
            {
                graph.optimize_graph_area = false;
            }

            if (Form1.analysis_filenames.Count > 200)  // Adattamento dei parametri del grafico al numero dei testi da visualizzare
            {
                if (graph.optimize_graph_area == true)
                {
                    graph.cluster_border_line_witdh = 1;
                    graph.cluster_elements_left_margin = 1;
                    graph.cluster_elements_upper_margin = 1;
                    graph.numbers_font_size = 7f;
                }
                else
                {
                    graph.cluster_border_line_witdh = 1;
                    graph.cluster_elements_left_margin = 2;
                    graph.cluster_elements_upper_margin = 2;
                }
            }
            else
            {
                if (Form1.analysis_filenames.Count > 100)
                {
                    graph.cluster_border_line_witdh = 2;
                    graph.cluster_elements_left_margin = 2;
                    graph.cluster_elements_upper_margin = 2;
                }
                // Else parametri restano a default!
            }




            graph.debug_show_duplicated_clusters = false;  // DA ELIMINARE!!! VERSIONE VECCHIA!!!

            // convert_clusters_to graph calcola anche max_distance e min_distance
            // ISTRUZIONE PER VESRIONE OLD!!!
            List<VisualCluster> graph_data = XPlotClusters.convert_clusters_to_graph(clustering_results, graph);

            Size graph_panel_size = new Size(mainFormClustering.splitContainer_Clustering_controls.Panel2.Width, mainFormClustering.splitContainer_Clustering_controls.Panel2.Height);
            //Bitmap image = XPlotClusters.DisplayGraph(graph_size, graph_data, graph);
            Bitmap image = XPlotClusters.DisplayGraphNew(graph_panel_size, clustering_resultsNew, graph);

            // Da quando ho inserito la picturebox scrollabile serve un Invoke (prima aggiornavo direttamente pictureBox.Image)
            mainFormClustering.Invoke((delegate_update_pictureBox_Clustering)update_pictureBox_Clustering, new object[] { image });

        }

        private static void update_pictureBox_Clustering(Bitmap image)
        {
            mainFormClustering.pictureBox_Clustering.Image = image;
        }








        private void pictureBox_Clustering_MouseMove(object sender, MouseEventArgs e)
        {
            mousehover_Timer.Stop();    // spegni subito il timer, altrimenti si corre il rischio che
                                        //  scada mentre stiamo processando l'evento mousehover, con
                                        //  conseguenti casini

            mouse_position.X = e.X;     // Coordinate del mouse rispetto alla _Image_ della pictureBox, questo ANCHE nel caso la pictureBox non venga visualizzata
            mouse_position.Y = e.Y;     //    completamente (per esempio se ha le scrollbars: le coordinate sono sempre riferite all'immagine _completa_).

            mousehover_Timer.Start(); // va fatto partire ad OGNI mousevent
        }
        private static void mousehover_Timer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (mainFormClustering.pictureBox_Clustering.InvokeRequired == true)
            {
                // se un eccezione viene alzata qua in realtà si è verificato qualche casino in ROUTINE_DI_VISUALIZZAZIONE,
                //   tipo oggetti non inizializzati o un loop infinito
                mainFormClustering.Invoke((delegate_display_mousehover)display_mousehover);
            }
            else
            {
                display_mousehover();
            }
        }

        private delegate void delegate_display_mousehover();
        private static void display_mousehover()
        {
            if (graph == null)
            {
                return;
            }

            Size image_size = new Size(mainFormClustering.pictureBox_Clustering_mousehover.Width, mainFormClustering.pictureBox_Clustering_mousehover.Height);
            Bitmap image = XPlotClusters.DisplayMouseCluster(image_size, mouse_position, graph);
            mainFormClustering.Invoke((delegate_update_pictureBox_Clustering_mousehover)update_pictureBox_Clustering_mousehover, new object[] { image });
        }

        private delegate void delegate_update_pictureBox_Clustering_mousehover(Bitmap image);
        private static void update_pictureBox_Clustering_mousehover(Bitmap image)
        {
            mainFormClustering.pictureBox_Clustering_mousehover.Image = image;
        }






        private void FormClustering_Resize(object sender, EventArgs e)
        {
            display_graph_data();
        }

        private void checkBox_Clustering_optimize_graph_area_CheckedChanged(object sender, EventArgs e)
        {
            display_graph_data();
        }

    }
}
