using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Template
{
    public class PreprocessText {

        public static mdError preprocess_remarks(String source, ref String preprocessed_text, ref String remainder_text, ref List<RegexCommand> regex_commands)
        {
            mdError error = new mdError();
            string message = "";

            preprocessed_text = "";
            remainder_text = "";
            regex_commands = new List<RegexCommand>();  // lista dei comandi Regex trovati nei 'commenti' del file
           

            // Nel file di testo una linea che contiene la sequenza '%c%' è considerata un 'commento' da lì in poi
            //  (avrei voluto usare una stringa più semplice imponendo che fosse a inizio riga, ma per qualche motivo "^%c%" non funzionava (^ è il comando per inizio riga),
            //    e inoltre avrei voluto supportare la possibiliutà di blocchi di commenti (tipo /* ... */) ma era troppo incasinato

            //    I commenti vengono scartati dal testo che viene poi processato 
            //    Ma dai commenti vengono estratti i comandi per il pre-processor, per esempio i comandi Regex o le opzioni

            // Ricava la lista dei commenti ed il testo depurato dai commenti
            MatchCollection remarks_set = Regex.Matches("dummy", "dum");    // ehrm, crea una MatchCollection vuota
            try
            {
                remarks_set = Regex.Matches(source, "%c%.*");
                preprocessed_text = Regex.Replace(source, "%c%.*", " ");        // non si può usare "" come stringa di sostituzione, deve contenere almeno un carattere perchè funzioni
            }
            catch
            {
                message = "ERROR: cannot perform basic pre-processing, possibly text is too long?";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                preprocessed_text = "Text Analyzer Error";
                remainder_text = "Text Analyzer Error";
            }



            // Creazione di remainder_text (tutti i commenti in un'unica stringa)
            foreach (Match match in remarks_set)
            {
                remainder_text += match.Value + "\n"; // basta ggiungere il \n, il \r viene restituito da regex in match.Value
            }


            // Estrazione dei comandi di preprocessing dai commenti OCCHIO CHE IL FORMATO E' RIGIDO!!

            //  %c%OPTION;name;value;remarks                        se un option manca resta il valore di default (file .cfg)
            //  %c%REGEX;string_search;string_replace;remarks

            foreach (Match match in remarks_set)
            {
                string[] user_command = new string[0];

                string line = match.Value;

                if (line.StartsWith("%c%OPTION;") == true)
                {
                    // NON IMPLEMENTATA!!!
                }

                if (line.StartsWith("%c%REGEX;") == true)
                {
                    try
                    {
                        user_command = Regex.Split(match.Value, ";");

                        if (user_command.Length == 0 || user_command.Length > 4)
                        {
                            message = "WARNING: found and discarded an invalid user-defined preprocessing Regex command: " + line;
                            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                        }
                        else
                        {
                            RegexCommand regex_command = new RegexCommand();
                            regex_command.search_string = user_command[1];
                            regex_command.replacement_string = user_command[2];
                            regex_command.user_remark = user_command[3];
                            regex_commands.Add(regex_command);
                        }
                    }
                    catch
                    {
                        message = "WARNING: problems splitting Regex user command ";
                        Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    }
                }
            }

            return (error);
        }


        public static mdError preprocess_options(string source, ref string processed_text, TextAnalysis_used_parameters options, ref TextAnalysis_results results, bool verbose)
        {
            mdError error = new mdError();
            string message = "";

            processed_text = source;


            // Trap per i caratteri balordi, che comunque non dovrebbero mai esserci da quanto in Form1 ho inserito, in load_and_analyze, la gestione che ricarica
            //   un file con codifica UTF7 nel caso che caricandolo con UTF8 avesse dei caratteri balordi
            int caratteri_balordi = Regex.Matches(processed_text, "�").Count;
            if (caratteri_balordi != 0)
            {
                error.root("", "Warning: source text(s) contain unrecognizable characters (�) which almost surely will result in wrong words being added to the vocabulary. Probably the file uses a coding which different from UTF-8 or UTF-7. You can try, as a workaround, to open the original file, select all and copy everything into a newly-created .txt file.");
                error.Display_and_Clear();
            }

            // Eliminazione numeri
            if (options.discard_all_arabic_numbers == true)
            {
                string new_text = "";
                try
                {
                    results.discarded_arabic_numbers_characters = Regex.Matches(processed_text, "\\d").Count;
                    new_text = Regex.Replace(processed_text, "\\d", "");
                }
                catch
                {
                    message = "ERROR: problems removing character digits";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });                
                }
                message = "Removed " + results.discarded_arabic_numbers_characters + " characters representing arabic numerals";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                processed_text = new_text;
            }


            // Le gestioni di options.words_linked_by_an_apostrophe_are_discarded e options.words_linked_by_a_dash_are_discarded sono
            //    difficili da fare con Regex: sono gestite in un altro modo e spostate alla fine, dopo il precleaning del file (in preprocess_Regex())

            // Gestione apostrofi
            if (options.apostrophe_is_a_separator == true)
            {
                if (options.words_linked_by_an_apostrophe_are_joined == true)
                {
                    string new_text = "";
                    try
                    {
                        results.discarded_apostrophes = Regex.Matches(processed_text, "'").Count;
                        new_text = Regex.Replace(processed_text, "'", "");
                    }
                    catch
                    {
                        message = "ERROR: problems removing apostrophes";
                        Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    }
                    message = "Removed " + results.discarded_apostrophes + " apostrophes, this may have joined words together, ie: [un'ora] becomes [unora]";
                    if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    processed_text = new_text;
                }
                if (options.words_linked_by_an_apostrophe_are_separated == true)
                {
                    string new_text = "";
                    try
                    {
                        results.discarded_apostrophes = Regex.Matches(processed_text, "'").Count;
                        new_text = Regex.Replace(processed_text, "'", " ");
                    }
                    catch
                    {
                        message = "ERROR: problems removing apostrophes";
                        Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    }
                    message = "Removed " + results.discarded_apostrophes + " apostrophes, this may have split words, ie: [un'ora] becomes [un ora]";
                    if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                    processed_text = new_text;
                }
            }


            if (options.words_linked_by_a_dash_are_joined == true)
            {
                string new_text = "";
                try
                {
                    results.discarded_dashes = Regex.Matches(processed_text, "-").Count;
                    new_text = Regex.Replace(processed_text, "-", "");
                }
                catch
                {
                    message = "ERROR: problems removing dashes";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                }
                message = "Removed " + results.discarded_dashes + " dashes, this may have joined words together, ie: [ex-cep-tio-nal] becomes [exceptional]";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                processed_text = new_text;
            }
            if (options.words_linked_by_a_dash_are_separated == true)
            {
                string new_text = "";
                try
                {
                    results.discarded_dashes = Regex.Matches(processed_text, "-").Count;
                    new_text = Regex.Replace(processed_text, "-", " ");
                }
                catch
                {
                    message = "ERROR: problems removing dashes";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                }
                message = "Removed " + results.discarded_dashes + " dashes, this may have split words, ie: [ex-cep-tio-nal] becomes [ex cep tio nal]";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                processed_text = new_text;
            }

            if (options.keep_distinction_between_upper_and_lowercase == false)
            {
                string new_text = "";
                new_text = processed_text.ToLower();
                processed_text = new_text;
            }

            return (error);
        }


        public static mdError preprocess_Regex(string source, ref string processed_text, List<RegexCommand> regex_commands, TextAnalysis_used_parameters options, bool verbose)
        {
            mdError error = new mdError();
            string message = "";

            processed_text = source;
            string new_text = "";

            foreach (RegexCommand command in regex_commands)
            {
                message = "User-defined Regex command: searching '" + command.search_string + "' and replacing with '" + command.replacement_string + "' (user remark: " + command.user_remark + ")";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

                try
                {
                    new_text = Regex.Replace(processed_text, command.search_string, command.replacement_string);
                    processed_text = new_text;
                }
                catch
                {
                    message = "WARNING: Regex engine could not process user-defined command: search '" + command.search_string + "' and replace with '" + command.replacement_string + "'. Result discarded.";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                }

            }

            // Rimozione di 'abbreviazioni': hanno la forma spazio-lettera-punto (si spera non ce ne sia una proprio ad inizio file, dove lo spazio non c'è e non viene rimossa xD)
            //   Viene ripetuta due volte, per rimuovere completamente anche abbreviazioni come a.C
            //   Purtroppo non è facile rimuovere anche abbreviazioni come q.o.d. o S.P.Q.R., questo perchè il replace che uso elimina tre chars in totale
            //     (il carattere non-letterale che precede la lettera, la lettera e il punto che la segue), col risultato che p.es. " q.o.d." diventa " o " dopo
            //      il primo passaggio, e la 'o' non viene eliminata. Dovrei rimpiazzare solo due caratteri per l'abbreviazioni, cos' " q.o.d." diventa "  o. " e al
            //      secondo passaggio verrebbe eliminata anche la "o.", ma non so come fare con Regex e sinceramente non ci voglio perdere tempo
            try
            {
                Form1.text_analyzer.analysis_results.discarded_putative_abbreviations = Regex.Matches(processed_text, "\\W.\\.").Count;
                new_text = Regex.Replace(processed_text, "\\W.\\.", " ");
                processed_text = new_text;

                Form1.text_analyzer.analysis_results.discarded_putative_abbreviations += Regex.Matches(processed_text, "\\W.\\.").Count;
                new_text = Regex.Replace(processed_text, "\\W.\\.", " ");
                processed_text = new_text;

                message = "Discarded " + Form1.text_analyzer.analysis_results.discarded_putative_abbreviations + " putative abbreviations (single characters followed by a dot)";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }
            catch
            {
                message = "WARNING: Regex engine could not remove putative abbreviations";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }




            // Gestioni di options.words_linked_by_an_apostrophe_are_discarded e options.words_linked_by_a_dash_are_discarded 
            //    E' difficile farle direttamente con Regex, per cui separo il testo in parole, scarto quelle che vanno scartate e poi ricostruisco il testo
            //    Poco elegante, ma non ho trovato di meglio

            if (options.words_linked_by_an_apostrophe_are_discarded == true || options.words_linked_by_a_dash_are_discarded == true)
            {
                message = "Removing words containing dashes and/or apostrophes, depending on the pre-processing options....";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

                // Come prima cosa devo compattare tutti gli spazi (per poter suddividere il testo in parole)
                try
                {
                    new_text = Regex.Replace(processed_text, "\\s+", " ");  // compattazione degli spazi
                    processed_text = new_text;
                }
                catch
                {
                    message = "ERROR: Regex engine could not remove redundant space characters 1";
                    Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
                }

                // Suddivisione del testo in parole (sweet mama Regex...)
                string[] scratch_tokenized_text = Regex.Split(processed_text, " ");

                // Ricostruzione del testo scartando le parole con dash e/o apostrofo
                List<string> out_tokenized_text = new List<string>();
                foreach (string word in scratch_tokenized_text)
                {
                    if ((options.words_linked_by_a_dash_are_discarded == true && word.Contains("-") == true) ||
                         (options.words_linked_by_an_apostrophe_are_discarded == true && word.Contains("'") == true))
                    {
                        if (options.words_linked_by_a_dash_are_discarded == true && word.Contains("-") == true) Form1.text_analyzer.analysis_results.discarded_words_containing_a_dash += 1;
                        if (options.words_linked_by_an_apostrophe_are_discarded == true && word.Contains("'") == true) Form1.text_analyzer.analysis_results.discarded_words_containing_an_apostrophe += 1;
                        continue;
                    }
                    else
                    {
                        out_tokenized_text.Add(word);
                    }
                }
                processed_text = string.Join(" ", out_tokenized_text);
            }



            if (Form1.text_analyzer.analysis_results.discarded_words_containing_a_dash != 0)
            {
                message = "Discarded " + Form1.text_analyzer.analysis_results.discarded_words_containing_a_dash + " words containing a dash '-'";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }
            if (Form1.text_analyzer.analysis_results.discarded_words_containing_an_apostrophe != 0)
            {
                message = "Discarded " + Form1.text_analyzer.analysis_results.discarded_words_containing_an_apostrophe + " words containing an apostrophe '''";
                if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }


            message = "Removing punctuation characters and some special character such as 'º' (degree symbol), may take a while...:";
            if (verbose) Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

            // Replacement di un apostrofo con-standard (ma purtroppo usato spesso) con quello standard
            //   Ps.: esiste almeno anche un altro apostrofo non-standard: '‛', se si inizia a tovarlo spesso si potrebbe rimpiazzare automaticamente anche quello
            try
            {
                new_text = Regex.Replace(processed_text, "’", "'");
                processed_text = new_text;
            }
            catch
            {
                message = "ERROR: Regex engine could not convert non-standard apostrophe '’' to standard apostrophe";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }


            // Pulizia finale del testo con rimozione di tutti i caratteri non letterali che vengono convertiti a spazi, dopodichè tutti gli spazi vengono compattati

            // Per rimuovere i caratteri non letterali uso la stringa \W di Regex, che però ha due difetti:
            //      - non rimuove gli underscore
            //      - ma rimuove gli apostrofi (se sono sopravvissuti alle varie opzioni apostrophe_is_a_separator)

            //      Uso uno schifoso trucco: rimuovo gli underscore, cambio gli apostrofi in underscore, elimino i non-letterali e rimetto gli apostrofi
            try
            {
                new_text = Regex.Replace(processed_text, "_", " "); // underscores convertiti in spazi
                processed_text = new_text;
                new_text = Regex.Replace(processed_text, "'", "_");
                processed_text = new_text;
                new_text = Regex.Replace(processed_text,"\\W"," "); // Rimozione caratteri non letterali (convertiti a spazi per non collegare assieme delle parole per sbaglio)
                processed_text = new_text;
                new_text = Regex.Replace(processed_text, "º", " "); // Anche il carattere grado 'º' non viene eliminato da \W
                processed_text = new_text;
            }
            catch
            {
                message = "ERROR: Regex engine could not remove non-word characters";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }

            // E alla fine ricompattiamo tutti gli spazi (potrebbe esserecene bisogno)
            try
            {
                new_text = Regex.Replace(processed_text, "\\s+", " ");  // compattazione degli spazi
                processed_text = new_text;
            }
            catch
            {
                message = "ERROR: Regex engine could not remove redundant space characters 2";
                Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });
            }
            // E riconvertiamo gli underscore in apostrofi
            new_text = Regex.Replace(processed_text, "_", "'");
            processed_text = new_text;


            return (error);
        }


    }
        
}

