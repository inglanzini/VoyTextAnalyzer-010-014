using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.IO;

namespace Template
{

    // File contenente una serie di assets di supporto 'standard'

    class XStandards // questa classe non serve a niente, ma mi sembra prudente avere una classe con
    {                //   lo stesso nome del file...   
    }


    // 1) Classe di gestione uniforme degli errori. Copiata da Words Reborn 010 010

    //  .root(): inizializza l'errore, per esempio ("DATA ERROR", "Zero unexpected")

    //  L'errore può essere ritornato alla routine chiamante, che 
    //   può aggiungere informazioni con .append(). L'uscita senza errori deve ritornare un new mdError(),
    //   che ha il flag di errore settato a false. Il flag di errore può essere testato nella routine
    //   chiamante con .is_set() 

    //  L'errore può essere consumato (visualizzandolo) in qualsiasi momento
    //   con .Display_and_Throw(), CHE ESCE CON UN EXCEPTION (errore irrecuperabile). PER CUI:
    //   UNA ROUTINE CHE CONSUMA UN ERRORE TRAMITE .Display_and_Throw() DEVE ESSERE ALL'INTERNO DI UN TRY/CATCH
    //   (e in generale ritornerà un void), MENTRE NON HA SENSO METTERE IN UN TRY/CATCH UNA ROUTINE CHE RITORNA
    //   UN mdError (sempre ammesso che la routine gestisca correttamente via l'mdError
    //   tutti i possibili errori interni, ovviamente, cioè che non possa generare exceptions non trappate
    //   in un mdError).

    //  Un altro modo di consumare l'errore è tramite la .Display_and_Clear(), che esce senza exceptions
    //     (errore recuperabile o warning)

    // L'ultima possibilità è consumare l'errore tramite .Clear(), che non visualizza niente (errori
    //    recuperabili o warnings di piccola entità, diciamo). Diventerebbe più utile aggiungendo un log
    //    degli errori su file

    //  Se ci si dimentica di consumare un errore il destructor se ne accorge
    //     e visualizza un errore suo. Non è affidabilissimo perchè il destructor
    //     viene chiamato quando il garbage collector decide di farlo e non quando
    //     l'errore va fuori scopo e scompare dal software, il che vuol dire
    //     che il messaggio di errore non consumato può saltar fuori in qualsiasi
    //     momento (spesso alla chiusura del programma) anche se l'errore non è stato
    //     consumato molto tempo prima. Meglio che niente comunque :).

    // Possibili espansioni: file di log
    public class mdError
    {
        private Boolean error_set = false;
        private String message = "";
        private String caption = "NOT INITIALIZED (check software)";

        // Notare il destructor esplicito
        ~mdError()
        {
            if (error_set != false)
            {
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                System.Windows.Forms.MessageBox.Show("An error was left unprocessed, caption '" + caption + "' message '" + message + "'", "SOFTWARE ERROR", buttons);
            }
        }

        public void root(String type, String error)
        {
            if (error_set != false)
            {
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                System.Windows.Forms.MessageBox.Show("Trying to root an already rooted error, caption '" + caption + "' message '" + message + "', new caption " + type + "', new message " + error + "'", "SOFTWARE ERROR", buttons);
            }
            error_set = true;
            caption = type;
            message = error;
        }
        public void append(String error)
        {
            if (error_set != true)
            {
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                System.Windows.Forms.MessageBox.Show("Trying to append " + error + " to an unrooted error", "SOFTWARE ERROR", buttons);
            }
            message = message + error;
        }

        public Boolean is_set()
        {
            if (error_set == false)
            {
                return (false);
            }
            return (true);
        }

        // Consuma l'errore (visualizzandolo) ed esci con un'exception
        //      (errore irrecuperabile = fatal error)!
        public void Display_and_Throw()
        {
            check_if_valid_error();

            caption = "UNRECOVERABLE " + caption;
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
            System.Windows.Forms.MessageBox.Show(message, caption, buttons);

            this.Clear(); // errore consumato
            throw (new SystemException());
        }

        // Oppure consuma l'errore (visualizzandolo) ma prosegui (errore recuperabile
        //   o warning)
        public void Display_and_Clear()
        {
            check_if_valid_error();

            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
            System.Windows.Forms.MessageBox.Show(message, caption, buttons);

            this.Clear(); // errore consumato
        }

