using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Template
{
    public class VocabularyStats
    {

        public static mdError get_vocabulary(string source_text, ref TextAnalysis_results analysis_results)
        {

            mdError error = new mdError();

            // Suddivisione del testo in parole (sweet mama Regex...)
            string[] scratch_tokenized_text = Regex.Split(source_text, " ");
            // Qua il problema è che all'inizio (o alla fine) di scratch_tokenized_text ci può essere una stringa nulla. Regex avrà i suoi motivi... comunque bisogna scartarle
            List<string> tokenized_text = new List<string>();
            foreach (string item in scratch_tokenized_text)
            {
                if (item != "")
                {
                    tokenized_text.Add(item);
                }
            }
           
            Form1.text_analyzer.analysis_results.total_characters_in_cleaned_text = source_text.Length;
            Form1.text_analyzer.analysis_results.total_spaces_in_cleaned_text = tokenized_text.Count;
            Form1.text_analyzer.analysis_results.total_literal_characters_in_cleaned_text = Form1.text_analyzer.analysis_results.total_characters_in_cleaned_text - Form1.text_analyzer.analysis_results.total_spaces_in_cleaned_text;

            // Costruiamo l'elenco completo delle parole (la chiave del dizionario) col loro numero di occorrenze
            Dictionary<string, EValueOcc> vocabulary = new Dictionary<string, EValueOcc>(); // Dictionary per velocità di elaborazione (ma non è sortabile)

            analysis_results.total_number_of_words_in_the_text = tokenized_text.Count;
            for (int token_index = 0; token_index < tokenized_text.Count; token_index++)
            {
                EValueOcc old_value = new EValueOcc();
                if (vocabulary.TryGetValue(tokenized_text[token_index], out old_value) == true)
                {
                    old_value.value += 1;
                    vocabulary.Remove(tokenized_text[token_index]);
                    vocabulary.Add(tokenized_text[token_index], old_value);
                }
                else
                {
                    EValueOcc new_value = new EValueOcc(); // impossibile riciclare old_value qua... provare per credere
                    new_value.element = tokenized_text[token_index];
                    new_value.value = 1;
                    vocabulary.Add(tokenized_text[token_index], new_value);
                }
            }

            // Dal dizionario compiliamo (e ordiniamo) tutte le statistiche derivate dal vocabolario
            get_and_format_all_vocabulary_stats(vocabulary, ref analysis_results);




            // Words length distribution: come prima cosa troviamo la lunghezza massima, in modo da poter dimensionare una lista
            int max_length = 0;
            foreach (EValueOcc evalue in vocabulary.Values)
            {
                if (evalue.element.Length > max_length) max_length = evalue.element.Length;
            }
            // Pre-inseriamo in lista i vari elements da 1 a max_length
            List<EValueOcc_extended> scratch_list = new List<EValueOcc_extended>();
            for (int i = 0; i < max_length; i++)
            {
                EValueOcc_extended evalue = new EValueOcc_extended();
                evalue.element = (i+1).ToString();  // da 1 lettera a max_length lettere
                scratch_list.Add(evalue);
            }

            // scansione del testo ed inserimento del numero delle occorrenze in lista
            foreach (string word in tokenized_text)
            {
                scratch_list[word.Length - 1].value += 1;
                scratch_list[word.Length - 1].element_additional = word; // Parola di esempio per la lunghezza considerata
            }

            // Trasferimento nella lista di output
            foreach (EValueOcc_extended occurrences in scratch_list)
            {
                EValueOcc_extended new_value = new EValueOcc_extended();
                new_value.element = occurrences.element;
                new_value.element_additional = occurrences.element_additional;   
                new_value.value = occurrences.value;
                analysis_results.words_length_distribution_in_text.Add(new_value);
            }
            // words-length_distribution è già ordinata per numero di caratteri


            // E ora ricaviamo la distribuzione lunghezza parole nel vocabolario
            Form1.text_analyzer.analysis_results.words_length_distribution_in_vocabulary = VocabularyStats.get_words_length_distribution_in_vocabulary(Form1.text_analyzer.analysis_results);

            return error;        
        }

        public static void get_and_format_all_vocabulary_stats(Dictionary<string, EValueOcc> vocabulary, ref TextAnalysis_results analysis_results)
        {

            // trasferiamo il dictionary in una lista, che è più comoda per elaborazioni successive (p.es. il dizionario non supporta il sorting)
            foreach (EValueOcc item in vocabulary.Values)
            {
                EValueOcc new_value = new EValueOcc();
                new_value.element = item.element;
                new_value.value = item.value;
                analysis_results.vocabulary_words_distribution.Add(new_value);
            }
            IComparer<EValueOcc> comparer = new TextCharsStats.CompareEValueOccByValue();
            analysis_results.vocabulary_words_distribution.Sort(comparer);

            // Calcoliamo anche il numero di hapax legomena
            int total_hapax = 0;
            foreach (EValueOcc word in analysis_results.vocabulary_words_distribution)
            {
                if (word.value == 1) total_hapax++;
            }
            analysis_results.total_hapax_legomena = total_hapax;
        }





        public static List<EValueOcc_extended> get_words_length_distribution_in_vocabulary(TextAnalysis_results analysis_results)
        {
            // Questa routine duplica parte del codice di calcolo della distribuzione sull'intero testo (poco sopra)
            List<EValueOcc_extended> distribution = new List<EValueOcc_extended>();

            // Words length distribution: come prima cosa troviamo la lunghezza massima, in modo da poter dimensionare una lista
            int max_length = 0;
            foreach (EValueOcc evalue in analysis_results.vocabulary_words_distribution)
            {
                if (evalue.element.Length > max_length) max_length = evalue.element.Length;
            }
            // Pre-inseriamo in lista i vari elements da 1 a max_length
            for (int i = 0; i < max_length; i++)
            {
                EValueOcc_extended evalue = new EValueOcc_extended();
                evalue.element = (i + 1).ToString();  // da 1 lettera a max_length lettere
                evalue.value = 0;
                distribution.Add(evalue);
            }

            // scansione del vocabolario ed inserimento del numero delle occorrenze in lista
            foreach (EValueOcc evalue in analysis_results.vocabulary_words_distribution)
            {
                distribution[evalue.element.Length - 1].value += 1;
                distribution[evalue.element.Length - 1].element_additional = evalue.element; // Parola di esempio per la lunghezza considerata
            }

            return distribution;
        }

    }
}
