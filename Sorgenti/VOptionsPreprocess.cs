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
    public partial class VOptionsPreprocess : Form
    {
        public VOptionsPreprocess()
        {
            InitializeComponent();
        }


        private void VOptions_Shown(object sender, EventArgs e)
        {
            checkBox_discard_all_arabics.Checked = Form1.configuration_data.discard_all_arabic_numbers;
            checkBox_apostrophe_is_a_separator.Checked = Form1.configuration_data.apostrophe_is_a_separator;
            checkBox_keep_upper_lower.Checked = Form1.configuration_data.keep_distinction_between_upper_and_lowercase;

            radioButton_discard_on_apostrophe.Checked = Form1.configuration_data.words_linked_by_an_apostrophe_are_discarded;
            radioButton_join_on_apostrophe.Checked = Form1.configuration_data.words_linked_by_an_apostrophe_are_joined;
            radioButton_split_on_apostrophe.Checked = Form1.configuration_data.words_linked_by_an_apostrophe_are_separated;
            if (Form1.configuration_data.apostrophe_is_a_separator == true)
            {
                radioButton_discard_on_apostrophe.Enabled = true;
                radioButton_join_on_apostrophe.Enabled = true;
                radioButton_split_on_apostrophe.Enabled = true;
            }
            else
            {
                radioButton_discard_on_apostrophe.Enabled = false;
                radioButton_join_on_apostrophe.Enabled = false;
                radioButton_split_on_apostrophe.Enabled = false;
            }


            radioButton_discard_on_dash.Checked = Form1.configuration_data.words_linked_by_a_dash_are_discarded;
            radioButton_join_on_dash.Checked = Form1.configuration_data.words_linked_by_a_dash_are_joined;
            radioButton_split_on_dash.Checked = Form1.configuration_data.words_linked_by_a_dash_are_separated;
        }


        private void checkBox_discard_all_arabics_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.discard_all_arabic_numbers = checkBox_discard_all_arabics.Checked;
        }

        private void checkBox_apostrophe_is_a_separator_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.apostrophe_is_a_separator = checkBox_apostrophe_is_a_separator.Checked;
            if (Form1.configuration_data.apostrophe_is_a_separator == true)
            {
                radioButton_discard_on_apostrophe.Enabled = true;
                radioButton_join_on_apostrophe.Enabled = true;
                radioButton_split_on_apostrophe.Enabled = true;
            }
            else
            {
                radioButton_discard_on_apostrophe.Enabled = false;
                radioButton_join_on_apostrophe.Enabled = false;
                radioButton_split_on_apostrophe.Enabled = false;
            }
        }

        private void radioButton_split_on_apostrophe_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_an_apostrophe_are_separated = radioButton_split_on_apostrophe.Checked;
        }

        private void radioButton_join_on_apostrophe_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_an_apostrophe_are_joined = radioButton_join_on_apostrophe.Checked;
        }

        private void radioButton_discard_on_apostrophe_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_an_apostrophe_are_discarded = radioButton_discard_on_apostrophe.Checked;
        }

        private void radioButton_join_on_dash_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_a_dash_are_joined = radioButton_join_on_dash.Checked;
        }

        private void radioButton_split_on_dash_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_a_dash_are_separated = radioButton_split_on_dash.Checked;
        }

        private void radioButton_discard_on_dash_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.words_linked_by_a_dash_are_discarded = radioButton_discard_on_dash.Checked;
        }

        private void checkBox_keep_upper_lower_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.keep_distinction_between_upper_and_lowercase = checkBox_keep_upper_lower.Checked;
        }


        private void button_save_preprocessing_options_Click(object sender, EventArgs e)
        {
            Form1.configuration_data.save(Form1.configuration_data_file, Form1.configuration_data);
            this.Close();
        }

        private void button_options_exit_Click(object sender, EventArgs e)
        {
            // Il file di configurazione verrà ricaricato in FormClosed per scartare le modifche effettuate
            this.Close();
        }
        private void VOptionsPreprocess_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Uscita dalla form: serve per trappare il caso in cui si esce usando il pulsante 'X?
            //   La gestione non è 'perfetta', nel senso che se si esce premendo 'Save and exit' il file di configurazione viene salvato,
            //     dopodichè viene ricaricato inutilmente qua in FormClosed. Vabbè.
            reload_config();
        }

        private void reload_config()
        {
            // re-load del file di configurazione per scartare le modifiche (questa parte è stata copiata da Form1.get_configuration_data)
            object objclass = Form1.configuration_data;  // Boxing
            if (Form1.configuration_data_file.direct_load(ref objclass, Form1.configuration_data.load) == false)
            {
                // Lettura file di configurazione non andata a buon fine
                Form1.newline_to_mainStatusWindow("Failed to load configuration file: '" + Form1.configuration_data_file.system_name + "'");
            }
            else
            {
                // Istruzione indispensabile affinchè la classe venga effettivamente aggiornata !!
                //  C'è un commento più esteso in loadToolStripMenu
                Form1.configuration_data = (XConfig)objclass;
            }
        }




    }
}