        // Oppure ancora consuma l'errore senza visualizzarlo e prosegui (errore
        //   recuperabile o warning senza segnalazioni, ma aggiungendo un log degli
        //   errori lo si potrebbe far loggare)
        public void Clear()
        {
            check_if_valid_error();

            mdError null_error = new mdError();
            error_set = false;
            message = null_error.message;
            caption = null_error.caption;
        }

        private void check_if_valid_error()
        {
            if (error_set == false)
            {
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                System.Windows.Forms.MessageBox.Show("An unrooted or empty error was consumed", "SOFTWARE ERROR", buttons);
            }
        }
    }




    // 2) Classe mdFile

    // A un mdFile viene associata una classe 'target' che deve essere definita seguendo il modello
    //   di ClasseDiEsempio. 

    // mdFile è usata per standardizzare le operazioni che servono sempre: load, save e new

    // In Form1 (inizializzazioni e toolStripMenus load, save, save_as e new) ci sono gli esempi d'uso

    public class mdFile
    {

        private String default_short_name = "no name";  // nome di un file 'nullo', non inizializzato

        // Sia il nome (corto) del file che la directory vengono impostate dall'utente quando apre un
        //   dialogo load o save_as
        public String current_file_short_name;         // nome del file: se è a default è considerato non valido

        public String current_directory;               // directory (disco incluso)

        public String extension;                        // l'estensione del file è un parametro del constructor

        public String target_class_human_oriented_name; // usato solo nei messaggi per renderli comprensibili

        // Nome completo di sistema. Viene inizializzato (in particolare la directory) dopo un dialogo
        //  save_as oppure load.
        public String system_name;                      // nome completo di sistema


        public mdFile(String file_extension, String class_human_oriented_name)
        {
            this.target_class_human_oriented_name = class_human_oriented_name;
            this.extension = file_extension;
            this.current_file_short_name = default_short_name;
        }



        public Boolean save(object source_class, Form1.delegate_save save_delegate, String user_files_directory)
        {
           
            if ((current_file_short_name == default_short_name) || (current_directory == ""))
            {
                // Nome e directory non inizializzati
                //   user_files_directory viene usato solo qua !
                return ( this.save_as(source_class, save_delegate, user_files_directory) );
            }

            system_name = current_directory + "\\" + current_file_short_name + "." + extension;
            string message = "Saving " + target_class_human_oriented_name + " file " + system_name + "...";
            Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });


            // Notare:

            //  - come si casta il valore di ritorno da un delegate
            //  - come lo si lancia tramite Invoke
            //  - come gli si passa la lista dei parametri
            mdError error = (mdError)Form1.mainForm.Invoke(save_delegate, new object[] { this, source_class});
       

