namespace Template
{
    partial class VSaveAnalysisDialog
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
            this.textBox_book_title = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_book_author = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_book_year = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_book_language = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_user_notes = new System.Windows.Forms.TextBox();
            this.button_confirm_and_save = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Text title:";
            // 
            // textBox_book_title
            // 
            this.textBox_book_title.Location = new System.Drawing.Point(15, 28);
            this.textBox_book_title.Name = "textBox_book_title";
            this.textBox_book_title.Size = new System.Drawing.Size(492, 20);
            this.textBox_book_title.TabIndex = 1;
            this.textBox_book_title.TextChanged += new System.EventHandler(this.textBox_book_title_TextChanged);
            this.textBox_book_title.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_book_title_KeyDown);
            this.textBox_book_title.Leave += new System.EventHandler(this.textBox_book_title_Leave);
            this.textBox_book_title.MouseLeave += new System.EventHandler(this.textBox_book_title_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text author:";
            // 
            // textBox_book_author
            // 
            this.textBox_book_author.Location = new System.Drawing.Point(15, 84);
            this.textBox_book_author.Name = "textBox_book_author";
            this.textBox_book_author.Size = new System.Drawing.Size(492, 20);
            this.textBox_book_author.TabIndex = 1;
            this.textBox_book_author.TextChanged += new System.EventHandler(this.textBox_book_author_TextChanged);
            this.textBox_book_author.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_book_author_KeyDown);
            this.textBox_book_author.Leave += new System.EventHandler(this.textBox_book_author_Leave);
            this.textBox_book_author.MouseLeave += new System.EventHandler(this.textBox_book_author_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Year:";
            // 
            // textBox_book_year
            // 
            this.textBox_book_year.Location = new System.Drawing.Point(15, 149);
            this.textBox_book_year.Name = "textBox_book_year";
            this.textBox_book_year.Size = new System.Drawing.Size(306, 20);
            this.textBox_book_year.TabIndex = 1;
            this.textBox_book_year.TextChanged += new System.EventHandler(this.textBox_book_year_TextChanged);
            this.textBox_book_year.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_book_year_KeyDown);
            this.textBox_book_year.Leave += new System.EventHandler(this.textBox_book_year_Leave);
            this.textBox_book_year.MouseLeave += new System.EventHandler(this.textBox_book_year_MouseLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Language:";
            // 
            // textBox_book_language
            // 
            this.textBox_book_language.Location = new System.Drawing.Point(14, 217);
            this.textBox_book_language.Name = "textBox_book_language";
            this.textBox_book_language.Size = new System.Drawing.Size(306, 20);
            this.textBox_book_language.TabIndex = 1;
            this.textBox_book_language.TextChanged += new System.EventHandler(this.textBox_book_language_TextChanged);
            this.textBox_book_language.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_book_language_KeyDown);
            this.textBox_book_language.Leave += new System.EventHandler(this.textBox_book_language_Leave);
            this.textBox_book_language.MouseLeave += new System.EventHandler(this.textBox_book_language_MouseLeave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 338);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "User notes:";
            // 
            // textBox_user_notes
            // 
            this.textBox_user_notes.Location = new System.Drawing.Point(15, 355);
            this.textBox_user_notes.Multiline = true;
            this.textBox_user_notes.Name = "textBox_user_notes";
            this.textBox_user_notes.Size = new System.Drawing.Size(707, 180);
            this.textBox_user_notes.TabIndex = 1;
            this.textBox_user_notes.TextChanged += new System.EventHandler(this.textBox_user_notes_TextChanged);
            this.textBox_user_notes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_user_notes_KeyDown);
            this.textBox_user_notes.Leave += new System.EventHandler(this.textBox_user_notes_Leave);
            this.textBox_user_notes.MouseLeave += new System.EventHandler(this.textBox_user_notes_MouseLeave);
            // 
            // button_confirm_and_save
            // 
            this.button_confirm_and_save.Location = new System.Drawing.Point(14, 557);
            this.button_confirm_and_save.Name = "button_confirm_and_save";
            this.button_confirm_and_save.Size = new System.Drawing.Size(133, 31);
            this.button_confirm_and_save.TabIndex = 2;
            this.button_confirm_and_save.Text = "Confirm and save";
            this.button_confirm_and_save.UseVisualStyleBackColor = true;
            this.button_confirm_and_save.Click += new System.EventHandler(this.button_confirm_and_save_Click);
            // 
            // button_exit
            // 
            this.button_exit.Location = new System.Drawing.Point(162, 557);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(133, 31);
            this.button_exit.TabIndex = 2;
            this.button_exit.Text = "Exit";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // VSaveAnalysisDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 600);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button_confirm_and_save);
            this.Controls.Add(this.textBox_user_notes);
            this.Controls.Add(this.textBox_book_language);
            this.Controls.Add(this.textBox_book_year);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_book_author);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_book_title);
            this.Controls.Add(this.label2);
            this.Name = "VSaveAnalysisDialog";
            this.Text = "Save analysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_book_title;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_book_author;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_book_year;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_book_language;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_user_notes;
        private System.Windows.Forms.Button button_confirm_and_save;
        private System.Windows.Forms.Button button_exit;
    }
}