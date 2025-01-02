using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class ParseGrammarsLibrary
    {

        public static ParseVoynich.ParsingGrammar LOOP_LDK_final()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-La1 (LDK last)";
            grammar.notes = "QCSHY at start, LDK at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Take care! Overlapping strings must be ordered by length to avoid queer chunkifications. Ie. [iii, ii, i], not [i, ii, iii]
            grammar.slots_table = new string[6][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh", "y" };
            grammar.slots_table[1] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[2] = new string[] { "o" };
            grammar.slots_table[3] = new string[] { "a" };
            grammar.slots_table[4] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[5] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };


            // Word types containing 'rare' characters which do not appear in the grammar cannot be found. The following list is used to esclude them from the counts
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // But, some rare characters could be included in some of the slot glyphs (this is typical with Voynic manuscript).
            //   For instance: let's say 'tchodypodar' is not found because it needs too many chunks. It contains the rare characters 'c' and 'h', but we need to avoid it to be
            //     discarded, because 'c' and 'h' are in the 'ch' group, which the grammar can generate. The following list defines the groups
            //     wherein rare characters are not considered 'rare'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar LOOP_LDKY_final()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-La (LDKY last)";
            grammar.notes = "QCSH at start, LDKY at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[6][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[2] = new string[] { "o" };
            grammar.slots_table[3] = new string[] { "a" };
            grammar.slots_table[4] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[5] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m", "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }



        public static ParseVoynich.ParsingGrammar LOOP_LDK_Y_final()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lay (LDK+Y last)";
            grammar.notes = "QCSH at start, LDK+Y at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[7][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[2] = new string[] { "o" };
            grammar.slots_table[3] = new string[] { "a" };
            grammar.slots_table[4] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[5] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[6] = new string[] { "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }

        public static ParseVoynich.ParsingGrammar LOOP_test()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP TEST";
            grammar.notes = "test";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[8][];

            grammar.slots_table[0] = new string[] { "q" };
            grammar.slots_table[1] = new string[] { "o" };
            grammar.slots_table[2] = new string[] { "a" };
            grammar.slots_table[3] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[4] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[5] = new string[] { "l", "k", "d", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[6] = new string[] { "y" };
            grammar.slots_table[7] = new string[] { "ch", "sh" };



            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }



        public static ParseVoynich.ParsingGrammar LOOP_LDK_initial()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lb1 (LDK initial)";
            grammar.notes = "QCSHY at start, followed by LDK";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[6][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh", "y" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[2] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[3] = new string[] { "o" };
            grammar.slots_table[4] = new string[] { "a" };
            grammar.slots_table[5] = new string[] { "iii", "ii", "i" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar LOOP_LDKY_initial()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lb (LDKY initial)";
            grammar.notes = "QCSH at start, followed by LDKY";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[6][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m", "y" };
            grammar.slots_table[2] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[3] = new string[] { "o" };
            grammar.slots_table[4] = new string[] { "a" };
            grammar.slots_table[5] = new string[] { "iii", "ii", "i" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar LOOP_LDK_Y_initial()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lby (LDK+Y initial)";
            grammar.notes = "QCSH at start, followed by LDK+Y";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[7][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[2] = new string[] { "y" };
            grammar.slots_table[3] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[4] = new string[] { "o" };
            grammar.slots_table[5] = new string[] { "a" };
            grammar.slots_table[6] = new string[] { "iii", "ii", "i" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }



        public static ParseVoynich.ParsingGrammar LOOP_LDK_doubled()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lc1 (LDK double)";
            grammar.notes = "QCSHY at start, followed by LDK + LDK at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[7][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh", "y" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[2] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[3] = new string[] { "o" };
            grammar.slots_table[4] = new string[] { "a" };
            grammar.slots_table[5] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[6] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar LOOP_LDKY_doubled()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lc (LDKY double)";
            grammar.notes = "QCSH at start, followed by LDKY, + LDKY at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[7][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m", "y" };
            grammar.slots_table[2] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[3] = new string[] { "o" };
            grammar.slots_table[4] = new string[] { "a" };
            grammar.slots_table[5] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[6] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m", "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar LOOP_LDK_Y_doubled()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "LOOP-Lcy (LDK+Y double)";
            grammar.notes = "QCSH at start, followed by LDK+Y, + LDK+Y at the end";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[9][];

            grammar.slots_table[0] = new string[] { "q", "ch", "sh" };
            grammar.slots_table[1] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[2] = new string[] { "y" };
            grammar.slots_table[3] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[4] = new string[] { "o" };
            grammar.slots_table[5] = new string[] { "a" };
            grammar.slots_table[6] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[7] = new string[] { "l", "d", "k", "r", "s", "t", "p", "f", "cth", "ckh", "cph", "cfh", "n", "m" };
            grammar.slots_table[8] = new string[] { "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }



        public static ParseVoynich.ParsingGrammar Zattera()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "Zattera's SLOT";
            grammar.notes = "";

            grammar.default_loop_repeats = 1;
            grammar.max_loop_repeats = 3;


            grammar.slots_table = new string[12][];

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table[0] = new string[] { "q", "s", "d" };
            grammar.slots_table[1] = new string[] { "o", "y" };
            grammar.slots_table[2] = new string[] { "l", "r" };
            grammar.slots_table[3] = new string[] { "t", "k", "p", "f" };
            grammar.slots_table[4] = new string[] { "ch", "sh" };
            grammar.slots_table[5] = new string[] { "cth", "ckh", "cph", "cfh" };
            grammar.slots_table[6] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[7] = new string[] { "s", "d" };
            grammar.slots_table[8] = new string[] { "o", "a" };
            grammar.slots_table[9] = new string[] { "iii", "ii", "i" };
            grammar.slots_table[10] = new string[] { "d", "l", "r", "m", "n" };
            grammar.slots_table[11] = new string[] { "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };


            return grammar;
        }






        public static ParseVoynich.ParsingGrammar ThomasCoon_v2()
        {
            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "ThomasCoon's Version2";
            grammar.notes = "";

            grammar.slots_table = new string[12][];

            grammar.default_loop_repeats = 1;
            grammar.max_loop_repeats = 3;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table[0] = new string[] { "q", "d" };  
            grammar.slots_table[1] = new string[] { "ch", "sh", "s" };
            grammar.slots_table[2] = new string[] { "ol", "o", "a", "y" };
            grammar.slots_table[3] = new string[] { "t", "k", "p", "f" };
            grammar.slots_table[4] = new string[] { "ch", "sh" };
            grammar.slots_table[5] = new string[] { "eee", "ee", "e" };
            grammar.slots_table[6] = new string[] { "o", "a", "y" };
            grammar.slots_table[7] = new string[] { "r", "l" };
            grammar.slots_table[8] = new string[] { "d", "s", "ch", "t" };
            grammar.slots_table[9] = new string[] { "o", "a" };
            grammar.slots_table[10] = new string[] { "m", "n", "r", "l", "iin", "in" };
            grammar.slots_table[11] = new string[] { "s", "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };

            return grammar;
        }


        public static ParseVoynich.ParsingGrammar CVC_Voynich()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "CVC Voynich";

            grammar.notes = "CVC 'syllable' structure";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[3][];

            grammar.slots_table[0] = new string[] { "ckh", "cfh", "cth", "cph", "ch", "sh", "d", "f", "k", "l", "m", "n", "p", "q", "r", "s", "t", "y" };
            grammar.slots_table[1] = new string[] { "a", "e", "i", "o" };
            grammar.slots_table[2] = new string[] { "ckh", "cfh", "cth", "cph", "ch", "sh", "d", "f", "k", "l", "m", "n", "p", "q", "r", "s", "t", "y" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'c', 'h', 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };

            return grammar;
        }





        public static ParseVoynich.ParsingGrammar CVC_generic_language()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "CVC generic language";

            grammar.notes = "CVC 'syllable' structure";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[3][];

            grammar.slots_table[0] = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            grammar.slots_table[1] = new string[] { "ai", "ia", "oi", "ie", "io", "iu", "ua", "ue", "ui", "uo", "a", "e", "i", "o", "u" };
            grammar.slots_table[2] = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. 'tchodypodar' non viene trovata perchè ha troppe sillabe: bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { };


            return grammar;
        }


        public static ParseVoynich.ParsingGrammar CCVC_generic_language()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "CCVC generic language";

            grammar.notes = "CCVC 'syllable' structure";

            grammar.default_loop_repeats = 5;
            grammar.max_loop_repeats = 8;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[4][];

            grammar.slots_table[0] = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            grammar.slots_table[1] = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            grammar.slots_table[2] = new string[] { "ai", "ia", "oi", "ie", "io", "iu", "ua", "ue", "ui", "uo", "a", "e", "i", "o", "u" };
            grammar.slots_table[3] = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. 'tchodypodar' non viene trovata perchè ha troppe sillabe: bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { };


            return grammar;
        }





        public static ParseVoynich.ParsingGrammar Trivial_CSET()
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "Trivial CSET for Voynich";

            grammar.notes = "One slot containing the whole character set";

            grammar.default_loop_repeats = 10;
            grammar.max_loop_repeats = 16;

            // Occhio che qua le stringhe che si overlappano DEVONO RIGOROSAMENTE essere in ordine di lunghezza !!!!
            grammar.slots_table = new string[1][];

            grammar.slots_table[0] = new string[] { "c", "h", "d", "f", "k", "l", "m", "n", "p", "q", "r", "s", "t", "y", "a", "e", "i", "o" };


            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { };

            return grammar;
        }






        public static List<ParseVoynich.ParsingGrammar> GenGrammarsList()
        {
            List<ParseVoynich.ParsingGrammar> grammars_list = new List<ParseVoynich.ParsingGrammar>();

            ParseVoynich.ParsingGrammar new_grammar = new ParseVoynich.ParsingGrammar();


            new_grammar = LOOP_LDK_Y_final();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDKY_final();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDK_final();
            grammars_list.Add(new_grammar);


            new_grammar = LOOP_LDK_Y_initial();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDKY_initial();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDK_initial();
            grammars_list.Add(new_grammar);


            new_grammar = LOOP_LDK_Y_doubled();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDKY_doubled();
            grammars_list.Add(new_grammar);

            new_grammar = LOOP_LDK_doubled();
            grammars_list.Add(new_grammar);


            new_grammar = Zattera();
            grammars_list.Add(new_grammar);

            new_grammar = ThomasCoon_v2();
            grammars_list.Add(new_grammar);

            new_grammar = Trivial_CSET();
            grammars_list.Add(new_grammar);


            new_grammar = LOOP_test();
            grammars_list.Add(new_grammar);


            new_grammar = CVC_Voynich();
            grammars_list.Add(new_grammar);

            new_grammar = CVC_generic_language();
            grammars_list.Add(new_grammar);

            new_grammar = CCVC_generic_language();
            grammars_list.Add(new_grammar);

            return grammars_list;
        }

        public static ParseVoynich.ParsingGrammar Trivial_WSET()
        {
            // Grammatica speciale!! viene creata direttamente dal vocabolario dei work tokens!

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar.name = "Trivial WSET";

            grammar.notes = "";

            grammar.default_loop_repeats = 1;
            grammar.max_loop_repeats = 1;

            // Le parole che contengono 'rare' characters che non compaiono come tali nella grammatica non possono essere trovate. La lista seguente serve
            //   per escluderle dai conteggi
            grammar.rare_characters = new char[] { 'g', 'x', 'v', 'z', 'b', 'j', 'u' };
            // Ma dato che certi caratteri rari potrebbero essere inclusi in alcuni glifi negli slots bisogna definire anche quelli (caso tipico per il Voynich)
            //   P.es. supponiamo che 'tchodypodar' non viene trovata perchè ha troppe sillabe e bisogna assolutamente evitare che venga invece considerata
            //   come 'non trovata ma contiene rarechars', dato che contiene sia 'c' che 'h'. 
            grammar.rare_chars_groups = new string[] { "ch", "sh", "ckh", "cth", "cph", "cfh" };

            grammar.slots_table = get_Trivial_WSET_slots_table(grammar.rare_characters, grammar.rare_chars_groups);

            return grammar;
        }

        private static string[][] get_Trivial_WSET_slots_table(char[] rare_characters, string[] rare_chars_groups)
        {
            // 1 solo slot per questa grammatica! Che però contiene tutte le parole, escluse quelle coi rare_characters
            int valid_word_types = 0;
            foreach (EValueOcc word_type in Form1.text_analyzer.analysis_results.vocabulary_words_distribution)
            {
                if (ParseVoynich.contains_rare_characters(word_type.element, rare_characters, rare_chars_groups) == false)
                {
                    valid_word_types++;
                }
            }

            string[][] slots_table = new string[1][];           // 1 solo slot per questa grammatica!        
            slots_table[0] = new string[valid_word_types];      // Ma tutte le parole!
            int counter = 0;
            foreach (EValueOcc word_type in Form1.text_analyzer.analysis_results.vocabulary_words_distribution)
            {
                if (ParseVoynich.contains_rare_characters(word_type.element, rare_characters, rare_chars_groups) == false)
                {
                    slots_table[0][counter] = word_type.element;
                    counter++;
                }
            }

            return slots_table;
        }







        public static string[] get_Voynich_grammars_combobox_list()        
        {

            List<ParseVoynich.ParsingGrammar> grammars_list = GenGrammarsList();

            string[] combobox_list = new string[grammars_list.Count];
            for (int i = 0; i < grammars_list.Count; i++)
            {
                combobox_list[i] = grammars_list[i].name;
            }

            return combobox_list;
        }



        public static void get_parsing_grammar(int combobox_index, ref ParseVoynich.ParsingResults Voynich_parsing_results)
        {

            ParseVoynich.ParsingGrammar grammar = new ParseVoynich.ParsingGrammar();

            grammar = ParseGrammarsLibrary.GenGrammarsList()[combobox_index];


            Voynich_parsing_results = new ParseVoynich.ParsingResults();


            Voynich_parsing_results.grammar = new ParseVoynich.ParsingGrammar();

            // Copia della grammatica
            Voynich_parsing_results.grammar.name = grammar.name;
            Voynich_parsing_results.grammar.notes = grammar.notes;

            int slots_number = grammar.slots_table.GetLength(0);
            Voynich_parsing_results.grammar.slots_table = new string[slots_number][];
            int counter = 0;
            foreach (string[] slot in grammar.slots_table)
            {
                Form1.Voynich_parsing_results.grammar.slots_table[counter] = slot;
                counter++;
            }

            Voynich_parsing_results.grammar.default_loop_repeats = grammar.default_loop_repeats;
            Voynich_parsing_results.grammar.max_loop_repeats = grammar.max_loop_repeats;

            Voynich_parsing_results.grammar.rare_characters = new char[grammar.rare_characters.Length];
            counter = 0;
            foreach (char rare_char in grammar.rare_characters)
            {
                Voynich_parsing_results.grammar.rare_characters[counter] = rare_char;
                counter++;
            }

            Voynich_parsing_results.grammar.rare_chars_groups = new string[grammar.rare_chars_groups.Length];
            counter = 0;
            foreach (string rare_char_group in grammar.rare_chars_groups)
            {
                Voynich_parsing_results.grammar.rare_chars_groups[counter] = rare_char_group;
                counter++;
            }
            // Fine copia della grammatica

        }



    }





}