            if (error.is_set() == false)
            {
                // nome etc. del file sono già stati settati da .save_dialog()

                message = " done";
                Form1.mainForm.Invoke((Form1.delegate_append_to_mainStatuswindow)Form1.append_to_mainStatusWindow, new object[] { message });
                return (true);
            }
            else
            {
                // nome etc. del file sono già stati settati da .save_dialog()

                error.append(" in mdFile.save()");
                error.Display_and_Clear();
                Form1.newline_to_mainStatusWindow("Problems saving file!");
                return (false);
            }


        }

        public Boolean save_as(object source_class, Form1.delegate_save save_delegate, String initial_directory)
        {

            // get il nome del file
            SaveFileDialog openFileDialog1 = new SaveFileDialog();

            openFileDialog1.Title = "Save " + target_class_human_oriented_name + " file";
            openFileDialog1.FileName = current_file_short_name;  // preset
            openFileDialog1.InitialDirectory = initial_directory;
            openFileDialog1.Filter = target_class_human_oriented_name + " file (*." + extension + ")|*."+ extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return(false);

            system_name = openFileDialog1.FileName;
            current_file_short_name = Path.GetFileNameWithoutExtension(system_name);
            current_directory = Path.GetDirectoryName(system_name);

            // Overwrite check non serve, lo fa già la SaveFileDialog

            Form1.newline_to_mainStatusWindow("Saving " + target_class_human_oriented_name + " file " + system_name + "...");

            mdError error = (mdError)Form1.mainForm.Invoke(save_delegate, new object[] { this, source_class });

            if (error.is_set() == false)
            {
                // nome etc. del file sono già stati settati da .save_as_dialog()

                Form1.append_to_mainStatusWindow(" done");
                return (true);
            }
            else
            {

                /// nome etc. del file sono già stati settati da .save_as_dialog()

                error.append(" in mdFile.save_as()");
                error.Display_and_Clear();
                Form1.newline_to_mainStatusWindow("\r\nProblems saving file!");
                return (true);
            }

        }


        public Boolean load(ref object original_class, Form1.delegate_load load_delegate, String user_files_directory)
        {


            // get il nome del file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Load "+ target_class_human_oriented_name + " file";
            openFileDialog1.FileName = "";  // preset
            openFileDialog1.InitialDirectory = user_files_directory;
            openFileDialog1.Filter = target_class_human_oriented_name + " file (*."+ extension + ")|*."+ extension + "|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                // notare che original_class non viene modificata
                return (false);
            }

            system_name = openFileDialog1.FileName;
            current_file_short_name = Path.GetFileNameWithoutExtension(system_name);
            current_directory = Path.GetDirectoryName(system_name);
            Form1.newline_to_mainStatusWindow("Loading file " + system_name + "...");




            mdError error = new mdError();

            // NOTARE BENE COME FUNZIONA IL PASSAGGIO DEI PARAMETRI AL DELEGATO!

            // original_class è passata ByRef al delegate (lo si vede solo nella definizione del delegate)
            //   ma per come si settano i parametri (tramite l'object[])  quello che viene effettivamente
            //   modificato è il valore contenuto in object[1], che viene poi assegnato a original_class
            //   (che è passata ByRef anche dalla routine chiamante)

            object[] params_list = new object[] { this, original_class };
            error = (mdError)Form1.mainForm.Invoke(load_delegate, params_list);

            if (error.is_set() == false)
            {

                // assegnazione effettiva della reference (puntatore xD) della nuova classe a original_class
                original_class = params_list[1];

                // nome etc. del file sono già stati settati da .load_dialog()

                Form1.append_to_mainStatusWindow(" done");
                return (true);
            }
            else
            {

                // rimetti a default il nome del file (se l'utente voleva caricare un file diverso
                //    quello corrente non gli interessa più, e defaultando il nome si evita che magari
                //    prema 'save' per sbaglio e sovrascriva quello che invece voleva caricare)
                current_file_short_name = default_short_name;
                // ma original_class non viene toccata e la classe è sempre valida

                error.Display_and_Clear();

                Form1.newline_to_mainStatusWindow("Problems loading file!");

                return (false);
            }

        }





        public Boolean new_file() // Impossibile chiamarlo solo new(), al compilatore non piace
        {

            string message = "This will clear all unsaved data, okay?";
            string caption = "Data will be cleared";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);
            if (result != System.Windows.Forms.DialogResult.Yes) return (false);

            current_file_short_name = default_short_name;

            return (true);
        }



        // Metodi specializzati per la gestione del file di configurazione (XConfig)

        // Attenzione a passare stringhe sensate per directory e nome file...
        public void set_current_file_path(String directory, String short_name)
        {
            this.current_directory = directory;
            this.current_file_short_name = short_name;
            this.system_name = current_directory + "\\" + current_file_short_name + "." + extension;
        }


        // direct_save() è identico a save(), tranne che per l'effetto del test iniziale su nome file e directory
        public Boolean direct_save(object source_class, Form1.delegate_save save_delegate)
        {

            mdError error = new mdError();

            if ((current_file_short_name == default_short_name) || (current_directory == ""))
            {
                // Nome o directory non inizializzati
                error.root("SOFTWARE ERROR", "Invalid file path found in mdFile.direct_save(), class = '" + target_class_human_oriented_name + "', directory = '" + current_directory + "', file name = '" + current_file_short_name + "'");
                error.Display_and_Clear();
                return (false);
            }

            system_name = current_directory + "\\" + current_file_short_name + "." + extension;
            Form1.newline_to_mainStatusWindow("Saving " + target_class_human_oriented_name + " file " + system_name + "...");

            error = (mdError)Form1.mainForm.Invoke(save_delegate, new object[] { this, source_class });

            if (error.is_set() == false)
            {
                Form1.append_to_mainStatusWindow(" done");
                return (true);
            }
            else
            {
                error.append(" in mdFile.direct_save()");
                error.Display_and_Clear();
                Form1.newline_to_mainStatusWindow("Problems saving file!");
                return (false);
            }


        }


        // Rispetto a load(), direct_load() non apre la finestra di dialogo, ma per lo meno verifica che
        //   directory e nome file esistano..
        public Boolean direct_load(ref object original_class, Form1.delegate_load load_delegate)
        {

            mdError error = new mdError();

            if ((current_file_short_name == default_short_name) || (current_directory == ""))
            {
                // Nome o directory non inizializzati
                error.root("SOFTWARE ERROR", "Invalid file path found in mdFile.direct_load(), class = '" + target_class_human_oriented_name + "', directory = '" + current_directory + "', file name = '" + current_file_short_name + "'");
                error.Display_and_Clear();
                return (false);
            }


            object[] params_list = new object[] { this, original_class };
            error = (mdError)Form1.mainForm.Invoke(load_delegate, params_list);

            if (error.is_set() == false)
            {

                // assegnazione effettiva della reference (puntatore xD) della nuova classe a original_class
                original_class = params_list[1];

                Form1.append_to_mainStatusWindow(" done");
                return (true);
            }
            else
            {

                // Il nome file non va toccato in direct_load !!

                error.Display_and_Clear();

                Form1.newline_to_mainStatusWindow("Problems loading file!");

                return (false);
            }

        }
        // Fine metodi specializzati per la gestione del file di configurazione


    }






    // 3) Classe che contiene nome e versione programma e 'copyright' (che in realtà è il CompanyName)
    //      (sono tutte informazioni che si trovano in Project/Properties/Application/Assembly Information)

    //      Usata sia per informazioni varie a schermo che per marcare i file salvati (viene inserita
    //      come sottoclasse nelle classi serializzabili modellate su ClasseDiEsempio)

    //  Dato che deve essere una classe serializzabile DEVE avere il default constructor, ma notare
    //      che qui non c'è alcun'altra informazione sulla serializzazione: questo perchè viene usata solo
    //      come sottoclasse della classe da serializzare, e lì è già marcata come [DataMember]
    //      Ai DataContracts questo basta... (Notare che c'è comunque l'on_deserialized).

    //  I campi _dovrebbero_ essere accessibili solo in lettura.. ma se si prova a definirli private
    //     con metodo get non vengono più serializzati.. dopo un pò di prove ci ho rinunciato

    //  FUNZIONAMENTO:

    //      - Un'istanza di questa classe viene creata in Form1 in inizializzazione

    //      - Quando una classe da serializzare contiene una Program_Identifier questa viene copiata automaticamente
    //          da quella di Form1 nel momento in cui viene creata la class (e in teoria i campi non dovrebbero più
    //          essere scrivibili, anche se visto che è tutto pubblico lo sono). 

    //      - A questo punto le informazioni saranno disponibili nei files salvati. Notare che NON è previsto
    //         che abbiano un uso software, servono solo per lasciare una traccia storica nei files: infatti
    //         quando un Program_identifier viene deserializzato viene immediatamente scartato (dall'on_deserialized)
    //         e sostituito con quello del programma corrente.

    public class Program_Identifier
    {

        public String name = "";
        public String version = "";
        public String copyright = "";

        // SEMPRE un default constructor (altrimenti impossibile serializzare) !!!
        public Program_Identifier() { }

        public Program_Identifier(Form1 control)
        {
            name = control.ProductName;
            version = control.ProductVersion;
            copyright = control.CompanyName;
        }

        // Copy constructor
        public Program_Identifier(Program_Identifier source)
        {
            name = source.name;
            version = source.version;
            copyright = source.copyright;
        }

        [OnDeserialized]
        void on_deserialized(StreamingContext context)
        {
            // la program_data letta dal file viene scartata e sostituita da quella del programma corrente            
            Program_Identifier newdata = new Program_Identifier(Form1.program_identifier);
            name = newdata.name;
            version = newdata.version;
            copyright = newdata.copyright;

        }

    }


}
