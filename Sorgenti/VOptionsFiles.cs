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
    public partial class VOptionsFiles : Form
    {
        public VOptionsFiles()
        {
            InitializeComponent();
        }

        private void VOptionsFiles_Shown(object sender, EventArgs e)
        {
            textBox_user_files_directory.Text = Form1.configuration_data.user_files_directory;
            textBox_user_text_files_directory.Text = Form1.configuration_data.user_text_files_directory;

            checkBox_save_also_cleaned_text.Checked = Form1.configuration_data.save_also_cleaned_text;
            checkBox_save_also_source_text.Checked = Form1.configuration_data.save_also_source_text;
        }

        private void checkBox_save_also_source_text_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.save_also_source_text = checkBox_save_also_source_text.Checked;
        }

        private void checkBox_save_also_cleaned_text_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.save_also_cleaned_text = checkBox_save_also_cleaned_text.Checked;
        }

        private void button_save_files_options_Click(object sender, EventArgs e)
        {
            Form1.configuration_data.save(Form1.configuration_data_file, Form1.configuration_data);
            this.Close();
        }

        private void button_files_options_exit_Click(object sender, EventArgs e)
        {
            // Il file di configurazione verrà ricaricato in FormClosed per scartare le modifche effettuate
            this.Close();
        }

        private void VOptionsFiles_FormClosed(object sender, FormClosedEventArgs e)
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
