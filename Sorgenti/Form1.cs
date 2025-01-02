using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Template
{
    public partial class Form1 : Form
    {

        // Questo è un bel posto dove definire le 'costanti' 'globali' (dati custom)

        public static readonly bool _display_all = true;            // parametri per le routines di display, servono per risparmiare tempo di esecuzione
        public static readonly bool _display_graphs_only = false;

        public static readonly bool _zero_evidenced_graph = false;      // parametri per scegliere il tipo di grafico 2d da visualizzare
        public static readonly bool _extremes_evidenced_graph = true;

        public static int min_linear_graph_length = 10;        // usati per gli sliders che controllano la lunghezza dei grafici
        public static int max_linear_graph_length = 2000;
        public static int default_linear_graph_length = 100;
        public static int min_table_graph_length = 10;
        public static int max_table_graph_length = 100;


        public static int default_max_displayed_linear_data = 5000; // Usato per non scrivere textBoxes troppo lunghe (p.es. col vocabolario) e risparmiare tempo, c'è un'opzione
                                                                    //  di configurazione (display_whole_linear_data) per disabilitarlo e scrivere le textBoxes complete

        public static int max_text_chars_to_display = 300000;       // Evita di visualizzare source text e cleaned text se troppo lunghee risparmiare tempo, c'è un'opzione
                                                                    //  di configurazione (display_whole_texts) per disabilitarlo e scrivere i testi completi



        // Qua vengono definite le variabili 'globali'

        //  1) PARTE "STANDARD"

        public static Form1 mainForm;  // Variabile che torna utile in vari casi per accedere alla mainForm
                                       // (per esempio vedi newline_to_mainStatusWindow, in questo file)

        // Nome, versione etc. del programma per abbellimenti e 'marcatura' files
        public static Program_Identifier program_identifier;

        // Caption base della finestra principale del programma
        public String base_caption;

        // Definizione dei delegate types per le routines di serializzazione (save) e deserializzazione (load)
        // di una generica classe costruita usando ClasseDiEsempio come modello (usati per la gestione di mdFile)
        // E' indispensabile usare un object per evitare di dover scrivere esplicitamente il nome della classe,
        // cosa che impedirebbe di usare i delegates su una classe qualsiasi.
        // Nota: ovviamente usare un object come parametro impedisce al compilatore di verificare che si stia
        // passando il tipo corretto, quindi attenzione...
        public delegate mdError delegate_save(mdFile file, object source);
        public delegate mdError delegate_load(mdFile file, ref object target);

        // Classe e file di configurazione
        public static XConfig configuration_data;
        public static mdFile configuration_data_file;




        // 2) PARTE "CUSTOM"

        // 'Classe fondamentale' e 'File fondamentale' del programma: ogni programma con un minimo
        //   di complessità ha bisogno di almeno una classe e un file fondamentale.
        //   La 'classe fondamentale' contiene i dati del programma, che vengono salvati/caricati
        //   nel/dal 'file fondamentale'. Queste variabili sono definite qua perchè, in generale, ci si accederà
        //   da molti moduli
        public static TextAnalyzerClass text_analyzer;
        public static mdFile file_text_analyzer;

        private Thread load_and_analyzeThread;
        private Thread mass_analyzeThread;
        public static bool a_threadIsStarted = false;





        // Delegates per routines che vanno richiamate dall'interno di una thread
        public delegate void delegate_newline_to_mainStatuswindow(String message);
        public delegate void delegate_append_to_mainStatuswindow(String message);
        public delegate void delegate_set_mainForm_caption(String caprion);
        public delegate void delegate_display_data(bool display_all);
        public delegate void delegate_disable_controls_while_threads_are_running();
        public delegate void delegate_enable_controls_when_thread_stops();
        public delegate void delegate_update_graphs_limits_display();
        // Fine delegates per routines che vanno richiamate dall'interno di una thread


        public static int graphs_limit_1d;     // limita la lunghezza dei grafici per migliorare la visualizzazione escludendo caterve di dati inutili nelle code delle distribuzioni
        public static int graphs_limit_2d = 100;     // simile, ma per le tabelle (è anche utile per escludere i caratteri più rari dalla visualizzazione)

        // max_symmetric_value e min_symmetric_value servono come parametri per limitare i risultati nei grafici 2d in un range più consono alla visualizzaione
        public static float max_symmetric_value = 50f;
        public static float min_symmetric_value = -50f;




        // Gestione del 'Compare'
        public static List<string> analysis_filenames = new List<string>();
        public static List<TextAnalyzerClass> analysis_to_compare = new List<TextAnalyzerClass>();


        // Variabili usate per passare i parametri a FormClustering (non sono il massimo dell'eleganza, ma...)
        public static int clustering_choices_linear_limit;
        public static VClusteringChoices.combo_clustering_choices clustering_choices_status;
        public static bool clustering_choices_remove_rare_characters;
        public static int clustering_choices_rare_characters_cutoff;


        // Variabili specializzate per il parsing con looped slot grammars. I nomi sono prefissati da 'Voynich' sia per motivi storici che per trovarli facilmente
        public static int Voynich_current_max_loop_repeats = 1;

        public static ParseVoynich.ParsingResults Voynich_parsing_results = new ParseVoynich.ParsingResults();              // risultati dell'elaborazione
        public static PackGrammarAndWrite.ChunksGrammar Voynich_chunkified_grammar = new PackGrammarAndWrite.ChunksGrammar();   // risultati dell'elaborazione

        public static int Voynich_asemic_random_seed;

        private static string[] Voynich_grammars_combobox_list = new string[0];
        private static bool Voynich_show_chunks_categories;

        // ESEMPIO DI QUELLO CHE SERVE PER GESTIRE UNA ROUTINE DI VISUALIZZAZIONE GRAFICA (draw_map)
        //   E EVENTI MOUSEHOVER
        /*
        public delegate void delegate_draw_map();
        delegate_draw_map delegate_draw_map_routine;
        public System.Timers.Timer mousehover_Timer = new System.Timers.Timer();
        */
        // FINE ESEMPIO GRAFICA + MOUSEHOVER

        public Form1()
        {
            InitializeComponent();

            // 1) PARTE "STANDARD"

            // Settare una 'culture' è importante per evitare problemi coi files salvati (esempio: 15.125 in certe lingue ma 15,125 in altre)
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("it-IT");       // date in formato Europeo

            // Modificata rispetto al template ma non sarebbe servito... dopo ho introdotto un'opzione per settare il formato dei numeri
            //NumberFormatInfo nfti = CultureInfo.CreateSpecificCulture("en-US").NumberFormat;        //   ma numeri in formato americano con punto decimale
            NumberFormatInfo nfti = CultureInfo.CreateSpecificCulture("it-IT").NumberFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = nfti;

            mainForm = this;  // Variabile che torna utile in vari casi per accedere alla mainForm

            // program_identifier: contiene nome, versione e copyright del programma
            program_identifier = new Program_Identifier(this);

            // Stringa base della caption standard della finestra principale del programma (abbellimento)
            base_caption = program_identifier.name + " Vr. " + program_identifier.version + "      File: ";

            // Inizializzazione della finestra di status
            // NOTARE CHE PER ACCEDERE ALLA STATUS WINDOW E' SEMPRE MEGLIO USARE LE ROUTINES APPOSITE!
            //   (en passant qua c'è l'unico caso in cui ha senso avere la append_to prima della newline_to)
            append_to_mainStatusWindow("Welcome to " + program_identifier.name + " Vr. " + program_identifier.version);
            newline_to_mainStatusWindow("  " + program_identifier.copyright);


            // Gestione del file di configurazione: creazione della classe e associazione al suo mdFile
            configuration_data = new XConfig("");
            configuration_data_file = new mdFile(configuration_data.file_extension, configuration_data.name);
            // Notare che NON si può caricare il file di configurazione adesso. Qua la Form, in effetti,
            //  non è ancora stata creata e se si cerca di caricare un file si va in errore sulle istruzioni
            //  Form1.Invoke in mdFile. Invece il caricamento del file di configurazione è nell'handler
            //  dell'evento Shown, che si verifica la prima volta che la Form viene visualizzata





            // 2) PARTE "CUSTOM"


            // Al programma è associata almeno una 'classe fondamentale' contenente i suoi dati,
            //  a sua volta associata ad un 'file fondamentale' mdFile nel quale viene salvata (serializzata)

            // Costruzione della classe fondamentale del programma
            text_analyzer = new TextAnalyzerClass();

            // E adesso associamola al suo file fondamentale (in effetti non è che la classe sia
            //   'associata' rigidamente: le uniche cose che vengono passate sono l'estensione del file
            //   e l' "human_oriented_name" della classe, variabile che hanno la loro importanza
            //   ma niente di trascendentale)

            // Il constructor di mdFile lo crea con un nome di default (che è definito in mdFile e è 'no name')
            // Il nome effettivo del file verrà poi settato quando si esegue un save_as oppure un load
            file_text_analyzer = new mdFile(text_analyzer.file_extension, text_analyzer.name);


            // Aggiorniamo la caption della finestra base col nome del file
            this.Text = base_caption + file_text_analyzer.current_file_short_name;



            // Questo acrostico dovrebbe settare un timeout di 30 secondi su tutte operazioni Regex, e' importante che ci sia!!!!
            AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(30));

            initialize_controls();
            display_controls();

            display_data(_display_all);






            // ESEMPIO DI QUELLO CHE SERVE PER VISUALIZZAZIONE GRAFICA + MOUSEHOVER (da ArsMilitaris)
            /*
            // Timer per evitare di lanciare draw_map (che visualizza anche le interazioni utente)
            //   ogni volta che si verifica un evento mousehover.
            // Il delegate serve perchè l'handler del timer è una thread diversa da quella di Form1
            //   e non è possibile accedere agli oggetti di Form1 quando si lancia draw_map dall'handler del timer
            //   per farlo bisogna usare il delegate
            // Il timer è un monostabile che viene lanciato ad ogni evento mousehover, se arriva un altro mousehover
            //   prima che il timer scada il timer riparte e ritarda la draw_map finchè il punto di hovering è 'stabile'
            delegate_draw_map_routine = draw_map;
            mousehover_Timer.Interval = 5;    // tempo in ms
            mousehover_Timer.Enabled = true;
            mousehover_Timer.AutoReset = false;
            mousehover_Timer.Stop();
            mousehover_Timer.Elapsed += new ElapsedEventHandler(mousehover_Timer_Elapsed);
            */
            // FINE ESEMPIO GRAFICA + MOUSEHOVER




        }


        private void initialize_controls()
        {
            graphs_limit_1d = default_linear_graph_length;
            trackBar_linear_size.Minimum = min_linear_graph_length;
            trackBar_linear_size.Maximum = max_linear_graph_length;
            trackBar_linear_size.Value = graphs_limit_1d;

            graphs_limit_2d = max_table_graph_length;
            trackBar_table_size.Minimum = min_table_graph_length;
            trackBar_table_size.Maximum = max_table_graph_length;
            trackBar_table_size.Value = graphs_limit_2d;

            enforce_controls_coherency();

            set_splitter_to_middle_horizontal(splitContainer_cXc_graphs);
            set_splitter_to_middle_horizontal(splitContainer_distances_graphs);

            set_splitter_to_middle_vertical(splitContainer_single_chars_graphs);
            set_splitter_to_middle_vertical(splitContainer_syllables_graphs);
            set_splitter_to_middle_vertical(splitContainer_vocabulary_graphs);

            // Controlli per pagine parsing (Voynich)

            // ComboBox per la tab 'Slot grammar' (non segue la struttura standard di XControls, è formata solo da stringhe senza enum (è un caso diverso)
            Voynich_grammars_combobox_list = ParseGrammarsLibrary.get_Voynich_grammars_combobox_list();
            comboBox_select_Voynich_grammar.Text = Voynich_grammars_combobox_list[0];
            comboBox_select_Voynich_grammar.Items.Clear();
            comboBox_select_Voynich_grammar.Items.AddRange(Voynich_grammars_combobox_list);
            comboBox_select_Voynich_grammar.Refresh();

            // Qua va creata subito la grammatica, perchè contiene il valore di default (e quello massimo) della casella numerica col # di ripetizioni del LOOP
            ParseGrammarsLibrary.get_parsing_grammar(0, ref Voynich_parsing_results);
            Voynich_current_max_loop_repeats = Voynich_parsing_results.grammar.default_loop_repeats;

            Voynich_show_chunks_categories = false;
            checkBoxVoynich_show_chunk_categories.Checked = Voynich_show_chunks_categories;

            Voynich_asemic_random_seed = 0;
        }

        private void display_controls()
        {
            mainForm.textBox_linear_size.Text = graphs_limit_1d.ToString();
            mainForm.textBox_table_size.Text = graphs_limit_2d.ToString();

            // Slot grammars
            mainForm.textBox_Voynich_loop_repeats.Text = Voynich_current_max_loop_repeats.ToString();
            textBox_WrVoynich_random_seed.Text = Voynich_asemic_random_seed.ToString();
        }

        private static void enforce_controls_coherency()
        {
            if (mainForm.checkBox_bigrams_delta.Checked == true)
            {
                mainForm.checkBox_bigrams_symmetric.Enabled = true;
                mainForm.checkBox_bigrams_symmetric.Checked = true;
            }
            else
            {
                mainForm.checkBox_bigrams_symmetric.Enabled = false;
                mainForm.checkBox_bigrams_symmetric.Checked = false;
            }
        }

        private static void set_splitter_to_middle_horizontal(SplitContainer container)
        {
            container.SplitterDistance = container.Width / 2;
        }
        private static void set_splitter_to_middle_vertical(SplitContainer container)
        {
            container.SplitterDistance = container.Height / 2;
        }


        // Routines (di 'libreria' ) CHE E' SEMPRE MEGLIO USARE per accedere a mainStatusWindow
        public static void newline_to_mainStatusWindow(String text)
        {
            mainForm.textBox_mainStatusWindow.AppendText("\r\n " + DateTime.Now + " " + text);
        }

        public static void append_to_mainStatusWindow(String text)
        {
            mainForm.textBox_mainStatusWindow.AppendText(text);
        }
        public static void set_mainForm_caption(String caption)
        {
            mainForm.Text = caption;
        }


        // Handlers degli eventi
        private void Form1_Shown(object sender, EventArgs e)
        {
            get_configuration_data();
        }

        private void get_configuration_data()
        {

            String working_directory;
            String file_name;

            // Il file di configurazione è nella working directory e il suo nome è il nome del programma
            //    (senza versioni, in modo da poter caricare lo stesso file di configurazione anche con versioni
            //    successive del programma). L'estensione è già stata settata al momento delle creazione
            //    dell'mdFile (dove il suo valore viene preso da un membro della classe XConfig)

            working_directory = Directory.GetCurrentDirectory();
            file_name = program_identifier.name;

            configuration_data_file.set_current_file_path(working_directory, file_name);

            // Se il file di configurazione esiste, caricalo. Se non esiste creane uno di default e salvalo
            if (File.Exists(configuration_data_file.system_name) == true)
            {
                newline_to_mainStatusWindow("Loading configuration file " + configuration_data_file.system_name + "...");
                object objclass = configuration_data;  // Boxing
                if (configuration_data_file.direct_load(ref objclass, configuration_data.load) == false)
                {
                    // Lettura file di configurazione non andata a buon fine
                    newline_to_mainStatusWindow("Failed to load configuration file: '" + configuration_data_file.system_name + "'");
                }
                else
                {
                    // Istruzione indispensabile affinchè la classe venga effettivamente aggiornata !!
                    //  C'è un commento più esteso in loadToolStripMenu
                    configuration_data = (XConfig)objclass;
                }
            }
            else
            {
                // Se non esiste un file di configurazione lo creiamo, e la user_files_directory
                //   la defaultiamo alla working directory
                configuration_data.user_files_directory = working_directory;

                // PARTE CUSTOM SPECIFICA PER TEXTANALYZER !!!! : settiamo anche la directory dei files di testo
                configuration_data.user_text_files_directory = working_directory;

                object objclass = new object();
                objclass = configuration_data;                     // Boxing
                if (configuration_data_file.direct_save(objclass, configuration_data.save) == false)
                {
                    newline_to_mainStatusWindow("Failed to save default configuration file: '" + configuration_data_file.system_name + "'");
                }
            }

            // e a questo punto assegniamo la user_files_directory agli mdFiles

            //  ATTENZIONE: PURTROPPO BISOGNA RICORDARSI DI SETTARE LA DIRECTORY PER OGNI CLASSE CON mdFile
            //   CHE VIENE ISTANZIATA, LA COSA NON E' AUTOMATICA (NON ERA AFFATTO FACILE FARLO...)
            //   Un problema analogo c'è anche in Form1.Close(), ma riferito ai flags is_saved delle classi

            file_text_analyzer.current_directory = configuration_data.user_files_directory;

        }



        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // ** ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **

            object objclass = new object();
            objclass = text_analyzer;                     // Boxing
            if (file_text_analyzer.load(ref objclass, text_analyzer.load, configuration_data.user_files_directory) == true)
            { 
                // Assegnazione dell'oggetto appena caricato alla nostra classe, dopo Unboxing
                //  E' l'unica cosa che mdFile.load() non può fare, dato che serve il nome esplicito della classe
                //     mentre mdFile lavora su classi (objects) generiche

                // ATTENZIONE PERCHE' SE CI SI DIMENTICA QUESTA ISTRUZIONE SEMBRA CHE FUNZIONI TUTTO...
                //    MA LA CLASSE NON VIENE AGGIORNATA !!!!!
                text_analyzer = (TextAnalyzerClass)objclass; 

            } // un else non serve, se il load fallisce mdFile.load() sistema già tutto per conto suo

            // ** FINE ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **

            // Se è stata modificata la directory dei files utente aggiorna il file .cfg
            if (file_text_analyzer.current_directory != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = file_text_analyzer.current_directory;
                configuration_data.save(configuration_data_file, configuration_data);
            }



            // Aggiorniamo la caption (anche in caso di load fallito, non che cambi molto in ogni caso)
            this.Text = base_caption + file_text_analyzer.current_file_short_name;

            // Warning in status window: tempi di attesa possono essere lunghi se certe opzioni sono settate
            if (configuration_data.display_whole_texts == true || configuration_data.display_whole_linear_data == true)
            {
                newline_to_mainStatusWindow("WARNING: due to the Visualization Options used, the data visualization can take a LONG time, even SEVERAL MINUTES, with a big analysis file. Be patient and have faith");
            }

            // IN GENERALE QUA ANDRA' RICHIAMATA UNA ROUTINE DI VISUALIZZAZIONE

            // Resetta i graphs_limits
            set_graphs_limits_defaults();
            // Aggiornamento delle due textBoxes dei graphs_limits
            update_graphs_limits_display();

            display_controls();
            display_data(_display_all);

        }



        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // PARTE CUSTOM
            check_save_analysis_file_options();  // Attenzione: elimina anche il testo dal file che viene salvato, IMPORTANTE PER EVITARE PROBLEMI DI COPYRIGHT!!

            // Finestra che chiede di confermare le informazioni relative al file
            VSaveAnalysisDialog info_dialog = new VSaveAnalysisDialog();
            if (info_dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            // FINE CUSTOM

            object objclass = new object();
            objclass = text_analyzer;                     // Boxing
            file_text_analyzer.save_as(objclass, text_analyzer.save, configuration_data.user_files_directory);

            // Se è stata modificata la directory dei files utente aggiorna il file .cfg
            if (file_text_analyzer.current_directory != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = file_text_analyzer.current_directory;
                configuration_data.save(configuration_data_file, configuration_data);
            }

            // Aggiorniamo la caption (anche in caso di save_as fallito, non che cambi molto in ogni caso)
            this.Text = base_caption + file_text_analyzer.current_file_short_name;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // PARTE CUSTOM
            check_save_analysis_file_options(); // Attenzione: elimina anche il testo dal files che viene salvato, IMPORTANTE PER EVITARE PROBLEMI DI COPYRIGHT!!

            // Finestra che chiede di confermare le informazioni relative al file
            VSaveAnalysisDialog info_dialog = new VSaveAnalysisDialog();
            if (info_dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            // FINE CUSTOM

            object objclass = new object();
            objclass = text_analyzer;                     // Boxing
            file_text_analyzer.save(objclass, text_analyzer.save, configuration_data.user_files_directory);

            // Se è stata modificata la directory dei files utente aggiorna il file .cfg
            if (file_text_analyzer.current_directory != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = file_text_analyzer.current_directory;
                configuration_data.save(configuration_data_file, configuration_data);
            }

            this.Text = base_caption + file_text_analyzer.current_file_short_name; // Safety, qua caption non cambia

        }

        private void check_save_analysis_file_options()
        {
            if (configuration_data.save_also_source_text == false)
            {
                text_analyzer.loaded_text = "Original source text (including user remarks/commands, if any) discarded to save space in the saved file. Check menu Options->Save files to enable saving the source text too";
                text_analyzer.raw_source_text = "Original source text discarded to ensure there are no copyright violations and to save space in the saved file. Check menu Options->Save files to enable saving the source text too";
            }
            if (configuration_data.save_also_cleaned_text == false)
            {
                text_analyzer.cleaned_text = "Cleaned text discarded to ensure there are no copyright violations and to save space in the saved file. Check menu Options->Save files to enable saving the cleaned text too";
            }
        }



        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // mdFile.new_file() gestisce solo i dialoghi
            if (file_text_analyzer.new_file() == true)
            {
                // Ricreiamo la classe 'vuota'
                text_analyzer = new TextAnalyzerClass();
            }

            this.Text = base_caption + file_text_analyzer.current_file_short_name;

            // IN GENERALE QUA ANDRA' RICHIAMATA UNA ROUTINE DI VISUALIZZAZIONE
            display_controls();
            display_data(_display_all);
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Notare che l'evento FormClosing viene trappato qua sotto, per gestire 'file non salvato'
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // NOTARE il test sul flag is_saved quando la Form viene chiusa per un qualsiasi motivo

            //  ATTENZIONE: PURTROPPO BISOGNA RICORDARSI DI TESTARE IL FLAG PER OGNI CLASSE CON mdFile
            //   CHE VIENE ISTANZIATA, LA COSA NON E' AUTOMATICA (NON ERA AFFATTO FACILE FARLO...)
            //   C'è un problema analogo (ma relativo alle directories degli mdFiles) in get_configuration_data()
        

            if (text_analyzer.is_saved == false)
            {
                string message = "File '" + file_text_analyzer.current_file_short_name + "' has been modified, exit anyway?";
                string caption = "File not saved";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true; // questo annulla la chiusura della Form !
                }
            }
        }



        // ESEMPIO DI CREAZIONE DI UNA FORM FIGLIA
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WAboutForm form = new WAboutForm();
            form.Show();
        }

        private void button_LoadTexts_Click(object sender, EventArgs e)
        {
            load_and_analyze();
        }


        private void load_and_analyze()
        {
            // get il nome del/dei file/s
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Load UTF-8 text files(s)";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_text_files_directory;
            openFileDialog1.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;

            // Tutta la routine è stata scritta per supportare la gestione multifiles, cosa che è stata molto utile nello sviluppo e debug.
            //  Adesso però non ha più senso e causa confusione (vedi vari punti nel readme), quindi limito l'analisi ad un file solo.
            //      ATTENZIONE: c'è solo un punto in cui rimettere il Multifile a true causa un 'difetto' nella gestione: la settatura della
            //      variabili file_charset_size e file_max_word_length (in text_analyzer_main, cercare ATTENZIONE SETTATURA FILE_CHARSET_SIZE ETC.),
            //      che è fatto solo sull'ultimo file caricato (se c'è solo un file non ci sono problemi)
            //openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                // notare che original_class non viene modificata
                return;
            }


            display_controls();

            text_analyzer = new TextAnalyzerClass();
            display_data(_display_all);                 // E' più bello se viene visualizzata la classe 'vuota' prima del load

            // Salva la lista delle opzioni nella classe primaria
            text_analyzer.used_parameters.discard_all_arabic_numbers = configuration_data.discard_all_arabic_numbers;
            text_analyzer.used_parameters.keep_distinction_between_upper_and_lowercase = configuration_data.keep_distinction_between_upper_and_lowercase;
            text_analyzer.used_parameters.apostrophe_is_a_separator = configuration_data.apostrophe_is_a_separator;
            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_discarded = configuration_data.words_linked_by_an_apostrophe_are_discarded;
            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_joined = configuration_data.words_linked_by_an_apostrophe_are_joined;
            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_separated = configuration_data.words_linked_by_an_apostrophe_are_separated;
            text_analyzer.used_parameters.words_linked_by_a_dash_are_discarded = configuration_data.words_linked_by_a_dash_are_discarded;
            text_analyzer.used_parameters.words_linked_by_a_dash_are_joined = configuration_data.words_linked_by_a_dash_are_joined;
            text_analyzer.used_parameters.words_linked_by_a_dash_are_separated = configuration_data.words_linked_by_a_dash_are_separated;


            // Assegna alla classe primaria il nome del primo file caricato
            file_text_analyzer.current_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
            file_text_analyzer.current_file_short_name = Path.GetFileNameWithoutExtension(openFileDialog1.FileNames[0]);
            if (openFileDialog1.FileNames.Length > 1)
            {
                file_text_analyzer.current_file_short_name += " (and " + (openFileDialog1.FileNames.Length - 1) + " more files)";
            }
            this.Text = base_caption + file_text_analyzer.current_file_short_name;

            text_analyzer.book_title = file_text_analyzer.current_file_short_name;  // Presetta anche il titolo

            //   Salva la lista dei files da caricare nella classe primaria
            for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
            {
                LoadedFileRecord new_file = new LoadedFileRecord();
                new_file.file_name = openFileDialog1.FileNames[i];
                text_analyzer.loaded_files_list.Add(new_file);
            }



            // E aggiorna la posizione della directory dei files di testo
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_text_files_directory)
            {
                configuration_data.user_text_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }


            // Caricamento del(i) file(s) sorgente(i)
            int file_counter = 0;
            foreach (LoadedFileRecord file in text_analyzer.loaded_files_list)
            {

                Form1.newline_to_mainStatusWindow("Loading file " + file.file_name + "...");

                try
                {
                    // Come prima cosa cerco di caricare il file di testo con codifica UTF8, ma se invece è nella vecchia codifica UTF7 quetso risulta
                    //    nella conversione errata di alcuni caratteri (p.es. quelli accentati), che diventano '�'. Se questo accade lo ricarico come UTF7
                    System.IO.StreamReader source_stream = new System.IO.StreamReader(file.file_name, System.Text.Encoding.UTF8);
                    string text = source_stream.ReadToEnd();
                    source_stream.Close();
                    int caratteri_balordi = Regex.Matches(text, "�").Count;
                    if (caratteri_balordi != 0)
                    {
                        Form1.append_to_mainStatusWindow(" probably UTF-7 encoded, trying opening it as such...");
                        source_stream = new System.IO.StreamReader(file.file_name, System.Text.Encoding.UTF7);
                        text = source_stream.ReadToEnd();
                        source_stream.Close();
                    }
                    // Controllo di sicurezza sul fatto che la stringa non contenga caratteri balordi, che fanno supporre un problema di encoding del file, o un problema nel file stesso
                    //    Il test viene fatto su un po' dei caratteri balordi che ho trovato in vari files, ma non è detto che trappi proprio tutto, casomai
                    //      aggiungere caratteri man mano che se ne trovano
                    caratteri_balordi = Regex.Matches(text, "\\u001A | [\\u007F-\\u009F]").Count;
                    if (caratteri_balordi != 0)
                    {
                        mdError error = new mdError();
                        error.root("Input data warning", "File " + file.file_name + " contains abnormal characters (for instance control characters from U+007F to U+009F) which suggest this files uses a character encoding " +
                                    "which cannot be read by TextAnalyzer, or the use of abnormal characters in the text itself. Both will probably cause the extraction of " +
                                    "'wrong' words from the text and the degradation of the quality of the vocabulary. Encoding problems can be detected because the text will appear " +
                                    "abnormal in TextAnalyzer, but normal in Notepad or Word. A workaround in this case is to open the .txt file with Notepad and then 'save as' choosing " +
                                    "UFT8 or UFT8 with BOM as the encoding. Or, you can select and copy all the text, then paste it into a newly-created .txt file. If the problem is in " +
                                    "the source file itself (you can see the strange characters even in Notepad): you can try to automatically replace the bad characters by using a " +
                                    "preprocessing Regex command, or you can manually correct the text file. If the offending characters are invisible: copy part of the text in a tool " +
                                    "such as https://regex101.com to identify them.");
                        error.Display_and_Clear();
                    }


                    text_analyzer.loaded_text += text;
                    text_analyzer.loaded_text += "\r\n"; // aggiungi un separatore fra i files (altrimenti ci si potrebbe trovare l'ultima parola di un file concatenata con la prima del successivo

                    // Metodo bruttino per accedere alla lista dei files... (che oltretutto compare anche nell'intestazione del foreach... yiikes...)
                    //    ma funziona ed è semplice...
                    text_analyzer.loaded_files_list[file_counter].file_length = text.Length;
                    file_counter++;

                }
                catch
                {
                    text_analyzer = new TextAnalyzerClass();
                    newline_to_mainStatusWindow("Cannot open text source file " + file + ", or file is too big to be loaded");
                    return;
                }
            }

            

            Form1.newline_to_mainStatusWindow("Pre-processing loaded file...");
            Form1.newline_to_mainStatusWindow("Using options:");
            Form1.newline_to_mainStatusWindow("\tDiscard all arabic numerals: " + text_analyzer.used_parameters.discard_all_arabic_numbers);
            Form1.newline_to_mainStatusWindow("\tApostrophe is a separator: " + text_analyzer.used_parameters.apostrophe_is_a_separator);
            if (text_analyzer.used_parameters.apostrophe_is_a_separator == true)
            {
                if (text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_discarded == true)
                {
                    Form1.append_to_mainStatusWindow(", words including an apostrophe are discarded");
                }
                if (text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_joined == true)
                {
                    Form1.append_to_mainStatusWindow(", words including an apostrophe are joined together");
                }
                if (text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_separated == true)
                {
                    Form1.append_to_mainStatusWindow(", words including an apostrophe are splitted");
                }

            }
            if (text_analyzer.used_parameters.words_linked_by_a_dash_are_discarded == true)
            {
                Form1.newline_to_mainStatusWindow("\tWords including a dash are discarded");
            }
            if (text_analyzer.used_parameters.words_linked_by_a_dash_are_joined == true)
            {
                Form1.newline_to_mainStatusWindow("\tWords including a dash are joined together");
            }
            if (text_analyzer.used_parameters.words_linked_by_a_dash_are_separated == true)
            {
                Form1.newline_to_mainStatusWindow("\tWords including a dash are splitted");
            }
            Form1.newline_to_mainStatusWindow("\tKeep the distinction between upper and lowercase: " + text_analyzer.used_parameters.keep_distinction_between_upper_and_lowercase);


            disable_controls_while_threads_are_running();

            // Ecco lo strano acrostico che passa dei parametri ad una Thread!
            load_and_analyzeThread = new Thread(new ThreadStart(() => text_analyzer_main(true)));
            load_and_analyzeThread.Start();

        }

        private static void disable_controls_while_threads_are_running()
        {
            a_threadIsStarted = true;

            mainForm.button_LoadTexts.Enabled = false;
            mainForm.menuStrip1.Enabled = false;
            
            mainForm.button_Voynich_run_parse.Enabled = false;
            mainForm.button_Voynich_run_WSET_trivial_grammar.Enabled = false;
            mainForm.button_WrVoynich_write_asemic.Enabled = false;
        }

        // L'enable dei controlli è richiamato alla fine della thread, quindi deve essere static e con le variabili referenziate tramite mainForm (e con delegate xD)
        private static void enable_controls_when_thread_stops()
        {
            mainForm.button_Voynich_run_parse.Enabled = true;
            mainForm.button_Voynich_run_WSET_trivial_grammar.Enabled = true;
            mainForm.button_WrVoynich_write_asemic.Enabled = true;

            mainForm.button_LoadTexts.Enabled = true;
            mainForm.menuStrip1.Enabled = true;

            a_threadIsStarted = false;
        }

        private void text_analyzer_main(bool verbose)
        {
            string message = "";

            // Preprocessing

            // Eliminazione dei commenti ed estrazione dei comandi Regex
            PreprocessText.preprocess_remarks(text_analyzer.loaded_text, ref text_analyzer.raw_source_text, ref text_analyzer.residual_remarks, ref text_analyzer.used_parameters.Regex_commands);

            // Pre-processing delle opzioni
            string text_after_pre_cleaning = "";
            PreprocessText.preprocess_options(text_analyzer.raw_source_text, ref text_after_pre_cleaning, text_analyzer.used_parameters, ref text_analyzer.analysis_results, verbose);

            // Pre-processing: lancio dei comandi Regex definiti dall'utente e cleaning finale del file (e rimozione parole con dash e/o apostrofo, se le opzioni sono abilitate)
            PreprocessText.preprocess_Regex(text_after_pre_cleaning, ref text_analyzer.cleaned_text, text_analyzer.used_parameters.Regex_commands, text_analyzer.used_parameters, verbose);

            message = "Preprocessing completed";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });



            // Analysis

            message = "Starting analysis: processing characters statistics...";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            string charset = "";
            TextCharsStats.get_monograms_stats(text_analyzer.cleaned_text, charset, ref text_analyzer.analysis_results);
            TextCharsStats.get_bigrams_stats(text_analyzer.cleaned_text, charset, ref text_analyzer.analysis_results);
            TextCharsStats.get_bigrams_cXc_table(ref text_analyzer.analysis_results);
            message = "Text uses " + text_analyzer.analysis_results.monograms_distribution.Count + " different characters (including space and, possibly, apostrophe)";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


            message = "Processing vocabulary statistics...";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            VocabularyStats.get_vocabulary(text_analyzer.cleaned_text, ref text_analyzer.analysis_results);
            message =  "Text contains "  + text_analyzer.analysis_results.total_literal_characters_in_cleaned_text + " literal characters and " + text_analyzer.analysis_results.total_spaces_in_cleaned_text + " spaces.";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            message = "Found " + text_analyzer.analysis_results.vocabulary_words_distribution.Count + " different words, maximum word length = " + text_analyzer.analysis_results.words_length_distribution_in_vocabulary.Count;
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


            message = "Extracting pseudo-syllables...";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            SyllablesStats.get_syllables(ref text_analyzer.analysis_results);
            if (text_analyzer.analysis_results.words_with_apostrophe_not_syllabified != 0)
            {
                message = text_analyzer.analysis_results.words_with_apostrophe_not_syllabified.ToString() + " words included an apostrophe and have been discarded from syllabification";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }
            message = "Found " + text_analyzer.analysis_results.syllables_distribution_single_vowels.Count + " different pseudo-syllables when every vowel is a single nucleus, or " + text_analyzer.analysis_results.syllables_distribution_multiple_vowels.Count + " when adiacent vowels are merged in a single nucleus";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


            message = "Analysis complete";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });



            // Visualizzazioni
            if (configuration_data.display_whole_linear_data == true || configuration_data.display_whole_texts == true)
            {
                message = "Formatting data for display. WARNING: may take some time with long input files, do not worry and let the program finish. See Options -> Visualization to drastically cut the time needed.";
            }
            else
            {
                message = "Formatting data for display...";
            }
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            // Resetta i graphs_limits
            set_graphs_limits_defaults();
            // Aggiornamento delle due textBoxes dei graphs_limits via Invoke
            Form1.mainForm.Invoke((Form1.delegate_update_graphs_limits_display)Form1.update_graphs_limits_display, new object[] { });

            // ATTENZIONE SETTATURA FILE_CHARSET_SIZE ETC.: qua vengono settati i valori solo per l'ULTIMO file della lista, la cosa non dà problemi se il file è unico
            //      (vedi commento all'apertura dell'OpenFileDialog in testa a load_and_analyze)
            if (text_analyzer.loaded_files_list.Count != 0)
            {
                text_analyzer.loaded_files_list[text_analyzer.loaded_files_list.Count - 1].file_charset_size = text_analyzer.analysis_results.monograms_distribution.Count;
                text_analyzer.loaded_files_list[text_analyzer.loaded_files_list.Count - 1].file_max_words_length = text_analyzer.analysis_results.words_length_distribution_in_vocabulary.Count;
                text_analyzer.loaded_files_list[text_analyzer.loaded_files_list.Count - 1].file_total_words = text_analyzer.analysis_results.total_number_of_words_in_the_text;
            }


            if (verbose) Form1.mainForm.Invoke((Form1.delegate_display_data)Form1.display_data, new object[] { _display_all });

            message = "Processing completed\r\n";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


            // Riabilitazione dei controlli
            Form1.mainForm.Invoke((Form1.delegate_enable_controls_when_thread_stops)Form1.enable_controls_when_thread_stops, new object[] { });
        }

        private static void set_graphs_limits_defaults()
        {
            // Il limite grafici 1d viene defaultato
            graphs_limit_1d = default_linear_graph_length;

            // Il limite dei grafici 2d viene settato al minimo necessario per visualizzare l'intero set dei caratteri
            //   il test evita un Exception se il # di caratteri è fuori dal range della trackBar di table_limits
            if (text_analyzer.analysis_results.monograms_distribution.Count >= min_table_graph_length && text_analyzer.analysis_results.monograms_distribution.Count <= max_table_graph_length)
            {
                graphs_limit_2d = text_analyzer.analysis_results.monograms_distribution.Count;
            }
            else
            {
                if (text_analyzer.analysis_results.monograms_distribution.Count < min_table_graph_length) graphs_limit_2d = min_table_graph_length;
                if (text_analyzer.analysis_results.monograms_distribution.Count > max_table_graph_length) graphs_limit_2d = max_table_graph_length;
            }
        }

        private static void update_graphs_limits_display()
        {
            // megacasino se non si disabilitano gli eventi, perchè altrimenti viene richiamato l'evento TextChanged sulle textBoxes
            mainForm.textBox_linear_size.TextChanged -= new System.EventHandler(mainForm.textBox_linear_size_TextChanged);
            mainForm.textBox_linear_size.Text = graphs_limit_1d.ToString();
            mainForm.textBox_linear_size.TextChanged += new System.EventHandler(mainForm.textBox_linear_size_TextChanged);
            mainForm.trackBar_linear_size.ValueChanged -= new System.EventHandler(mainForm.trackBar_linear_size_ValueChanged);
            mainForm.trackBar_linear_size.Value = graphs_limit_1d;
            mainForm.trackBar_linear_size.ValueChanged += new System.EventHandler(mainForm.trackBar_linear_size_ValueChanged);

            mainForm.textBox_table_size.TextChanged -= new System.EventHandler(mainForm.textBox_table_size_TextChanged);
            mainForm.textBox_table_size.Text = graphs_limit_2d.ToString();
            mainForm.textBox_table_size.TextChanged += new System.EventHandler(mainForm.textBox_table_size_TextChanged);
            mainForm.trackBar_table_size.ValueChanged -= new System.EventHandler(mainForm.trackBar_table_size_ValueChanged);
            mainForm.trackBar_table_size.Value = graphs_limit_2d;
            mainForm.trackBar_table_size.ValueChanged += new System.EventHandler(mainForm.trackBar_table_size_ValueChanged);
        }


        public static void display_data(bool display_all)
        {

            // Opzione formato dei numeri (per esportazione in Excel!)
            NumberFormatInfo nfti = new NumberFormatInfo();
            if (configuration_data.display_numbers_in_american_format == true)
            {
                nfti = CultureInfo.CreateSpecificCulture("en-US").NumberFormat;  // numeri in formato americano con punto decimale
            }
            else
            {
                nfti = CultureInfo.CreateSpecificCulture("it-IT").NumberFormat;  // numeri in formato europeo
            }
            Thread.CurrentThread.CurrentCulture.NumberFormat = nfti;

            // coerenza delle caselle di scelta
            enforce_controls_coherency();


            display_textual_tabs(display_all);             // Tabs solo testuali: report, cleaned text, preprocessing, source text

            display_monograms_data(display_all);

            display_bigrams_theoric_data(display_all);

            display_bigrams_data(display_all);

            display_cXc_data(display_all);

            display_distances_data(display_all);

            display_vocabulary_data(display_all);

            display_syllables_data(display_all);
        }




        private static void display_textual_tabs(bool display_all)
        {
            if (display_all == false) return;  // in questa routine non ci sono grafici

            // Tab Reports
            string report = program_identifier.name + " " + program_identifier.version + " Analysis report\r\n\r\n";
            report += "Total characters analyzed in cleaned text:\t" + text_analyzer.analysis_results.total_characters_in_cleaned_text + "\t";
            report += "Total literal characters:\t" + text_analyzer.analysis_results.total_literal_characters_in_cleaned_text + "\t";
            report += "Total space characters:\t" + text_analyzer.analysis_results.total_spaces_in_cleaned_text + "\r\n";
            report += "\r\n";
            report += "Character set size (including space character):\t" + text_analyzer.analysis_results.monograms_distribution.Count + "\t";
            report += "Character set:\t";
            foreach (EValueOcc carattere in text_analyzer.analysis_results.monograms_distribution)
            {
                report += carattere.element;
            }
            report += "\r\n";
            report += "\r\n";
            report += "Total number of words:\t" + text_analyzer.analysis_results.total_number_of_words_in_the_text + "\r\n";
            report += "Total words in dictionary:\t" + text_analyzer.analysis_results.vocabulary_words_distribution.Count;
            if (text_analyzer.analysis_results.vocabulary_words_distribution.Count != 0)
            {
                report += " (one every " + ((float)text_analyzer.analysis_results.total_number_of_words_in_the_text / (float)text_analyzer.analysis_results.vocabulary_words_distribution.Count) + " text words)";
            }
            report += "\r\n";
            report += "Total hapax legomena:\t" + text_analyzer.analysis_results.total_hapax_legomena;
            if (text_analyzer.analysis_results.total_hapax_legomena != 0)
            {
                report += " (one every " + ((float)text_analyzer.analysis_results.vocabulary_words_distribution.Count / (float)text_analyzer.analysis_results.total_hapax_legomena) + " dictionary words)";
            }
            report += "\r\n";
            report += "Total pseudo-syllables (single vowels):\t" + text_analyzer.analysis_results.syllables_distribution_single_vowels.Count + "\r\n";
            report += "Total pseudo-syllables (vowels clusters):\t" + text_analyzer.analysis_results.syllables_distribution_multiple_vowels.Count + "\r\n";
            report += "\r\n";

            // La formula per le entropie è sempre la stessa: -SUM(Pi*ln2(Pi))
            float entropy = 0;
            foreach (EValueOcc value in text_analyzer.analysis_results.monograms_distribution)
            {
                float probability = (float)value.value / (float)text_analyzer.analysis_results.total_characters_in_cleaned_text;
                entropy -= (float)probability * (float)(Math.Log(probability) / Math.Log(2));
            }
            report += "1st-order entropy (single chars including space):\t" + entropy.ToString() + " bit/character\r\n";

            entropy = 0;
            foreach (EValueOcc value in text_analyzer.analysis_results.monograms_distribution_excluding_spaces)
            {
                float probability = (float)value.value / (float)text_analyzer.analysis_results.total_literal_characters_in_cleaned_text;
                entropy -= (float)probability * (float)(Math.Log(probability) / Math.Log(2));
            }
            report += "1st-order entropy (single chars excluding space):\t" + entropy.ToString() + " bit/character\r\n";

            entropy = 0;
            foreach (EValueOcc value in text_analyzer.analysis_results.bigrams_distribution)
            {
                // Qua servirebbe il numero dei bigrammi, non dei caratteri, ma per testi lunghi la differenza è minima
                float probability = (float)value.value / (float)text_analyzer.analysis_results.total_characters_in_cleaned_text;
                if (probability != 0) // qua PUO' essere zero (bigrammi proibiti)
                {
                    entropy -= (float)probability * (float)(Math.Log(probability) / Math.Log(2));
                }
            }
            report += "2nd-order entropy (bigrams):\t" + entropy.ToString() + " bit/bigram\r\n";

            entropy = 0;
            foreach (EValueOcc value in text_analyzer.analysis_results.vocabulary_words_distribution)
            {
                float probability = (float)value.value / (float)text_analyzer.analysis_results.total_number_of_words_in_the_text;
                entropy -= (float)probability * (float)(Math.Log(probability) / Math.Log(2));
            }
            report += "Words entropy:\t" + entropy.ToString() + " bit/word\r\n";


            report += "\r\nFiles used in the analysis (" + text_analyzer.loaded_files_list.Count + " files). Length is the raw length in characters (if .txt) or the cleaned length (if .txalysis)";
            report += "\r\n\tName\tLength\tCharacter set size\tWords\tMaximum word length\r\n";
            foreach (LoadedFileRecord file in text_analyzer.loaded_files_list)
            {
                report += "\t" + Path.GetFileName(file.file_name) + "\t" + file.file_length + "\t" + file.file_charset_size + "\t" + file.file_total_words + "\t" + file.file_max_words_length + "\r\n";
            }




            mainForm.textBox_report.Text = report;


            // Tabs Source Text e Cleaned text
            if (text_analyzer.raw_source_text != null)
            {
                if (configuration_data.display_whole_texts == true || text_analyzer.raw_source_text.Length <= max_text_chars_to_display)
                {
                    mainForm.textBox_raw_source_text.Text = text_analyzer.raw_source_text;
                }
                else
                {
                    mainForm.textBox_raw_source_text.Text = "Visualization suppressed to save time, text is longer than " + max_text_chars_to_display + " characters. Check Options->Visualization to display it whole.";
                }
            }
            if (text_analyzer.cleaned_text != null)
            {
                if (configuration_data.display_whole_texts == true || text_analyzer.cleaned_text.Length <= max_text_chars_to_display)
                {
                    mainForm.textBox_source_text.Text = text_analyzer.cleaned_text;
                }
                else
                {
                    mainForm.textBox_source_text.Text = "Visualization suppressed to save time, text is longer than " + max_text_chars_to_display + " characters Check Options->Visualization to display it whole.";
                }
            }


            // Tab preprocessing
            mainForm.textBox_remarks.Text = text_analyzer.residual_remarks;
            mainForm.textBox_Regex_commands.Text = "";
            foreach (RegexCommand command in text_analyzer.used_parameters.Regex_commands)
            {
                mainForm.textBox_Regex_commands.Text += "\r\nSearch '" + command.search_string + "' and replace with '" + command.replacement_string + "'\t(user remark: " + command.user_remark + ")";
            }
        }

        private static void display_monograms_data(bool display_all)
        {
            string title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.monograms_distribution.Count)
            {
                title_addendum_1d = " of the first " + graphs_limit_1d + " characters";
            }
            if (display_all == true) display_1d_data("Single characters frequencies (spaces included)", EVconvert(text_analyzer.analysis_results.monograms_distribution), mainForm.textBox_single_characters_distribution);
            display_1d_graphs("Single characters frequencies (spaces included)" + title_addendum_1d, EVconvert(text_analyzer.analysis_results.monograms_distribution), mainForm.pictureBox_single_characters_distribution_graph);

            if (display_all == true) display_1d_data("Single characters frequencies (without spaces)", EVconvert(text_analyzer.analysis_results.monograms_distribution_excluding_spaces), mainForm.textBox_single_characters_distribution_nospaces);
            display_1d_graphs("Single characters frequencies (without spaces)" + title_addendum_1d, EVconvert(text_analyzer.analysis_results.monograms_distribution_excluding_spaces), mainForm.pictureBox_single_characters_distribution_graph_no_spaces);
        }


        private static void display_bigrams_theoric_data(bool display_all)
        {
            string title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.bigrams_theoretical_distribution.Count)
            {
                title_addendum_1d = " of the first " + graphs_limit_1d + " bigrams";
            }
            string title_addendum_2d = "";
            if (graphs_limit_2d < text_analyzer.analysis_results.monograms_distribution.Count)
            {
                title_addendum_2d = " of the first " + graphs_limit_2d + " characters";
            }
            if (mainForm.checkBox_bigrams_theoric_displaytable.Checked == true)
            {
                if (display_all == true) display_2d_data("Bigrams theoretical frequencies table (The first bigram character is the rows heading. Reading by rows: how often the row character is followed by the column character? Reading by columns: how often the column character is _preceded_ by the row character? In this table the two values are always equal)",
                                             text_analyzer.analysis_results.bigrams_theoretical_distribution_table, mainForm.textBox_bigrams_theoric);
                display_2d_graphs("Bigrams theoretical frequencies" + title_addendum_2d, _zero_evidenced_graph, text_analyzer.analysis_results.bigrams_theoretical_distribution_table, mainForm.pictureBox_bigrams_theoric);
            }
            else
            {
                if (display_all == true) display_1d_data("Bigrams theoretical frequencies", text_analyzer.analysis_results.bigrams_theoretical_distribution, mainForm.textBox_bigrams_theoric);
                display_1d_graphs("Bigrams theoretical frequencies" + title_addendum_1d, text_analyzer.analysis_results.bigrams_theoretical_distribution, mainForm.pictureBox_bigrams_theoric);
            }
        }


        private static void display_bigrams_data(bool display_all)
        {
            string title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.bigrams_distribution.Count)
            {
                title_addendum_1d = " of the first " + graphs_limit_1d + " bigrams";
            }
            string title_addendum_2d = "";
            if (graphs_limit_2d < text_analyzer.analysis_results.monograms_distribution.Count)
            {
                title_addendum_2d = " of the first " + graphs_limit_2d + " characters";
            }
            if (mainForm.checkBox_bigrams_displaytable.Checked == true)
            {
                if (mainForm.checkBox_bigrams_delta.Checked == false)
                {
                    if (display_all == true) display_2d_data("Bigrams frequencies table (The first bigram character is the rows heading. Reading by rows: how often the row character is followed by the column character? Reading by columns: how often the column character is _preceded_ by the row character? In this table the two values are generally different)",
                                                                EVconvert(text_analyzer.analysis_results.bigrams_distribution_table), mainForm.textBox_bigrams);
                    display_2d_graphs("Bigrams frequencies" + title_addendum_2d, _zero_evidenced_graph, EVconvert(text_analyzer.analysis_results.bigrams_distribution_table), mainForm.pictureBox_bigrams);
                }
                else
                {
                    if (mainForm.checkBox_bigrams_symmetric.Checked == true)
                    {
                        string title = "Bigrams frequencies vs. theoric frequencies table (The first bigram character is the rows heading. Reading by rows: how often the row character is followed by the column character? Reading by columns: how often the column character is _preceded_ by the row character? In this table the two values are generally different) (data are unsorted)";
                        title = add_symmetrization_info(title);
                        if (display_all == true) display_2d_data(title, Symmetrize(text_analyzer.analysis_results.bigrams_distribution_delta_table), mainForm.textBox_bigrams);
                        display_2d_graphs(add_symmetrization_info("Bigrams frequencies vs. theoric frequencies" + title_addendum_2d), _extremes_evidenced_graph, Symmetrize(text_analyzer.analysis_results.bigrams_distribution_delta_table), mainForm.pictureBox_bigrams);
                    }
                    else
                    {
                        string title = "Bigrams frequencies vs. theoric frequencies table (The first bigram character is the rows heading. Reading by rows: how often the row character is followed by the column character? Reading by columns: how often the column character is _preceded_ by the row character? In this table the two values are generally different) (data are unsorted)";
                        title += " (raw data)";
                        if (display_all == true) display_2d_data(title, text_analyzer.analysis_results.bigrams_distribution_delta_table, mainForm.textBox_bigrams);
                        display_2d_graphs("Bigrams frequencies vs. theoric frequencies (raw data)" + title_addendum_2d, _zero_evidenced_graph, text_analyzer.analysis_results.bigrams_distribution_delta_table, mainForm.pictureBox_bigrams);
                    }
                }
            }
            else
            {
                if (mainForm.checkBox_bigrams_delta.Checked == false)
                {
                    if (display_all == true) display_1d_data("Bigrams frequencies", EVconvert(text_analyzer.analysis_results.bigrams_distribution), mainForm.textBox_bigrams);
                    display_1d_graphs("Bigrams frequencies" + title_addendum_1d, EVconvert(text_analyzer.analysis_results.bigrams_distribution), mainForm.pictureBox_bigrams);
                }
                else
                {
                    if (mainForm.checkBox_bigrams_symmetric.Checked == true)
                    {
                        display_1d_data(add_symmetrization_info("Bigrams frequencies vs. theoric frequencies"), Symmetrize(text_analyzer.analysis_results.bigrams_distribution_delta), mainForm.textBox_bigrams);
                        display_1d_graphs(add_symmetrization_info("Bigrams frequencies  vs. theoric frequencies") + title_addendum_1d, Symmetrize(text_analyzer.analysis_results.bigrams_distribution_delta), mainForm.pictureBox_bigrams);
                    }
                    else
                    {
                        display_1d_data("Bigrams frequencies vs. theoric frequencies (raw data)", text_analyzer.analysis_results.bigrams_distribution_delta, mainForm.textBox_bigrams);
                        display_1d_graphs("Bigrams frequencies  vs. theoric frequencies (raw data)" + title_addendum_1d, text_analyzer.analysis_results.bigrams_distribution_delta, mainForm.pictureBox_bigrams);
                    }

                }
            }
        }
        


        private static void display_cXc_data(bool display_all)
        {
            string title_addendum_2d = "";
            if (graphs_limit_2d < text_analyzer.analysis_results.monograms_distribution.Count)
            {
                title_addendum_2d = " (first " + graphs_limit_2d + " characters)";
            }
            if (display_all == true) display_2d_data("Reading by rows: probability of character in row 'r' to be FOLLOWED by the character in column 'c'",
                                                         text_analyzer.analysis_results.following_character_distribution, mainForm.textBox_Xc_table);
            display_2d_graphs("Probability of character in row 'r' to be FOLLOWED by the character in column 'c" + title_addendum_2d, _zero_evidenced_graph, text_analyzer.analysis_results.following_character_distribution, mainForm.pictureBox_Xc);

            if (display_all == true) display_2d_data("Reading by rows: probability of character in row 'r' to be PRECEDED by the character in column 'c'",
                                                        text_analyzer.analysis_results.previous_character_distribution, mainForm.textBox_cX_table);
            display_2d_graphs("Probability of character in row 'r' to be PRECEDED by the character in column 'c" + title_addendum_2d, _zero_evidenced_graph, text_analyzer.analysis_results.previous_character_distribution, mainForm.pictureBox_cX);
        }

        private static void display_distances_data(bool display_all)
        {
            string title_addendum = "";
            if (graphs_limit_2d < text_analyzer.analysis_results.monograms_distribution.Count)
            {
                title_addendum = " (first " + graphs_limit_2d + " characters)";
            }
            if (display_all == true) display_2d_data("Geometric distances between characters according to their FOLLOWING character",
                                                         text_analyzer.analysis_results.monograms_distances_according_to_following_character, mainForm.textBox_chars_distances_Xc);
            display_2d_graphs("Geometric distances between characters according to their FOLLOWING character" + title_addendum, _zero_evidenced_graph, text_analyzer.analysis_results.monograms_distances_according_to_following_character, mainForm.pictureBox_distances_Xc);

            if (display_all == true) display_2d_data("Geometric distances between characters according to their PREVIOUS character",
                                                        text_analyzer.analysis_results.monograms_distances_according_to_previous_character, mainForm.textBox_chars_distances_cX);
            display_2d_graphs("Geometric distances between characters according to their PREVIOUS character" + title_addendum, _zero_evidenced_graph, text_analyzer.analysis_results.monograms_distances_according_to_previous_character, mainForm.pictureBox_distances_cX);
        }

        private static void display_vocabulary_data(bool display_all)
        {
            string title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.vocabulary_words_distribution.Count)
            {
                title_addendum_1d = " of the first " + graphs_limit_1d + " words";
            }
            string title = "Vocabulary |word|frequency in the text|word length|, sorted by frequency\r\n";
            title += "Total words in source text:\t" + text_analyzer.analysis_results.total_number_of_words_in_the_text + "\r\n";
            title += "Total words in dictionary:\t" + text_analyzer.analysis_results.vocabulary_words_distribution.Count + "\r\n";
            title += "Total hapax legomena:\t" + text_analyzer.analysis_results.total_hapax_legomena;
            if (display_all == true)
            {
                display_1d_data(title, EVconvert(text_analyzer.analysis_results.vocabulary_words_distribution), mainForm.textBox_vocabulary);
            }
            title = "Frequencies";
            display_1d_graphs(title + title_addendum_1d, EVconvert(text_analyzer.analysis_results.vocabulary_words_distribution), mainForm.pictureBox_vocabulary_distribution);

            title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.words_length_distribution_in_text.Count)
            {
                title_addendum_1d = " of the words up to " + graphs_limit_1d + " characters long";
            }
            if (mainForm.checkBox_words_length_distribution_in_text.Checked == true)
            {
                title = "words length distribution (in the whole text) |length|frequency|";
                if (display_all == true) display_1d_data(title, EV_extendedconvert(text_analyzer.analysis_results.words_length_distribution_in_text), mainForm.textBox_words_length_distribution);
                title = "Words length distribution (in the whole text)";
                display_1d_graphs(title + title_addendum_1d, EV_extendedconvert(text_analyzer.analysis_results.words_length_distribution_in_text), mainForm.pictureBox_words_length_distribution);
            }
            else
            {
                title = "Words length distribution (in vocabulary) |length|frequency|";
                if (display_all == true) display_1d_data(title, EV_extendedconvert(text_analyzer.analysis_results.words_length_distribution_in_vocabulary), mainForm.textBox_words_length_distribution);
                title = "Words length distribution (in vocabulary)";
                display_1d_graphs(title + title_addendum_1d, EV_extendedconvert(text_analyzer.analysis_results.words_length_distribution_in_vocabulary), mainForm.pictureBox_words_length_distribution);
            }
        }



        private static void display_syllables_data(bool display_all)
        {
            string title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.syllables_distribution_single_vowels.Count)
            {
                title_addendum_1d = " for the first " + graphs_limit_1d + " pseudo-syllables";
            }
            string title = "Vocabulary pseudo-syllables (each vowel is a different nucleus) |syllable|frequency|\r\n";
            title += "Total number of pseudo-syllables found in vocabolary:" + text_analyzer.analysis_results.syllables_distribution_single_vowels.Count;
            if (display_all == true) display_1d_data(title, EVconvert(text_analyzer.analysis_results.syllables_distribution_single_vowels), mainForm.textBox_syllables_single_vowel);
            title = "Frequencies of pseudo-syllables in vocabulary (each vowel is a different nucleus)";
            display_1d_graphs(title + title_addendum_1d, EVconvert(text_analyzer.analysis_results.syllables_distribution_single_vowels), mainForm.pictureBox_syllables_single_vowel);

            title_addendum_1d = "";
            if (graphs_limit_1d < text_analyzer.analysis_results.syllables_distribution_multiple_vowels.Count)
            {
                title_addendum_1d = " for the first " + graphs_limit_1d + " pseudo-syllables";
            }
            title = "Vocabulary pseudo-syllables (consecutive vowels form a single nucleus) |syllable|frequency|\r\n";
            title += "Total number of pseudo-syllables found in vocabolary: " + text_analyzer.analysis_results.syllables_distribution_multiple_vowels.Count;
            if (display_all == true) display_1d_data(title, EVconvert(text_analyzer.analysis_results.syllables_distribution_multiple_vowels), mainForm.textBox_syllables_multiple_vowels);
            title = "Frequencies of pseudo-syllables in vocabulary (consecutive vowels form a single nucleus)";
            display_1d_graphs(title + title_addendum_1d, EVconvert(text_analyzer.analysis_results.syllables_distribution_multiple_vowels), mainForm.pictureBox_syllables_multiple_vowels);
        }





        // Convertitore (overloaded, per dati 1d, 1d multipli, o 2d o 2d multipli) da liste di EValueOcc (occorrenze) a liste di EValue (frequenze)
        //   ATTENZIONE: l'overload permette di distinguere i dati 1d (a 1 dimensione) da quelli 2d (a due dimensioni) da quelli 2df multipli (a 3 dimensioni)
        //               MA NON PERMETTE di distinguere i dati 2d multipli (che hanno due dimensioni, come i dati 2d). Per questo motivo il convertitore
        //                 per i dati 1d multipli ha un nome separato. FARE ATTENZIONE
        public static List<EValue> EVconvert(List<EValueOcc> input) // Dati 1D
        {
            // Calcolo numero totale delle occorrenze
            long total_occurrences = 0;
            foreach (EValueOcc item in input)
            {
                total_occurrences += item.value;
            }

            List<EValue> converted_data = new List<EValue>();
            foreach (EValueOcc item in input)
            {
                EValue new_value = new EValue();
                new_value.element = item.element;
                new_value.value = (float)((double)item.value / (double)total_occurrences);
                converted_data.Add(new_value);
            }
            return converted_data;
        }

        public static List<List<EValue>> EVconvert_1d_multipli(List<List<EValueOcc>> input) // Dati 1D multipli
        {
            // Qua basta richiamare più volte la routine di conversione dei dati 1d, dato che il calcolo delle occorrenze
            //          va effettuato TABELLA PER TABELLA
            List<List<EValue>> converted_data = new List<List<EValue>>();
            foreach (List<EValueOcc> linear_data in input)
            {
                converted_data.Add(EVconvert(linear_data));
            }
            return converted_data;
        }
        public static List<List<EValue>> EVconvert(List<List<EValueOcc>> input) // Dati 2D
        {
            // Calcolo numero totale delle occorrenze SULL'INTERA TABELLA!!
            //    Qua NON si può semplicemente richiamare l'overload precedente, dato che calcolerebbe i totali riga per riga!!
            long total_occurrences = 0;
            foreach (List<EValueOcc> row in input)
            {
                foreach (EValueOcc item in row)
                {
                    total_occurrences += item.value;
                }
            }

            List<List<EValue>> converted_data = new List<List<EValue>>();
            foreach (List<EValueOcc> row in input)
            {
                List<EValue> new_value_list = new List<EValue>();
                foreach (EValueOcc item in row)
                {
                    EValue new_value = new EValue();
                    new_value.element = item.element;
                    new_value.value = (float)((double)item.value / (double)total_occurrences);
                    new_value_list.Add(new_value);
                }
                converted_data.Add(new_value_list);
            }
            return converted_data;
        }

        public static List<List<List<EValue>>> EVconvert(List<List<List<EValueOcc>>> input) // Dati 2D multipli
        {
            // Qua basta richiamare più volte la routine di conversione dei dati 2d, dato che il calcolo delle occorrenze
            //          va effettuato TABELLA PER TABELLA
            List<List<List<EValue>>> converted_data = new List<List<List<EValue>>>();
            foreach (List<List<EValueOcc>> table_data in input)
            {
                converted_data.Add(EVconvert(table_data));
            }
            return converted_data;
        }



        // Varianti per conversione degli _extended

        public static List<EValue> EVconvert(List<EValue_extended> input) 
        {
            List<EValue> converted_list = new List<EValue>();
            foreach (EValue_extended value in input)
            {
                EValue new_value = new EValue();
                new_value.element = value.element; // additional_element viene scartato
                new_value.value = value.value;
                converted_list.Add(new_value);
            }
            return converted_list;
        }

        public static List<List<EValue>> EVconvert_1d_multipli(List<List<EValueOcc_extended>> input) // Dati 1D multipli
        {
            // Qua basta richiamare più volte la routine di conversione dei dati 1d, dato che il calcolo delle occorrenze
            //          va effettuato TABELLA PER TABELLA
            List<List<EValue>> converted_data = new List<List<EValue>>();
            foreach (List<EValueOcc_extended> linear_data in input)
            {
                converted_data.Add(EVconvert(linear_data));
            }
            return converted_data;
        }

        public static List<EValue> EVconvert(List<EValueOcc_extended> input) // Dati 1D
        {
            // Calcolo numero totale delle occorrenze
            long total_occurrences = 0;
            foreach (EValueOcc_extended item in input)
            {
                total_occurrences += item.value;
            }

            List<EValue> converted_data = new List<EValue>();
            foreach (EValueOcc_extended item in input)
            {
                EValue new_value = new EValue();
                new_value.element = item.element;
                new_value.value = (float)((double)item.value / (double)total_occurrences);
                converted_data.Add(new_value);
            }
            return converted_data;
        }
        public static List<EValue_extended> EV_extendedconvert(List<EValueOcc_extended> input) // Dati 1D
        {
            // Calcolo numero totale delle occorrenze
            long total_occurrences = 0;
            foreach (EValueOcc_extended item in input)
            {
                total_occurrences += item.value;
            }

            List<EValue_extended> converted_data = new List<EValue_extended>();
            foreach (EValueOcc_extended item in input)
            {
                EValue_extended new_value = new EValue_extended();
                new_value.element = item.element;
                new_value.element_additional = item.element_additional;
                new_value.value = (float)((double)item.value / (double)total_occurrences);
                converted_data.Add(new_value);
            }
            return converted_data;
        }







        // Routines (overloaded, per dati 1d o 2d o 2d multipli) che applicano la 'simmetrizzazione' ai dati
        public static List<EValue> Symmetrize(List<EValue> input)
        {
            List<EValue> converted_data = new List<EValue>();
            foreach (EValue item in input)
            {
                EValue new_value = new EValue();
                new_value.element = item.element;
                new_value.value = calculate_symmetric_value(item.value, max_symmetric_value, min_symmetric_value);
                converted_data.Add(new_value);
            }
            return converted_data;
        }

        public static List<List<EValue>> Symmetrize(List<List<EValue>> input)
        {
            List<List<EValue>> converted_data = new List<List<EValue>>();
            foreach (List<EValue> row in input)
            {
                // Qua si sarebbe potuto semplicemente richiamare l'overload precedente di Symmetrize
                List<EValue> new_value_list = new List<EValue>();
                foreach (EValue item in row)
                {
                    EValue new_value = new EValue();
                    new_value.element = item.element;
                    new_value.value = calculate_symmetric_value(item.value, max_symmetric_value, min_symmetric_value);
                    new_value_list.Add(new_value);
                }
                converted_data.Add(new_value_list);
            }
            return converted_data;
        }

        public static List<List<List<EValue>>> Symmetrize(List<List<List<EValue>>> input)
        {
            // Qua basta richiamare più volte l'overload precedente di Symmetrize
            List<List<List<EValue>>> converted_data = new List<List<List<EValue>>>();
            foreach (List<List<EValue>> table_data in input)
            {
                converted_data.Add(Symmetrize(table_data));
            }
            return converted_data;
        }


        private static void display_1d_data(string title, List<EValue> linear_data, TextBox text_box)
        {
            string out_text = title + "\r\n";
            int values_to_display = linear_data.Count;
            if (configuration_data.display_whole_linear_data == false && linear_data.Count > default_max_displayed_linear_data)
            {
                values_to_display = default_max_displayed_linear_data;
                out_text += "Only the first " + values_to_display + " records are shown, check Options->Visualization to show all records\r\n";
            }
            foreach (EValue scratch_EValue in linear_data.Take<EValue>(values_to_display)) 
            {
                // Intestazione
                // E' un casino perchè ci sono vari casi:
                //   - Un solo carattere (single characters distributions), con due casi: se è uno spazio va sostituito da SPACE, altrimenti resta com'è
                //   - Due caratteri (tabelle bigrammi di vario tipo), con quattro casi:
                //          "  ": va sostituito da SPACE-SPACE
                //          " c": va sostituito da SPACE-c
                //          "c ": va sostituito da c-SPACE
                //          "cc": resta com'è
                //   - Più di due caratteri (vocabolario e sillabe): resta com'è
                if (scratch_EValue.element.Length == 1)
                {
                    if (scratch_EValue.element == " ")
                    {
                        out_text += "SPACE\t";
                    }
                    else
                    {
                        out_text += scratch_EValue.element + "\t";
                    }
                }
                else
                {
                    if (scratch_EValue.element.Length == 2)
                    {
                        if (scratch_EValue.element == "  ")
                        {
                            out_text += "SPACE-SPACE\t";
                        }
                        else
                        {
                            if (scratch_EValue.element[0] == ' ')
                            {
                                out_text += "SPACE-" + scratch_EValue.element[1] + "\t";
                            }
                            else
                            {
                                if (scratch_EValue.element[1] == ' ')
                                {
                                    out_text += scratch_EValue.element[0] + "-SPACE\t";
                                }
                                else
                                {
                                    out_text += scratch_EValue.element + "\t";
                                }
                            }
                        }
                    }
                    else
                    {
                        out_text += scratch_EValue.element + "\t";
                    }
                }

                // Dati
                out_text += scratch_EValue.value + "\r\n";

            }

            text_box.Text = out_text;
        }


        private static void display_1d_data(string title, List<EValue_extended> linear_data, TextBox text_box)
        {
            // Questo overload di display_1d_data visualizza sia l'element che l'additional_element degli EValue_extended
            string out_text = title + "\r\n";
            int values_to_display = linear_data.Count;
            if (configuration_data.display_whole_linear_data == false && linear_data.Count > default_max_displayed_linear_data)
            {
                values_to_display = default_max_displayed_linear_data;
                out_text += "Only the first " + values_to_display + " records are shown, check Options->Visualization to show all records\r\n";
            }
            foreach (EValue_extended scratch_EValue in linear_data.Take<EValue_extended>(values_to_display))
            {
                // Intestazione
                // E' un casino perchè ci sono vari casi:
                //   - Un solo carattere (single characters distributions), con due casi: se è uno spazio va sostituito da SPACE, altrimenti resta com'è
                //   - Due caratteri (tabelle bigrammi di vario tipo), con quattro casi:
                //          "  ": va sostituito da SPACE-SPACE
                //          " c": va sostituito da SPACE-c
                //          "c ": va sostituito da c-SPACE
                //          "cc": resta com'è
                //   - Più di due caratteri (vocabolario e sillabe): resta com'è
                if (scratch_EValue.element.Length == 1)
                {
                    if (scratch_EValue.element == " ")
                    {
                        out_text += "SPACE\t";
                    }
                    else
                    {
                        out_text += scratch_EValue.element + "\t";
                    }
                }
                else
                {
                    if (scratch_EValue.element.Length == 2)
                    {
                        if (scratch_EValue.element == "  ")
                        {
                            out_text += "SPACE-SPACE\t";
                        }
                        else
                        {
                            if (scratch_EValue.element[0] == ' ')
                            {
                                out_text += "SPACE-" + scratch_EValue.element[1] + "\t";
                            }
                            else
                            {
                                if (scratch_EValue.element[1] == ' ')
                                {
                                    out_text += scratch_EValue.element[0] + "-SPACE\t";
                                }
                                else
                                {
                                    out_text += scratch_EValue.element + "\t";
                                }
                            }
                        }
                    }
                    else
                    {
                        out_text += scratch_EValue.element + "\t";
                    }
                }

                // Dati
                out_text += scratch_EValue.value + "\t" + scratch_EValue.element_additional + "\r\n";

            }

            text_box.Text = out_text;
        }


        // L'opzione is_symmetric fa passare i dati dalla funzione if( x<1 then x=-1/x), che porta i valori minori di uno in un range comparabile
        //   a quello dei maggiori di uno, ma negativi. Il tutto facilita l'applicazione delle colorazioni alla tabella (anche in Excel, per quanto con Excel non si riesca
        //   a separare chiaramente i valori positivi dai negativi)
        private static void display_2d_data(string title, List<List<EValue>> table_data, TextBox text_box)
        {
            string out_text = title;

            out_text += "\r\n";

            // riga di intestazione (intestazioni colonne)
            if (table_data.Count <= 1)
            {
                out_text += "\r\nError in display_2d_data, no data found\r\n";
                return;
            }
            out_text += "\t";
            foreach (EValue scratch_EValue in table_data[1])
            {
                if (scratch_EValue.element[1] == ' ')
                {
                    out_text += "SPACE\t";
                }
                else
                {
                    out_text += scratch_EValue.element[1] + "\t";
                }

            }
            out_text += "\r\n";

            // Dati
            foreach (List<EValue> scratch_list in table_data)
            {
                // intestazione riga
                if (scratch_list[0].element[0] == ' ')
                {
                    out_text += "SPACE\t";
                }
                else
                {
                    out_text += scratch_list[0].element[0] + "\t";
                }

                // dati
                foreach (EValue scratch_EValue in scratch_list)
                {
                    out_text += scratch_EValue.value + "\t";
                }
                out_text += "\r\n";
            }
            text_box.Text = out_text;

        }


        public static float calculate_symmetric_value(float input, float maxvalue, float minvalue)
        {
            float out_value = 0;  
            if (input >= 1)
            {
                out_value = input;
            }
            else
            {
                if ( input == 0)
                {
                    out_value = float.MinValue;
                }
                else
                {
                    out_value = -1 * (1 / input);
                }
            }            

            if (out_value > maxvalue) out_value = maxvalue;
            if (out_value < minvalue) out_value = minvalue;

            return out_value;
        }


        public static string add_symmetrization_info(string input)
        {
            string out_string = input;
            out_string += " (values symmetrized and limited to " + min_symmetric_value + " +" + max_symmetric_value + " )";
            return out_string;
        }





        private static void display_1d_graphs(string title, List<EValue> linear_data, PictureBox picture_box)
        {
            XPlotGraphs1d graph = new XPlotGraphs1d();
            graph.title = title;
            graph.y_units = "";
            graph.x_units = "";
            graph.histogram = true;
            graph.join_points = true;
            List<string> x_labels = new List<string>();
            List<float> graphs_data = new List<float>();
            foreach (EValue item in linear_data.Take<EValue>(graphs_limit_1d))
            {
                x_labels.Add(item.element);
                graphs_data.Add(item.value);
            }
            Size graph_size = new Size(picture_box.Width, picture_box.Height);
;
            Bitmap image = XPlotGraphs1d.DisplayGraph(graph_size, graphs_data, x_labels, graph);
            picture_box.Image = image;
        }
        private static void display_1d_graphs(string title, List<EValue_extended> linear_data, PictureBox picture_box)
        {
            // Overload di display_1d_graphs che gestisce gli EValue_extended: nel grafico di uscita non cambia niente (l'additional_element
            //    dell'EValue_extended non supera la EVconvert e non viene visualizzato
            display_1d_graphs(title, EVconvert(linear_data), picture_box);
        }




        private static void display_2d_graphs(string title, bool graph_type, List<List<EValue>> table_data, PictureBox picture_box)
        {

            XPlotGraphs2d graph = new XPlotGraphs2d();
            graph.title = title;

            if (graph_type == true)
            {
                graph.put_extremes_in_evidence = true;
                graph.high_extreme = max_symmetric_value;
                graph.low_extreme = min_symmetric_value;
            }
            else
            {
                graph.put_zero_in_evidence = true;
            }

            List<string> x_labels = new List<string>();
            List<List<float>> graphs_data = new List<List<float>>();
            foreach (List<EValue> row in table_data.Take<List<EValue>>(graphs_limit_2d))
            {
                List<float> scratch_list = new List<float>();
                foreach (EValue cell in row.Take<EValue>(graphs_limit_2d))
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
                    //  riga/colonna delle tabelle "a", "b", "c"). Ergo ho dovuto correggere. C'è una cosa analoga in FormCompare.display_2d_graphs
                    if (row[0].element.Length < 1) x_labels.Add("ERROR1");
                    else
                    {
                        x_labels.Add("" + row[0].element[0]);
                    }
                }
            }
            Size graph_size = new Size(picture_box.Width, picture_box.Height);
            Bitmap image = XPlotGraphs2d.DisplayGraph(graph_size, graphs_data, x_labels, graph);

            picture_box.Image = image;

        }








        private void compareAnalysisFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Se non c'è già un'analisi caricata avvisa che ne serve una per poter fare la comparazione
            if (text_analyzer.analysis_results.total_characters_in_cleaned_text == 0)
            {
                mdError warning = new mdError();
                warning.root("No data to make a comparison", "Load an analysis file (or analyze text files) to get the base data for the comparison");
                warning.Display_and_Clear();
                return;
            }


            // get il nome del/dei file/s di analisi da aprire
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Load analysis files";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_files_directory;
            openFileDialog1.Filter = file_text_analyzer.target_class_human_oriented_name + " file (*." + file_text_analyzer.extension + ")|*." + file_text_analyzer.extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }



            int max_compare_file_before_warning = 100;
            if(openFileDialog1.FileNames.Length > max_compare_file_before_warning)
            {
                VWarningCompare form_warning = new VWarningCompare();
                if (form_warning.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }



            // Classe che contiene tutte le analisi da comparare: verrà poi usata da FormCompare
            analysis_to_compare = new List<TextAnalyzerClass>();

            // Inserisci la classe attuale come primo elemento della comparazione
            analysis_to_compare.Add(text_analyzer);

            // e aggiorna la posizione della directory dei files utente
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }

            // Classe che contiene i nomi di tutte le analisi da comparare: verrà poi usata da FormCompare
            analysis_filenames = new List<string>();
            // Inserisci la classe attuale come primo elemento
            analysis_filenames.Add(Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name));
            foreach (var filename in openFileDialog1.FileNames)
            {
                // Se è lo stesso file dell'analisi base non inserirlo (verrà scartato anche nel seguito dell'elaborazione)
                if (Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name) == Path.GetFileNameWithoutExtension(filename))
                {
                    continue;
                }
                analysis_filenames.Add(Path.GetFileNameWithoutExtension(filename));
            }


            // Caricamento dei files da comparare
            foreach (var filename in openFileDialog1.FileNames)
            {
                if (Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name) == Path.GetFileNameWithoutExtension(filename))
                {
                    // Se è lo stesso file dell'analisi base non inserirlo, inutile calcolare distanze nulle e complicare i grafici per niente....
                    continue;
                }

                Form1.newline_to_mainStatusWindow("Loading file " + filename + "...");
                try
                {
                    TextAnalyzerClass analysis = new TextAnalyzerClass();

                    // ** ISTRUZIONI NECESSARIE PER SETTARE UN mdFile VALIDO
                    mdFile file_analysis = new mdFile(text_analyzer.file_extension, text_analyzer.name);
                    file_analysis.system_name = filename;
                    file_analysis.current_directory = Path.GetDirectoryName(filename);
                    file_analysis.current_file_short_name = Path.GetFileName(filename);

                    // ** ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                    object objclass = new object();
                    objclass = analysis;                     // Boxing
                    if (file_analysis.direct_load(ref objclass, text_analyzer.load) == true)
                    {
                        // Assegnazione dell'oggetto appena caricato alla nostra classe, dopo Unboxing
                        //  E' l'unica cosa che mdFile.load() non può fare, dato che serve il nome esplicito della classe
                        //     mentre mdFile lavora su classi (objects) generiche

                        // ATTENZIONE PERCHE' SE CI SI DIMENTICA QUESTA ISTRUZIONE SEMBRA CHE FUNZIONI TUTTO...
                        //    MA LA CLASSE NON VIENE AGGIORNATA !!!!!
                        analysis = (TextAnalyzerClass)objclass;

                        // Parte CUSTOM: inserimento dell'analisi nella lista
                        analysis_to_compare.Add(analysis);
                        //  TUTTI gli analysis_filename sono stati già settati precdentemente!

                        // Check che il file appena aggiunto abbia le stesse opzioni di preprocessing nel file di riferimento
                        if (preprocessing_options_are_different(analysis, analysis_to_compare[0]) == true)
                        {
                            mdError error = new mdError();
                            string message = "Warning: file " + filename + " was elaborated using different pre-processing options than the reference file, this may degrade the performances of the comparison function";
                            error.root("Warning", message);

                            error.Display_and_Clear();
                        }



                    } // un else non serve, se il load fallisce mdFile.load() sistema già tutto per conto suo
                    // ** FINE ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                }
                catch
                {
                    analysis_to_compare = new List<TextAnalyzerClass>();
                    analysis_filenames = new List<string>();
                    newline_to_mainStatusWindow("Cannot open analysis file " + filename);
                    return;
                }
            }



            FormCompare form = new FormCompare();
            form.Show();

        }

        private bool preprocessing_options_are_different(TextAnalyzerClass analysis1, TextAnalyzerClass analysis2)
        {

            if (analysis1.used_parameters.discard_all_arabic_numbers != analysis2.used_parameters.discard_all_arabic_numbers) return true;
            if (analysis1.used_parameters.keep_distinction_between_upper_and_lowercase != analysis2.used_parameters.keep_distinction_between_upper_and_lowercase) return true;

            if (analysis1.used_parameters.apostrophe_is_a_separator != analysis2.used_parameters.apostrophe_is_a_separator) return true;
            if (analysis1.used_parameters.words_linked_by_an_apostrophe_are_separated != analysis2.used_parameters.words_linked_by_an_apostrophe_are_separated) return true;
            if (analysis1.used_parameters.words_linked_by_an_apostrophe_are_joined != analysis2.used_parameters.words_linked_by_an_apostrophe_are_joined) return true;
            if (analysis1.used_parameters.words_linked_by_an_apostrophe_are_discarded != analysis2.used_parameters.words_linked_by_an_apostrophe_are_discarded) return true;

            if (analysis1.used_parameters.words_linked_by_a_dash_are_separated != analysis2.used_parameters.words_linked_by_a_dash_are_separated) return true;
            if (analysis1.used_parameters.words_linked_by_a_dash_are_joined != analysis2.used_parameters.words_linked_by_a_dash_are_joined) return true;
            if (analysis1.used_parameters.words_linked_by_a_dash_are_discarded != analysis2.used_parameters.words_linked_by_a_dash_are_discarded) return true;

            return false;
        }





        private void preprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VOptionsPreprocess preprocessing_options_form = new VOptionsPreprocess();
            preprocessing_options_form.Show();
        }

        private void checkBox_bigrams_theoric_displaytable_CheckedChanged(object sender, EventArgs e)
        {
            enforce_controls_coherency();
            display_bigrams_theoric_data(_display_all);
        }

        private void button_select_all_text_bigrams_theoric_Click(object sender, EventArgs e)
        {
            textBox_bigrams_theoric.Select();
        }

        private void checkBox_bigrams_displaytable_CheckedChanged(object sender, EventArgs e)
        {
            enforce_controls_coherency();
            display_bigrams_data(_display_all);
        }

        private void button_select_all_text_bigrams_Click(object sender, EventArgs e)
        {
            textBox_bigrams.Select();
        }

        private void checkBox_bigrams_delta_CheckedChanged(object sender, EventArgs e)
        {
            enforce_controls_coherency();
            display_bigrams_data(_display_all);
        }

        private void checkBox_bigrams_delta_symmetrized_CheckedChanged(object sender, EventArgs e)
        {
            display_bigrams_data(_display_all);
        }


        private void button_Xc_select_all_Click(object sender, EventArgs e)
        {
            textBox_Xc_table.Select();
        }
        private void button_cX_select_all_Click(object sender, EventArgs e)
        {
            textBox_cX_table.Select();
        }

        private void button_distances_Xc_select_all_Click(object sender, EventArgs e)
        {
            textBox_chars_distances_Xc.Select();
        }
        private void button_distances_cX_select_all_Click(object sender, EventArgs e)
        {
            textBox_chars_distances_cX.Select();
        }
 
        private void button_words_length_select_all_Click(object sender, EventArgs e)
        {
            textBox_words_length_distribution.Select();
        }

        private void button_singlechars_with_spaces_select_all_Click(object sender, EventArgs e)
        {
            textBox_single_characters_distribution.Select();
        }

        private void button_singlechars_without_spaces_select_all_Click(object sender, EventArgs e)
        {
            textBox_single_characters_distribution_nospaces.Select();
        }

        private void button_syllables_single_vowels_select_all_Click(object sender, EventArgs e)
        {
            textBox_syllables_single_vowel.Select();
        }

        private void button_syllables_multiple_vowels_select_all_Click(object sender, EventArgs e)
        {
            textBox_syllables_multiple_vowels.Select();
        }

        private void button_report_select_all_Click(object sender, EventArgs e)
        {
            textBox_report.Select();
        }

        private void button_vocabulary_select_all_Click(object sender, EventArgs e)
        {
            textBox_vocabulary.Select();
        }



        private void Form1_Resize(object sender, EventArgs e)
        {
            if (a_threadIsStarted == false) display_data(_display_graphs_only);
        }
        private void MainTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            display_data(_display_graphs_only);
        }



        // TUTTO DA INSERIRE NEL TEMPLATE!!!!! NON E' STATO FACILE ARRIVARCI!!

        // GESTIONE DI TRACKBARS ASSIEME A CASELLE DI TESTO:
        //   L'evento TextChanged serve per far aggiornare immediatamente la posizione dello slider nella trackbar
        //   L'evento Leave controlla che nella casella di testo ci sia un valore valido, e se no lo resetta
        //   
        //  NOTARE come vengano disabilitati gli eventi per evitare che una routine che modifica un controllo causi anche il lancio dell'evento sul controllo modificato
        //     col che salterebbe fuori un casino
        private void trackBar_linear_size_ValueChanged(object sender, EventArgs e)
        {
            graphs_limit_1d = trackBar_linear_size.Value;
            this.textBox_linear_size.TextChanged -= new System.EventHandler(this.textBox_linear_size_TextChanged);
            textBox_linear_size.Text = graphs_limit_1d.ToString();
            this.textBox_linear_size.TextChanged += new System.EventHandler(this.textBox_linear_size_TextChanged);

            display_data(_display_graphs_only);
        }

        private void textBox_linear_size_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_linear_size.Text, out value) == true)
            {
                if (value >= min_linear_graph_length && value <= max_linear_graph_length)
                {
                    graphs_limit_1d = value;
                    this.trackBar_linear_size.ValueChanged -= new System.EventHandler(this.trackBar_linear_size_ValueChanged);
                    trackBar_linear_size.Value = graphs_limit_1d;
                    this.trackBar_linear_size.ValueChanged += new System.EventHandler(this.trackBar_linear_size_ValueChanged);
                }
                else
                {
                    this.trackBar_linear_size.ValueChanged -= new System.EventHandler(this.trackBar_linear_size_ValueChanged);
                    if (value < min_linear_graph_length && trackBar_linear_size.Value > min_linear_graph_length) trackBar_linear_size.Value = min_linear_graph_length;
                    if (value > max_linear_graph_length && trackBar_linear_size.Value < max_linear_graph_length) trackBar_linear_size.Value = max_linear_graph_length;
                    this.trackBar_linear_size.ValueChanged += new System.EventHandler(this.trackBar_linear_size_ValueChanged);
                }
            }
            display_data(_display_graphs_only);
        }

        private void textBox_linear_size_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_linear_size.Text, out value) == true)
            {
                if (value >= min_linear_graph_length && value <= max_linear_graph_length)
                {
                    graphs_limit_1d = value;
                    trackBar_linear_size.Value = graphs_limit_1d;
                }
                {
                    if (value < min_linear_graph_length) textBox_linear_size.Text = min_linear_graph_length.ToString();
                    if (value > max_linear_graph_length) textBox_linear_size.Text = max_linear_graph_length.ToString();
                }
            }
            display_data(_display_graphs_only);
        }


        private void textBox_linear_size_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_linear_size_Leave(sender, e);
        }






        private void trackBar_table_size_ValueChanged(object sender, EventArgs e)
        {
            graphs_limit_2d = trackBar_table_size.Value;
            this.textBox_table_size.TextChanged -= new System.EventHandler(this.textBox_table_size_TextChanged);
            textBox_table_size.Text = graphs_limit_2d.ToString();
            this.textBox_table_size.TextChanged += new System.EventHandler(this.textBox_table_size_TextChanged);

            display_data(_display_graphs_only);
        }

        private void textBox_table_size_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_table_size.Text, out value) == true)
            {
                if (value >= min_table_graph_length && value <= max_table_graph_length)
                {
                    graphs_limit_2d = value;
                    this.trackBar_table_size.ValueChanged -= new System.EventHandler(this.trackBar_table_size_ValueChanged);
                    trackBar_table_size.Value = graphs_limit_2d;
                    this.trackBar_table_size.ValueChanged += new System.EventHandler(this.trackBar_table_size_ValueChanged);
                }
                else
                {
                    this.trackBar_table_size.ValueChanged -= new System.EventHandler(this.trackBar_table_size_ValueChanged);
                    if (value < min_table_graph_length && trackBar_table_size.Value > min_table_graph_length) trackBar_table_size.Value = min_table_graph_length;
                    if (value > max_table_graph_length && trackBar_table_size.Value < max_table_graph_length) trackBar_table_size.Value = max_table_graph_length;
                    this.trackBar_table_size.ValueChanged += new System.EventHandler(this.trackBar_table_size_ValueChanged);
                }
            }
            display_data(_display_graphs_only);
        }

        private void textBox_table_size_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_table_size.Text, out value) == true)
            {
                if (value >= min_table_graph_length && value <= max_table_graph_length)
                {
                    graphs_limit_2d = value;
                    trackBar_table_size.Value = graphs_limit_2d;
                }
                {
                    if (value < min_table_graph_length) textBox_table_size.Text = min_table_graph_length.ToString();
                    if (value > max_table_graph_length) textBox_table_size.Text = max_table_graph_length.ToString();
                }
            }
            display_data(_display_graphs_only);
        }



        private void textBox_table_size_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_table_size_Leave(sender, e);
        }

        private void visualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VOptionsVisualization form = new VOptionsVisualization();
            form.Show();
        }

        private void loadAndAnalyzeTextFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            load_and_analyze();
        }

        private void savedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VOptionsFiles form = new VOptionsFiles();
            form.Show();
        }

        private void splitContainer_single_chars_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_monograms_data(_display_graphs_only);
        }

        private void splitContainer_single_chars_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_monograms_data(_display_graphs_only);
        }


        private void splitContainer_bigrams_theoric_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_bigrams_theoric_data(_display_graphs_only);
        }

        private void splitContainer_bigrams_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_bigrams_data(_display_graphs_only);
        }

        private void splitContainer_cXc_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_cXc_data(_display_graphs_only);
        }

        private void splitContainer_cXc_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_cXc_data(_display_graphs_only);
        }

        private void splitContainer_distances_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_distances_data(_display_graphs_only);
        }

        private void splitContainer_distances_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_distances_data(_display_graphs_only);
        }

        private void splitContainer_vocabulary_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_vocabulary_data(_display_graphs_only);
        }

        private void splitContainer_vocabulary_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_vocabulary_data(_display_graphs_only);
        }

        private void splitContainer_syllables_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_syllables_data(_display_graphs_only);
        }

        private void splitContainer_syllables_graphs_SplitterMoved(object sender, SplitterEventArgs e)
        {
            display_syllables_data(_display_graphs_only);
        }

        private void showAnalysisInfosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VSaveAnalysisDialog form = new VSaveAnalysisDialog();
            form.ShowDialog();
        }

        private void checkBox_words_length_distribution_in_text_CheckedChanged(object sender, EventArgs e)
        {
            display_vocabulary_data(_display_all);
        }


        private void massAnalyzeAndSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Questa funzione carica tutti i files .txt selezionati, li analizza e poi li salva uno per uno come .txalysis (con lo stesso nome del file di testo)

            // get il nome del/dei file/s di analisi da aprire
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Choose text files for mass analysis & save";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_text_files_directory;            
            openFileDialog1.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            // e aggiorna la posizione della directory dei files di testo utente
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_text_files_directory)
            {
                configuration_data.user_text_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }

            
            disable_controls_while_threads_are_running();
            // Ecco lo strano acrostico che passa dei parametri ad una Thread!
            mass_analyzeThread = new Thread(new ThreadStart(() => mass_analyze(openFileDialog1)));
            mass_analyzeThread.Start();

        }

        private void mass_analyze(OpenFileDialog openFileDialog1)
        {
            string message;

            // Caricamento dei files da analizzare e trasformare in .txalysis
            foreach (var filename in openFileDialog1.FileNames)
            {
                message = "Loading file " + filename + "...";                
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

                try
                {
                    // Qua dovrei creare una nuova classe, così non altero text_analyzer... ma è un casino perchè la text_analyzer_main fa
                    //   riferiemnto specificatamente a text_analyzer... e sarebbe troppo una menata modificarla
                    text_analyzer = new TextAnalyzerClass();

                    // Parte parzialmente copiata da load_and_analyze (male)

                    // Salva la lista delle opzioni nella classe primaria
                    text_analyzer.used_parameters.discard_all_arabic_numbers = configuration_data.discard_all_arabic_numbers;
                    text_analyzer.used_parameters.keep_distinction_between_upper_and_lowercase = configuration_data.keep_distinction_between_upper_and_lowercase;
                    text_analyzer.used_parameters.apostrophe_is_a_separator = configuration_data.apostrophe_is_a_separator;
                    text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_discarded = configuration_data.words_linked_by_an_apostrophe_are_discarded;
                    text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_joined = configuration_data.words_linked_by_an_apostrophe_are_joined;
                    text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_separated = configuration_data.words_linked_by_an_apostrophe_are_separated;
                    text_analyzer.used_parameters.words_linked_by_a_dash_are_discarded = configuration_data.words_linked_by_a_dash_are_discarded;
                    text_analyzer.used_parameters.words_linked_by_a_dash_are_joined = configuration_data.words_linked_by_a_dash_are_joined;
                    text_analyzer.used_parameters.words_linked_by_a_dash_are_separated = configuration_data.words_linked_by_a_dash_are_separated;

                    // Presetta il nome nel campo all'interno della classe
                    text_analyzer.book_title = Path.GetFileNameWithoutExtension(filename);  // Presetta anche il titolo

                    // Come prima cosa cerco di caricare il file di testo con codifica UTF8, ma se invece è nella vecchia codifica UTF7 quetso risulta
                    //    nella conversione errata di alcuni caratteri (p.es. quelli accentati), che diventano '�'. Se questo accade lo ricarico come UTF7
                    System.IO.StreamReader source_stream = new System.IO.StreamReader(filename, System.Text.Encoding.UTF8);
                    string text = source_stream.ReadToEnd();
                    source_stream.Close();
                    int caratteri_balordi = Regex.Matches(text, "�").Count;
                    if (caratteri_balordi != 0)
                    {
                        message = " probably UTF-7 encoded, trying opening it as such...";
                        Form1.mainForm.Invoke((Form1.delegate_append_to_mainStatuswindow)Form1.append_to_mainStatusWindow, new object[] { message });
                        source_stream = new System.IO.StreamReader(filename, System.Text.Encoding.UTF7);
                        text = source_stream.ReadToEnd();
                        source_stream.Close();
                    }
                    // Controllo di sicurezza sul fatto che la stringa non contenga caratteri balordi, che fanno supporre un problema di encoding del file, o un problema nel file stesso
                    //    Il test viene fatto su un po' dei caratteri balordi che ho trovato in vari files, ma non è detto che trappi proprio tutto, casomai
                    //      aggiungere caratteri man mano che se ne trovano
                    caratteri_balordi = Regex.Matches(text, "\\u001A | [\\u007F-\\u009F]").Count;
                    if (caratteri_balordi != 0)
                    {
                        // TEMO CHE SERVA UN INVOKE ANCHE QUA... 
                        mdError error = new mdError();
                        error.root("Input data warning", "File " + filename + " contains abnormal characters (for instance control characters from U+007F to U+009F) which suggest this files uses a character encoding " +
                                    "which cannot be read by TextAnalyzer, or the use of abnormal characters in the text itself. Both will probably cause the extraction of " +
                                    "'wrong' words from the text and the degradation of the quality of the vocabulary. Encoding problems can be detected because the text will appear " +
                                    "abnormal in TextAnalyzer, but normal in Notepad or Word. A workaround in this case is to open the .txt file with Notepad and then 'save as' choosing " +
                                    "UFT8 or UFT8 with BOM as the encoding. Or, you can select and copy all the text, then paste it into a newly-created .txt file. If the problem is in " +
                                    "the source file itself (you can see the strange characters even in Notepad): you can try to automatically replace the bad characters by using a " +
                                    "preprocessing Regex command, or you can manually correct the text file. If the offending characters are invisible: copy part of the text in a tool " +
                                    "such as https://regex101.com to identify them.");
                        error.Display_and_Clear();
                    }

                    text_analyzer.loaded_text += text;
                    text_analyzer.loaded_text += "\r\n"; // aggiungi un separatore fra i files (non servirebbe poerchè abbiamo un solo file, ma c'è per uniformità con load_and_analyze)

                }
                catch
                {
                    message = "Cannot open source text file " + filename;
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    return;
                }

                text_analyzer_main(false); // false: flag 'verbose'

                // Inserimento nome e parametri del file caricato
                text_analyzer.loaded_files_list = new List<LoadedFileRecord>();
                LoadedFileRecord new_value = new LoadedFileRecord();
                new_value.file_name = Path.GetFileName(filename);
                new_value.file_length = text_analyzer.loaded_text.Length;
                new_value.file_charset_size = text_analyzer.analysis_results.monograms_distribution.Count;
                new_value.file_total_words = text_analyzer.analysis_results.total_number_of_words_in_the_text;
                new_value.file_max_words_length = text_analyzer.analysis_results.words_length_distribution_in_vocabulary.Count;
                text_analyzer.loaded_files_list.Add(new_value);

                message = "\tFile length: " + new_value.file_length + ", character set size: " + new_value.file_charset_size + ", maximum word length: " + new_value.file_max_words_length;
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


                // Necessario rilanciare la disable_controls, perchè text_analyzer_main riabilita in controlli quando termina
                Form1.mainForm.Invoke((Form1.delegate_disable_controls_while_threads_are_running)Form1.disable_controls_while_threads_are_running, new object[] { });

                // Ci resta solo da salvare il file .txalysis (parte parzialmente copiata da saveToolStripMenuItem_Click)
                check_save_analysis_file_options(); // Attenzione: elimina anche il testo dal file che viene salvato, IMPORTANTE PER EVITARE PROBLEMI DI COPYRIGHT!!

                // Settatura dei parametri di file_text_analyzer
                file_text_analyzer.system_name = filename;
                file_text_analyzer.current_directory = Path.GetDirectoryName(filename);
                file_text_analyzer.current_file_short_name = Path.GetFileNameWithoutExtension(filename);
                // FINE CUSTOM


                object objclass = new object();
                objclass = text_analyzer;                     // Boxing
                file_text_analyzer.save(objclass, text_analyzer.save, configuration_data.user_files_directory);

            }


            // Resetta i graphs_limits (si sa mai...)
            set_graphs_limits_defaults();
            // Aggiornamento delle due textBoxes dei graphs_limits via Invoke
            Form1.mainForm.Invoke((Form1.delegate_update_graphs_limits_display)Form1.update_graphs_limits_display, new object[] { });

            // lancio della display data (visualizza l'ultima analisi effettuata)
            Form1.mainForm.Invoke((Form1.delegate_display_data)Form1.display_data, new object[] { _display_all });

            // Bisogna anche aggiornare la caption della finestra principale
            string caption = base_caption + file_text_analyzer.current_file_short_name;
            Form1.mainForm.Invoke((Form1.delegate_set_mainForm_caption)Form1.set_mainForm_caption, new object[] { caption });


            message = "Processing completed\r\n";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            // Riabilitazione dei controlli
            Form1.mainForm.Invoke((Form1.delegate_enable_controls_when_thread_stops)Form1.enable_controls_when_thread_stops, new object[] { });
        }

        private void findBestMatchesInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Questa funzione compara l'analisi corrente (text_analyzer) con ognuno dei files .txalysis selezionati e sceglie
            //   quelli che con distanza minore (viene considerata solo la distanza unblinded dei bigrammi)

            // Se non c'è già un'analisi caricata avvisa che ne serve una per poter fare la comparazione
            if (text_analyzer.analysis_results.total_characters_in_cleaned_text == 0)
            {
                mdError warning = new mdError();
                warning.root("No data to make a comparison", "Load an analysis file (or analyze text files) to get the base data for the comparison");
                warning.Display_and_Clear();
                return;
            }

            // get il nome del/dei file/s di analisi da aprire
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Choose analysis files to search";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_files_directory;
            openFileDialog1.Filter = file_text_analyzer.target_class_human_oriented_name + " file (*." + file_text_analyzer.extension + ")|*." + file_text_analyzer.extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Aggiorna la posizione della directory dei files utente
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }

            // Questa lista è quella che viene compilata confrontando le distanze dai vari files, e che poi verrà
            //   trasferita in compare_results per passarla alla FormCompare per la visualizzazione
            //   Usa un tipo complesso perchè va ordinata per distanza
            List<DistanceAnalysis> analysis_best_matches = new List<DistanceAnalysis>();
            IComparer<DistanceAnalysis> comparer_by_distance = new CompareAnalysisByDistance();

            // Caricamento dei files da comparare
            foreach (var filename in openFileDialog1.FileNames)
            {
                // Se è lo stesso file dell'analisi base non inserirlo (verrà scartato anche nel seguito dell'elaborazione)
                if (Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name) == Path.GetFileNameWithoutExtension(filename))
                {
                    continue;
                }


                Form1.newline_to_mainStatusWindow("Loading file " + filename + "...");
                try
                {
                    TextAnalyzerClass loaded_analysis = new TextAnalyzerClass();

                    // ** ISTRUZIONI NECESSARIE PER SETTARE UN mdFile VALIDO
                    mdFile file_analysis = new mdFile(text_analyzer.file_extension, text_analyzer.name);
                    file_analysis.system_name = filename;
                    file_analysis.current_directory = Path.GetDirectoryName(filename);
                    file_analysis.current_file_short_name = Path.GetFileName(filename);

                    // ** ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                    object objclass = new object();
                    objclass = loaded_analysis;                     // Boxing
                    if (file_analysis.direct_load(ref objclass, loaded_analysis.load) == true)
                    {
                        // Assegnazione dell'oggetto appena caricato alla nostra classe, dopo Unboxing
                        //  E' l'unica cosa che mdFile.load() non può fare, dato che serve il nome esplicito della classe
                        //     mentre mdFile lavora su classi (objects) generiche

                        // ATTENZIONE PERCHE' SE CI SI DIMENTICA QUESTA ISTRUZIONE SEMBRA CHE FUNZIONI TUTTO...
                        //    MA LA CLASSE NON VIENE AGGIORNATA !!!!!
                        loaded_analysis = (TextAnalyzerClass)objclass;

                        // Parte CUSTOM

                        // Check che il file abbbia le stesse opzioni di preprocessing nel file di riferimento
                        if (preprocessing_options_are_different(loaded_analysis, text_analyzer) == true)
                        {
                            mdError error = new mdError();
                            string message = "Warning: file " + filename + " was elaborated using different pre-processing options than the reference file, this may degrade the performances of the comparison function";
                            error.root("Warning", message);

                            error.Display_and_Clear();
                        }


                        // PROCESSING EFFETTIVO (SAREBBE DA METTERE IN UNA SUBROUTINE...)
                        analysis_to_compare = new List<TextAnalyzerClass>();
                        // Inserisci la classe attuale come primo elemento della comparazione
                        analysis_to_compare.Add(text_analyzer);
                        // Inserisci il file appena caricato come secondo elemento
                        analysis_to_compare.Add(loaded_analysis);
                        analysis_filenames = new List<string>();
                        // Inserisci la classe attuale come primo elemento
                        analysis_filenames.Add(Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name));
                        // Inserisci il file appena caricato come secondo elemento
                        analysis_filenames.Add(Path.GetFileNameWithoutExtension(filename));

                        // NOTARE CHE PER I GRAPHS-LIMITS POSSO USARE SOLO IL VALORE DI DEFAULT PER IL rare_characters_cutoff
                        List<int> current_graphs_limit_2d = new List<int>();
                        current_graphs_limit_2d = FormCompare.get_2d_limits(FormCompare.rare_characters_default_cutoff);

                        CompareResults find_best_matches_compare_results = new CompareResults();
                        find_best_matches_compare_results.bigrams_distribution = new List<List<List<EValueOcc>>>();
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
                                    single_row.Add(single_value);
                                }
                                single_analysis.Add(single_row);
                            }
                            find_best_matches_compare_results.bigrams_distribution.Add(single_analysis);
                        }


                        find_best_matches_compare_results.bigrams_distances_from_first_unblinded =FormCompare.calculate_distances_from_first_2d(Form1.EVconvert(find_best_matches_compare_results.bigrams_distribution), FormCompare.BlindFactor2d._unblinded, current_graphs_limit_2d);

                        int max_output_results = 24;

                        if (analysis_best_matches.Count < max_output_results)
                        {
                            // La lista non è ancora piena
                            DistanceAnalysis this_match = new DistanceAnalysis();
                            this_match.filename = Path.GetFileNameWithoutExtension(filename);
                            this_match.distance = find_best_matches_compare_results.bigrams_distances_from_first_unblinded[1].value;
                            this_match.analysis = loaded_analysis;
                            analysis_best_matches.Add(this_match);

                            analysis_best_matches.Sort(comparer_by_distance); // e ordiniamo la lista!
                        }
                        else
                        {
                            // La lista è piena: se il file appena elaborato ha distanza minore di quella dell'ultimo elemento in lista (che è quello con distanza più alta,
                            //    dato che la lista è ordinata) l'ultimo elemento viene eliminato e rimpiazzato con quello attuale
                            if (find_best_matches_compare_results.bigrams_distances_from_first_unblinded[1].value < analysis_best_matches[analysis_best_matches.Count - 1].distance)
                            {
                                analysis_best_matches.RemoveAt(analysis_best_matches.Count - 1);

                                DistanceAnalysis this_match = new DistanceAnalysis();
                                this_match.filename = Path.GetFileNameWithoutExtension(filename);
                                this_match.distance = find_best_matches_compare_results.bigrams_distances_from_first_unblinded[1].value;
                                this_match.analysis = loaded_analysis;
                                analysis_best_matches.Add(this_match);

                                analysis_best_matches.Sort(comparer_by_distance); // e ordiniamo la lista!
                            }
                        }                      
                        // FINE PROCESSING EFFETTIVO (SAREBBE DA METTERE IN UNA SUBROUTINE...)


                    } // un else non serve, se il load fallisce mdFile.load() sistema già tutto per conto suo
                    // ** FINE ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                }
                catch
                {
                    newline_to_mainStatusWindow("Cannot open analysis file " + filename);
                    return;
                }
            }

            // Trasferimento della lista best matches in analysis_to_compare (e dei nomi files in analysis_filename)
            analysis_to_compare = new List<TextAnalyzerClass>();
            analysis_filenames = new List<string>();

            analysis_to_compare.Add(text_analyzer);  // analisi base
            analysis_filenames.Add(Path.GetFileNameWithoutExtension(file_text_analyzer.current_file_short_name));

            foreach (DistanceAnalysis analysis in analysis_best_matches)
            {
                analysis_to_compare.Add(analysis.analysis);
                analysis_filenames.Add(analysis.filename);
            }

            FormCompare form_compare = new FormCompare();
            form_compare.Show();
        }




        private class CompareAnalysisByDistance : IComparer<DistanceAnalysis>
        {
            public int Compare(DistanceAnalysis x, DistanceAnalysis y)
            {
                if (x.distance == y.distance) return (0);

                if (x.distance > y.distance)
                {
                    return (1);
                }
                return (-1);
            }
        }


        private void massAggregateAnalysisInASingleCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Lo scheletro di questa routine è derivato da findBestMatchesInToolStripMenuItemk_Click

            // get il nome del/dei file/s di analisi da aprire
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Choose analysis files to search";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_files_directory;
            openFileDialog1.Filter = file_text_analyzer.target_class_human_oriented_name + " file (*." + file_text_analyzer.extension + ")|*." + file_text_analyzer.extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Aggiorna la posizione della directory dei files utente
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }


            text_analyzer = new TextAnalyzerClass();


            // per aggregare i dati senza metterci una vita mi servono un botto di dictionaries...
            Dictionary<string, EValueOcc> monograms_dictionary = new Dictionary<string, EValueOcc>();
            Dictionary<string, EValueOcc> bigrams_dictionary = new Dictionary<string, EValueOcc>();
            Dictionary<string, EValueOcc> vocabulary_dictionary = new Dictionary<string, EValueOcc>();
            Dictionary<string, EValueOcc_extended> wordslength_text_dictionary = new Dictionary<string, EValueOcc_extended>();

            // TUTTO IL RESTO NON E' AGGREGABILE E VA RICREATO EX-NOVO!
            //  Questo è ovvio per le statistiche che usano gli EValue: dato che sono frequenze non sono aggregabili
            //  E' molto meno ovvio per quanto riguarda le wordslength nel vocabolario e le sillabe, ma è proprio così!! E' un problema sottile 
            //      che dipende dal fatto che sono statistiche fatte sul vocabolario e non sul testo. Non è affatto semplice da spiegare
            //      e ci vorrebbero due pagine di testo, ma anche disegni...(ci ho messo una vita a capire il perchè), credeteci.


            foreach (var filename in openFileDialog1.FileNames)
            {
                Form1.newline_to_mainStatusWindow("Loading file " + filename + "...");
                try
                {
                    TextAnalyzerClass loaded_analysis = new TextAnalyzerClass();

                    // ** ISTRUZIONI NECESSARIE PER SETTARE UN mdFile VALIDO
                    mdFile file_analysis = new mdFile(text_analyzer.file_extension, text_analyzer.name);
                    file_analysis.system_name = filename;
                    file_analysis.current_directory = Path.GetDirectoryName(filename);
                    file_analysis.current_file_short_name = Path.GetFileName(filename);

                    // ** ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                    object objclass = new object();
                    objclass = loaded_analysis;                     // Boxing
                    if (file_analysis.direct_load(ref objclass, loaded_analysis.load) == true)
                    {
                        // Assegnazione dell'oggetto appena caricato alla nostra classe, dopo Unboxing
                        //  E' l'unica cosa che mdFile.load() non può fare, dato che serve il nome esplicito della classe
                        //     mentre mdFile lavora su classi (objects) generiche

                        // ATTENZIONE PERCHE' SE CI SI DIMENTICA QUESTA ISTRUZIONE SEMBRA CHE FUNZIONI TUTTO...
                        //    MA LA CLASSE NON VIENE AGGIORNATA !!!!!
                        loaded_analysis = (TextAnalyzerClass)objclass;

                        // Parte CUSTOM


                        if (text_analyzer.loaded_files_list.Count == 0)
                        {
                            // Sul primo file caricato settiamo le opzioni di preprocessing
                            text_analyzer.used_parameters.discard_all_arabic_numbers = loaded_analysis.used_parameters.discard_all_arabic_numbers;
                            text_analyzer.used_parameters.keep_distinction_between_upper_and_lowercase = loaded_analysis.used_parameters.keep_distinction_between_upper_and_lowercase;
                            text_analyzer.used_parameters.apostrophe_is_a_separator = loaded_analysis.used_parameters.apostrophe_is_a_separator;
                            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_discarded = loaded_analysis.used_parameters.words_linked_by_an_apostrophe_are_discarded;
                            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_joined = loaded_analysis.used_parameters.words_linked_by_an_apostrophe_are_joined;
                            text_analyzer.used_parameters.words_linked_by_an_apostrophe_are_separated = loaded_analysis.used_parameters.words_linked_by_an_apostrophe_are_separated;
                            text_analyzer.used_parameters.words_linked_by_a_dash_are_discarded = loaded_analysis.used_parameters.words_linked_by_a_dash_are_discarded;
                            text_analyzer.used_parameters.words_linked_by_a_dash_are_joined = loaded_analysis.used_parameters.words_linked_by_a_dash_are_joined;
                            text_analyzer.used_parameters.words_linked_by_a_dash_are_separated = loaded_analysis.used_parameters.words_linked_by_a_dash_are_separated;


                            // Presetta il titolo
                            text_analyzer.book_title = "!Corpus (" + Path.GetFileNameWithoutExtension(filename) + " + " + (openFileDialog1.FileNames.Length - 1) + " more files)";
                        }
                        else
                        {
                            // Check che il file caricato usi le stesse opzioni degli altri
                            if (preprocessing_options_are_different(loaded_analysis, text_analyzer) == true)
                            {
                                mdError error = new mdError();
                                string message = "Warning: file " + filename + " was elaborated using different pre-processing options than the reference file, this may degrade the performances of the comparison function";
                                error.root("Warning", message);

                                error.Display_and_Clear();
                            }
                        }

                        // Aggiornamento della lista dei file
                        LoadedFileRecord this_file_name = new LoadedFileRecord();
                        this_file_name.file_name = Path.GetFileName(filename);
                        this_file_name.file_length = loaded_analysis.analysis_results.total_characters_in_cleaned_text; // Notare che non ha esattamente lo stesso significato di quando si
                                                                                                                        //  esegue l'analisi sui testi, in quel caso viene usata la raw length
                                                                                                                        //  prima della ripulitura, ma vabbè, importa poco
                        this_file_name.file_charset_size = loaded_analysis.analysis_results.monograms_distribution.Count;
                        this_file_name.file_total_words = loaded_analysis.analysis_results.total_number_of_words_in_the_text;
                        this_file_name.file_max_words_length = loaded_analysis.analysis_results.words_length_distribution_in_vocabulary.Count;
                        text_analyzer.loaded_files_list.Add(this_file_name);


                        // PROCESSING VERO E PROPRIO: E' DIFFICILE INSERIRLO IN UNA SUBROUTINE A CAUSA DEI DICTIONARIES
                        TextAnalysis_results aggregated_results = new TextAnalysis_results();

                        aggregated_results.discarded_arabic_numbers_characters = text_analyzer.analysis_results.discarded_arabic_numbers_characters + loaded_analysis.analysis_results.discarded_arabic_numbers_characters;
                        aggregated_results.discarded_apostrophes = text_analyzer.analysis_results.discarded_apostrophes + loaded_analysis.analysis_results.discarded_apostrophes;
                        aggregated_results.discarded_dashes = text_analyzer.analysis_results.discarded_dashes + loaded_analysis.analysis_results.discarded_dashes;
                        aggregated_results.discarded_words_containing_an_apostrophe = text_analyzer.analysis_results.discarded_words_containing_an_apostrophe + loaded_analysis.analysis_results.discarded_words_containing_an_apostrophe;
                        aggregated_results.discarded_words_containing_a_dash = text_analyzer.analysis_results.discarded_words_containing_a_dash + loaded_analysis.analysis_results.discarded_words_containing_a_dash;
                        aggregated_results.discarded_putative_abbreviations = text_analyzer.analysis_results.discarded_putative_abbreviations + loaded_analysis.analysis_results.discarded_putative_abbreviations;
                        aggregated_results.total_characters_in_cleaned_text = text_analyzer.analysis_results.total_characters_in_cleaned_text + loaded_analysis.analysis_results.total_characters_in_cleaned_text;
                        aggregated_results.total_literal_characters_in_cleaned_text = text_analyzer.analysis_results.total_literal_characters_in_cleaned_text + loaded_analysis.analysis_results.total_literal_characters_in_cleaned_text;
                        aggregated_results.total_spaces_in_cleaned_text = text_analyzer.analysis_results.total_spaces_in_cleaned_text + loaded_analysis.analysis_results.total_spaces_in_cleaned_text;
                        aggregated_results.total_number_of_words_in_the_text = text_analyzer.analysis_results.total_number_of_words_in_the_text + loaded_analysis.analysis_results.total_number_of_words_in_the_text;

                        aggregate_stats(loaded_analysis.analysis_results.monograms_distribution, ref monograms_dictionary);
                        aggregate_stats_2d(loaded_analysis.analysis_results.bigrams_distribution, ref bigrams_dictionary, monograms_dictionary);
                        aggregate_stats(loaded_analysis.analysis_results.vocabulary_words_distribution, ref vocabulary_dictionary);
                        aggregate_stats(loaded_analysis.analysis_results.words_length_distribution_in_text, ref wordslength_text_dictionary);

                        text_analyzer.analysis_results = aggregated_results;  // istruzione da non dimenticare :p                        
                        
                        // FINE PROCESSING VERO E PROPRIO: E' DIFFICILE INSERIRLO IN UNA SUBROUTINE A CAUSA DEI DICTIONARIES

                    } // un else non serve, se il load fallisce mdFile.load() sistema già tutto per conto suo
                    // ** FINE ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                }
                catch
                {
                    newline_to_mainStatusWindow("Cannot open analysis file " + filename);
                    return;
                }
            }

            newline_to_mainStatusWindow("Completing aggregation (may take a while)...");

            // Arrivati qua dobbiamo trasferire i dictionaries nelle distribuzioni EValueOcc di text_analyzer, e calcolare tutte le varie statistiche derivate
            TextCharsStats.get_and_format_all_monograms_based_stats(monograms_dictionary, ref text_analyzer.analysis_results);

            // E' IMPORTANTE CHE I BIGRAMMI SIANO INSERITI NEL DIZIONARIO NELLO STESSO ORDINE DI FREQUENZA DEI MONOGRAMMI! QUA DEVONO ESSERE RIORDINATI!!
            //   EHH MA CHE CAZZO DI CASINO... IL DICTIONARY NON E' SORTABLE, ERGO DEVO TRASFERIRLO IN UNA LIST, MA POI LA LIST NON LA POSSO
            //     PASSARE A get_and_format_all_bigrams_based_stats ... E QUINDI DEVO RITRASFERIRLA IN UN DICTONARY (CHE A QUEL PUNTO SARA' ORDINATO)...
            List<EValueOcc> scratch_list = new List<EValueOcc>();
            foreach (EValueOcc item in bigrams_dictionary.Values)
            {
                scratch_list.Add(item);
            }
            IComparer<EValueOcc> comparer = new TextCharsStats.CompareEValueOcc_FollowList_monograms();
            scratch_list.Sort(comparer);
            bigrams_dictionary = new Dictionary<string, EValueOcc>();
            foreach (EValueOcc item in scratch_list)
            {
                bigrams_dictionary.Add(item.element, item);
            }
            TextCharsStats.get_and_format_all_bigrams_based_stats(bigrams_dictionary, ref text_analyzer.analysis_results);
            TextCharsStats.get_bigrams_cXc_table(ref text_analyzer.analysis_results);

            VocabularyStats.get_and_format_all_vocabulary_stats(vocabulary_dictionary, ref text_analyzer.analysis_results);


            // La words_lenght_distribution_in_text ha una gestione particolare in VocabularyStats, viene compilata direttamente e non passa da un dictionary
            //     quindi la gestione del trasferimento è qua
            foreach (EValueOcc_extended item in wordslength_text_dictionary.Values)
            {
                text_analyzer.analysis_results.words_length_distribution_in_text.Add(item);
            }

            // Ricreiamo ex-novo la words_length_distribution_in_vocabulary
            text_analyzer.analysis_results.words_length_distribution_in_vocabulary = VocabularyStats.get_words_length_distribution_in_vocabulary(text_analyzer.analysis_results);

            // Ricreiamo ex-novo le sillabe
            SyllablesStats.get_syllables(ref text_analyzer.analysis_results);


            // E alla fine salvataggio file e display
            // Settatura dei parametri di file_text_analyzer
            file_text_analyzer.system_name = configuration_data.user_files_directory + text_analyzer.book_title;
            file_text_analyzer.current_directory = configuration_data.user_files_directory;
            file_text_analyzer.current_file_short_name = text_analyzer.book_title;

            check_save_analysis_file_options(); // Attenzione: elimina anche il testo dal file che viene salvato, IMPORTANTE PER EVITARE PROBLEMI DI COPYRIGHT!!

            object objclass_out = new object();
            objclass_out = text_analyzer;                     // Boxing
            file_text_analyzer.save(objclass_out, text_analyzer.save, configuration_data.user_files_directory);

            newline_to_mainStatusWindow("Formatting data for display (may take a while)...");
            display_data(_display_all);
            newline_to_mainStatusWindow("Aggregation completed\r\n");

            // caption della finestra principale
            string caption = base_caption + file_text_analyzer.current_file_short_name;
            Form1.mainForm.Invoke((Form1.delegate_set_mainForm_caption)Form1.set_mainForm_caption, new object[] { caption });

        }




        private void aggregate_stats(List<EValueOcc> addendum, ref Dictionary<string, EValueOcc> dictionary)
        {
            foreach (EValueOcc new_value in addendum)
            {
                EValueOcc old_value = new EValueOcc();
                if (dictionary.TryGetValue(new_value.element, out old_value) == false)
                {
                    EValueOcc scratch_value = new EValueOcc();
                    scratch_value.element = new_value.element;
                    scratch_value.value = new_value.value;
                    dictionary.Add(scratch_value.element, scratch_value);
                }
                else
                {
                    EValueOcc scratch_value = new EValueOcc();
                    scratch_value.element = old_value.element;
                    scratch_value.value = old_value.value + new_value.value;
                    dictionary.Remove(old_value.element);
                    dictionary.Add(scratch_value.element, scratch_value);
                }
            }
        }

        private void aggregate_stats_2d(List<EValueOcc> addendum, ref Dictionary<string, EValueOcc> dictionary, Dictionary<string, EValueOcc> monograms)
        {
            // Perchè tutto funzioni in modalità 2D anche se addendum e dictionary sono basati su set di caratteri diffrerenti devo:

            //      - Costruire lo scheletro del dizionario bigrammi con tutti i bigrammi ricavati dalla lista caratteri complessiva (che è in dictionary_monograms)
            //      - Popolarlo coi dati sia di dictionary che da addendum

            Dictionary <string, EValueOcc> result = new Dictionary<string, EValueOcc>();

            foreach (EValueOcc item1 in monograms.Values)
            {
                foreach (EValueOcc item2 in monograms.Values)
                {
                    EValueOcc new_value = new EValueOcc();
                    new_value.element = item1.element + item2.element;
                    result.Add(new_value.element, new_value);
                }
            }

            // Inseriamo gli elementi da dictionary
            foreach (EValueOcc item in dictionary.Values)
            {
                EValueOcc old_value = new EValueOcc();
                if (result.TryGetValue(item.element, out old_value) == false)
                {
                    // QUI CI ANDREBBE UN SOFTWARE ERROR!!!
                }
                else
                {
                    EValueOcc scratch_value = new EValueOcc();
                    scratch_value.element = item.element;
                    scratch_value.value = item.value;
                    result.Remove(old_value.element);
                    result.Add(scratch_value.element, scratch_value);
                }
            }

            // E quelli da addendum
            foreach (EValueOcc new_value in addendum)
            {
                EValueOcc old_value = new EValueOcc();
                if (result.TryGetValue(new_value.element, out old_value) == false)
                {
                    // QUI CI ANDREBBE UN SOFTWARE ERROR!!!
                }
                else
                {
                    EValueOcc scratch_value = new EValueOcc();
                    scratch_value.element = old_value.element;
                    scratch_value.value = old_value.value + new_value.value;
                    result.Remove(old_value.element);
                    result.Add(scratch_value.element, scratch_value);
                }
            }

            dictionary = result;
        }

        private void aggregate_stats(List<EValueOcc_extended> addendum, ref Dictionary<string, EValueOcc_extended> dictionary)
        {
            foreach (EValueOcc_extended new_value in addendum)
            {
                EValueOcc_extended old_value = new EValueOcc_extended();
                if (dictionary.TryGetValue(new_value.element, out old_value) == false)
                {
                    EValueOcc_extended scratch_value = new EValueOcc_extended();
                    scratch_value.element = new_value.element;
                    scratch_value.element_additional = new_value.element_additional;
                    scratch_value.value = new_value.value;
                    dictionary.Add(scratch_value.element, scratch_value);
                }
                else
                {
                    EValueOcc_extended scratch_value = new EValueOcc_extended();
                    scratch_value.element = old_value.element;
                    scratch_value.element_additional = old_value.element_additional;
                    scratch_value.value = old_value.value + new_value.value;
                    dictionary.Remove(old_value.element);
                    dictionary.Add(scratch_value.element, scratch_value);
                }
            }
        }

        private void findAnalysisClustersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Questa routine è molto simile a CompareAnalysis

            // Finestra di opzioni
            VClusteringChoices options_form = new VClusteringChoices();
            if (options_form.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            // get il nome del/dei file/s di analisi da aprire
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Load analysis files";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = configuration_data.user_files_directory;
            openFileDialog1.Filter = file_text_analyzer.target_class_human_oriented_name + " file (*." + file_text_analyzer.extension + ")|*." + file_text_analyzer.extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }



            // Classe che contiene tutte le analisi da comparare: verrà poi usata da FormClustering
            analysis_to_compare = new List<TextAnalyzerClass>();


            // Aggiorna la posizione della directory dei files utente
            //  se è stata modificata la directory aggiorna il file .cfg
            if (Path.GetDirectoryName(openFileDialog1.FileNames[0]) != configuration_data.user_files_directory)
            {
                configuration_data.user_files_directory = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                configuration_data.save(configuration_data_file, configuration_data);
                configuration_data.is_saved = true;
            }

            // Classe che contiene i nomi di tutte le analisi da comparare: verrà poi usata da FormClustering
            analysis_filenames = new List<string>();
            foreach (var filename in openFileDialog1.FileNames)
            {
                analysis_filenames.Add(Path.GetFileNameWithoutExtension(filename));
            }


            // Caricamento dei files da comparare
            foreach (var filename in openFileDialog1.FileNames)
            {

                Form1.newline_to_mainStatusWindow("Loading file " + filename + "...");
                try
                {
                    TextAnalyzerClass analysis = new TextAnalyzerClass();

                    // ** ISTRUZIONI NECESSARIE PER SETTARE UN mdFile VALIDO
                    mdFile file_analysis = new mdFile(text_analyzer.file_extension, text_analyzer.name);
                    file_analysis.system_name = filename;
                    file_analysis.current_directory = Path.GetDirectoryName(filename);
                    file_analysis.current_file_short_name = Path.GetFileName(filename);

                    // ** ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                    object objclass = new object();
                    objclass = analysis;                     // Boxing
                    if (file_analysis.direct_load(ref objclass, text_analyzer.load) == true)
                    {
                        // Assegnazione dell'oggetto appena caricato alla nostra classe, dopo Unboxing
                        //  E' l'unica cosa che mdFile.load() non può fare, dato che serve il nome esplicito della classe
                        //     mentre mdFile lavora su classi (objects) generiche

                        // ATTENZIONE PERCHE' SE CI SI DIMENTICA QUESTA ISTRUZIONE SEMBRA CHE FUNZIONI TUTTO...
                        //    MA LA CLASSE NON VIENE AGGIORNATA !!!!!
                        analysis = (TextAnalyzerClass)objclass;

                        // Parte CUSTOM: inserimento dell'analisi nella lista
                        analysis_to_compare.Add(analysis);
                        //  TUTTI gli analysis_filename sono stati già settati precdentemente!

                    } // un else non serve, se il load fallisce mdFile.load() sistema già tutto per conto suo
                    // ** FINE ISTRUZIONI NECESSARIE PER FAR FUNZIONARE mdFile.load() **
                }
                catch
                {
                    analysis_to_compare = new List<TextAnalyzerClass>();
                    analysis_filenames = new List<string>();
                    newline_to_mainStatusWindow("Cannot open analysis file " + filename);
                    return;
                }
            }



            FormClustering form = new FormClustering();
            form.Show();

        }








        // Routines per il parsing seguendo una slot grammar e asemic writer (Voyinch)

        private void button_Voynich_run_parse_Click(object sender, EventArgs e)
        {

            if (text_analyzer.analysis_results.vocabulary_words_distribution.Count == 0)
            {
                mdError error = new mdError();
                error.root("", "No analysis loaded: create an analysis from .txt file or load an analysis first");
                error.Display_and_Clear();
                return;
            }

            disable_controls_while_threads_are_running();

            newline_to_mainStatusWindow("Starting parsing along the slot grammar...");

            // Calcolo della tabella con tutti i word types 'chunkificati' [Voynich_parsing_results.data]
            // NON reinizializzare Voynich_parsing_results qua... contiene la grammatica come selezionata dalla ComboBox!
            ParseVoynich.parse_words(text_analyzer.analysis_results, ref Voynich_parsing_results);

            if (Voynich_parsing_results.total_wordtypes_found == 0)
            {
                // Questo trappa un caso jellato: nessuna parola è stata trovata dalla grammatica (p.es. tutte le parole contenevano un carattere che la grammatica non gestisce)

                enable_controls_when_thread_stops();

                mdError error = new mdError();
                error.root("", "Grammar did not find any words (probably grammar and text use different character sets, or every word of the text contains a character not included in the grammar)");
                error.Display_and_Clear();
                return;
            }

            newline_to_mainStatusWindow("Parsing completed, computing chunks dictionary and Huffmann codes...");

            // Ricava il dizionario dei chunks (chunks_dictionary)
            //  e le classi coi chunks divisi in 'categorie' (che sono tutte sottoclassi di Voynich_parsing_results, passata come ref)
            ParseVoynich.get_chunks_data(Voynich_parsing_results.data, ref Voynich_parsing_results);

            // Trovato chunks_dictionary ricaviamo i codici di Huffmann
            ParseHuffmann.get_Huffmann_codes(ref Voynich_parsing_results.chunks_dictionary);


            newline_to_mainStatusWindow("Chunks dictionary and codes completed, computing transitions table for Asemic Markov chain...");

            // Calcolo della grammatica 'chunkificata' e della tabella delle transizioni per il Markov asemic engine
            Voynich_chunkified_grammar = new PackGrammarAndWrite.ChunksGrammar();
            PackGrammarAndWrite.calculate_chunkified_grammar(Voynich_parsing_results, ref Voynich_chunkified_grammar);


            newline_to_mainStatusWindow("Formatting data for display...");

            // VISUALIZZAZIONI, + IL CALCOLO DELLE 'CATEGORIE' DEI CHUNKS (CHE AL MOMENTO E' PIU' CHE ALTRO UN ACCESSORIO CHE NON VIENE USATO OLTRE)
            string result = "";

            // PRIMA TAB
            // Lista delle parole trovate e dei chunks as-parsed, + lista delle parole NOTFOUND, + lista delle parole RARECHARS
            result = "";
            float word_tokens_coverage = 0;  // inzializzazione dummy, passato come ref (serve dopo per il report)
            result = display_chunkified_words_list(ref word_tokens_coverage);
            textBox_Voynich_parsing.Text = result;


            // PRIMA TAB
            //  Chunk categories
            result = "";
            result = display_word_chunks_data();
            textBox_Voynich_chunks_types.Text = result;


            // SECONDA TAB
            // Chunkified slot grammar
            result = "";
            result = display_chunkified_slot_grammar(Voynich_chunkified_grammar);
            textBox_WrVoynich_chunkified_grammar.Text = result;


            // SECONDA TAB
            // Transitions table
            result = "";
            result = display_transitions_table(Voynich_chunkified_grammar);
            textBox_WrVoynich_transitions.Text = result;


            // PRIMA TAB
            // Report (alla fine che è meglio, così ha sempre a disposizione tutte le strutture)
            result = "";
            result = display_parsing_report(word_tokens_coverage);
            textBox_Voynich_report.Text = result;

            enable_controls_when_thread_stops();

            newline_to_mainStatusWindow("Processing completed\r\n");
        }





        private string display_parsing_report(float word_tokens_coverage)
        {

            // Report
            string result = "";
            result += program_identifier.name + " " + program_identifier.version + " " + DateTime.Now + "\r\n\r\n";
            result += "Transliteration file used: " + file_text_analyzer.current_file_short_name + "\r\n";

            result += "Grammar name: " + Voynich_parsing_results.grammar.name + ", max repeats = " + Form1.Voynich_current_max_loop_repeats + "\r\n";
            result += "Grammar notes: " + Voynich_parsing_results.grammar.notes + "\r\n\r\n";
            int counter = 1;
            foreach (string[] slot in Voynich_parsing_results.grammar.slots_table)
            {
                result += "Slot " + counter + ": ";
                counter++;
                foreach (string glyph in slot)
                {
                    result += glyph + " ";
                }
                result += "\r\n";
            }
            result += "\r\n";



            float efficiency = (float)Voynich_parsing_results.total_wordtypes_found / (float)Voynich_parsing_results.total_grammar_wordtypes;
            float coverage = (float)Voynich_parsing_results.total_wordtypes_found / (float)text_analyzer.analysis_results.vocabulary_words_distribution.Count;

            int total_words_without_rare_characters = text_analyzer.analysis_results.vocabulary_words_distribution.Count - Voynich_parsing_results.total_wordstypes_not_found_but_with_rare_characters;
            float coverage_excluding_rare_characters = (float)Voynich_parsing_results.total_wordtypes_found / (float)total_words_without_rare_characters;

            result += "Word types coverage (excluding word types with rare characters): " + coverage_excluding_rare_characters + "\r\n\r\n";


            // Nbits metric: length of the Huffmann-compressed text (text + chunks dictionary)
            int charset_size = text_analyzer.analysis_results.monograms_distribution_excluding_spaces.Count - Voynich_parsing_results.grammar.rare_characters.Length;
            double bits_per_character = Math.Log(charset_size, 2);

            long Huffmann_compressed_text_total_required_bits = 0;
            double Huffmann_dictionary_total_required_bits = 0;
            foreach (ParseVoynich.Chunk chunk in Voynich_parsing_results.chunks_dictionary.Values)
            {
                Huffmann_compressed_text_total_required_bits += chunk.number_of_times_used * chunk.Huffman_code.Length;

                Huffmann_dictionary_total_required_bits += chunk.chunk.Length * bits_per_character + chunk.Huffman_code.Length;
            }
            result += "Total number of bits required for the Huffmann codes dictionary: " + Huffmann_dictionary_total_required_bits + " (character set = " + charset_size + " characters)" + "\r\n";
            result += "Total number of bits required for the compressed text: " + Huffmann_compressed_text_total_required_bits + "\r\n";
            result += "Total number of bits required: " + (Huffmann_compressed_text_total_required_bits + Huffmann_dictionary_total_required_bits)  + "\r\n\r\n";



            result += "Chunkified grammar Nchunktypes: " + Voynich_parsing_results.total_number_of_chunktypes + "\r\n";

            // Calcolo di Nchunktokens
            int Nchunktokens = 0;
            foreach (Dictionary<string, EValueOcc> slot in Voynich_chunkified_grammar.slots_table)
            {
                Nchunktokens += slot.Count;
            }
            result += "Chunkified grammar Nchunktokens: " + Nchunktokens + "\r\n";

            // Calcolo di Ntransitions
            int Ntransitions = 0;
            foreach (Dictionary<string, PackGrammarAndWrite.SlotTransition> slot_transitions in Voynich_chunkified_grammar.transitions_table)
            {
                Ntransitions += slot_transitions.Count;
            }
            result += "Chunkified grammar Ntransitions: " + Ntransitions + "\r\n";
            result += "\r\n";


            result += "Grammar generated " + Voynich_parsing_results.total_wordtypes_found + " valid word types. ";
            result += "Grammar can generate " + Voynich_parsing_results.total_grammar_wordtypes + " total word types\r\n";

            result += "Total number of word types in the Voynich text: " + text_analyzer.analysis_results.vocabulary_words_distribution.Count;
            result += " (" + total_words_without_rare_characters + " excluding word types with rare characters)\r\n";

            result += "Word types coverage = " + coverage + ", Efficiency = " + efficiency + ", F1 score = " + 2 * efficiency * coverage / (coverage + efficiency) + "\r\n";
            result += "Word types coverage is " + coverage_excluding_rare_characters + " excluding word types with rare characters\r\n\r\n";
            result += "Word tokens coverage (excluding word tokens with rare characters): " + word_tokens_coverage + "\r\n\r\n";



            // Completiamo con la distribuzione word types vs. # of chunks
            long[] occurrences_vs_chunks_number = new long[Form1.Voynich_current_max_loop_repeats];
            long total_occurrences = 0;
            foreach (ParseVoynich.WordParseData word in Voynich_parsing_results.data)
            {
                if (word.word_found == true)
                {
                    occurrences_vs_chunks_number[word.number_of_words_chunks - 1] += 1;
                    total_occurrences += 1;
                }
            }

            result += "Word types distribution vs. number of chunks:\r\n\r\n";
            int chunk_number = 1;
            foreach (long occurrence in occurrences_vs_chunks_number)
            {
                result += chunk_number + " chunk(s): \t" + ((float)occurrence / (float)total_occurrences) +"\r\n" ;
                chunk_number++;
            }


            return result;
        }








        private string display_chunkified_words_list(ref float word_tokens_coverage)
        {

            // Lista delle parole trovate e dei chunks as-parsed, + lista delle parole NOTFOUND, + lista delle parole RARECHARS

            // Quello che voglio è ottenere tre 'colonne': parole FOUND, parole NOTFOUND e parole NOTFOUND_BUT_WITH_RARECHARS
            //   Ergo devo creare tre liste separate
            List<ParseVoynich.WordParseData> found_words = new List<ParseVoynich.WordParseData>();
            List<ParseVoynich.WordParseData> notfound_words = new List<ParseVoynich.WordParseData>();
            List<ParseVoynich.WordParseData> rarechars_words = new List<ParseVoynich.WordParseData>();

            // Già che ci siamo calcoliamo anche il numero totale di _tokens_ trovati e il numero totale di _tokens_ con RARECHARS
            //   per poter calcolare il Word _tokens_ coverage
            long total_tokens_found = 0;
            long total_tokens_with_rarechars_discarded = 0;

            foreach (ParseVoynich.WordParseData word_data in Voynich_parsing_results.data)
            {
                if (word_data.word_found == true)
                {
                    found_words.Add(word_data);

                    total_tokens_found += word_data.occurrences;
                    continue;
                }
                if (word_data.word_found == false && word_data.word_not_found_but_has_rare_characters == false)
                {
                    notfound_words.Add(word_data);
                    continue;
                }
                rarechars_words.Add(word_data);
                total_tokens_with_rarechars_discarded += word_data.occurrences;
            }

            word_tokens_coverage = (float)total_tokens_found / (float)(text_analyzer.analysis_results.total_number_of_words_in_the_text - total_tokens_with_rarechars_discarded);


            string result = "";

            int max_list_length = found_words.Count;
            if (notfound_words.Count > max_list_length) max_list_length = notfound_words.Count;
            if (rarechars_words.Count > max_list_length) max_list_length = rarechars_words.Count;

            // La prima 'colonna' è composta da [word + \t] + [occurrences + \t] + [chunksnumber + \t] + [(chunk + \t)*max_loop_repeats campi] + [separatore]

            int first_column_fields = 1 + 1 + 1 + Voynich_parsing_results.grammar.max_loop_repeats + 2;

            // Intestazione
            result += "Word type" + "\t" + "Occurrences" + "\t" + "# of chunks" + "\t" + "Chunkification" + "\t";  // consuma 4 'campi'
            for (int i = 0; i < first_column_fields - 4; i++)  // tab-filling per la seconda 'colonna'
            {
                result += " \t";
            }

            // altre colonne
            result += "Words not found" + "\t" + " \t";
            result += "Words discarded (with 'rare' characters)";
            result += "\r\n\r\n";



            for (int i = 0; i < max_list_length; i++)
            {
                if (i < found_words.Count)
                {
                    result += write_word_and_chunks(found_words[i], first_column_fields);  // aggiunge anche le tabs che servono
                }
                else
                {
                    // Fill with tabs
                    for (int j = 0; j < first_column_fields; j++)
                    {
                        result += " \t";
                    }
                }

                if (i < notfound_words.Count)
                {
                    result += notfound_words[i].word + "\t" + notfound_words[i].occurrences + "\t \t";
                }
                else
                {
                    // Fill with tabs
                    result += " \t" + " \t" + " \t";
                }

                if (i < rarechars_words.Count)
                {
                    result += rarechars_words[i].word + "\t" + rarechars_words[i].occurrences;
                }


                result += "\r\n";
            }

            return result;

        }

        private string write_word_and_chunks(ParseVoynich.WordParseData word_data, int max_number_of_chunks)
        {
            string out_string = "";

            out_string += word_data.word + "\t" + word_data.occurrences + "\t" + word_data.number_of_words_chunks + "\t";

            int displayed_chunks_counter = 0;       //Serve per incolonnare la seconda parte
            if (word_data.parsed_word != null)    // si sa mai...
            {
                for (int i = 0; i < word_data.parsed_word.Count; i++)
                {
                    for (int j = 0; j < word_data.parsed_word[i].Count; j++)
                    {
                        out_string += word_data.parsed_word[i][j].element;
                    }
                    out_string += "\t";
                    displayed_chunks_counter++;
                }
            }

            // aggiungiamo i campi e le tabs che servono per incolonnare la seconda parte (va lasciato abbastanza spazio per max_number_of_chunks records possibili)
            //   Il -3 tiene conto dei campi iniziali nella prima 'colonna' (word, occurrences, # of chunks)
            for (int i = 0; i < max_number_of_chunks - displayed_chunks_counter - 3; i++)
            {
                out_string += " \t";
            }

            return out_string;
        }






        private string display_word_chunks_data()
        {

            string out_string = "";


            if (Voynich_show_chunks_categories == false)
            {
                // Visualizzazione chunks dictionary

                // Solito trasferimento in una list
                List<ParseVoynich.Chunk> chunks_list = new List<ParseVoynich.Chunk> ();
                foreach (ParseVoynich.Chunk chunk in Voynich_parsing_results.chunks_dictionary.Values)
                {
                    ParseVoynich.Chunk new_chunk = new ParseVoynich.Chunk();
                    new_chunk.chunk = chunk.chunk;
                    new_chunk.number_of_times_used = chunk.number_of_times_used;
                    new_chunk.Huffman_code = chunk.Huffman_code;
                    chunks_list.Add(new_chunk);
                }
                IComparer<ParseVoynich.Chunk> comparer = new ParseVoynich.CompareChunk_by_times_used();
                chunks_list.Sort(comparer);

                // E adesso scriviamo la stringa

                // Intestazione
                out_string += "Chunk\t# of times used\tHuffmann code\tCode length\t \tWARNING: when copying to Excel it removes leading zeroes, creating an apparent discrepancy with the code length\r\n";
                out_string += " \t \t \t \t \tTo avoid this, pre-format as 'Text' the column cells where the codes will be copied.\r\n\r\n";

                foreach (ParseVoynich.Chunk chunk in chunks_list)
                {
                    out_string += chunk.chunk + "\t" + chunk.number_of_times_used + "\t" + chunk.Huffman_code + "\t" + chunk.Huffman_code.Length + "\r\n";
                }

            }
            else
            {
                // Visualizzazione chunks categories

                // trasferiamo il vocabolario chunks_categories in una List per poterlo ordinare per occorrenze
                List<EValueOcc_extended> chunks_categories_list = new List<EValueOcc_extended>();
                foreach (EValueOcc_extended item in Voynich_parsing_results.chunks_categories.Values)
                {
                    EValueOcc_extended new_value = new EValueOcc_extended();
                    new_value.element = item.element;
                    new_value.element_additional = item.element_additional;
                    new_value.value = item.value;
                    chunks_categories_list.Add(new_value);
                }
                IComparer<EValueOcc_extended> comparer_by_value_occ = new TextCharsStats.CompareEValueOcc_extendedByValue();
                chunks_categories_list.Sort(comparer_by_value_occ);

                // Emmò scriviamo la stringa...

                // Intestazione
                out_string += "Category\t# of times used\tExample\tChunks list\r\n\r\n";

                foreach (EValueOcc_extended item in chunks_categories_list)
                {
                    out_string += item.element + "\t" + item.value + "\t(ie. found in " + item.element_additional + ")\t";

                    // E la lista dei chunks associata...
                    Dictionary<string, EValueOcc_extended> chunks_list = new Dictionary<string, EValueOcc_extended>();
                    if (Voynich_parsing_results.chunks_vs_categories.TryGetValue(item.element, out chunks_list) == true)
                    {
                        foreach (EValueOcc_extended chunks in chunks_list.Values)
                        {
                            out_string += chunks.element + " ";
                        }
                    }
                    else
                    {
                        mdError error = new mdError();
                        error.root("Software problem", "Cannot find chunks list");
                        error.Display_and_Clear();
                    }
                    out_string += "\r\n";
                }

                out_string += "\r\nFound " + Voynich_parsing_results.total_number_of_chunktypes + " chunks in " + Voynich_parsing_results.chunks_categories.Count + " categories.";
            }

          
            return out_string;
        }






        private static string display_chunkified_slot_grammar(PackGrammarAndWrite.ChunksGrammar chunks_grammar)
        {

            // Solita palla Dictionaries -> lists... uhhhh
            List<List<EValueOcc>> slots_table_list = new List<List<EValueOcc>>();
            foreach (Dictionary<string, EValueOcc> slot_dictionary in chunks_grammar.slots_table)
            {
                slots_table_list.Add(new List<EValueOcc>());
                foreach (EValueOcc chunk in slot_dictionary.Values)
                {
                    slots_table_list[slots_table_list.Count - 1].Add(chunk);
                }
                IComparer<EValueOcc> comparer = new TextCharsStats.CompareEValueOccByValue();
                slots_table_list[slots_table_list.Count - 1].Sort(comparer);
            }



            // Chunks in ogni slot
            string result = "";

            int max_chunks_number = 0;
            for (int i = 0; i < slots_table_list.Count; i++)
            {
                if (slots_table_list[i].Count > max_chunks_number)
                {
                    max_chunks_number = slots_table_list[i].Count;
                }
            }


            for (int i = 0; i < slots_table_list.Count; i++)
            {
                result += "SLOT" + (i + 1) + " " + "\t\t\t";  // Intestazione
            }
            result += "\r\n";
            for (int i = 0; i < slots_table_list.Count; i++)
            {
                result += slots_table_list[i].Count + "\ttotal chunks\t\t";  // Intestazione
            }
            result += "\r\n\r\n";
            for (int i = 0; i < slots_table_list.Count; i++)
            {
                result += "Chunk" + "\t" + "# of uses" + "\t\t";     // Intestazione
            }
            result += "\r\n";

            // Dati
            for (int chunk_slots_pointer = 0; chunk_slots_pointer < max_chunks_number; chunk_slots_pointer++)
            {
                for (int i = 0; i < slots_table_list.Count; i++)
                {
                    if (chunk_slots_pointer < slots_table_list[i].Count)
                    {
                        result += slots_table_list[i][chunk_slots_pointer].element + "\t" + slots_table_list[i][chunk_slots_pointer].value + "\t\t";
                    }
                    else
                    {
                        result += " " + "\t" + " " + "\t\t";
                    }
                }
                result += "\r\n";
            }

            return result;
        }






        private string display_transitions_table(PackGrammarAndWrite.ChunksGrammar chunks_grammar)
        {
            // Liste delle transizioni
            string result = "";

            // solita palla dictionaries -> list, con ordinamento
            List<List<PackGrammarAndWrite.SlotTransition>> transitions_table_list = new List<List<PackGrammarAndWrite.SlotTransition>>();
            foreach (Dictionary<string, PackGrammarAndWrite.SlotTransition> slot_dictionary in chunks_grammar.transitions_table)
            {
                transitions_table_list.Add(new List<PackGrammarAndWrite.SlotTransition>());
                foreach (PackGrammarAndWrite.SlotTransition transition in slot_dictionary.Values)
                {
                    transitions_table_list[transitions_table_list.Count - 1].Add(transition);
                }
                IComparer<PackGrammarAndWrite.SlotTransition> comparer = new PackGrammarAndWrite.CompareSlotTransitionByValue();
                transitions_table_list[transitions_table_list.Count - 1].Sort(comparer);
            }

            // Intestazione
            for (int i = 0; i < transitions_table_list.Count; i++)
            {
                result += "Found: " + transitions_table_list[i].Count + "\t";
                if (i == 0)
                {
                    result += "START -> Chunk 1" + "\t" + "\t\t";
                }
                else
                {
                    if (i == (transitions_table_list.Count - 1))
                    {
                        result += "Chunk " + i + " -> END" + "\t" + "\t\t";
                    }
                    else
                    {
                        result += "Chunk " + i + " -> Chunk " + (i + 1) + "\t" + "\t\t";
                    }
                }
            }
            result += "\r\n";
            for (int i = 0; i < transitions_table_list.Count; i++)
            {
                result += "Preceding" + "\t" + "Following" + "\t" + "Occurrences" + "\t\t";  // Intestazione
            }
            result += "\r\n\r\n";

            int max_transitions_number = 0;
            for (int i = 0; i < transitions_table_list.Count; i++)
            {
                if (transitions_table_list[i].Count > max_transitions_number)
                {
                    max_transitions_number = transitions_table_list[i].Count;
                }
            }


            for (int transitions_pointer = 0; transitions_pointer < max_transitions_number; transitions_pointer++)
            {
                for (int i = 0; i < transitions_table_list.Count; i++)
                {
                    if (transitions_pointer < transitions_table_list[i].Count)
                    {
                        result += transitions_table_list[i][transitions_pointer].preceding_chunk + "\t" + transitions_table_list[i][transitions_pointer].following_chunk + "\t";
                        result += transitions_table_list[i][transitions_pointer].occurrences + "\t\t";
                    }
                    else
                    {
                        result += " " + "\t" + " " + "\t" + " " + "\t\t";
                    }
                }
                result += "\r\n";
            }

            return result;
        }




        private void comboBox_select_Voynich_grammar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ricreiamo la grammatica
            ParseGrammarsLibrary.get_parsing_grammar(comboBox_select_Voynich_grammar.SelectedIndex, ref Voynich_parsing_results);

            // E settiamo il default del # ripetizioni
            Voynich_current_max_loop_repeats = Voynich_parsing_results.grammar.default_loop_repeats;
            textBox_Voynich_loop_repeats.Text = Voynich_current_max_loop_repeats.ToString();
        }

        private void button_WrVoynich_write_asemic_Click(object sender, EventArgs e)
        {

            mdError error = new mdError();

            if (Voynich_chunkified_grammar == null)
            {
                error.root("", "Parse a Voynich file first");
                error.Display_and_Clear();
                return;
            }

            disable_controls_while_threads_are_running();

            newline_to_mainStatusWindow("Starting generation of the asemic text...");
            string asemic_text = PackGrammarAndWrite.Markov_asemic_writer(Voynich_chunkified_grammar.transitions_table);

            newline_to_mainStatusWindow("Formatting data for display...");
            textBox_WrVoynich_text_out.Text = asemic_text;  // questa è facile da visualizzare....

            enable_controls_when_thread_stops();

            newline_to_mainStatusWindow("Processing completed\r\n");
        }






        private void textBox_Voynich_loop_repeats_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_Voynich_loop_repeats.Text, out value) == true)
            {
                if (value >= 1 && value <= Voynich_parsing_results.grammar.max_loop_repeats)
                {
                    Voynich_current_max_loop_repeats = value;
                }
            }
            textBox_Voynich_loop_repeats.Text = Voynich_current_max_loop_repeats.ToString();
        }

        private void textBox_Voynich_loop_repeats_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_Voynich_loop_repeats_Leave(sender, e);
        }




        private void button_Voynich_run_WSET_trivial_grammar_Click(object sender, EventArgs e)
        {
            if (text_analyzer.analysis_results.vocabulary_words_distribution.Count == 0)
            {
                mdError error = new mdError();
                error.root("", "No analysis loaded: create an analysis from .txt file or load an analysis first");
                error.Display_and_Clear();
                return;
            }

            // Else creiamo la grammatica Trivial WSET (che ha bisogno del vocabolario per poter essere calcolata!)
            Voynich_parsing_results.grammar = ParseGrammarsLibrary.Trivial_WSET();

            int old_max_repeats = Voynich_current_max_loop_repeats;
            Voynich_current_max_loop_repeats = 1;    // solo 1 ripetizione per questa grammatica!               

            // E lanciamo il resto dell'elaborazione (che include già disable_controls e enable_controls)
            button_Voynich_run_parse_Click(sender, e);

            Voynich_current_max_loop_repeats = old_max_repeats;   // restore
        }



        private void checkBoxVoynich_show_chunk_categories_CheckedChanged(object sender, EventArgs e)
        {
            Voynich_show_chunks_categories = checkBoxVoynich_show_chunk_categories.Checked;

            string result = "";

            if (Voynich_parsing_results.chunks_categories.Count != 0)
            {
                result = display_word_chunks_data();
                textBox_Voynich_chunks_types.Text = result;
            }

        }



        private void textBox_WrVoynich_random_seed_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_WrVoynich_random_seed_Leave(sender, e);
        }

        private void textBox_WrVoynich_random_seed_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_WrVoynich_random_seed.Text, out value) == true)
            {
                Voynich_asemic_random_seed = value;
            }
            textBox_WrVoynich_random_seed.Text = Voynich_asemic_random_seed.ToString();
        }








        // ESEMPIO DI ACQUISIZIONE DI UN VALORE DA UNA textBox (da AHelper)
        //  Notare la gestione di Enter e Esc, E SOPRATTUTTO LA GESTIONE DEL FLAG is_saved

        /*        
        private void textBox_pioneers_Leave(object sender, EventArgs e)
        {
            num_pioneers_getvalue();
        }

        private void textBox_pioneers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) num_pioneers_getvalue();
        }
        private void num_pioneers_getvalue()
        {
            int value;
            if (int.TryParse(textBox_pioneers.Text, out value) == true)
            {
                if (value >= 0)
                {
                    kingdom.isles[current_isle].num_pioneers = value;
                    file_di_esempio.is_saved = false;
                }
            }
            draw_map();
        }
        */
        // FINE ESEMPIO ACQUISIZIONE VALORI DA UNA textBox


        // ESEMPIO DI ACQUISIZIONE VALORE DA UNA comboList (da Digiworm 010 014)
        //private void plot1_name_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (plot1_name_comboBox.SelectedIndex < 0) return;
        //    plots_settings.plots_settings[0].number = plot1_name_comboBox.SelectedIndex;


        //    RunModel();

        //}
        // FINE ESEMPIO ACQUISIZIONE VALORE DA UNA comboList


        // ESEMPIO DI USO DI MOUSEHOVER (E ANCHE DI MOUSEMOVE)
        /*

        void mousehover_Timer_Elapsed(object source, ElapsedEventArgs e)
        {
            if (this.pictureBox1.InvokeRequired == true)
            {
                // se un eccezione viene alzata qua in realtà si è verificato qualche casino in draw_map !!
                //   di solito oggetti complessi non inizializzati, o un loop infinito
                this.Invoke(delegate_draw_map_routine);
            }
            else
            {
                draw_map();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            mousehover_Timer.Stop();    // spegni subito il timer, altrimenti si corre il rischio che
                                        //  scada mentre stiamo processando l'evento mousehover, con
                                        //  conseguenti casini

            mouse_position.X = e.X;
            mouse_position.Y = e.Y;

            mousehover_Timer.Start(); // va fatto partire ad OGNI mousevents
        }
        */
        // FINE ESEMPIO USO DI MOUSEHOVER E MOUSEMOVE





    }


}
