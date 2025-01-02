namespace Template
{
    partial class VOptionsVisualization
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
            this.button_save_visualization_options = new System.Windows.Forms.Button();
            this.button_visualization_options_exit = new System.Windows.Forms.Button();
            this.checkBox_display_all_records = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_display_whole_texts = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_numbers_in_american_format = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_save_visualization_options
            // 
            this.button_save_visualization_options.Location = new System.Drawing.Point(37, 389);
            this.button_save_visualization_options.Name = "button_save_visualization_options";
            this.button_save_visualization_options.Size = new System.Drawing.Size(162, 23);
            this.button_save_visualization_options.TabIndex = 5;
            this.button_save_visualization_options.Text = "Save options and exit";
            this.button_save_visualization_options.UseVisualStyleBackColor = true;
            this.button_save_visualization_options.Click += new System.EventHandler(this.button_save_visualization_options_Click);
            // 
            // button_visualization_options_exit
            // 
            this.button_visualization_options_exit.Location = new System.Drawing.Point(220, 389);
            this.button_visualization_options_exit.Name = "button_visualization_options_exit";
            this.button_visualization_options_exit.Size = new System.Drawing.Size(152, 23);
            this.button_visualization_options_exit.TabIndex = 6;
            this.button_visualization_options_exit.Text = "Discard changes and exit";
            this.button_visualization_options_exit.UseVisualStyleBackColor = true;
            this.button_visualization_options_exit.Click += new System.EventHandler(this.button_visualization_options_exit_Click);
            // 
            // checkBox_display_all_records
            // 
            this.checkBox_display_all_records.AutoSize = true;
            this.checkBox_display_all_records.Location = new System.Drawing.Point(12, 25);
            this.checkBox_display_all_records.Name = "checkBox_display_all_records";
            this.checkBox_display_all_records.Size = new System.Drawing.Size(591, 17);
            this.checkBox_display_all_records.TabIndex = 7;
            this.checkBox_display_all_records.Text = "Display all records. By default only a number of records are displayed (for insta" +
    "nce in the vocabulary and syllables pages)";
            this.checkBox_display_all_records.UseVisualStyleBackColor = true;
            this.checkBox_display_all_records.CheckedChanged += new System.EventHandler(this.checkBox_display_all_records_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(922, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "WARNING: Activating this option will display all records, but it will slow the pr" +
    "ogram down. Closing this window, and creating and loading analysis files, may ta" +
    "ke MINUTES with large vocabularies!";
            // 
            // checkBox_display_whole_texts
            // 
            this.checkBox_display_whole_texts.AutoSize = true;
            this.checkBox_display_whole_texts.Location = new System.Drawing.Point(12, 87);
            this.checkBox_display_whole_texts.Name = "checkBox_display_whole_texts";
            this.checkBox_display_whole_texts.Size = new System.Drawing.Size(533, 17);
            this.checkBox_display_whole_texts.TabIndex = 7;
            this.checkBox_display_whole_texts.Text = "Display whole texts. By default only short texts will be displayed in the Cleaned" +
    " Text  and Source Text pages";
            this.checkBox_display_whole_texts.UseVisualStyleBackColor = true;
            this.checkBox_display_whole_texts.CheckedChanged += new System.EventHandler(this.checkBox_display_whole_texts_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(941, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "WARNING: activating this option will always display the whole texts, but it will " +
    "slow the program down. Closing this window, and creating and loading text files," +
    " may take MINUTES with very long texts!";
            // 
            // checkBox_numbers_in_american_format
            // 
            this.checkBox_numbers_in_american_format.AutoSize = true;
            this.checkBox_numbers_in_american_format.Location = new System.Drawing.Point(12, 167);
            this.checkBox_numbers_in_american_format.Name = "checkBox_numbers_in_american_format";
            this.checkBox_numbers_in_american_format.Size = new System.Drawing.Size(455, 17);
            this.checkBox_numbers_in_american_format.TabIndex = 9;
            this.checkBox_numbers_in_american_format.Text = "Visualize numbers in American format. By default numbers are visualized in Europe" +
    "an format";
            this.checkBox_numbers_in_american_format.UseVisualStyleBackColor = true;
            this.checkBox_numbers_in_american_format.CheckedChanged += new System.EventHandler(this.checkBox_numbers_in_american_format_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(398, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "You may need to change it to correctly export numerical data (for instance to Exc" +
    "el)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Do not worry and give the program time to finish";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Do not worry and give the program time to finish";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 373);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "See the WARNINGS above!";
            // 
            // VOptionsVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 450);
            this.Controls.Add(this.checkBox_numbers_in_american_format);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_display_whole_texts);
            this.Controls.Add(this.checkBox_display_all_records);
            this.Controls.Add(this.button_visualization_options_exit);
            this.Controls.Add(this.button_save_visualization_options);
            this.Name = "VOptionsVisualization";
            this.Text = "Visualization options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VOptionsVisualization_FormClosed);
            this.Shown += new System.EventHandler(this.VOptionsVisualization_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save_visualization_options;
        private System.Windows.Forms.Button button_visualization_options_exit;
        private System.Windows.Forms.CheckBox checkBox_display_all_records;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_display_whole_texts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_numbers_in_american_format;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}