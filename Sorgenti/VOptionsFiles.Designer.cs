namespace Template
{
    partial class VOptionsFiles
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
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_save_also_cleaned_text = new System.Windows.Forms.CheckBox();
            this.checkBox_save_also_source_text = new System.Windows.Forms.CheckBox();
            this.button_files_options_exit = new System.Windows.Forms.Button();
            this.button_save_files_options = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_user_files_directory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_user_text_files_directory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 368);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(603, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "WARNING: be careful not to infinge any copyrights by activating this option, whic" +
    "h will also increase the size of the analysis file";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 430);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "See the WARNINGS above!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(603, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "WARNING: be careful not to infinge any copyrights by activating this option, whic" +
    "h will also increase the size of the analysis file";
            // 
            // checkBox_save_also_cleaned_text
            // 
            this.checkBox_save_also_cleaned_text.AutoSize = true;
            this.checkBox_save_also_cleaned_text.Location = new System.Drawing.Point(10, 352);
            this.checkBox_save_also_cleaned_text.Name = "checkBox_save_also_cleaned_text";
            this.checkBox_save_also_cleaned_text.Size = new System.Drawing.Size(285, 17);
            this.checkBox_save_also_cleaned_text.TabIndex = 12;
            this.checkBox_save_also_cleaned_text.Text = "Save the pre-processed cleaned text in the analysis file";
            this.checkBox_save_also_cleaned_text.UseVisualStyleBackColor = true;
            this.checkBox_save_also_cleaned_text.CheckedChanged += new System.EventHandler(this.checkBox_save_also_cleaned_text_CheckedChanged);
            // 
            // checkBox_save_also_source_text
            // 
            this.checkBox_save_also_source_text.AutoSize = true;
            this.checkBox_save_also_source_text.Location = new System.Drawing.Point(10, 290);
            this.checkBox_save_also_source_text.Name = "checkBox_save_also_source_text";
            this.checkBox_save_also_source_text.Size = new System.Drawing.Size(245, 17);
            this.checkBox_save_also_source_text.TabIndex = 13;
            this.checkBox_save_also_source_text.Text = "Save the original source text in the analysis file";
            this.checkBox_save_also_source_text.UseVisualStyleBackColor = true;
            this.checkBox_save_also_source_text.CheckedChanged += new System.EventHandler(this.checkBox_save_also_source_text_CheckedChanged);
            // 
            // button_files_options_exit
            // 
            this.button_files_options_exit.Location = new System.Drawing.Point(218, 446);
            this.button_files_options_exit.Name = "button_files_options_exit";
            this.button_files_options_exit.Size = new System.Drawing.Size(152, 23);
            this.button_files_options_exit.TabIndex = 11;
            this.button_files_options_exit.Text = "Discard changes and exit";
            this.button_files_options_exit.UseVisualStyleBackColor = true;
            this.button_files_options_exit.Click += new System.EventHandler(this.button_files_options_exit_Click);
            // 
            // button_save_files_options
            // 
            this.button_save_files_options.Location = new System.Drawing.Point(35, 446);
            this.button_save_files_options.Name = "button_save_files_options";
            this.button_save_files_options.Size = new System.Drawing.Size(162, 23);
            this.button_save_files_options.TabIndex = 10;
            this.button_save_files_options.Text = "Save options and exit";
            this.button_save_files_options.UseVisualStyleBackColor = true;
            this.button_save_files_options.Click += new System.EventHandler(this.button_save_files_options_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "User analysis files current directory:";
            // 
            // textBox_user_files_directory
            // 
            this.textBox_user_files_directory.Enabled = false;
            this.textBox_user_files_directory.Location = new System.Drawing.Point(15, 26);
            this.textBox_user_files_directory.Name = "textBox_user_files_directory";
            this.textBox_user_files_directory.Size = new System.Drawing.Size(825, 20);
            this.textBox_user_files_directory.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "User text files current directory:";
            // 
            // textBox_user_text_files_directory
            // 
            this.textBox_user_text_files_directory.Enabled = false;
            this.textBox_user_text_files_directory.Location = new System.Drawing.Point(15, 67);
            this.textBox_user_text_files_directory.Name = "textBox_user_text_files_directory";
            this.textBox_user_text_files_directory.Size = new System.Drawing.Size(825, 20);
            this.textBox_user_text_files_directory.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(575, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "(Both directories are managed automatically,  they will always point to the last " +
    "directory a file was loaded from or saved to)";
            // 
            // VOptionsFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 508);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_user_text_files_directory);
            this.Controls.Add(this.textBox_user_files_directory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_save_also_cleaned_text);
            this.Controls.Add(this.checkBox_save_also_source_text);
            this.Controls.Add(this.button_files_options_exit);
            this.Controls.Add(this.button_save_files_options);
            this.Name = "VOptionsFiles";
            this.Text = "Save files options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VOptionsFiles_FormClosed);
            this.Shown += new System.EventHandler(this.VOptionsFiles_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_save_also_cleaned_text;
        private System.Windows.Forms.CheckBox checkBox_save_also_source_text;
        private System.Windows.Forms.Button button_files_options_exit;
        private System.Windows.Forms.Button button_save_files_options;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_user_files_directory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_user_text_files_directory;
        private System.Windows.Forms.Label label5;
    }
}