using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class TextAnalysis_results
    {


        // Preprocessing
        public int discarded_arabic_numbers_characters;
        public int discarded_apostrophes;
        public int discarded_dashes;

        public int discarded_words_containing_a_dash;
        public int discarded_words_containing_an_apostrophe;

        public int discarded_putative_abbreviations;

        public int total_characters_in_cleaned_text = 0;
        public int total_literal_characters_in_cleaned_text = 0;
        public int total_spaces_in_cleaned_text = 0;



        // I dati espressi in OCCORRENZE (liste di EvalueOcc) sono AGGREGABILI (basta sommare i valori)
        //   I dati espressi come frequenze o probabilità o altri numeri NON sono aggregabili! Vanno ricalcolati in toto partendo da dati aggregabili!

        // Statistiche estese a tutto il testo
        public List<EValueOcc> monograms_distribution = new List<EValueOcc>();                          // OCCORRENZE dei caratteri (e include l'elenco caratteri negli elements)
        public List<EValueOcc> monograms_distribution_excluding_spaces = new List<EValueOcc>();         //      idem, ma senza il carattere 'spazio'

        public List<EValue> bigrams_theoretical_distribution = new List<EValue>();                      // FREQUENZE teoriche dei bigrammi, ricavate dalla distribuzione dei monogrammi (supposta uniforme)
        public List<List<EValue>> bigrams_theoretical_distribution_table = new List<List<EValue>>();    //     come la precedente, ma in forma lineare



        public List<EValueOcc> bigrams_distribution = new List<EValueOcc>();                            // OCCORRENZE effettive dei bigrammi
        public List<List<EValueOcc>> bigrams_distribution_table = new List<List<EValueOcc>>();          //     come la precedente, ma in forma lineare

        public List<EValue> bigrams_distribution_delta = new List<EValue>();                            //  Occorrenze effettive dei bigrammi divise per le teoriche in modo da evidenziare i bigrammi soppressi/enhanched
        public List<List<EValue>> bigrams_distribution_delta_table = new List<List<EValue>>();          //    Sono semplicemente numeri, non sono nè occorrenze nè frequenze/probabilità   


        public List<List<EValue>> following_character_distribution = new List<List<EValue>>();          // PROBABILITA' che un carattere 'X' sia seguito da un carattere 'c'
        public List<List<EValue>> previous_character_distribution = new List<List<EValue>>();           // PROBABILITA' che un carattere 'X' sia preceduto da un carattere 'c'


        public List<List<EValue>> monograms_distances_according_to_following_character = new List<List<EValue>>(); // Distanze geometriche di un carattere dall'altro
        public List<List<EValue>> monograms_distances_according_to_previous_character = new List<List<EValue>>();  //   Si tratta di numeri (distanze rms), min= 0 max = SQRT(2)



        // Statistiche estese al solo vocabolario
        public int total_number_of_words_in_the_text;
        public List<EValueOcc> vocabulary_words_distribution = new List<EValueOcc>();                         // OCCORRENZE di ogni parola nel testo (e include il vocabolario negli elements)
        public int total_hapax_legomena;

        public List<EValueOcc_extended> words_length_distribution_in_text = new List<EValueOcc_extended>();   // OCCORRENZE delle parole nel testo in funzione della loro lunghezza, con parola di esempio (nell' _extended)
        public List<EValueOcc_extended> words_length_distribution_in_vocabulary = new List<EValueOcc_extended>();   //  Idem, ma nel vocabolario

        public int words_with_apostrophe_not_syllabified;
        public List<EValueOcc> syllables_distribution_single_vowels = new List<EValueOcc>();            // OCCORRENZE pseudosillabe, qua tutte le vocali ('ortografiche'!) sono considerate nuclei separati
        public List<EValueOcc> syllables_distribution_multiple_vowels = new List<EValueOcc>();          // OCCORRENZE pseudosillabe, qua tutte le 'vocali ('ortografiche'!) adiacenti sono riunite in un unico nucle


        // Sempre un constructor di default per la serializzabilità
        public TextAnalysis_results() { }
    }


    // Un EValue non è altro che un numero (float) labellato da una stringa
    public class EValue
    {
        public string element;

        public float value;
    }

    // Un EValueOcc è un EValue, che però è specializzato per contenere OCCORRENZE, che sono numeri INTERI (e long, dato che i corpus di testi possono essere assai long xD)
    public class EValueOcc
    {
        public string element;

        public long value;
    }



    // EHHHH... ad EValue_2d AVREI DOVUTO PENSARCI PRIMA....
    //      Mi sarei evitato varie stranezze software dovute al fatto che negli EValue normali le stringhe delle intestazioni di riga e di colonna sono mischiate
    //        assieme in un unico element. Modificare tutte le gestioni adesso sarebbe troppo menoso
    public class EValue_2d
    {
        public string element_row;
        public string element_column;

        public float value;
    }


    // EValue_extended e EValueOcc_extended sono degli EValue/EValueOcc, ma oltre all'element ha anche un'altra stringa associata. Usata p.es. per le distribuzioni delle words_length,
    //   dove l'element contiene la lunghezza della parola, mentre element_additional contiene la parola di esempio per una certa lunghezza
   
    public class EValue_extended
    {
        public string element;
        public float value;

        public string element_additional;
    }
    public class EValueOcc_extended
    {
        public string element;
        public long value;

        public string element_additional;
    }



    // Questa classe è usata da find best matches (in Form1), serve per ordinare la lista dei matches
    public class DistanceAnalysis
    {
        public string filename;

        public float distance;

        public TextAnalyzerClass analysis;    
    }


}
