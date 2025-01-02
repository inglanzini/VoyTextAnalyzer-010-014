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
    public partial class VClusteringChoices : Form
    {

        // ComboBox di scelta

        public enum combo_clustering_choices { bigrams = 0, monograms = 5, following_char = 6, previous_char = 7, following_distance = 8, previous_distance = 9,
                                        vocabulary = 12, wordslength_text = 13, wordslength_vocabulary = 14 }

        public static List<XComboBoxElement> combo_clustering_choices_structure = new List<XComboBoxElement>()
            {
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.bigrams, name = "Bigrams frequencies"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.monograms, name = "Single characters frequencies (space included)"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.following_char, name = "Following character probabilities"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.previous_char, name = "Previous character probabilities"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.following_distance, name = "Characters distances according to the following character"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.previous_distance, name = "Characters distances according to the previous character"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.vocabulary, name = "Vocabulary"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.wordslength_text, name = "Words length (in the whole text)"},
                new XComboBoxElement { enum_value = (int)combo_clustering_choices.wordslength_vocabulary, name = "Words length (in vocabulary)"}
            };




        public VClusteringChoices()
        {
            InitializeComponent();

            initialize_controls();

            enforce_controls_coherency();
        }


        private void initialize_controls() 
        {
            Form1.clustering_choices_status = (combo_clustering_choices)XComboBox.XComboBox_initialize(combo_clustering_choices_structure, comboBox_clustering_choices);

            Form1.clustering_choices_linear_limit = Form1.default_linear_graph_length;
            textBox_clustering_choices_linear_size_limit.Text = Form1.clustering_choices_linear_limit.ToString();

            Form1.clustering_choices_remove_rare_characters = checkBox_clustering_choices_remove_rare_characters.Checked;
            Form1.clustering_choices_rare_characters_cutoff = FormCompare.rare_characters_default_cutoff;
            textBox_clustering_choices_rare_characters_cutoff.Text = Form1.clustering_choices_rare_characters_cutoff.ToString();
        }

        private void enforce_controls_coherency()
        {
            bool limits_are_2d = true;
            switch (Form1.clustering_choices_status)
            {
                case combo_clustering_choices.bigrams:
                    break;
                case combo_clustering_choices.monograms:
                    break;
                case combo_clustering_choices.following_char:
                    break;
                case combo_clustering_choices.previous_char:
                    break;
                case combo_clustering_choices.following_distance:
                    break;
                case combo_clustering_choices.previous_distance:
                    break;
                case combo_clustering_choices.vocabulary:
                    limits_are_2d = false;
                    break;
                case combo_clustering_choices.wordslength_text:
                    limits_are_2d = false;
                    break;
                case combo_clustering_choices.wordslength_vocabulary:
                    limits_are_2d = false;
                    break;
                default:
                    mdError error = new mdError();
                    error.root("SOFTWARE ERROR", "Cannot find clustering_choices_status = " + Form1.clustering_choices_status + " in FormClustering.enforce_controls_coherency");
                    break;
            }

            if (limits_are_2d == true)
            {
                textBox_clustering_choices_linear_size_limit.Enabled = false;
                label_clustering_choices_linear.Enabled = false;
                checkBox_clustering_choices_remove_rare_characters.Enabled = true;
                textBox_clustering_choices_rare_characters_cutoff.Enabled = true;
                label_clustering_choices_2d.Enabled = true;
            }
            else
            {
                textBox_clustering_choices_linear_size_limit.Enabled = true;
                label_clustering_choices_linear.Enabled = true;
                checkBox_clustering_choices_remove_rare_characters.Enabled = false;
                textBox_clustering_choices_rare_characters_cutoff.Enabled = false;
                label_clustering_choices_2d.Enabled = false;
            }

        }



        private void button_clustering_choices_proceed_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox_clustering_choices_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.clustering_choices_status = (combo_clustering_choices)combo_clustering_choices_structure[comboBox_clustering_choices.SelectedIndex].enum_value;

            enforce_controls_coherency();
        }

        private void checkBox_clustering_choices_remove_rare_characters_CheckedChanged(object sender, EventArgs e)
        {
            Form1.clustering_choices_remove_rare_characters = checkBox_clustering_choices_remove_rare_characters.Checked;
        }

        private void textBox_clustering_choices_linear_size_limit_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_clustering_choices_linear_size_limit.Text, out value) == true)
            {
                if (value >= Form1.min_linear_graph_length && value <= Form1.min_linear_graph_length)
                {
                    Form1.clustering_choices_linear_limit = value;
                }
            }
        }

        private void textBox_clustering_choices_linear_size_limit_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_clustering_choices_linear_size_limit_TextChanged(sender, e);
        }

        private void textBox_clustering_choices_linear_size_limit_Leave(object sender, EventArgs e)
        {
            textBox_clustering_choices_linear_size_limit_TextChanged(sender, e);
        }

        private void textBox_clustering_choices_rare_characters_cutoff_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox_clustering_choices_rare_characters_cutoff.Text, out value) == true)
            {
                if (value >= FormCompare.rare_characters_min_cutoff && value <= FormCompare.rare_characters_max_cutoff)
                {
                    Form1.clustering_choices_rare_characters_cutoff = value;
                }
            }
        }

        private void textBox_clustering_choices_rare_characters_cutoff_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape)) textBox_clustering_choices_rare_characters_cutoff_TextChanged(sender, e);
        }

        private void textBox_clustering_choices_rare_characters_cutoff_Leave(object sender, EventArgs e)
        {
            textBox_clustering_choices_rare_characters_cutoff_TextChanged(sender, e);
        }
    }
}
