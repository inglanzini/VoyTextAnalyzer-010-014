using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class CompareResults
    {
        // Dati 2D
        public List<List<List<EValueOcc>>> bigrams_distribution = new List<List<List<EValueOcc>>>();
        public List<EValue> bigrams_distances_from_first_blind = new List<EValue>();
        public List<EValue> bigrams_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> bigrams_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> bigrams_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<List<EValue>>> bigrams_vs_theoric_distribution = new List<List<List<EValue>>>();
        public List<EValue> bigrams_vs_theoric_distances_from_first_blind = new List<EValue>();
        public List<EValue> bigrams_vs_theoric_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> bigrams_vs_theoric_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> bigrams_vs_theoric_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<List<EValue>>> following_character_distribution = new List<List<List<EValue>>>();
        public List<EValue> following_character_distances_from_first_blind = new List<EValue>();
        public List<EValue> following_character_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> following_character_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> following_character_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<List<EValue>>> previous_character_distribution = new List<List<List<EValue>>>();
        public List<EValue> previous_character_distances_from_first_blind = new List<EValue>();
        public List<EValue> previous_character_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> previous_character_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> previous_character_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<List<EValue>>> following_chardistances_distribution = new List<List<List<EValue>>>();
        public List<EValue> following_chardistances_distances_from_first_blind = new List<EValue>();
        public List<EValue> following_chardistances_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> following_chardistances_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> following_chardistances_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<List<EValue>>> previous_chardistances_distribution = new List<List<List<EValue>>>();
        public List<EValue> previous_chardistances_distances_from_first_blind = new List<EValue>();
        public List<EValue> previous_chardistances_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> previous_chardistances_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> previous_chardistances_all_distances_unblinded = new List<List<EValue_2d>>();

        // Dati 1D
        public List<List<EValueOcc>> vocabulary_words_distribution = new List<List<EValueOcc>>();
        public List<EValue> vocabulary_words_distances_from_first_blind = new List<EValue>();
        public List<EValue> vocabulary_words_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> vocabulary_words_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> vocabulary_words_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValueOcc_extended>> wordslength_text_distribution = new List<List<EValueOcc_extended>>();
        public List<EValue> wordslength_text_distances_from_first_blind = new List<EValue>();
        public List<EValue> wordslength_text_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> wordslength_text_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> wordslength_text_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValue>> wordslength_vocabulary_distribution = new List<List<EValue>>();
        public List<EValue> wordslength_vocabulary_distances_from_first_blind = new List<EValue>();
        public List<EValue> wordslength_vocabulary_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> wordslength_vocabulary_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> wordslength_vocabulary_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValueOcc>> syllables_singlevowel_distribution = new List<List<EValueOcc>>();
        public List<EValue> syllables_singlevowel_distances_from_first_blind = new List<EValue>();
        public List<EValue> syllables_singlevowel_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> syllables_singlevowel_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> syllables_singlevowel_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValueOcc>> syllables_multiplevowel_distribution = new List<List<EValueOcc>>();
        public List<EValue> syllables_multiplevowel_distances_from_first_blind = new List<EValue>();
        public List<EValue> syllables_multiplevowel_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> syllables_multiplevowel_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> syllables_multiplevowel_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValueOcc>> monograms_distribution = new List<List<EValueOcc>>();
        public List<EValue> monograms_distances_from_first_blind = new List<EValue>();
        public List<EValue> monograms_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> monograms_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> monograms_all_distances_unblinded = new List<List<EValue_2d>>();

        public List<List<EValueOcc>> monograms_no_spaces_distribution = new List<List<EValueOcc>>();
        public List<EValue> monograms_no_spaces_distances_from_first_blind = new List<EValue>();
        public List<EValue> monograms_no_spaces_distances_from_first_unblinded = new List<EValue>();
        public List<List<EValue_2d>> monograms_no_spaces_all_distances_blind = new List<List<EValue_2d>>();
        public List<List<EValue_2d>> monograms_no_spaces_all_distances_unblinded = new List<List<EValue_2d>>();

    }
}
