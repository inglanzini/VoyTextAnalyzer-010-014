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
    public partial class VSaveAnalysisDialog : Form
    {
        public VSaveAnalysisDialog()
        {
            InitializeComponent();

            textBox_book_title.Text = Form1.text_analyzer.book_title;
            textBox_book_author.Text = Form1.text_analyzer.book_author;
            textBox_book_year.Text = Form1.text_analyzer.book_year;
            textBox_book_language.Text = Form1.text_analyzer.book_language;
            textBox_user_notes.Text = Form1.text_analyzer.user_notes;
        }

        private void button_confirm_and_save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.Close();
        }





        private void textBox_book_title_TextChanged(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_title = textBox_book_title.Text;
        }

        private void textBox_book_title_MouseLeave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_title = textBox_book_title.Text;
        }

        private void textBox_book_title_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) Form1.text_analyzer.book_title = textBox_book_title.Text;
        }

        private void textBox_book_title_Leave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_title = textBox_book_title.Text;
        }

        private void textBox_book_author_TextChanged(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_author = textBox_book_author.Text;
        }

        private void textBox_book_author_Leave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_author = textBox_book_author.Text;
        }

        private void textBox_book_author_MouseLeave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_author = textBox_book_author.Text;
        }

        private void textBox_book_author_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) Form1.text_analyzer.book_author = textBox_book_author.Text;
        }

        private void textBox_book_year_TextChanged(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_year = textBox_book_year.Text;
        }

        private void textBox_book_year_Leave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_year = textBox_book_year.Text;
        }

        private void textBox_book_year_MouseLeave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_year = textBox_book_year.Text;
        }

        private void textBox_book_year_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) Form1.text_analyzer.book_year = textBox_book_year.Text;
        }

        private void textBox_book_language_TextChanged(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_language = textBox_book_language.Text;
        }

        private void textBox_book_language_Leave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_language = textBox_book_language.Text;
        }

        private void textBox_book_language_MouseLeave(object sender, EventArgs e)
        {
            Form1.text_analyzer.book_language = textBox_book_language.Text;
        }

        private void textBox_book_language_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) Form1.text_analyzer.book_language = textBox_book_language.Text;
        }

        private void textBox_user_notes_TextChanged(object sender, EventArgs e)
        {
            Form1.text_analyzer.user_notes = textBox_user_notes.Text;
        }

        private void textBox_user_notes_Leave(object sender, EventArgs e)
        {
            Form1.text_analyzer.user_notes = textBox_user_notes.Text;
        }

        private void textBox_user_notes_MouseLeave(object sender, EventArgs e)
        {
            Form1.text_analyzer.user_notes = textBox_user_notes.Text;
        }

        private void textBox_user_notes_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) Form1.text_analyzer.user_notes = textBox_user_notes.Text;
        }
    }
}
