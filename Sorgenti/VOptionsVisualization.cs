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
    public partial class VOptionsVisualization : Form
    {
        public VOptionsVisualization()
        {
            InitializeComponent();
        }

        private void VOptionsVisualization_Shown(object sender, EventArgs e)
        {
            checkBox_display_all_records.Checked = Form1.configuration_data.display_whole_linear_data;
            checkBox_display_whole_texts.Checked = Form1.configuration_data.display_whole_texts;
            checkBox_numbers_in_american_format.Checked = Form1.configuration_data.display_numbers_in_american_format;
        }

        private void checkBox_display_all_records_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.display_whole_linear_data = checkBox_display_all_records.Checked;
        }

        private void checkBox_display_whole_texts_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.display_whole_texts = checkBox_display_whole_texts.Checked;
        }

        private void checkBox_numbers_in_american_format_CheckedChanged(object sender, EventArgs e)
        {
            Form1.configuration_data.display_numbers_in_american_format = checkBox_numbers_in_american_format.Checked;
        }

        private void button_save_visualization_options_Click(object sender, EventArgs e)
        {
            Form1.configuration_data.save(Form1.configuration_data_file, Form1.configuration_data);
            Form1.display_data(Form1._display_all);  // Necessaria: stiamo variando le opzioni di visualizzazione!
            this.Close();
        }

        private void button_visualization_options_exit_Click(object sender, EventArgs e)
        {
            // Il file di configurazione verrà ricaricato in FormClosed per scartare le modifiche effettuate
            this.Close();
        }

        private void VOptionsVisualization_FormClosed(object sender, FormClosedEventArgs e)
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
