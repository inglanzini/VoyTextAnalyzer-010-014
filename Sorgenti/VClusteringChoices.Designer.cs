namespace Template
{
    partial class VClusteringChoices
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
            this.comboBox_clustering_choices = new System.Windows.Forms.ComboBox();
            this.textBox_clustering_choices_linear_size_limit = new System.Windows.Forms.TextBox();
            this.label_clustering_choices_2d = new System.Windows.Forms.Label();
            this.textBox_clustering_choices_rare_characters_cutoff = new System.Windows.Forms.TextBox();
            this.checkBox_clustering_choices_remove_rare_characters = new System.Windows.Forms.CheckBox();
            this.label_clustering_choices_linear = new System.Windows.Forms.Label();
            this.button_clustering_choices_proceed = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_clustering_choices
            // 
            this.comboBox_clustering_choices.FormattingEnabled = true;
            this.comboBox_clustering_choices.Location = new System.Drawing.Point(24, 44);
            this.comboBox_clustering_choices.Name = "comboBox_clustering_choices";
            this.comboBox_clustering_choices.Size = new System.Drawing.Size(384, 21);
            this.comboBox_clustering_choices.TabIndex = 0;
            this.comboBox_clustering_choices.SelectedIndexChanged += new System.EventHandler(this.comboBox_clustering_choices_SelectedIndexChanged);
            // 
            // textBox_clustering_choices_linear_size_limit
            // 
            this.textBox_clustering_choices_linear_size_limit.Location = new System.Drawing.Point(23, 95);
            this.textBox_clustering_choices_linear_size_limit.Name = "textBox_clustering_choices_linear_size_limit";
            this.textBox_clustering_choices_linear_size_limit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_clustering_choices_linear_size_limit.Size = new System.Drawing.Size(45, 20);
            this.textBox_clustering_choices_linear_size_limit.TabIndex = 13;
            this.textBox_clustering_choices_linear_size_limit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_clustering_choices_linear_size_limit.TextChanged += new System.EventHandler(this.textBox_clustering_choices_linear_size_limit_TextChanged);
            this.textBox_clustering_choices_linear_size_limit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_clustering_choices_linear_size_limit_KeyDown);
            this.textBox_clustering_choices_linear_size_limit.Leave += new System.EventHandler(this.textBox_clustering_choices_linear_size_limit_Leave);
            // 
            // label_clustering_choices_2d
            // 
            this.label_clustering_choices_2d.AutoSize = true;
            this.label_clustering_choices_2d.Location = new System.Drawing.Point(401, 137);
            this.label_clustering_choices_2d.Name = "label_clustering_choices_2d";
            this.label_clustering_choices_2d.Size = new System.Drawing.Size(209, 13);
            this.label_clustering_choices_2d.TabIndex = 12;
            this.label_clustering_choices_2d.Text = "characters) from 2D tables and monograms";
            // 
            // textBox_clustering_choices_rare_characters_cutoff
            // 
            this.textBox_clustering_choices_rare_characters_cutoff.Location = new System.Drawing.Point(352, 134);
            this.textBox_clustering_choices_rare_characters_cutoff.Name = "textBox_clustering_choices_rare_characters_cutoff";
            this.textBox_clustering_choices_rare_characters_cutoff.Size = new System.Drawing.Size(46, 20);
            this.textBox_clustering_choices_rare_characters_cutoff.TabIndex = 11;
            this.textBox_clustering_choices_rare_characters_cutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_clustering_choices_rare_characters_cutoff.TextChanged += new System.EventHandler(this.textBox_clustering_choices_rare_characters_cutoff_TextChanged);
            this.textBox_clustering_choices_rare_characters_cutoff.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_clustering_choices_rare_characters_cutoff_KeyDown);
            this.textBox_clustering_choices_rare_characters_cutoff.Leave += new System.EventHandler(this.textBox_clustering_choices_rare_characters_cutoff_Leave);
            // 
            // checkBox_clustering_choices_remove_rare_characters
            // 
            this.checkBox_clustering_choices_remove_rare_characters.AutoSize = true;
            this.checkBox_clustering_choices_remove_rare_characters.Checked = true;
            this.checkBox_clustering_choices_remove_rare_characters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_clustering_choices_remove_rare_characters.Location = new System.Drawing.Point(23, 136);
            this.checkBox_clustering_choices_remove_rare_characters.Name = "checkBox_clustering_choices_remove_rare_characters";
            this.checkBox_clustering_choices_remove_rare_characters.Size = new System.Drawing.Size(329, 17);
            this.checkBox_clustering_choices_remove_rare_characters.TabIndex = 10;
            this.checkBox_clustering_choices_remove_rare_characters.Text = "Remove rare characters (characters appearing less than once in";
            this.checkBox_clustering_choices_remove_rare_characters.UseVisualStyleBackColor = true;
            this.checkBox_clustering_choices_remove_rare_characters.CheckedChanged += new System.EventHandler(this.checkBox_clustering_choices_remove_rare_characters_CheckedChanged);
            // 
            // label_clustering_choices_linear
            // 
            this.label_clustering_choices_linear.AutoSize = true;
            this.label_clustering_choices_linear.Location = new System.Drawing.Point(77, 100);
            this.label_clustering_choices_linear.Name = "label_clustering_choices_linear";
            this.label_clustering_choices_linear.Size = new System.Drawing.Size(375, 13);
            this.label_clustering_choices_linear.TabIndex = 14;
            this.label_clustering_choices_linear.Text = "1D data limit (all data beyond the limit are removed while calculating distances)" +
    "";
            // 
            // button_clustering_choices_proceed
            // 
            this.button_clustering_choices_proceed.Location = new System.Drawing.Point(23, 182);
            this.button_clustering_choices_proceed.Name = "button_clustering_choices_proceed";
            this.button_clustering_choices_proceed.Size = new System.Drawing.Size(75, 23);
            this.button_clustering_choices_proceed.TabIndex = 15;
            this.button_clustering_choices_proceed.Text = "Proceed";
            this.button_clustering_choices_proceed.UseVisualStyleBackColor = true;
            this.button_clustering_choices_proceed.Click += new System.EventHandler(this.button_clustering_choices_proceed_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(396, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Choose on which statistic distances will be calculated (they\'re always \"unblinded" +
    "\"):";
            // 
            // VClusteringChoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 232);
            this.Controls.Add(this.button_clustering_choices_proceed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_clustering_choices_linear);
            this.Controls.Add(this.textBox_clustering_choices_linear_size_limit);
            this.Controls.Add(this.label_clustering_choices_2d);
            this.Controls.Add(this.textBox_clustering_choices_rare_characters_cutoff);
            this.Controls.Add(this.checkBox_clustering_choices_remove_rare_characters);
            this.Controls.Add(this.comboBox_clustering_choices);
            this.Name = "VClusteringChoices";
            this.Text = "Clustering options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_clustering_choices;
        private System.Windows.Forms.TextBox textBox_clustering_choices_linear_size_limit;
        private System.Windows.Forms.Label label_clustering_choices_2d;
        private System.Windows.Forms.TextBox textBox_clustering_choices_rare_characters_cutoff;
        private System.Windows.Forms.CheckBox checkBox_clustering_choices_remove_rare_characters;
        private System.Windows.Forms.Label label_clustering_choices_linear;
        private System.Windows.Forms.Button button_clustering_choices_proceed;
        private System.Windows.Forms.Label label3;
    }
}