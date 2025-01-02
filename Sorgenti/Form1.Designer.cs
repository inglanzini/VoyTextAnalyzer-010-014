namespace Template
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAndAnalyzeTextFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analysisFileHeaderInfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.massAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.massAnalyzeAndSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.massAggregateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findBestMatchesInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareAnalysisFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAnalysisClustersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preprocessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox_mainStatusWindow = new System.Windows.Forms.TextBox();
            this.button_LoadTexts = new System.Windows.Forms.Button();
            this.MainTab = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.splitContainer_single_chars_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_single_chars_data = new System.Windows.Forms.SplitContainer();
            this.button_singlechars_with_spaces_select_all = new System.Windows.Forms.Button();
            this.textBox_single_characters_distribution = new System.Windows.Forms.TextBox();
            this.button_singlechars_without_spaces_select_all = new System.Windows.Forms.Button();
            this.textBox_single_characters_distribution_nospaces = new System.Windows.Forms.TextBox();
            this.splitContainer_single_chars_graphs = new System.Windows.Forms.SplitContainer();
            this.pictureBox_single_characters_distribution_graph = new System.Windows.Forms.PictureBox();
            this.pictureBox_single_characters_distribution_graph_no_spaces = new System.Windows.Forms.PictureBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.splitContainer_bigrams_main = new System.Windows.Forms.SplitContainer();
            this.checkBox_bigrams_symmetric = new System.Windows.Forms.CheckBox();
            this.button_select_all_text_bigrams = new System.Windows.Forms.Button();
            this.checkBox_bigrams_delta = new System.Windows.Forms.CheckBox();
            this.checkBox_bigrams_displaytable = new System.Windows.Forms.CheckBox();
            this.textBox_bigrams = new System.Windows.Forms.TextBox();
            this.pictureBox_bigrams = new System.Windows.Forms.PictureBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.splitContainer_cXc_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_cXc_data = new System.Windows.Forms.SplitContainer();
            this.textBox_Xc_table = new System.Windows.Forms.TextBox();
            this.button_Xc_select_all = new System.Windows.Forms.Button();
            this.textBox_cX_table = new System.Windows.Forms.TextBox();
            this.button_cX_select_all = new System.Windows.Forms.Button();
            this.splitContainer_cXc_graphs = new System.Windows.Forms.SplitContainer();
            this.pictureBox_Xc = new System.Windows.Forms.PictureBox();
            this.pictureBox_cX = new System.Windows.Forms.PictureBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.splitContainer_distances_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_distances_data = new System.Windows.Forms.SplitContainer();
            this.textBox_chars_distances_Xc = new System.Windows.Forms.TextBox();
            this.button_distances_Xc_select_all = new System.Windows.Forms.Button();
            this.textBox_chars_distances_cX = new System.Windows.Forms.TextBox();
            this.button_distances_cX_select_all = new System.Windows.Forms.Button();
            this.splitContainer_distances_graphs = new System.Windows.Forms.SplitContainer();
            this.pictureBox_distances_Xc = new System.Windows.Forms.PictureBox();
            this.pictureBox_distances_cX = new System.Windows.Forms.PictureBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.splitContainer_vocabulary_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_vocabulary_data = new System.Windows.Forms.SplitContainer();
            this.button_vocabulary_select_all = new System.Windows.Forms.Button();
            this.textBox_vocabulary = new System.Windows.Forms.TextBox();
            this.checkBox_words_length_distribution_in_text = new System.Windows.Forms.CheckBox();
            this.button_words_length_select_all = new System.Windows.Forms.Button();
            this.textBox_words_length_distribution = new System.Windows.Forms.TextBox();
            this.splitContainer_vocabulary_graphs = new System.Windows.Forms.SplitContainer();
            this.pictureBox_vocabulary_distribution = new System.Windows.Forms.PictureBox();
            this.pictureBox_words_length_distribution = new System.Windows.Forms.PictureBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.splitContainer_syllables_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_syllables_data = new System.Windows.Forms.SplitContainer();
            this.button_syllables_single_vowels_select_all = new System.Windows.Forms.Button();
            this.textBox_syllables_single_vowel = new System.Windows.Forms.TextBox();
            this.button_syllables_multiple_vowels_select_all = new System.Windows.Forms.Button();
            this.textBox_syllables_multiple_vowels = new System.Windows.Forms.TextBox();
            this.splitContainer_syllables_graphs = new System.Windows.Forms.SplitContainer();
            this.pictureBox_syllables_single_vowel = new System.Windows.Forms.PictureBox();
            this.pictureBox_syllables_multiple_vowels = new System.Windows.Forms.PictureBox();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.splitContainer_report = new System.Windows.Forms.SplitContainer();
            this.button_report_select_all = new System.Windows.Forms.Button();
            this.textBox_report = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer_bigrams_theoric_main = new System.Windows.Forms.SplitContainer();
            this.button_select_all_text_bigrams_theoric = new System.Windows.Forms.Button();
            this.checkBox_bigrams_theoric_displaytable = new System.Windows.Forms.CheckBox();
            this.textBox_bigrams_theoric = new System.Windows.Forms.TextBox();
            this.pictureBox_bigrams_theoric = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer_preprocessing = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_remarks = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Regex_commands = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox_source_text = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_raw_source_text = new System.Windows.Forms.TextBox();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.splitContainer_Voynich_main = new System.Windows.Forms.SplitContainer();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_Voynich_loop_repeats = new System.Windows.Forms.TextBox();
            this.comboBox_select_Voynich_grammar = new System.Windows.Forms.ComboBox();
            this.button_Voynich_run_parse = new System.Windows.Forms.Button();
            this.textBox_Voynich_report = new System.Windows.Forms.TextBox();
            this.splitContainer_Voynich_outpanel = new System.Windows.Forms.SplitContainer();
            this.button_Voynich_run_WSET_trivial_grammar = new System.Windows.Forms.Button();
            this.textBox_Voynich_parsing = new System.Windows.Forms.TextBox();
            this.checkBoxVoynich_show_chunk_categories = new System.Windows.Forms.CheckBox();
            this.textBox_Voynich_chunks_types = new System.Windows.Forms.TextBox();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.splitContainer_WrVoynich_main = new System.Windows.Forms.SplitContainer();
            this.textBox_WrVoynich_chunkified_grammar = new System.Windows.Forms.TextBox();
            this.splitContainer_WrVoynich_outpanel = new System.Windows.Forms.SplitContainer();
            this.textBox_WrVoynich_transitions = new System.Windows.Forms.TextBox();
            this.button_WrVoynich_write_asemic = new System.Windows.Forms.Button();
            this.textBox_WrVoynich_text_out = new System.Windows.Forms.TextBox();
            this.trackBar_linear_size = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_linear_size = new System.Windows.Forms.TextBox();
            this.trackBar_table_size = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_table_size = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox_WrVoynich_random_seed = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_main)).BeginInit();
            this.splitContainer_single_chars_main.Panel1.SuspendLayout();
            this.splitContainer_single_chars_main.Panel2.SuspendLayout();
            this.splitContainer_single_chars_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_data)).BeginInit();
            this.splitContainer_single_chars_data.Panel1.SuspendLayout();
            this.splitContainer_single_chars_data.Panel2.SuspendLayout();
            this.splitContainer_single_chars_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_graphs)).BeginInit();
            this.splitContainer_single_chars_graphs.Panel1.SuspendLayout();
            this.splitContainer_single_chars_graphs.Panel2.SuspendLayout();
            this.splitContainer_single_chars_graphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single_characters_distribution_graph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single_characters_distribution_graph_no_spaces)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bigrams_main)).BeginInit();
            this.splitContainer_bigrams_main.Panel1.SuspendLayout();
            this.splitContainer_bigrams_main.Panel2.SuspendLayout();
            this.splitContainer_bigrams_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bigrams)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_main)).BeginInit();
            this.splitContainer_cXc_main.Panel1.SuspendLayout();
            this.splitContainer_cXc_main.Panel2.SuspendLayout();
            this.splitContainer_cXc_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_data)).BeginInit();
            this.splitContainer_cXc_data.Panel1.SuspendLayout();
            this.splitContainer_cXc_data.Panel2.SuspendLayout();
            this.splitContainer_cXc_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_graphs)).BeginInit();
            this.splitContainer_cXc_graphs.Panel1.SuspendLayout();
            this.splitContainer_cXc_graphs.Panel2.SuspendLayout();
            this.splitContainer_cXc_graphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Xc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cX)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_main)).BeginInit();
            this.splitContainer_distances_main.Panel1.SuspendLayout();
            this.splitContainer_distances_main.Panel2.SuspendLayout();
            this.splitContainer_distances_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_data)).BeginInit();
            this.splitContainer_distances_data.Panel1.SuspendLayout();
            this.splitContainer_distances_data.Panel2.SuspendLayout();
            this.splitContainer_distances_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_graphs)).BeginInit();
            this.splitContainer_distances_graphs.Panel1.SuspendLayout();
            this.splitContainer_distances_graphs.Panel2.SuspendLayout();
            this.splitContainer_distances_graphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_distances_Xc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_distances_cX)).BeginInit();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_main)).BeginInit();
            this.splitContainer_vocabulary_main.Panel1.SuspendLayout();
            this.splitContainer_vocabulary_main.Panel2.SuspendLayout();
            this.splitContainer_vocabulary_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_data)).BeginInit();
            this.splitContainer_vocabulary_data.Panel1.SuspendLayout();
            this.splitContainer_vocabulary_data.Panel2.SuspendLayout();
            this.splitContainer_vocabulary_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_graphs)).BeginInit();
            this.splitContainer_vocabulary_graphs.Panel1.SuspendLayout();
            this.splitContainer_vocabulary_graphs.Panel2.SuspendLayout();
            this.splitContainer_vocabulary_graphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_vocabulary_distribution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_words_length_distribution)).BeginInit();
            this.tabPage10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_main)).BeginInit();
            this.splitContainer_syllables_main.Panel1.SuspendLayout();
            this.splitContainer_syllables_main.Panel2.SuspendLayout();
            this.splitContainer_syllables_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_data)).BeginInit();
            this.splitContainer_syllables_data.Panel1.SuspendLayout();
            this.splitContainer_syllables_data.Panel2.SuspendLayout();
            this.splitContainer_syllables_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_graphs)).BeginInit();
            this.splitContainer_syllables_graphs.Panel1.SuspendLayout();
            this.splitContainer_syllables_graphs.Panel2.SuspendLayout();
            this.splitContainer_syllables_graphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_syllables_single_vowel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_syllables_multiple_vowels)).BeginInit();
            this.tabPage11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_report)).BeginInit();
            this.splitContainer_report.Panel1.SuspendLayout();
            this.splitContainer_report.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bigrams_theoric_main)).BeginInit();
            this.splitContainer_bigrams_theoric_main.Panel1.SuspendLayout();
            this.splitContainer_bigrams_theoric_main.Panel2.SuspendLayout();
            this.splitContainer_bigrams_theoric_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bigrams_theoric)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_preprocessing)).BeginInit();
            this.splitContainer_preprocessing.Panel1.SuspendLayout();
            this.splitContainer_preprocessing.Panel2.SuspendLayout();
            this.splitContainer_preprocessing.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Voynich_main)).BeginInit();
            this.splitContainer_Voynich_main.Panel1.SuspendLayout();
            this.splitContainer_Voynich_main.Panel2.SuspendLayout();
            this.splitContainer_Voynich_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Voynich_outpanel)).BeginInit();
            this.splitContainer_Voynich_outpanel.Panel1.SuspendLayout();
            this.splitContainer_Voynich_outpanel.Panel2.SuspendLayout();
            this.splitContainer_Voynich_outpanel.SuspendLayout();
            this.tabPage13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_WrVoynich_main)).BeginInit();
            this.splitContainer_WrVoynich_main.Panel1.SuspendLayout();
            this.splitContainer_WrVoynich_main.Panel2.SuspendLayout();
            this.splitContainer_WrVoynich_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_WrVoynich_outpanel)).BeginInit();
            this.splitContainer_WrVoynich_outpanel.Panel1.SuspendLayout();
            this.splitContainer_WrVoynich_outpanel.Panel2.SuspendLayout();
            this.splitContainer_WrVoynich_outpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_linear_size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_table_size)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.massAnalysisToolStripMenuItem,
            this.massAggregateToolStripMenuItem,
            this.compareToolStripMenuItem,
            this.clusteringToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1283, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAndAnalyzeTextFilesToolStripMenuItem,
            this.toolStripSeparator1,
            this.loadToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.analysisFileHeaderInfosToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(62, 20);
            this.toolStripMenuItem1.Text = "Analysis";
            // 
            // loadAndAnalyzeTextFilesToolStripMenuItem
            // 
            this.loadAndAnalyzeTextFilesToolStripMenuItem.Name = "loadAndAnalyzeTextFilesToolStripMenuItem";
            this.loadAndAnalyzeTextFilesToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.loadAndAnalyzeTextFilesToolStripMenuItem.Text = "Create analysis from text";
            this.loadAndAnalyzeTextFilesToolStripMenuItem.Click += new System.EventHandler(this.loadAndAnalyzeTextFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.loadToolStripMenuItem.Text = "Load analysis file";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.saveAsToolStripMenuItem.Text = "Save analysis file as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // analysisFileHeaderInfosToolStripMenuItem
            // 
            this.analysisFileHeaderInfosToolStripMenuItem.Name = "analysisFileHeaderInfosToolStripMenuItem";
            this.analysisFileHeaderInfosToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.analysisFileHeaderInfosToolStripMenuItem.Text = "Analysis file header infos..";
            this.analysisFileHeaderInfosToolStripMenuItem.Click += new System.EventHandler(this.showAnalysisInfosToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // massAnalysisToolStripMenuItem
            // 
            this.massAnalysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.massAnalyzeAndSaveToolStripMenuItem});
            this.massAnalysisToolStripMenuItem.Name = "massAnalysisToolStripMenuItem";
            this.massAnalysisToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.massAnalysisToolStripMenuItem.Text = "Mass analysis";
            // 
            // massAnalyzeAndSaveToolStripMenuItem
            // 
            this.massAnalyzeAndSaveToolStripMenuItem.Name = "massAnalyzeAndSaveToolStripMenuItem";
            this.massAnalyzeAndSaveToolStripMenuItem.Size = new System.Drawing.Size(303, 22);
            this.massAnalyzeAndSaveToolStripMenuItem.Text = "Mass analyze texts and save all analysis files";
            this.massAnalyzeAndSaveToolStripMenuItem.Click += new System.EventHandler(this.massAnalyzeAndSaveToolStripMenuItem_Click);
            // 
            // massAggregateToolStripMenuItem
            // 
            this.massAggregateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1});
            this.massAggregateToolStripMenuItem.Name = "massAggregateToolStripMenuItem";
            this.massAggregateToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.massAggregateToolStripMenuItem.Text = "Mass aggregate";
            // 
            // massAggregateAnalysisInASingleCorpusToolStripMenuItem1
            // 
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1.Name = "massAggregateAnalysisInASingleCorpusToolStripMenuItem1";
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1.Size = new System.Drawing.Size(296, 22);
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1.Text = "Mass aggregate analysis in a single corpus";
            this.massAggregateAnalysisInASingleCorpusToolStripMenuItem1.Click += new System.EventHandler(this.massAggregateAnalysisInASingleCorpusToolStripMenuItem_Click);
            // 
            // compareToolStripMenuItem
            // 
            this.compareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findBestMatchesInToolStripMenuItem,
            this.compareAnalysisFilesToolStripMenuItem});
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compareToolStripMenuItem.Text = "Compare";
            // 
            // findBestMatchesInToolStripMenuItem
            // 
            this.findBestMatchesInToolStripMenuItem.Name = "findBestMatchesInToolStripMenuItem";
            this.findBestMatchesInToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.findBestMatchesInToolStripMenuItem.Text = "Find best matches in...";
            this.findBestMatchesInToolStripMenuItem.Click += new System.EventHandler(this.findBestMatchesInToolStripMenuItem_Click);
            // 
            // compareAnalysisFilesToolStripMenuItem
            // 
            this.compareAnalysisFilesToolStripMenuItem.Name = "compareAnalysisFilesToolStripMenuItem";
            this.compareAnalysisFilesToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.compareAnalysisFilesToolStripMenuItem.Text = "Compare current analysis with...";
            this.compareAnalysisFilesToolStripMenuItem.Click += new System.EventHandler(this.compareAnalysisFilesToolStripMenuItem_Click);
            // 
            // clusteringToolStripMenuItem
            // 
            this.clusteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findAnalysisClustersToolStripMenuItem});
            this.clusteringToolStripMenuItem.Name = "clusteringToolStripMenuItem";
            this.clusteringToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.clusteringToolStripMenuItem.Text = "Clustering";
            // 
            // findAnalysisClustersToolStripMenuItem
            // 
            this.findAnalysisClustersToolStripMenuItem.Name = "findAnalysisClustersToolStripMenuItem";
            this.findAnalysisClustersToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.findAnalysisClustersToolStripMenuItem.Text = "Find clusters in...";
            this.findAnalysisClustersToolStripMenuItem.Click += new System.EventHandler(this.findAnalysisClustersToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preprocessingToolStripMenuItem,
            this.visualizationToolStripMenuItem,
            this.savedFilesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // preprocessingToolStripMenuItem
            // 
            this.preprocessingToolStripMenuItem.Name = "preprocessingToolStripMenuItem";
            this.preprocessingToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.preprocessingToolStripMenuItem.Text = "Preprocessing";
            this.preprocessingToolStripMenuItem.Click += new System.EventHandler(this.preprocessingToolStripMenuItem_Click);
            // 
            // visualizationToolStripMenuItem
            // 
            this.visualizationToolStripMenuItem.Name = "visualizationToolStripMenuItem";
            this.visualizationToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.visualizationToolStripMenuItem.Text = "Visualization";
            this.visualizationToolStripMenuItem.Click += new System.EventHandler(this.visualizationToolStripMenuItem_Click);
            // 
            // savedFilesToolStripMenuItem
            // 
            this.savedFilesToolStripMenuItem.Name = "savedFilesToolStripMenuItem";
            this.savedFilesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.savedFilesToolStripMenuItem.Text = "Saved files";
            this.savedFilesToolStripMenuItem.Click += new System.EventHandler(this.savedFilesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // textBox_mainStatusWindow
            // 
            this.textBox_mainStatusWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_mainStatusWindow.Location = new System.Drawing.Point(0, 460);
            this.textBox_mainStatusWindow.Multiline = true;
            this.textBox_mainStatusWindow.Name = "textBox_mainStatusWindow";
            this.textBox_mainStatusWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_mainStatusWindow.Size = new System.Drawing.Size(1283, 109);
            this.textBox_mainStatusWindow.TabIndex = 1;
            // 
            // button_LoadTexts
            // 
            this.button_LoadTexts.Location = new System.Drawing.Point(4, 27);
            this.button_LoadTexts.Name = "button_LoadTexts";
            this.button_LoadTexts.Size = new System.Drawing.Size(141, 36);
            this.button_LoadTexts.TabIndex = 2;
            this.button_LoadTexts.Text = "Create analysis from text";
            this.button_LoadTexts.UseVisualStyleBackColor = true;
            this.button_LoadTexts.Click += new System.EventHandler(this.button_LoadTexts_Click);
            // 
            // MainTab
            // 
            this.MainTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTab.Controls.Add(this.tabPage4);
            this.MainTab.Controls.Add(this.tabPage6);
            this.MainTab.Controls.Add(this.tabPage7);
            this.MainTab.Controls.Add(this.tabPage8);
            this.MainTab.Controls.Add(this.tabPage9);
            this.MainTab.Controls.Add(this.tabPage10);
            this.MainTab.Controls.Add(this.tabPage11);
            this.MainTab.Controls.Add(this.tabPage5);
            this.MainTab.Controls.Add(this.tabPage2);
            this.MainTab.Controls.Add(this.tabPage3);
            this.MainTab.Controls.Add(this.tabPage1);
            this.MainTab.Controls.Add(this.tabPage12);
            this.MainTab.Controls.Add(this.tabPage13);
            this.MainTab.Location = new System.Drawing.Point(0, 68);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.ShowToolTips = true;
            this.MainTab.Size = new System.Drawing.Size(1283, 385);
            this.MainTab.TabIndex = 3;
            this.MainTab.SelectedIndexChanged += new System.EventHandler(this.MainTab_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.splitContainer_single_chars_main);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1275, 359);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Single chars";
            this.tabPage4.ToolTipText = "Shows the statistics of single characters";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // splitContainer_single_chars_main
            // 
            this.splitContainer_single_chars_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_single_chars_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_single_chars_main.Name = "splitContainer_single_chars_main";
            // 
            // splitContainer_single_chars_main.Panel1
            // 
            this.splitContainer_single_chars_main.Panel1.Controls.Add(this.splitContainer_single_chars_data);
            // 
            // splitContainer_single_chars_main.Panel2
            // 
            this.splitContainer_single_chars_main.Panel2.Controls.Add(this.splitContainer_single_chars_graphs);
            this.splitContainer_single_chars_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_single_chars_main.SplitterDistance = 425;
            this.splitContainer_single_chars_main.TabIndex = 0;
            this.splitContainer_single_chars_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_single_chars_main_SplitterMoved);
            // 
            // splitContainer_single_chars_data
            // 
            this.splitContainer_single_chars_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_single_chars_data.Location = new System.Drawing.Point(0, 3);
            this.splitContainer_single_chars_data.Name = "splitContainer_single_chars_data";
            // 
            // splitContainer_single_chars_data.Panel1
            // 
            this.splitContainer_single_chars_data.Panel1.Controls.Add(this.button_singlechars_with_spaces_select_all);
            this.splitContainer_single_chars_data.Panel1.Controls.Add(this.textBox_single_characters_distribution);
            // 
            // splitContainer_single_chars_data.Panel2
            // 
            this.splitContainer_single_chars_data.Panel2.Controls.Add(this.button_singlechars_without_spaces_select_all);
            this.splitContainer_single_chars_data.Panel2.Controls.Add(this.textBox_single_characters_distribution_nospaces);
            this.splitContainer_single_chars_data.Size = new System.Drawing.Size(422, 353);
            this.splitContainer_single_chars_data.SplitterDistance = 214;
            this.splitContainer_single_chars_data.TabIndex = 1;
            // 
            // button_singlechars_with_spaces_select_all
            // 
            this.button_singlechars_with_spaces_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_singlechars_with_spaces_select_all.Location = new System.Drawing.Point(136, 3);
            this.button_singlechars_with_spaces_select_all.Name = "button_singlechars_with_spaces_select_all";
            this.button_singlechars_with_spaces_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_singlechars_with_spaces_select_all.TabIndex = 5;
            this.button_singlechars_with_spaces_select_all.Text = "Select all";
            this.button_singlechars_with_spaces_select_all.UseVisualStyleBackColor = true;
            this.button_singlechars_with_spaces_select_all.Click += new System.EventHandler(this.button_singlechars_with_spaces_select_all_Click);
            // 
            // textBox_single_characters_distribution
            // 
            this.textBox_single_characters_distribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_single_characters_distribution.Location = new System.Drawing.Point(3, 30);
            this.textBox_single_characters_distribution.Multiline = true;
            this.textBox_single_characters_distribution.Name = "textBox_single_characters_distribution";
            this.textBox_single_characters_distribution.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_single_characters_distribution.Size = new System.Drawing.Size(208, 320);
            this.textBox_single_characters_distribution.TabIndex = 1;
            this.textBox_single_characters_distribution.WordWrap = false;
            // 
            // button_singlechars_without_spaces_select_all
            // 
            this.button_singlechars_without_spaces_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_singlechars_without_spaces_select_all.Location = new System.Drawing.Point(126, 3);
            this.button_singlechars_without_spaces_select_all.Name = "button_singlechars_without_spaces_select_all";
            this.button_singlechars_without_spaces_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_singlechars_without_spaces_select_all.TabIndex = 5;
            this.button_singlechars_without_spaces_select_all.Text = "Select all";
            this.button_singlechars_without_spaces_select_all.UseVisualStyleBackColor = true;
            this.button_singlechars_without_spaces_select_all.Click += new System.EventHandler(this.button_singlechars_without_spaces_select_all_Click);
            // 
            // textBox_single_characters_distribution_nospaces
            // 
            this.textBox_single_characters_distribution_nospaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_single_characters_distribution_nospaces.Location = new System.Drawing.Point(3, 30);
            this.textBox_single_characters_distribution_nospaces.Multiline = true;
            this.textBox_single_characters_distribution_nospaces.Name = "textBox_single_characters_distribution_nospaces";
            this.textBox_single_characters_distribution_nospaces.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_single_characters_distribution_nospaces.Size = new System.Drawing.Size(198, 320);
            this.textBox_single_characters_distribution_nospaces.TabIndex = 1;
            this.textBox_single_characters_distribution_nospaces.WordWrap = false;
            // 
            // splitContainer_single_chars_graphs
            // 
            this.splitContainer_single_chars_graphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_single_chars_graphs.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_single_chars_graphs.Name = "splitContainer_single_chars_graphs";
            this.splitContainer_single_chars_graphs.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_single_chars_graphs.Panel1
            // 
            this.splitContainer_single_chars_graphs.Panel1.Controls.Add(this.pictureBox_single_characters_distribution_graph);
            // 
            // splitContainer_single_chars_graphs.Panel2
            // 
            this.splitContainer_single_chars_graphs.Panel2.Controls.Add(this.pictureBox_single_characters_distribution_graph_no_spaces);
            this.splitContainer_single_chars_graphs.Size = new System.Drawing.Size(846, 359);
            this.splitContainer_single_chars_graphs.SplitterDistance = 157;
            this.splitContainer_single_chars_graphs.TabIndex = 0;
            this.splitContainer_single_chars_graphs.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_single_chars_graphs_SplitterMoved);
            // 
            // pictureBox_single_characters_distribution_graph
            // 
            this.pictureBox_single_characters_distribution_graph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_single_characters_distribution_graph.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_single_characters_distribution_graph.Name = "pictureBox_single_characters_distribution_graph";
            this.pictureBox_single_characters_distribution_graph.Size = new System.Drawing.Size(846, 157);
            this.pictureBox_single_characters_distribution_graph.TabIndex = 0;
            this.pictureBox_single_characters_distribution_graph.TabStop = false;
            // 
            // pictureBox_single_characters_distribution_graph_no_spaces
            // 
            this.pictureBox_single_characters_distribution_graph_no_spaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_single_characters_distribution_graph_no_spaces.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_single_characters_distribution_graph_no_spaces.Name = "pictureBox_single_characters_distribution_graph_no_spaces";
            this.pictureBox_single_characters_distribution_graph_no_spaces.Size = new System.Drawing.Size(846, 198);
            this.pictureBox_single_characters_distribution_graph_no_spaces.TabIndex = 0;
            this.pictureBox_single_characters_distribution_graph_no_spaces.TabStop = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.splitContainer_bigrams_main);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1275, 359);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Bigrams";
            this.tabPage6.ToolTipText = "Shows the statistics of bigrams";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // splitContainer_bigrams_main
            // 
            this.splitContainer_bigrams_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_bigrams_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_bigrams_main.Name = "splitContainer_bigrams_main";
            // 
            // splitContainer_bigrams_main.Panel1
            // 
            this.splitContainer_bigrams_main.Panel1.Controls.Add(this.checkBox_bigrams_symmetric);
            this.splitContainer_bigrams_main.Panel1.Controls.Add(this.button_select_all_text_bigrams);
            this.splitContainer_bigrams_main.Panel1.Controls.Add(this.checkBox_bigrams_delta);
            this.splitContainer_bigrams_main.Panel1.Controls.Add(this.checkBox_bigrams_displaytable);
            this.splitContainer_bigrams_main.Panel1.Controls.Add(this.textBox_bigrams);
            // 
            // splitContainer_bigrams_main.Panel2
            // 
            this.splitContainer_bigrams_main.Panel2.Controls.Add(this.pictureBox_bigrams);
            this.splitContainer_bigrams_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_bigrams_main.SplitterDistance = 425;
            this.splitContainer_bigrams_main.TabIndex = 0;
            this.splitContainer_bigrams_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_bigrams_main_SplitterMoved);
            // 
            // checkBox_bigrams_symmetric
            // 
            this.checkBox_bigrams_symmetric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_bigrams_symmetric.AutoSize = true;
            this.checkBox_bigrams_symmetric.Location = new System.Drawing.Point(219, 10);
            this.checkBox_bigrams_symmetric.Name = "checkBox_bigrams_symmetric";
            this.checkBox_bigrams_symmetric.Size = new System.Drawing.Size(125, 17);
            this.checkBox_bigrams_symmetric.TabIndex = 8;
            this.checkBox_bigrams_symmetric.Text = "Symmetric +/- values";
            this.toolTip1.SetToolTip(this.checkBox_bigrams_symmetric, "Improves readability: bigrams suppressed by a factor X are shown as -X, making th" +
        "em comparable (but of opposite sign) to bigrams which are enhanched");
            this.checkBox_bigrams_symmetric.UseVisualStyleBackColor = true;
            this.checkBox_bigrams_symmetric.CheckedChanged += new System.EventHandler(this.checkBox_bigrams_delta_symmetrized_CheckedChanged);
            // 
            // button_select_all_text_bigrams
            // 
            this.button_select_all_text_bigrams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select_all_text_bigrams.Location = new System.Drawing.Point(345, 6);
            this.button_select_all_text_bigrams.Name = "button_select_all_text_bigrams";
            this.button_select_all_text_bigrams.Size = new System.Drawing.Size(75, 23);
            this.button_select_all_text_bigrams.TabIndex = 7;
            this.button_select_all_text_bigrams.Text = "Select all";
            this.button_select_all_text_bigrams.UseVisualStyleBackColor = true;
            this.button_select_all_text_bigrams.Click += new System.EventHandler(this.button_select_all_text_bigrams_Click);
            // 
            // checkBox_bigrams_delta
            // 
            this.checkBox_bigrams_delta.AutoSize = true;
            this.checkBox_bigrams_delta.Location = new System.Drawing.Point(88, 10);
            this.checkBox_bigrams_delta.Name = "checkBox_bigrams_delta";
            this.checkBox_bigrams_delta.Size = new System.Drawing.Size(124, 17);
            this.checkBox_bigrams_delta.TabIndex = 6;
            this.checkBox_bigrams_delta.Text = "Variations vs. theoric";
            this.toolTip1.SetToolTip(this.checkBox_bigrams_delta, "Shows the bigram frequencies divided by the theoretical frequencies: this shows w" +
        "hich bigrams are enhanched (or suppressed) in the text (which depends on the spe" +
        "cific language)");
            this.checkBox_bigrams_delta.UseVisualStyleBackColor = true;
            this.checkBox_bigrams_delta.CheckedChanged += new System.EventHandler(this.checkBox_bigrams_delta_CheckedChanged);
            // 
            // checkBox_bigrams_displaytable
            // 
            this.checkBox_bigrams_displaytable.AutoSize = true;
            this.checkBox_bigrams_displaytable.Checked = true;
            this.checkBox_bigrams_displaytable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_bigrams_displaytable.Location = new System.Drawing.Point(3, 10);
            this.checkBox_bigrams_displaytable.Name = "checkBox_bigrams_displaytable";
            this.checkBox_bigrams_displaytable.Size = new System.Drawing.Size(62, 17);
            this.checkBox_bigrams_displaytable.TabIndex = 6;
            this.checkBox_bigrams_displaytable.Text = "Tabular";
            this.checkBox_bigrams_displaytable.UseVisualStyleBackColor = true;
            this.checkBox_bigrams_displaytable.CheckedChanged += new System.EventHandler(this.checkBox_bigrams_displaytable_CheckedChanged);
            // 
            // textBox_bigrams
            // 
            this.textBox_bigrams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_bigrams.Location = new System.Drawing.Point(-2, 33);
            this.textBox_bigrams.Multiline = true;
            this.textBox_bigrams.Name = "textBox_bigrams";
            this.textBox_bigrams.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_bigrams.Size = new System.Drawing.Size(429, 319);
            this.textBox_bigrams.TabIndex = 5;
            this.textBox_bigrams.WordWrap = false;
            // 
            // pictureBox_bigrams
            // 
            this.pictureBox_bigrams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_bigrams.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_bigrams.Name = "pictureBox_bigrams";
            this.pictureBox_bigrams.Size = new System.Drawing.Size(840, 353);
            this.pictureBox_bigrams.TabIndex = 2;
            this.pictureBox_bigrams.TabStop = false;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.splitContainer_cXc_main);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(1275, 359);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Following/previous";
            this.tabPage7.ToolTipText = "For each character, shows the probability of it being followed (or preceded) by a" +
    "nother character";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // splitContainer_cXc_main
            // 
            this.splitContainer_cXc_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_cXc_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_cXc_main.Name = "splitContainer_cXc_main";
            // 
            // splitContainer_cXc_main.Panel1
            // 
            this.splitContainer_cXc_main.Panel1.Controls.Add(this.splitContainer_cXc_data);
            // 
            // splitContainer_cXc_main.Panel2
            // 
            this.splitContainer_cXc_main.Panel2.Controls.Add(this.splitContainer_cXc_graphs);
            this.splitContainer_cXc_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_cXc_main.SplitterDistance = 425;
            this.splitContainer_cXc_main.TabIndex = 0;
            this.splitContainer_cXc_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_cXc_main_SplitterMoved);
            // 
            // splitContainer_cXc_data
            // 
            this.splitContainer_cXc_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_cXc_data.Location = new System.Drawing.Point(0, 3);
            this.splitContainer_cXc_data.Name = "splitContainer_cXc_data";
            // 
            // splitContainer_cXc_data.Panel1
            // 
            this.splitContainer_cXc_data.Panel1.Controls.Add(this.textBox_Xc_table);
            this.splitContainer_cXc_data.Panel1.Controls.Add(this.button_Xc_select_all);
            // 
            // splitContainer_cXc_data.Panel2
            // 
            this.splitContainer_cXc_data.Panel2.Controls.Add(this.textBox_cX_table);
            this.splitContainer_cXc_data.Panel2.Controls.Add(this.button_cX_select_all);
            this.splitContainer_cXc_data.Size = new System.Drawing.Size(426, 353);
            this.splitContainer_cXc_data.SplitterDistance = 210;
            this.splitContainer_cXc_data.TabIndex = 9;
            // 
            // textBox_Xc_table
            // 
            this.textBox_Xc_table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Xc_table.Location = new System.Drawing.Point(3, 32);
            this.textBox_Xc_table.Multiline = true;
            this.textBox_Xc_table.Name = "textBox_Xc_table";
            this.textBox_Xc_table.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Xc_table.Size = new System.Drawing.Size(204, 318);
            this.textBox_Xc_table.TabIndex = 6;
            this.textBox_Xc_table.WordWrap = false;
            // 
            // button_Xc_select_all
            // 
            this.button_Xc_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Xc_select_all.Location = new System.Drawing.Point(132, 3);
            this.button_Xc_select_all.Name = "button_Xc_select_all";
            this.button_Xc_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_Xc_select_all.TabIndex = 8;
            this.button_Xc_select_all.Text = "Select all";
            this.button_Xc_select_all.UseVisualStyleBackColor = true;
            this.button_Xc_select_all.Click += new System.EventHandler(this.button_Xc_select_all_Click);
            // 
            // textBox_cX_table
            // 
            this.textBox_cX_table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cX_table.Location = new System.Drawing.Point(4, 32);
            this.textBox_cX_table.Multiline = true;
            this.textBox_cX_table.Name = "textBox_cX_table";
            this.textBox_cX_table.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_cX_table.Size = new System.Drawing.Size(208, 318);
            this.textBox_cX_table.TabIndex = 6;
            this.textBox_cX_table.WordWrap = false;
            // 
            // button_cX_select_all
            // 
            this.button_cX_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cX_select_all.Location = new System.Drawing.Point(134, 3);
            this.button_cX_select_all.Name = "button_cX_select_all";
            this.button_cX_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_cX_select_all.TabIndex = 8;
            this.button_cX_select_all.Text = "Select all";
            this.button_cX_select_all.UseVisualStyleBackColor = true;
            this.button_cX_select_all.Click += new System.EventHandler(this.button_cX_select_all_Click);
            // 
            // splitContainer_cXc_graphs
            // 
            this.splitContainer_cXc_graphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_cXc_graphs.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_cXc_graphs.Name = "splitContainer_cXc_graphs";
            // 
            // splitContainer_cXc_graphs.Panel1
            // 
            this.splitContainer_cXc_graphs.Panel1.Controls.Add(this.pictureBox_Xc);
            // 
            // splitContainer_cXc_graphs.Panel2
            // 
            this.splitContainer_cXc_graphs.Panel2.Controls.Add(this.pictureBox_cX);
            this.splitContainer_cXc_graphs.Size = new System.Drawing.Size(846, 359);
            this.splitContainer_cXc_graphs.SplitterDistance = 404;
            this.splitContainer_cXc_graphs.TabIndex = 0;
            this.splitContainer_cXc_graphs.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_cXc_graphs_SplitterMoved);
            // 
            // pictureBox_Xc
            // 
            this.pictureBox_Xc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Xc.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Xc.Name = "pictureBox_Xc";
            this.pictureBox_Xc.Size = new System.Drawing.Size(398, 353);
            this.pictureBox_Xc.TabIndex = 4;
            this.pictureBox_Xc.TabStop = false;
            // 
            // pictureBox_cX
            // 
            this.pictureBox_cX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_cX.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_cX.Name = "pictureBox_cX";
            this.pictureBox_cX.Size = new System.Drawing.Size(432, 353);
            this.pictureBox_cX.TabIndex = 4;
            this.pictureBox_cX.TabStop = false;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.splitContainer_distances_main);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(1275, 359);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Chars distances";
            this.tabPage8.ToolTipText = "Shows the \'distance\' between each character, calculated by taking the geometrical" +
    " distance of the probabilities of each character to be followed (or preceded) by" +
    " another character";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // splitContainer_distances_main
            // 
            this.splitContainer_distances_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_distances_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_distances_main.Name = "splitContainer_distances_main";
            // 
            // splitContainer_distances_main.Panel1
            // 
            this.splitContainer_distances_main.Panel1.Controls.Add(this.splitContainer_distances_data);
            // 
            // splitContainer_distances_main.Panel2
            // 
            this.splitContainer_distances_main.Panel2.Controls.Add(this.splitContainer_distances_graphs);
            this.splitContainer_distances_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_distances_main.SplitterDistance = 425;
            this.splitContainer_distances_main.TabIndex = 0;
            this.splitContainer_distances_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_distances_main_SplitterMoved);
            // 
            // splitContainer_distances_data
            // 
            this.splitContainer_distances_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_distances_data.Location = new System.Drawing.Point(8, 3);
            this.splitContainer_distances_data.Name = "splitContainer_distances_data";
            // 
            // splitContainer_distances_data.Panel1
            // 
            this.splitContainer_distances_data.Panel1.Controls.Add(this.textBox_chars_distances_Xc);
            this.splitContainer_distances_data.Panel1.Controls.Add(this.button_distances_Xc_select_all);
            // 
            // splitContainer_distances_data.Panel2
            // 
            this.splitContainer_distances_data.Panel2.Controls.Add(this.textBox_chars_distances_cX);
            this.splitContainer_distances_data.Panel2.Controls.Add(this.button_distances_cX_select_all);
            this.splitContainer_distances_data.Size = new System.Drawing.Size(414, 353);
            this.splitContainer_distances_data.SplitterDistance = 206;
            this.splitContainer_distances_data.TabIndex = 11;
            // 
            // textBox_chars_distances_Xc
            // 
            this.textBox_chars_distances_Xc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chars_distances_Xc.Location = new System.Drawing.Point(3, 31);
            this.textBox_chars_distances_Xc.Multiline = true;
            this.textBox_chars_distances_Xc.Name = "textBox_chars_distances_Xc";
            this.textBox_chars_distances_Xc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_chars_distances_Xc.Size = new System.Drawing.Size(200, 319);
            this.textBox_chars_distances_Xc.TabIndex = 7;
            this.textBox_chars_distances_Xc.WordWrap = false;
            // 
            // button_distances_Xc_select_all
            // 
            this.button_distances_Xc_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_distances_Xc_select_all.Location = new System.Drawing.Point(128, 3);
            this.button_distances_Xc_select_all.Name = "button_distances_Xc_select_all";
            this.button_distances_Xc_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_distances_Xc_select_all.TabIndex = 9;
            this.button_distances_Xc_select_all.Text = "Select all";
            this.button_distances_Xc_select_all.UseVisualStyleBackColor = true;
            this.button_distances_Xc_select_all.Click += new System.EventHandler(this.button_distances_Xc_select_all_Click);
            // 
            // textBox_chars_distances_cX
            // 
            this.textBox_chars_distances_cX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chars_distances_cX.Location = new System.Drawing.Point(3, 31);
            this.textBox_chars_distances_cX.Multiline = true;
            this.textBox_chars_distances_cX.Name = "textBox_chars_distances_cX";
            this.textBox_chars_distances_cX.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_chars_distances_cX.Size = new System.Drawing.Size(198, 318);
            this.textBox_chars_distances_cX.TabIndex = 7;
            this.textBox_chars_distances_cX.WordWrap = false;
            // 
            // button_distances_cX_select_all
            // 
            this.button_distances_cX_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_distances_cX_select_all.Location = new System.Drawing.Point(126, 3);
            this.button_distances_cX_select_all.Name = "button_distances_cX_select_all";
            this.button_distances_cX_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_distances_cX_select_all.TabIndex = 9;
            this.button_distances_cX_select_all.Text = "Select all";
            this.button_distances_cX_select_all.UseVisualStyleBackColor = true;
            this.button_distances_cX_select_all.Click += new System.EventHandler(this.button_distances_cX_select_all_Click);
            // 
            // splitContainer_distances_graphs
            // 
            this.splitContainer_distances_graphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_distances_graphs.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_distances_graphs.Name = "splitContainer_distances_graphs";
            // 
            // splitContainer_distances_graphs.Panel1
            // 
            this.splitContainer_distances_graphs.Panel1.Controls.Add(this.pictureBox_distances_Xc);
            // 
            // splitContainer_distances_graphs.Panel2
            // 
            this.splitContainer_distances_graphs.Panel2.Controls.Add(this.pictureBox_distances_cX);
            this.splitContainer_distances_graphs.Size = new System.Drawing.Size(846, 359);
            this.splitContainer_distances_graphs.SplitterDistance = 423;
            this.splitContainer_distances_graphs.TabIndex = 0;
            this.splitContainer_distances_graphs.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_distances_graphs_SplitterMoved);
            // 
            // pictureBox_distances_Xc
            // 
            this.pictureBox_distances_Xc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_distances_Xc.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_distances_Xc.Name = "pictureBox_distances_Xc";
            this.pictureBox_distances_Xc.Size = new System.Drawing.Size(421, 353);
            this.pictureBox_distances_Xc.TabIndex = 5;
            this.pictureBox_distances_Xc.TabStop = false;
            // 
            // pictureBox_distances_cX
            // 
            this.pictureBox_distances_cX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_distances_cX.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_distances_cX.Name = "pictureBox_distances_cX";
            this.pictureBox_distances_cX.Size = new System.Drawing.Size(413, 353);
            this.pictureBox_distances_cX.TabIndex = 5;
            this.pictureBox_distances_cX.TabStop = false;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.splitContainer_vocabulary_main);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(1275, 359);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "Vocabulary";
            this.tabPage9.ToolTipText = "Shows the vocabulary (this assumes the character \'space\' is the actual word separ" +
    "ator in the text)";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // splitContainer_vocabulary_main
            // 
            this.splitContainer_vocabulary_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_vocabulary_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_vocabulary_main.Name = "splitContainer_vocabulary_main";
            // 
            // splitContainer_vocabulary_main.Panel1
            // 
            this.splitContainer_vocabulary_main.Panel1.Controls.Add(this.splitContainer_vocabulary_data);
            // 
            // splitContainer_vocabulary_main.Panel2
            // 
            this.splitContainer_vocabulary_main.Panel2.Controls.Add(this.splitContainer_vocabulary_graphs);
            this.splitContainer_vocabulary_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_vocabulary_main.SplitterDistance = 425;
            this.splitContainer_vocabulary_main.TabIndex = 0;
            this.splitContainer_vocabulary_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_vocabulary_main_SplitterMoved);
            // 
            // splitContainer_vocabulary_data
            // 
            this.splitContainer_vocabulary_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_vocabulary_data.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_vocabulary_data.Name = "splitContainer_vocabulary_data";
            // 
            // splitContainer_vocabulary_data.Panel1
            // 
            this.splitContainer_vocabulary_data.Panel1.Controls.Add(this.button_vocabulary_select_all);
            this.splitContainer_vocabulary_data.Panel1.Controls.Add(this.textBox_vocabulary);
            // 
            // splitContainer_vocabulary_data.Panel2
            // 
            this.splitContainer_vocabulary_data.Panel2.Controls.Add(this.checkBox_words_length_distribution_in_text);
            this.splitContainer_vocabulary_data.Panel2.Controls.Add(this.button_words_length_select_all);
            this.splitContainer_vocabulary_data.Panel2.Controls.Add(this.textBox_words_length_distribution);
            this.splitContainer_vocabulary_data.Size = new System.Drawing.Size(425, 359);
            this.splitContainer_vocabulary_data.SplitterDistance = 245;
            this.splitContainer_vocabulary_data.TabIndex = 0;
            // 
            // button_vocabulary_select_all
            // 
            this.button_vocabulary_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_vocabulary_select_all.Location = new System.Drawing.Point(167, 3);
            this.button_vocabulary_select_all.Name = "button_vocabulary_select_all";
            this.button_vocabulary_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_vocabulary_select_all.TabIndex = 10;
            this.button_vocabulary_select_all.Text = "Select all";
            this.button_vocabulary_select_all.UseVisualStyleBackColor = true;
            this.button_vocabulary_select_all.Click += new System.EventHandler(this.button_vocabulary_select_all_Click);
            // 
            // textBox_vocabulary
            // 
            this.textBox_vocabulary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_vocabulary.Location = new System.Drawing.Point(3, 32);
            this.textBox_vocabulary.Multiline = true;
            this.textBox_vocabulary.Name = "textBox_vocabulary";
            this.textBox_vocabulary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_vocabulary.Size = new System.Drawing.Size(239, 324);
            this.textBox_vocabulary.TabIndex = 8;
            this.textBox_vocabulary.WordWrap = false;
            // 
            // checkBox_words_length_distribution_in_text
            // 
            this.checkBox_words_length_distribution_in_text.AutoSize = true;
            this.checkBox_words_length_distribution_in_text.Checked = true;
            this.checkBox_words_length_distribution_in_text.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_words_length_distribution_in_text.Location = new System.Drawing.Point(10, 9);
            this.checkBox_words_length_distribution_in_text.Name = "checkBox_words_length_distribution_in_text";
            this.checkBox_words_length_distribution_in_text.Size = new System.Drawing.Size(47, 17);
            this.checkBox_words_length_distribution_in_text.TabIndex = 11;
            this.checkBox_words_length_distribution_in_text.Text = "Text";
            this.checkBox_words_length_distribution_in_text.UseVisualStyleBackColor = true;
            this.checkBox_words_length_distribution_in_text.CheckedChanged += new System.EventHandler(this.checkBox_words_length_distribution_in_text_CheckedChanged);
            // 
            // button_words_length_select_all
            // 
            this.button_words_length_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_words_length_select_all.Location = new System.Drawing.Point(98, 3);
            this.button_words_length_select_all.Name = "button_words_length_select_all";
            this.button_words_length_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_words_length_select_all.TabIndex = 10;
            this.button_words_length_select_all.Text = "Select all";
            this.button_words_length_select_all.UseVisualStyleBackColor = true;
            this.button_words_length_select_all.Click += new System.EventHandler(this.button_words_length_select_all_Click);
            // 
            // textBox_words_length_distribution
            // 
            this.textBox_words_length_distribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_words_length_distribution.Location = new System.Drawing.Point(3, 32);
            this.textBox_words_length_distribution.Multiline = true;
            this.textBox_words_length_distribution.Name = "textBox_words_length_distribution";
            this.textBox_words_length_distribution.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_words_length_distribution.Size = new System.Drawing.Size(170, 324);
            this.textBox_words_length_distribution.TabIndex = 8;
            this.textBox_words_length_distribution.WordWrap = false;
            // 
            // splitContainer_vocabulary_graphs
            // 
            this.splitContainer_vocabulary_graphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_vocabulary_graphs.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_vocabulary_graphs.Name = "splitContainer_vocabulary_graphs";
            this.splitContainer_vocabulary_graphs.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_vocabulary_graphs.Panel1
            // 
            this.splitContainer_vocabulary_graphs.Panel1.Controls.Add(this.pictureBox_vocabulary_distribution);
            // 
            // splitContainer_vocabulary_graphs.Panel2
            // 
            this.splitContainer_vocabulary_graphs.Panel2.Controls.Add(this.pictureBox_words_length_distribution);
            this.splitContainer_vocabulary_graphs.Size = new System.Drawing.Size(846, 359);
            this.splitContainer_vocabulary_graphs.SplitterDistance = 177;
            this.splitContainer_vocabulary_graphs.TabIndex = 0;
            this.splitContainer_vocabulary_graphs.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_vocabulary_graphs_SplitterMoved);
            // 
            // pictureBox_vocabulary_distribution
            // 
            this.pictureBox_vocabulary_distribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_vocabulary_distribution.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_vocabulary_distribution.Name = "pictureBox_vocabulary_distribution";
            this.pictureBox_vocabulary_distribution.Size = new System.Drawing.Size(840, 171);
            this.pictureBox_vocabulary_distribution.TabIndex = 1;
            this.pictureBox_vocabulary_distribution.TabStop = false;
            // 
            // pictureBox_words_length_distribution
            // 
            this.pictureBox_words_length_distribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_words_length_distribution.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_words_length_distribution.Name = "pictureBox_words_length_distribution";
            this.pictureBox_words_length_distribution.Size = new System.Drawing.Size(840, 171);
            this.pictureBox_words_length_distribution.TabIndex = 1;
            this.pictureBox_words_length_distribution.TabStop = false;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.splitContainer_syllables_main);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(1275, 359);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "Pseudo-syllables";
            this.tabPage10.ToolTipText = "Shows pseudo-syllables, a minimal syllabic decomposition which assumes vowels are" +
    " represented by the usual vowel symbols (aeiouàèìòù etc.)";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // splitContainer_syllables_main
            // 
            this.splitContainer_syllables_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_syllables_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_syllables_main.Name = "splitContainer_syllables_main";
            // 
            // splitContainer_syllables_main.Panel1
            // 
            this.splitContainer_syllables_main.Panel1.Controls.Add(this.splitContainer_syllables_data);
            // 
            // splitContainer_syllables_main.Panel2
            // 
            this.splitContainer_syllables_main.Panel2.Controls.Add(this.splitContainer_syllables_graphs);
            this.splitContainer_syllables_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_syllables_main.SplitterDistance = 425;
            this.splitContainer_syllables_main.TabIndex = 0;
            this.splitContainer_syllables_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_syllables_main_SplitterMoved);
            // 
            // splitContainer_syllables_data
            // 
            this.splitContainer_syllables_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_syllables_data.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_syllables_data.Name = "splitContainer_syllables_data";
            // 
            // splitContainer_syllables_data.Panel1
            // 
            this.splitContainer_syllables_data.Panel1.Controls.Add(this.button_syllables_single_vowels_select_all);
            this.splitContainer_syllables_data.Panel1.Controls.Add(this.textBox_syllables_single_vowel);
            // 
            // splitContainer_syllables_data.Panel2
            // 
            this.splitContainer_syllables_data.Panel2.Controls.Add(this.button_syllables_multiple_vowels_select_all);
            this.splitContainer_syllables_data.Panel2.Controls.Add(this.textBox_syllables_multiple_vowels);
            this.splitContainer_syllables_data.Size = new System.Drawing.Size(425, 359);
            this.splitContainer_syllables_data.SplitterDistance = 217;
            this.splitContainer_syllables_data.TabIndex = 0;
            // 
            // button_syllables_single_vowels_select_all
            // 
            this.button_syllables_single_vowels_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_syllables_single_vowels_select_all.Location = new System.Drawing.Point(139, 3);
            this.button_syllables_single_vowels_select_all.Name = "button_syllables_single_vowels_select_all";
            this.button_syllables_single_vowels_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_syllables_single_vowels_select_all.TabIndex = 11;
            this.button_syllables_single_vowels_select_all.Text = "Select all";
            this.button_syllables_single_vowels_select_all.UseVisualStyleBackColor = true;
            this.button_syllables_single_vowels_select_all.Click += new System.EventHandler(this.button_syllables_single_vowels_select_all_Click);
            // 
            // textBox_syllables_single_vowel
            // 
            this.textBox_syllables_single_vowel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_syllables_single_vowel.Location = new System.Drawing.Point(3, 32);
            this.textBox_syllables_single_vowel.Multiline = true;
            this.textBox_syllables_single_vowel.Name = "textBox_syllables_single_vowel";
            this.textBox_syllables_single_vowel.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_syllables_single_vowel.Size = new System.Drawing.Size(211, 324);
            this.textBox_syllables_single_vowel.TabIndex = 9;
            this.textBox_syllables_single_vowel.WordWrap = false;
            // 
            // button_syllables_multiple_vowels_select_all
            // 
            this.button_syllables_multiple_vowels_select_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_syllables_multiple_vowels_select_all.Location = new System.Drawing.Point(126, 3);
            this.button_syllables_multiple_vowels_select_all.Name = "button_syllables_multiple_vowels_select_all";
            this.button_syllables_multiple_vowels_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_syllables_multiple_vowels_select_all.TabIndex = 11;
            this.button_syllables_multiple_vowels_select_all.Text = "Select all";
            this.button_syllables_multiple_vowels_select_all.UseVisualStyleBackColor = true;
            this.button_syllables_multiple_vowels_select_all.Click += new System.EventHandler(this.button_syllables_multiple_vowels_select_all_Click);
            // 
            // textBox_syllables_multiple_vowels
            // 
            this.textBox_syllables_multiple_vowels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_syllables_multiple_vowels.Location = new System.Drawing.Point(3, 32);
            this.textBox_syllables_multiple_vowels.Multiline = true;
            this.textBox_syllables_multiple_vowels.Name = "textBox_syllables_multiple_vowels";
            this.textBox_syllables_multiple_vowels.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_syllables_multiple_vowels.Size = new System.Drawing.Size(198, 324);
            this.textBox_syllables_multiple_vowels.TabIndex = 9;
            this.textBox_syllables_multiple_vowels.WordWrap = false;
            // 
            // splitContainer_syllables_graphs
            // 
            this.splitContainer_syllables_graphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_syllables_graphs.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_syllables_graphs.Name = "splitContainer_syllables_graphs";
            this.splitContainer_syllables_graphs.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_syllables_graphs.Panel1
            // 
            this.splitContainer_syllables_graphs.Panel1.Controls.Add(this.pictureBox_syllables_single_vowel);
            // 
            // splitContainer_syllables_graphs.Panel2
            // 
            this.splitContainer_syllables_graphs.Panel2.Controls.Add(this.pictureBox_syllables_multiple_vowels);
            this.splitContainer_syllables_graphs.Size = new System.Drawing.Size(846, 359);
            this.splitContainer_syllables_graphs.SplitterDistance = 183;
            this.splitContainer_syllables_graphs.TabIndex = 0;
            this.splitContainer_syllables_graphs.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_syllables_graphs_SplitterMoved);
            // 
            // pictureBox_syllables_single_vowel
            // 
            this.pictureBox_syllables_single_vowel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_syllables_single_vowel.Location = new System.Drawing.Point(3, 6);
            this.pictureBox_syllables_single_vowel.Name = "pictureBox_syllables_single_vowel";
            this.pictureBox_syllables_single_vowel.Size = new System.Drawing.Size(840, 171);
            this.pictureBox_syllables_single_vowel.TabIndex = 2;
            this.pictureBox_syllables_single_vowel.TabStop = false;
            // 
            // pictureBox_syllables_multiple_vowels
            // 
            this.pictureBox_syllables_multiple_vowels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_syllables_multiple_vowels.Location = new System.Drawing.Point(2, 1);
            this.pictureBox_syllables_multiple_vowels.Name = "pictureBox_syllables_multiple_vowels";
            this.pictureBox_syllables_multiple_vowels.Size = new System.Drawing.Size(840, 171);
            this.pictureBox_syllables_multiple_vowels.TabIndex = 2;
            this.pictureBox_syllables_multiple_vowels.TabStop = false;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.splitContainer_report);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(1275, 359);
            this.tabPage11.TabIndex = 10;
            this.tabPage11.Text = "Report";
            this.tabPage11.ToolTipText = "Shows the general report of the analysis";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // splitContainer_report
            // 
            this.splitContainer_report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_report.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_report.Name = "splitContainer_report";
            // 
            // splitContainer_report.Panel1
            // 
            this.splitContainer_report.Panel1.Controls.Add(this.button_report_select_all);
            this.splitContainer_report.Panel1.Controls.Add(this.textBox_report);
            this.splitContainer_report.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_report.SplitterDistance = 812;
            this.splitContainer_report.TabIndex = 0;
            // 
            // button_report_select_all
            // 
            this.button_report_select_all.Location = new System.Drawing.Point(734, 3);
            this.button_report_select_all.Name = "button_report_select_all";
            this.button_report_select_all.Size = new System.Drawing.Size(75, 23);
            this.button_report_select_all.TabIndex = 11;
            this.button_report_select_all.Text = "Select all";
            this.button_report_select_all.UseVisualStyleBackColor = true;
            this.button_report_select_all.Click += new System.EventHandler(this.button_report_select_all_Click);
            // 
            // textBox_report
            // 
            this.textBox_report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_report.Location = new System.Drawing.Point(3, 27);
            this.textBox_report.Multiline = true;
            this.textBox_report.Name = "textBox_report";
            this.textBox_report.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_report.Size = new System.Drawing.Size(806, 329);
            this.textBox_report.TabIndex = 10;
            this.textBox_report.WordWrap = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.splitContainer_bigrams_theoric_main);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1275, 359);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Bigrams Theoric";
            this.tabPage5.ToolTipText = "Shows the theoretical bigrams frequencies, calculated from the frequencies of sin" +
    "gle characters and assuming the text is generated randomly";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // splitContainer_bigrams_theoric_main
            // 
            this.splitContainer_bigrams_theoric_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_bigrams_theoric_main.Location = new System.Drawing.Point(8, 3);
            this.splitContainer_bigrams_theoric_main.Name = "splitContainer_bigrams_theoric_main";
            // 
            // splitContainer_bigrams_theoric_main.Panel1
            // 
            this.splitContainer_bigrams_theoric_main.Panel1.Controls.Add(this.button_select_all_text_bigrams_theoric);
            this.splitContainer_bigrams_theoric_main.Panel1.Controls.Add(this.checkBox_bigrams_theoric_displaytable);
            this.splitContainer_bigrams_theoric_main.Panel1.Controls.Add(this.textBox_bigrams_theoric);
            // 
            // splitContainer_bigrams_theoric_main.Panel2
            // 
            this.splitContainer_bigrams_theoric_main.Panel2.Controls.Add(this.pictureBox_bigrams_theoric);
            this.splitContainer_bigrams_theoric_main.Size = new System.Drawing.Size(1264, 353);
            this.splitContainer_bigrams_theoric_main.SplitterDistance = 435;
            this.splitContainer_bigrams_theoric_main.TabIndex = 3;
            this.splitContainer_bigrams_theoric_main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_bigrams_theoric_main_SplitterMoved);
            // 
            // button_select_all_text_bigrams_theoric
            // 
            this.button_select_all_text_bigrams_theoric.Location = new System.Drawing.Point(76, 4);
            this.button_select_all_text_bigrams_theoric.Name = "button_select_all_text_bigrams_theoric";
            this.button_select_all_text_bigrams_theoric.Size = new System.Drawing.Size(75, 23);
            this.button_select_all_text_bigrams_theoric.TabIndex = 4;
            this.button_select_all_text_bigrams_theoric.Text = "Select all";
            this.button_select_all_text_bigrams_theoric.UseVisualStyleBackColor = true;
            this.button_select_all_text_bigrams_theoric.Click += new System.EventHandler(this.button_select_all_text_bigrams_theoric_Click);
            // 
            // checkBox_bigrams_theoric_displaytable
            // 
            this.checkBox_bigrams_theoric_displaytable.AutoSize = true;
            this.checkBox_bigrams_theoric_displaytable.Checked = true;
            this.checkBox_bigrams_theoric_displaytable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_bigrams_theoric_displaytable.Location = new System.Drawing.Point(8, 8);
            this.checkBox_bigrams_theoric_displaytable.Name = "checkBox_bigrams_theoric_displaytable";
            this.checkBox_bigrams_theoric_displaytable.Size = new System.Drawing.Size(62, 17);
            this.checkBox_bigrams_theoric_displaytable.TabIndex = 3;
            this.checkBox_bigrams_theoric_displaytable.Text = "Tabular";
            this.checkBox_bigrams_theoric_displaytable.UseVisualStyleBackColor = true;
            this.checkBox_bigrams_theoric_displaytable.CheckedChanged += new System.EventHandler(this.checkBox_bigrams_theoric_displaytable_CheckedChanged);
            // 
            // textBox_bigrams_theoric
            // 
            this.textBox_bigrams_theoric.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_bigrams_theoric.Location = new System.Drawing.Point(3, 31);
            this.textBox_bigrams_theoric.Multiline = true;
            this.textBox_bigrams_theoric.Name = "textBox_bigrams_theoric";
            this.textBox_bigrams_theoric.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_bigrams_theoric.Size = new System.Drawing.Size(429, 319);
            this.textBox_bigrams_theoric.TabIndex = 2;
            this.textBox_bigrams_theoric.WordWrap = false;
            // 
            // pictureBox_bigrams_theoric
            // 
            this.pictureBox_bigrams_theoric.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_bigrams_theoric.Location = new System.Drawing.Point(-1, 3);
            this.pictureBox_bigrams_theoric.Name = "pictureBox_bigrams_theoric";
            this.pictureBox_bigrams_theoric.Size = new System.Drawing.Size(833, 350);
            this.pictureBox_bigrams_theoric.TabIndex = 3;
            this.pictureBox_bigrams_theoric.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer_preprocessing);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1275, 359);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Preprocessing";
            this.tabPage2.ToolTipText = "Shows the comments (lines beginning with %c%) extracted from the text, and eventu" +
    "al pre-processing commands found in the comments";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer_preprocessing
            // 
            this.splitContainer_preprocessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_preprocessing.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_preprocessing.Name = "splitContainer_preprocessing";
            this.splitContainer_preprocessing.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_preprocessing.Panel1
            // 
            this.splitContainer_preprocessing.Panel1.Controls.Add(this.label2);
            this.splitContainer_preprocessing.Panel1.Controls.Add(this.textBox_remarks);
            // 
            // splitContainer_preprocessing.Panel2
            // 
            this.splitContainer_preprocessing.Panel2.Controls.Add(this.label3);
            this.splitContainer_preprocessing.Panel2.Controls.Add(this.textBox_Regex_commands);
            this.splitContainer_preprocessing.Size = new System.Drawing.Size(1269, 353);
            this.splitContainer_preprocessing.SplitterDistance = 174;
            this.splitContainer_preprocessing.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Comments (lines beginning with %c%) extracted from  text:";
            // 
            // textBox_remarks
            // 
            this.textBox_remarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_remarks.Location = new System.Drawing.Point(6, 14);
            this.textBox_remarks.Multiline = true;
            this.textBox_remarks.Name = "textBox_remarks";
            this.textBox_remarks.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_remarks.Size = new System.Drawing.Size(1221, 157);
            this.textBox_remarks.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Regex pre-processing commands found in the comments:";
            // 
            // textBox_Regex_commands
            // 
            this.textBox_Regex_commands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Regex_commands.Location = new System.Drawing.Point(6, 16);
            this.textBox_Regex_commands.Multiline = true;
            this.textBox_Regex_commands.Name = "textBox_Regex_commands";
            this.textBox_Regex_commands.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Regex_commands.Size = new System.Drawing.Size(1221, 136);
            this.textBox_Regex_commands.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_source_text);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1275, 359);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Cleaned text";
            this.tabPage3.ToolTipText = "Shows the pre-processed text actually used for the analysis";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox_source_text
            // 
            this.textBox_source_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_source_text.Location = new System.Drawing.Point(3, 20);
            this.textBox_source_text.Multiline = true;
            this.textBox_source_text.Name = "textBox_source_text";
            this.textBox_source_text.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_source_text.Size = new System.Drawing.Size(1264, 336);
            this.textBox_source_text.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(222, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Text after preprocessing, used in the analysis:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox_raw_source_text);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1275, 359);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Raw source text";
            this.tabPage1.ToolTipText = "Shows the raw source text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source text:";
            // 
            // textBox_raw_source_text
            // 
            this.textBox_raw_source_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_raw_source_text.Location = new System.Drawing.Point(6, 20);
            this.textBox_raw_source_text.Multiline = true;
            this.textBox_raw_source_text.Name = "textBox_raw_source_text";
            this.textBox_raw_source_text.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_raw_source_text.Size = new System.Drawing.Size(1263, 333);
            this.textBox_raw_source_text.TabIndex = 0;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.splitContainer_Voynich_main);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(1275, 359);
            this.tabPage12.TabIndex = 11;
            this.tabPage12.Text = "Slot grammar";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // splitContainer_Voynich_main
            // 
            this.splitContainer_Voynich_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Voynich_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Voynich_main.Name = "splitContainer_Voynich_main";
            // 
            // splitContainer_Voynich_main.Panel1
            // 
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.label8);
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.label7);
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.textBox_Voynich_loop_repeats);
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.comboBox_select_Voynich_grammar);
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.button_Voynich_run_parse);
            this.splitContainer_Voynich_main.Panel1.Controls.Add(this.textBox_Voynich_report);
            // 
            // splitContainer_Voynich_main.Panel2
            // 
            this.splitContainer_Voynich_main.Panel2.Controls.Add(this.splitContainer_Voynich_outpanel);
            this.splitContainer_Voynich_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_Voynich_main.SplitterDistance = 448;
            this.splitContainer_Voynich_main.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(200, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Max repeats";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Select grammar";
            // 
            // textBox_Voynich_loop_repeats
            // 
            this.textBox_Voynich_loop_repeats.Location = new System.Drawing.Point(210, 14);
            this.textBox_Voynich_loop_repeats.Name = "textBox_Voynich_loop_repeats";
            this.textBox_Voynich_loop_repeats.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_Voynich_loop_repeats.Size = new System.Drawing.Size(45, 20);
            this.textBox_Voynich_loop_repeats.TabIndex = 6;
            this.textBox_Voynich_loop_repeats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_Voynich_loop_repeats.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Voynich_loop_repeats_KeyDown);
            this.textBox_Voynich_loop_repeats.Leave += new System.EventHandler(this.textBox_Voynich_loop_repeats_Leave);
            // 
            // comboBox_select_Voynich_grammar
            // 
            this.comboBox_select_Voynich_grammar.FormattingEnabled = true;
            this.comboBox_select_Voynich_grammar.Location = new System.Drawing.Point(9, 14);
            this.comboBox_select_Voynich_grammar.Name = "comboBox_select_Voynich_grammar";
            this.comboBox_select_Voynich_grammar.Size = new System.Drawing.Size(185, 21);
            this.comboBox_select_Voynich_grammar.TabIndex = 4;
            this.comboBox_select_Voynich_grammar.SelectedIndexChanged += new System.EventHandler(this.comboBox_select_Voynich_grammar_SelectedIndexChanged);
            // 
            // button_Voynich_run_parse
            // 
            this.button_Voynich_run_parse.Location = new System.Drawing.Point(266, 3);
            this.button_Voynich_run_parse.Name = "button_Voynich_run_parse";
            this.button_Voynich_run_parse.Size = new System.Drawing.Size(93, 33);
            this.button_Voynich_run_parse.TabIndex = 3;
            this.button_Voynich_run_parse.Text = "Parse Text";
            this.button_Voynich_run_parse.UseVisualStyleBackColor = true;
            this.button_Voynich_run_parse.Click += new System.EventHandler(this.button_Voynich_run_parse_Click);
            // 
            // textBox_Voynich_report
            // 
            this.textBox_Voynich_report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Voynich_report.Location = new System.Drawing.Point(0, 36);
            this.textBox_Voynich_report.Multiline = true;
            this.textBox_Voynich_report.Name = "textBox_Voynich_report";
            this.textBox_Voynich_report.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Voynich_report.Size = new System.Drawing.Size(445, 320);
            this.textBox_Voynich_report.TabIndex = 2;
            this.textBox_Voynich_report.WordWrap = false;
            // 
            // splitContainer_Voynich_outpanel
            // 
            this.splitContainer_Voynich_outpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Voynich_outpanel.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Voynich_outpanel.Name = "splitContainer_Voynich_outpanel";
            // 
            // splitContainer_Voynich_outpanel.Panel1
            // 
            this.splitContainer_Voynich_outpanel.Panel1.Controls.Add(this.button_Voynich_run_WSET_trivial_grammar);
            this.splitContainer_Voynich_outpanel.Panel1.Controls.Add(this.textBox_Voynich_parsing);
            // 
            // splitContainer_Voynich_outpanel.Panel2
            // 
            this.splitContainer_Voynich_outpanel.Panel2.Controls.Add(this.checkBoxVoynich_show_chunk_categories);
            this.splitContainer_Voynich_outpanel.Panel2.Controls.Add(this.textBox_Voynich_chunks_types);
            this.splitContainer_Voynich_outpanel.Size = new System.Drawing.Size(823, 359);
            this.splitContainer_Voynich_outpanel.SplitterDistance = 481;
            this.splitContainer_Voynich_outpanel.TabIndex = 0;
            // 
            // button_Voynich_run_WSET_trivial_grammar
            // 
            this.button_Voynich_run_WSET_trivial_grammar.Location = new System.Drawing.Point(5, 2);
            this.button_Voynich_run_WSET_trivial_grammar.Name = "button_Voynich_run_WSET_trivial_grammar";
            this.button_Voynich_run_WSET_trivial_grammar.Size = new System.Drawing.Size(126, 34);
            this.button_Voynich_run_WSET_trivial_grammar.TabIndex = 3;
            this.button_Voynich_run_WSET_trivial_grammar.Text = "Parse text using WSET trivial garmmar";
            this.button_Voynich_run_WSET_trivial_grammar.UseVisualStyleBackColor = true;
            this.button_Voynich_run_WSET_trivial_grammar.Click += new System.EventHandler(this.button_Voynich_run_WSET_trivial_grammar_Click);
            // 
            // textBox_Voynich_parsing
            // 
            this.textBox_Voynich_parsing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Voynich_parsing.Location = new System.Drawing.Point(5, 36);
            this.textBox_Voynich_parsing.Multiline = true;
            this.textBox_Voynich_parsing.Name = "textBox_Voynich_parsing";
            this.textBox_Voynich_parsing.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Voynich_parsing.Size = new System.Drawing.Size(473, 320);
            this.textBox_Voynich_parsing.TabIndex = 2;
            this.textBox_Voynich_parsing.WordWrap = false;
            // 
            // checkBoxVoynich_show_chunk_categories
            // 
            this.checkBoxVoynich_show_chunk_categories.AutoSize = true;
            this.checkBoxVoynich_show_chunk_categories.Location = new System.Drawing.Point(14, 14);
            this.checkBoxVoynich_show_chunk_categories.Name = "checkBoxVoynich_show_chunk_categories";
            this.checkBoxVoynich_show_chunk_categories.Size = new System.Drawing.Size(138, 17);
            this.checkBoxVoynich_show_chunk_categories.TabIndex = 4;
            this.checkBoxVoynich_show_chunk_categories.Text = "Show chunk categories";
            this.checkBoxVoynich_show_chunk_categories.UseVisualStyleBackColor = true;
            this.checkBoxVoynich_show_chunk_categories.CheckedChanged += new System.EventHandler(this.checkBoxVoynich_show_chunk_categories_CheckedChanged);
            // 
            // textBox_Voynich_chunks_types
            // 
            this.textBox_Voynich_chunks_types.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Voynich_chunks_types.Location = new System.Drawing.Point(3, 36);
            this.textBox_Voynich_chunks_types.Multiline = true;
            this.textBox_Voynich_chunks_types.Name = "textBox_Voynich_chunks_types";
            this.textBox_Voynich_chunks_types.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Voynich_chunks_types.Size = new System.Drawing.Size(327, 320);
            this.textBox_Voynich_chunks_types.TabIndex = 3;
            this.textBox_Voynich_chunks_types.WordWrap = false;
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.splitContainer_WrVoynich_main);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Size = new System.Drawing.Size(1275, 359);
            this.tabPage13.TabIndex = 12;
            this.tabPage13.Text = "Asemic writer";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // splitContainer_WrVoynich_main
            // 
            this.splitContainer_WrVoynich_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_WrVoynich_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_WrVoynich_main.Name = "splitContainer_WrVoynich_main";
            // 
            // splitContainer_WrVoynich_main.Panel1
            // 
            this.splitContainer_WrVoynich_main.Panel1.Controls.Add(this.textBox_WrVoynich_chunkified_grammar);
            // 
            // splitContainer_WrVoynich_main.Panel2
            // 
            this.splitContainer_WrVoynich_main.Panel2.Controls.Add(this.splitContainer_WrVoynich_outpanel);
            this.splitContainer_WrVoynich_main.Size = new System.Drawing.Size(1275, 359);
            this.splitContainer_WrVoynich_main.SplitterDistance = 448;
            this.splitContainer_WrVoynich_main.TabIndex = 0;
            // 
            // textBox_WrVoynich_chunkified_grammar
            // 
            this.textBox_WrVoynich_chunkified_grammar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WrVoynich_chunkified_grammar.Location = new System.Drawing.Point(2, 37);
            this.textBox_WrVoynich_chunkified_grammar.Multiline = true;
            this.textBox_WrVoynich_chunkified_grammar.Name = "textBox_WrVoynich_chunkified_grammar";
            this.textBox_WrVoynich_chunkified_grammar.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_WrVoynich_chunkified_grammar.Size = new System.Drawing.Size(445, 320);
            this.textBox_WrVoynich_chunkified_grammar.TabIndex = 3;
            this.textBox_WrVoynich_chunkified_grammar.WordWrap = false;
            // 
            // splitContainer_WrVoynich_outpanel
            // 
            this.splitContainer_WrVoynich_outpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_WrVoynich_outpanel.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_WrVoynich_outpanel.Name = "splitContainer_WrVoynich_outpanel";
            // 
            // splitContainer_WrVoynich_outpanel.Panel1
            // 
            this.splitContainer_WrVoynich_outpanel.Panel1.Controls.Add(this.textBox_WrVoynich_transitions);
            // 
            // splitContainer_WrVoynich_outpanel.Panel2
            // 
            this.splitContainer_WrVoynich_outpanel.Panel2.Controls.Add(this.label9);
            this.splitContainer_WrVoynich_outpanel.Panel2.Controls.Add(this.textBox_WrVoynich_random_seed);
            this.splitContainer_WrVoynich_outpanel.Panel2.Controls.Add(this.button_WrVoynich_write_asemic);
            this.splitContainer_WrVoynich_outpanel.Panel2.Controls.Add(this.textBox_WrVoynich_text_out);
            this.splitContainer_WrVoynich_outpanel.Size = new System.Drawing.Size(823, 359);
            this.splitContainer_WrVoynich_outpanel.SplitterDistance = 481;
            this.splitContainer_WrVoynich_outpanel.TabIndex = 0;
            // 
            // textBox_WrVoynich_transitions
            // 
            this.textBox_WrVoynich_transitions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WrVoynich_transitions.Location = new System.Drawing.Point(4, 37);
            this.textBox_WrVoynich_transitions.Multiline = true;
            this.textBox_WrVoynich_transitions.Name = "textBox_WrVoynich_transitions";
            this.textBox_WrVoynich_transitions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_WrVoynich_transitions.Size = new System.Drawing.Size(473, 320);
            this.textBox_WrVoynich_transitions.TabIndex = 3;
            this.textBox_WrVoynich_transitions.WordWrap = false;
            // 
            // button_WrVoynich_write_asemic
            // 
            this.button_WrVoynich_write_asemic.Location = new System.Drawing.Point(6, 2);
            this.button_WrVoynich_write_asemic.Name = "button_WrVoynich_write_asemic";
            this.button_WrVoynich_write_asemic.Size = new System.Drawing.Size(106, 33);
            this.button_WrVoynich_write_asemic.TabIndex = 4;
            this.button_WrVoynich_write_asemic.Text = "Write asemic";
            this.button_WrVoynich_write_asemic.UseVisualStyleBackColor = true;
            this.button_WrVoynich_write_asemic.Click += new System.EventHandler(this.button_WrVoynich_write_asemic_Click);
            // 
            // textBox_WrVoynich_text_out
            // 
            this.textBox_WrVoynich_text_out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WrVoynich_text_out.Location = new System.Drawing.Point(6, 36);
            this.textBox_WrVoynich_text_out.Multiline = true;
            this.textBox_WrVoynich_text_out.Name = "textBox_WrVoynich_text_out";
            this.textBox_WrVoynich_text_out.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_WrVoynich_text_out.Size = new System.Drawing.Size(327, 320);
            this.textBox_WrVoynich_text_out.TabIndex = 4;
            this.textBox_WrVoynich_text_out.WordWrap = false;
            // 
            // trackBar_linear_size
            // 
            this.trackBar_linear_size.AutoSize = false;
            this.trackBar_linear_size.Location = new System.Drawing.Point(163, 34);
            this.trackBar_linear_size.Name = "trackBar_linear_size";
            this.trackBar_linear_size.Size = new System.Drawing.Size(101, 31);
            this.trackBar_linear_size.TabIndex = 4;
            this.toolTip1.SetToolTip(this.trackBar_linear_size, "Sets how many elements are displayed, at most, in a linear graph");
            this.trackBar_linear_size.ValueChanged += new System.EventHandler(this.trackBar_linear_size_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Max graphs size";
            // 
            // textBox_linear_size
            // 
            this.textBox_linear_size.Location = new System.Drawing.Point(263, 34);
            this.textBox_linear_size.Name = "textBox_linear_size";
            this.textBox_linear_size.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_linear_size.Size = new System.Drawing.Size(45, 20);
            this.textBox_linear_size.TabIndex = 6;
            this.textBox_linear_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_linear_size.TextChanged += new System.EventHandler(this.textBox_linear_size_TextChanged);
            this.textBox_linear_size.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_linear_size_KeyDown);
            this.textBox_linear_size.Leave += new System.EventHandler(this.textBox_linear_size_Leave);
            // 
            // trackBar_table_size
            // 
            this.trackBar_table_size.AutoSize = false;
            this.trackBar_table_size.Location = new System.Drawing.Point(325, 31);
            this.trackBar_table_size.Name = "trackBar_table_size";
            this.trackBar_table_size.Size = new System.Drawing.Size(101, 31);
            this.trackBar_table_size.TabIndex = 4;
            this.toolTip1.SetToolTip(this.trackBar_table_size, "Sets how many elements are displayed, at most, in a tabular graph");
            this.trackBar_table_size.ValueChanged += new System.EventHandler(this.trackBar_table_size_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(340, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Max tables size";
            // 
            // textBox_table_size
            // 
            this.textBox_table_size.Location = new System.Drawing.Point(425, 31);
            this.textBox_table_size.Name = "textBox_table_size";
            this.textBox_table_size.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_table_size.Size = new System.Drawing.Size(45, 20);
            this.textBox_table_size.TabIndex = 6;
            this.textBox_table_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_table_size.TextChanged += new System.EventHandler(this.textBox_table_size_TextChanged);
            this.textBox_table_size.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_table_size_KeyDown);
            this.textBox_table_size.Leave += new System.EventHandler(this.textBox_table_size_Leave);
            // 
            // textBox_WrVoynich_random_seed
            // 
            this.textBox_WrVoynich_random_seed.Location = new System.Drawing.Point(167, 10);
            this.textBox_WrVoynich_random_seed.Name = "textBox_WrVoynich_random_seed";
            this.textBox_WrVoynich_random_seed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_WrVoynich_random_seed.Size = new System.Drawing.Size(88, 20);
            this.textBox_WrVoynich_random_seed.TabIndex = 7;
            this.textBox_WrVoynich_random_seed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_WrVoynich_random_seed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_WrVoynich_random_seed_KeyDown);
            this.textBox_WrVoynich_random_seed.Leave += new System.EventHandler(this.textBox_WrVoynich_random_seed_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(132, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Seed:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 563);
            this.Controls.Add(this.textBox_table_size);
            this.Controls.Add(this.textBox_linear_size);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trackBar_table_size);
            this.Controls.Add(this.trackBar_linear_size);
            this.Controls.Add(this.button_LoadTexts);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.textBox_mainStatusWindow);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.MainTab.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.splitContainer_single_chars_main.Panel1.ResumeLayout(false);
            this.splitContainer_single_chars_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_main)).EndInit();
            this.splitContainer_single_chars_main.ResumeLayout(false);
            this.splitContainer_single_chars_data.Panel1.ResumeLayout(false);
            this.splitContainer_single_chars_data.Panel1.PerformLayout();
            this.splitContainer_single_chars_data.Panel2.ResumeLayout(false);
            this.splitContainer_single_chars_data.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_data)).EndInit();
            this.splitContainer_single_chars_data.ResumeLayout(false);
            this.splitContainer_single_chars_graphs.Panel1.ResumeLayout(false);
            this.splitContainer_single_chars_graphs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_single_chars_graphs)).EndInit();
            this.splitContainer_single_chars_graphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single_characters_distribution_graph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single_characters_distribution_graph_no_spaces)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.splitContainer_bigrams_main.Panel1.ResumeLayout(false);
            this.splitContainer_bigrams_main.Panel1.PerformLayout();
            this.splitContainer_bigrams_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bigrams_main)).EndInit();
            this.splitContainer_bigrams_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bigrams)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.splitContainer_cXc_main.Panel1.ResumeLayout(false);
            this.splitContainer_cXc_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_main)).EndInit();
            this.splitContainer_cXc_main.ResumeLayout(false);
            this.splitContainer_cXc_data.Panel1.ResumeLayout(false);
            this.splitContainer_cXc_data.Panel1.PerformLayout();
            this.splitContainer_cXc_data.Panel2.ResumeLayout(false);
            this.splitContainer_cXc_data.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_data)).EndInit();
            this.splitContainer_cXc_data.ResumeLayout(false);
            this.splitContainer_cXc_graphs.Panel1.ResumeLayout(false);
            this.splitContainer_cXc_graphs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_cXc_graphs)).EndInit();
            this.splitContainer_cXc_graphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Xc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cX)).EndInit();
            this.tabPage8.ResumeLayout(false);
            this.splitContainer_distances_main.Panel1.ResumeLayout(false);
            this.splitContainer_distances_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_main)).EndInit();
            this.splitContainer_distances_main.ResumeLayout(false);
            this.splitContainer_distances_data.Panel1.ResumeLayout(false);
            this.splitContainer_distances_data.Panel1.PerformLayout();
            this.splitContainer_distances_data.Panel2.ResumeLayout(false);
            this.splitContainer_distances_data.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_data)).EndInit();
            this.splitContainer_distances_data.ResumeLayout(false);
            this.splitContainer_distances_graphs.Panel1.ResumeLayout(false);
            this.splitContainer_distances_graphs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_distances_graphs)).EndInit();
            this.splitContainer_distances_graphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_distances_Xc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_distances_cX)).EndInit();
            this.tabPage9.ResumeLayout(false);
            this.splitContainer_vocabulary_main.Panel1.ResumeLayout(false);
            this.splitContainer_vocabulary_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_main)).EndInit();
            this.splitContainer_vocabulary_main.ResumeLayout(false);
            this.splitContainer_vocabulary_data.Panel1.ResumeLayout(false);
            this.splitContainer_vocabulary_data.Panel1.PerformLayout();
            this.splitContainer_vocabulary_data.Panel2.ResumeLayout(false);
            this.splitContainer_vocabulary_data.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_data)).EndInit();
            this.splitContainer_vocabulary_data.ResumeLayout(false);
            this.splitContainer_vocabulary_graphs.Panel1.ResumeLayout(false);
            this.splitContainer_vocabulary_graphs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_vocabulary_graphs)).EndInit();
            this.splitContainer_vocabulary_graphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_vocabulary_distribution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_words_length_distribution)).EndInit();
            this.tabPage10.ResumeLayout(false);
            this.splitContainer_syllables_main.Panel1.ResumeLayout(false);
            this.splitContainer_syllables_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_main)).EndInit();
            this.splitContainer_syllables_main.ResumeLayout(false);
            this.splitContainer_syllables_data.Panel1.ResumeLayout(false);
            this.splitContainer_syllables_data.Panel1.PerformLayout();
            this.splitContainer_syllables_data.Panel2.ResumeLayout(false);
            this.splitContainer_syllables_data.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_data)).EndInit();
            this.splitContainer_syllables_data.ResumeLayout(false);
            this.splitContainer_syllables_graphs.Panel1.ResumeLayout(false);
            this.splitContainer_syllables_graphs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_syllables_graphs)).EndInit();
            this.splitContainer_syllables_graphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_syllables_single_vowel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_syllables_multiple_vowels)).EndInit();
            this.tabPage11.ResumeLayout(false);
            this.splitContainer_report.Panel1.ResumeLayout(false);
            this.splitContainer_report.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_report)).EndInit();
            this.splitContainer_report.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.splitContainer_bigrams_theoric_main.Panel1.ResumeLayout(false);
            this.splitContainer_bigrams_theoric_main.Panel1.PerformLayout();
            this.splitContainer_bigrams_theoric_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bigrams_theoric_main)).EndInit();
            this.splitContainer_bigrams_theoric_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bigrams_theoric)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer_preprocessing.Panel1.ResumeLayout(false);
            this.splitContainer_preprocessing.Panel1.PerformLayout();
            this.splitContainer_preprocessing.Panel2.ResumeLayout(false);
            this.splitContainer_preprocessing.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_preprocessing)).EndInit();
            this.splitContainer_preprocessing.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.splitContainer_Voynich_main.Panel1.ResumeLayout(false);
            this.splitContainer_Voynich_main.Panel1.PerformLayout();
            this.splitContainer_Voynich_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Voynich_main)).EndInit();
            this.splitContainer_Voynich_main.ResumeLayout(false);
            this.splitContainer_Voynich_outpanel.Panel1.ResumeLayout(false);
            this.splitContainer_Voynich_outpanel.Panel1.PerformLayout();
            this.splitContainer_Voynich_outpanel.Panel2.ResumeLayout(false);
            this.splitContainer_Voynich_outpanel.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Voynich_outpanel)).EndInit();
            this.splitContainer_Voynich_outpanel.ResumeLayout(false);
            this.tabPage13.ResumeLayout(false);
            this.splitContainer_WrVoynich_main.Panel1.ResumeLayout(false);
            this.splitContainer_WrVoynich_main.Panel1.PerformLayout();
            this.splitContainer_WrVoynich_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_WrVoynich_main)).EndInit();
            this.splitContainer_WrVoynich_main.ResumeLayout(false);
            this.splitContainer_WrVoynich_outpanel.Panel1.ResumeLayout(false);
            this.splitContainer_WrVoynich_outpanel.Panel1.PerformLayout();
            this.splitContainer_WrVoynich_outpanel.Panel2.ResumeLayout(false);
            this.splitContainer_WrVoynich_outpanel.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_WrVoynich_outpanel)).EndInit();
            this.splitContainer_WrVoynich_outpanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_linear_size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_table_size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox_mainStatusWindow;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button_LoadTexts;
        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox_raw_source_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_remarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Regex_commands;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox_source_text;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preprocessingToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer_preprocessing;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer_single_chars_main;
        private System.Windows.Forms.TextBox textBox_single_characters_distribution;
        private System.Windows.Forms.SplitContainer splitContainer_single_chars_data;
        private System.Windows.Forms.TextBox textBox_single_characters_distribution_nospaces;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox textBox_bigrams_theoric;
        private System.Windows.Forms.SplitContainer splitContainer_bigrams_theoric_main;
        private System.Windows.Forms.Button button_select_all_text_bigrams_theoric;
        private System.Windows.Forms.CheckBox checkBox_bigrams_theoric_displaytable;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.SplitContainer splitContainer_bigrams_main;
        private System.Windows.Forms.Button button_select_all_text_bigrams;
        private System.Windows.Forms.CheckBox checkBox_bigrams_displaytable;
        private System.Windows.Forms.TextBox textBox_bigrams;
        private System.Windows.Forms.CheckBox checkBox_bigrams_delta;
        private System.Windows.Forms.CheckBox checkBox_bigrams_symmetric;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.SplitContainer splitContainer_cXc_main;
        private System.Windows.Forms.Button button_Xc_select_all;
        private System.Windows.Forms.TextBox textBox_Xc_table;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.SplitContainer splitContainer_distances_main;
        private System.Windows.Forms.TextBox textBox_chars_distances_Xc;
        private System.Windows.Forms.Button button_distances_Xc_select_all;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.SplitContainer splitContainer_vocabulary_main;
        private System.Windows.Forms.TextBox textBox_vocabulary;
        private System.Windows.Forms.Button button_vocabulary_select_all;
        private System.Windows.Forms.SplitContainer splitContainer_vocabulary_data;
        private System.Windows.Forms.Button button_words_length_select_all;
        private System.Windows.Forms.TextBox textBox_words_length_distribution;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.SplitContainer splitContainer_syllables_main;
        private System.Windows.Forms.TextBox textBox_syllables_single_vowel;
        private System.Windows.Forms.SplitContainer splitContainer_syllables_data;
        private System.Windows.Forms.TextBox textBox_syllables_multiple_vowels;
        private System.Windows.Forms.Button button_singlechars_with_spaces_select_all;
        private System.Windows.Forms.Button button_singlechars_without_spaces_select_all;
        private System.Windows.Forms.Button button_syllables_single_vowels_select_all;
        private System.Windows.Forms.Button button_syllables_multiple_vowels_select_all;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.SplitContainer splitContainer_report;
        private System.Windows.Forms.TextBox textBox_report;
        private System.Windows.Forms.Button button_report_select_all;
        private System.Windows.Forms.SplitContainer splitContainer_single_chars_graphs;
        private System.Windows.Forms.PictureBox pictureBox_single_characters_distribution_graph;
        private System.Windows.Forms.PictureBox pictureBox_single_characters_distribution_graph_no_spaces;
        private System.Windows.Forms.SplitContainer splitContainer_vocabulary_graphs;
        private System.Windows.Forms.PictureBox pictureBox_vocabulary_distribution;
        private System.Windows.Forms.PictureBox pictureBox_words_length_distribution;
        private System.Windows.Forms.SplitContainer splitContainer_syllables_graphs;
        private System.Windows.Forms.PictureBox pictureBox_syllables_single_vowel;
        private System.Windows.Forms.PictureBox pictureBox_syllables_multiple_vowels;
        private System.Windows.Forms.PictureBox pictureBox_bigrams;
        private System.Windows.Forms.PictureBox pictureBox_bigrams_theoric;
        private System.Windows.Forms.SplitContainer splitContainer_cXc_data;
        private System.Windows.Forms.TextBox textBox_cX_table;
        private System.Windows.Forms.SplitContainer splitContainer_cXc_graphs;
        private System.Windows.Forms.PictureBox pictureBox_Xc;
        private System.Windows.Forms.PictureBox pictureBox_cX;
        private System.Windows.Forms.SplitContainer splitContainer_distances_data;
        private System.Windows.Forms.TextBox textBox_chars_distances_cX;
        private System.Windows.Forms.SplitContainer splitContainer_distances_graphs;
        private System.Windows.Forms.PictureBox pictureBox_distances_Xc;
        private System.Windows.Forms.PictureBox pictureBox_distances_cX;
        private System.Windows.Forms.Button button_cX_select_all;
        private System.Windows.Forms.Button button_distances_cX_select_all;
        private System.Windows.Forms.TrackBar trackBar_linear_size;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_linear_size;
        private System.Windows.Forms.TrackBar trackBar_table_size;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_table_size;
        private System.Windows.Forms.ToolStripMenuItem visualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAndAnalyzeTextFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem savedFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_words_length_distribution_in_text;
        private System.Windows.Forms.ToolStripMenuItem massAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem massAnalyzeAndSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareAnalysisFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analysisFileHeaderInfosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findBestMatchesInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem massAggregateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem massAggregateAnalysisInASingleCorpusToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAnalysisClustersToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.SplitContainer splitContainer_Voynich_main;
        private System.Windows.Forms.Button button_Voynich_run_parse;
        private System.Windows.Forms.TextBox textBox_Voynich_report;
        private System.Windows.Forms.TextBox textBox_Voynich_parsing;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_select_Voynich_grammar;
        private System.Windows.Forms.SplitContainer splitContainer_Voynich_outpanel;
        private System.Windows.Forms.TextBox textBox_Voynich_chunks_types;
        private System.Windows.Forms.TabPage tabPage13;
        private System.Windows.Forms.SplitContainer splitContainer_WrVoynich_main;
        private System.Windows.Forms.SplitContainer splitContainer_WrVoynich_outpanel;
        private System.Windows.Forms.TextBox textBox_WrVoynich_chunkified_grammar;
        private System.Windows.Forms.TextBox textBox_WrVoynich_transitions;
        private System.Windows.Forms.TextBox textBox_WrVoynich_text_out;
        private System.Windows.Forms.Button button_WrVoynich_write_asemic;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Voynich_loop_repeats;
        private System.Windows.Forms.Button button_Voynich_run_WSET_trivial_grammar;
        private System.Windows.Forms.CheckBox checkBoxVoynich_show_chunk_categories;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_WrVoynich_random_seed;
    }
}

