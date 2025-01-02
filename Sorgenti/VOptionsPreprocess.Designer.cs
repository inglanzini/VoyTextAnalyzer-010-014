namespace Template
{
    partial class VOptionsPreprocess
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
            this.checkBox_discard_all_arabics = new System.Windows.Forms.CheckBox();
            this.checkBox_apostrophe_is_a_separator = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_discard_on_apostrophe = new System.Windows.Forms.RadioButton();
            this.radioButton_join_on_apostrophe = new System.Windows.Forms.RadioButton();
            this.radioButton_split_on_apostrophe = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_discard_on_dash = new System.Windows.Forms.RadioButton();
            this.radioButton_split_on_dash = new System.Windows.Forms.RadioButton();
            this.radioButton_join_on_dash = new System.Windows.Forms.RadioButton();
            this.button_save_preprocessing_options = new System.Windows.Forms.Button();
            this.button_options_exit = new System.Windows.Forms.Button();
            this.checkBox_keep_upper_lower = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_discard_all_arabics
            // 
            this.checkBox_discard_all_arabics.AutoSize = true;
            this.checkBox_discard_all_arabics.Location = new System.Drawing.Point(12, 12);
            this.checkBox_discard_all_arabics.Name = "checkBox_discard_all_arabics";
            this.checkBox_discard_all_arabics.Size = new System.Drawing.Size(152, 17);
            this.checkBox_discard_all_arabics.TabIndex = 1;
            this.checkBox_discard_all_arabics.Text = "Discard all arabic numerals";
            this.checkBox_discard_all_arabics.UseVisualStyleBackColor = true;
            this.checkBox_discard_all_arabics.CheckedChanged += new System.EventHandler(this.checkBox_discard_all_arabics_CheckedChanged);
            // 
            // checkBox_apostrophe_is_a_separator
            // 
            this.checkBox_apostrophe_is_a_separator.AutoSize = true;
            this.checkBox_apostrophe_is_a_separator.Location = new System.Drawing.Point(12, 35);
            this.checkBox_apostrophe_is_a_separator.Name = "checkBox_apostrophe_is_a_separator";
            this.checkBox_apostrophe_is_a_separator.Size = new System.Drawing.Size(607, 17);
            this.checkBox_apostrophe_is_a_separator.TabIndex = 1;
            this.checkBox_apostrophe_is_a_separator.Text = "Apostrophe is a separator (if unchecked: apostrophe is treated like a normal char" +
    "acter (unchecked is the suggested option))";
            this.checkBox_apostrophe_is_a_separator.UseVisualStyleBackColor = true;
            this.checkBox_apostrophe_is_a_separator.CheckedChanged += new System.EventHandler(this.checkBox_apostrophe_is_a_separator_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_discard_on_apostrophe);
            this.groupBox1.Controls.Add(this.radioButton_join_on_apostrophe);
            this.groupBox1.Controls.Add(this.radioButton_split_on_apostrophe);
            this.groupBox1.Location = new System.Drawing.Point(38, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 94);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // radioButton_discard_on_apostrophe
            // 
            this.radioButton_discard_on_apostrophe.AutoSize = true;
            this.radioButton_discard_on_apostrophe.Location = new System.Drawing.Point(6, 65);
            this.radioButton_discard_on_apostrophe.Name = "radioButton_discard_on_apostrophe";
            this.radioButton_discard_on_apostrophe.Size = new System.Drawing.Size(239, 17);
            this.radioButton_discard_on_apostrophe.TabIndex = 3;
            this.radioButton_discard_on_apostrophe.TabStop = true;
            this.radioButton_discard_on_apostrophe.Text = "Words including an apostrophe are discarded";
            this.radioButton_discard_on_apostrophe.UseVisualStyleBackColor = true;
            this.radioButton_discard_on_apostrophe.CheckedChanged += new System.EventHandler(this.radioButton_discard_on_apostrophe_CheckedChanged);
            // 
            // radioButton_join_on_apostrophe
            // 
            this.radioButton_join_on_apostrophe.AutoSize = true;
            this.radioButton_join_on_apostrophe.Location = new System.Drawing.Point(6, 42);
            this.radioButton_join_on_apostrophe.Name = "radioButton_join_on_apostrophe";
            this.radioButton_join_on_apostrophe.Size = new System.Drawing.Size(263, 17);
            this.radioButton_join_on_apostrophe.TabIndex = 3;
            this.radioButton_join_on_apostrophe.TabStop = true;
            this.radioButton_join_on_apostrophe.Text = "Words including an apostrophe are joined together";
            this.radioButton_join_on_apostrophe.UseVisualStyleBackColor = true;
            this.radioButton_join_on_apostrophe.CheckedChanged += new System.EventHandler(this.radioButton_join_on_apostrophe_CheckedChanged);
            // 
            // radioButton_split_on_apostrophe
            // 
            this.radioButton_split_on_apostrophe.AutoSize = true;
            this.radioButton_split_on_apostrophe.Location = new System.Drawing.Point(6, 19);
            this.radioButton_split_on_apostrophe.Name = "radioButton_split_on_apostrophe";
            this.radioButton_split_on_apostrophe.Size = new System.Drawing.Size(321, 17);
            this.radioButton_split_on_apostrophe.TabIndex = 3;
            this.radioButton_split_on_apostrophe.TabStop = true;
            this.radioButton_split_on_apostrophe.Text = "Words including an apostrophe are splitted into separate words";
            this.radioButton_split_on_apostrophe.UseVisualStyleBackColor = true;
            this.radioButton_split_on_apostrophe.CheckedChanged += new System.EventHandler(this.radioButton_split_on_apostrophe_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_discard_on_dash);
            this.groupBox2.Controls.Add(this.radioButton_split_on_dash);
            this.groupBox2.Controls.Add(this.radioButton_join_on_dash);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(391, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // radioButton_discard_on_dash
            // 
            this.radioButton_discard_on_dash.AutoSize = true;
            this.radioButton_discard_on_dash.Location = new System.Drawing.Point(6, 66);
            this.radioButton_discard_on_dash.Name = "radioButton_discard_on_dash";
            this.radioButton_discard_on_dash.Size = new System.Drawing.Size(293, 17);
            this.radioButton_discard_on_dash.TabIndex = 0;
            this.radioButton_discard_on_dash.TabStop = true;
            this.radioButton_discard_on_dash.Text = "Words including a dash are discarded (suggested option)";
            this.radioButton_discard_on_dash.UseVisualStyleBackColor = true;
            this.radioButton_discard_on_dash.CheckedChanged += new System.EventHandler(this.radioButton_discard_on_dash_CheckedChanged);
            // 
            // radioButton_split_on_dash
            // 
            this.radioButton_split_on_dash.AutoSize = true;
            this.radioButton_split_on_dash.Location = new System.Drawing.Point(6, 19);
            this.radioButton_split_on_dash.Name = "radioButton_split_on_dash";
            this.radioButton_split_on_dash.Size = new System.Drawing.Size(285, 17);
            this.radioButton_split_on_dash.TabIndex = 0;
            this.radioButton_split_on_dash.TabStop = true;
            this.radioButton_split_on_dash.Text = "Words including a dash are splitted into separate words";
            this.radioButton_split_on_dash.UseVisualStyleBackColor = true;
            this.radioButton_split_on_dash.CheckedChanged += new System.EventHandler(this.radioButton_split_on_dash_CheckedChanged);
            // 
            // radioButton_join_on_dash
            // 
            this.radioButton_join_on_dash.AutoSize = true;
            this.radioButton_join_on_dash.Location = new System.Drawing.Point(6, 43);
            this.radioButton_join_on_dash.Name = "radioButton_join_on_dash";
            this.radioButton_join_on_dash.Size = new System.Drawing.Size(227, 17);
            this.radioButton_join_on_dash.TabIndex = 0;
            this.radioButton_join_on_dash.TabStop = true;
            this.radioButton_join_on_dash.Text = "Words including a dash are joined together";
            this.radioButton_join_on_dash.UseVisualStyleBackColor = true;
            this.radioButton_join_on_dash.CheckedChanged += new System.EventHandler(this.radioButton_join_on_dash_CheckedChanged);
            // 
            // button_save_preprocessing_options
            // 
            this.button_save_preprocessing_options.Location = new System.Drawing.Point(38, 386);
            this.button_save_preprocessing_options.Name = "button_save_preprocessing_options";
            this.button_save_preprocessing_options.Size = new System.Drawing.Size(162, 23);
            this.button_save_preprocessing_options.TabIndex = 4;
            this.button_save_preprocessing_options.Text = "Save options and exit";
            this.button_save_preprocessing_options.UseVisualStyleBackColor = true;
            this.button_save_preprocessing_options.Click += new System.EventHandler(this.button_save_preprocessing_options_Click);
            // 
            // button_options_exit
            // 
            this.button_options_exit.Location = new System.Drawing.Point(231, 386);
            this.button_options_exit.Name = "button_options_exit";
            this.button_options_exit.Size = new System.Drawing.Size(152, 23);
            this.button_options_exit.TabIndex = 4;
            this.button_options_exit.Text = "Discard changes and exit";
            this.button_options_exit.UseVisualStyleBackColor = true;
            this.button_options_exit.Click += new System.EventHandler(this.button_options_exit_Click);
            // 
            // checkBox_keep_upper_lower
            // 
            this.checkBox_keep_upper_lower.AutoSize = true;
            this.checkBox_keep_upper_lower.Location = new System.Drawing.Point(12, 287);
            this.checkBox_keep_upper_lower.Name = "checkBox_keep_upper_lower";
            this.checkBox_keep_upper_lower.Size = new System.Drawing.Size(381, 17);
            this.checkBox_keep_upper_lower.TabIndex = 5;
            this.checkBox_keep_upper_lower.Text = "Keep the distinction between upper and lowercase letters in the original text";
            this.checkBox_keep_upper_lower.UseVisualStyleBackColor = true;
            this.checkBox_keep_upper_lower.CheckedChanged += new System.EventHandler(this.checkBox_keep_upper_lower_CheckedChanged);
            // 
            // VOptionsPreprocess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBox_keep_upper_lower);
            this.Controls.Add(this.button_options_exit);
            this.Controls.Add(this.button_save_preprocessing_options);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox_apostrophe_is_a_separator);
            this.Controls.Add(this.checkBox_discard_all_arabics);
            this.Name = "VOptionsPreprocess";
            this.Text = "Preprocessing options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VOptionsPreprocess_FormClosed);
            this.Shown += new System.EventHandler(this.VOptions_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_discard_all_arabics;
        private System.Windows.Forms.CheckBox checkBox_apostrophe_is_a_separator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_discard_on_apostrophe;
        private System.Windows.Forms.RadioButton radioButton_join_on_apostrophe;
        private System.Windows.Forms.RadioButton radioButton_split_on_apostrophe;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_discard_on_dash;
        private System.Windows.Forms.RadioButton radioButton_split_on_dash;
        private System.Windows.Forms.RadioButton radioButton_join_on_dash;
        private System.Windows.Forms.Button button_save_preprocessing_options;
        private System.Windows.Forms.Button button_options_exit;
        private System.Windows.Forms.CheckBox checkBox_keep_upper_lower;
    }
}