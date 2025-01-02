using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;


namespace Template
{

    // Classe modellata sulla ClasseDiEsempio (da Template)
    
    // Contiene i dati di configurazione del programma
    //   L'unico dato predefinito è la directory di salvataggio dei dati utente

    // ATTENZIONE: alla fine di Form1.get_configuration_data() bisona settare la directory di
    //   salvataggio per ognuno degli mdFiles che sono stati creati. Sarebbe stato bello farlo
    //   automaticamente, ma non era affatto facile.

    [DataContract]
    public class XConfig
    {

        public Boolean is_saved = true; //  Di default è true perchè una classe 'vuota' è automaticamente salvata

        [DataMember]
        public String file_extension = "cfg";  // Estensione del file che verrà associato a questa classe

        [DataMember]
        public String name = "XConfig";

        // Versione della classe
        [DataMember]
        private int version = 0;

        // User remarks
        [DataMember]
        public String notes = "";

        // Identifier nome/versione programma che ha eseguito il salvataggio
        [DataMember]
        public Program_Identifier program_identifier = new Program_Identifier(Form1.program_identifier);

        // Data di salvataggio
        [DataMember]
        public DateTime save_date;





        // 2) PARTE 'CUSTOM' DELLA STRUTTURA DATI

        [DataMember]
        public String user_files_directory;


        [DataMember]
        public String user_text_files_directory;


        // Opzioni di visualizzazione dati
        [DataMember]
        public bool display_whole_linear_data = false;       // Di default visualizzo solo default_max_displayed_linear_data records di dati lineari (il caso tipico in cui ce ne sono di più
                                                             //   sono le parole del vocabolario o le sillabe), altrimenti la gestione della textBox richiede molto tempo
        [DataMember]
        public bool display_whole_texts = false;             // Come sopra, ma per la visualizzazione del source text e cleaned text
        [DataMember]
        public bool display_numbers_in_american_format = false;     // per compatibilità con diverse localizzazioni di Excel


        // Opzioni salvataggio file .txa
        [DataMember]
        public bool save_also_source_text = false;            // Occupa un mucchio di spazio
        [DataMember]
        public bool save_also_cleaned_text = false;           // Occupa un mucchio di spazio


        // Opzioni di preprocessing
        [DataMember]
        public Boolean discard_all_arabic_numbers = true;

        [DataMember]
        public Boolean apostrophe_is_a_separator = false;                       // se false l'apostrofo è trattato come un carattere normale (opzione suggerita)
        [DataMember]
        public Boolean words_linked_by_an_apostrophe_are_separated = true;      // valida solo se apostrophe_is_a_separator = true
        [DataMember]
        public Boolean words_linked_by_an_apostrophe_are_discarded = false;     // valida solo se apostrophe_is_a_separator = true
        [DataMember]
        public Boolean words_linked_by_an_apostrophe_are_joined = false;        // valida solo se apostrophe_is_a_separator = true, ma in generale ha poco senso usarla

        [DataMember]
        public Boolean words_linked_by_a_dash_are_separated = false;
        [DataMember]
        public Boolean words_linked_by_a_dash_are_discarded = true;             // opzione suggerita
        [DataMember]
        public Boolean words_linked_by_a_dash_are_joined = false;

        [DataMember]
        public Boolean keep_distinction_between_upper_and_lowercase = false;




        // 3) PARTE 'STANDARD' DEI METODI

        // SEMPRE un default constructor, altrimenti è impossibile serializzare !!
        //  ma vedi commenti all'inizio: qua, e solo qua (nella classe 'top'), potrebbe anche non esserci
        public XConfig() { }

        // Constructor DEVE avere almeno un parametro per distinguerlo da quello di default (e, in generale,
        //   verrà passato al constructor effettivo per dimensionare gli arrays o altre strutture)!
        // Qua viene settato solo is_saved, il constructor effettivo (custom) è in una routine separata,
        //   per separare le parti standard da quelle custom
        public XConfig(String user_files_directory)
        {

            this.is_saved = true;   // una classe appena costruita è automaticamente 'salvata'

            effective_constructor(user_files_directory);
        }


        public int get_version() { return (version); }

    

        // Serializzazione su file
        public mdError save(mdFile file, object source_class)
        {

            DataContractSerializer serializer;
            FileStream writer;

            // E qua facciamo l'unboxing da object alla nostra classe
            XConfig source = (XConfig)source_class;

            save_date = DateTime.Now;

            try
            { serializer = new DataContractSerializer(typeof(XConfig)); }
            catch
            {
                mdError error = new mdError();
                error.root("SOFTWARE ERROR", "Problems istancing serializer (save), class:' " + source.name + "'");
                return (error);
            }

            try
            { writer = new FileStream(file.system_name, FileMode.Create); }
            catch
            {
                mdError error = new mdError();
                error.root("SOFTWARE OR DATA ERROR", "Problems opening source file: " + file.system_name);
                return (error);
            }

            try
            {
                serializer.WriteObject(writer, source);
                writer.Close();
            }
            catch
            {
                writer.Close();
                mdError error = new mdError();
                error.root("SOFTWARE ERROR", "Problems serializing source class: '" + source.name + "'");
                return (error);
            }

            // manca solo is_saved, accessibile dopo Unboxing
            XConfig current_class = (XConfig)source;
            current_class.is_saved = true;

            return (new mdError());
        }



        // Deserializzazione da file
        public mdError load(mdFile file, ref object original_class)
        {

            DataContractSerializer serializer;
            FileStream reader;

            // E qua facciamo l'unboxing da object alla nostra classe
            XConfig loaded_class = (XConfig)original_class;

            try
            { serializer = new DataContractSerializer(typeof(XConfig)); }
            catch
            {
                mdError error = new mdError();
                // notare che qua loaded_class.name è uguale a original_class.name (ma da lì non lo si
                //    può prendere, perchè original_class è un object)
                error.root("SOFTWARE ERROR", "Problems istancing serializer (load), class:' " + loaded_class.name + "'");

                // notare che original_class non viene modificato

                return (error);
            }

            try { reader = new FileStream(file.system_name, FileMode.Open); }
            catch
            {
                mdError error = new mdError();
                error.root("SOFTWARE OR DATA ERROR", "Problems opening source file: " + file.system_name);

                // notare che original_class non viene modificato

                return (error);
            }

            try
            {
                loaded_class = (XConfig)serializer.ReadObject(reader);
                reader.Close();

                // assegna alla reference della classe (original_class) quella appena deserializzata
                original_class = loaded_class;


                XConfig current_class = (XConfig)original_class;
                current_class.is_saved = true; // e setta la classe come 'salvata su disco' (anche se è stata appena caricata xD)

                return (new mdError());

            }
            catch
            {
                reader.Close();

                mdError error = new mdError();
                error.root("SOFTWARE ERROR", "Problems deserializing class"); // qua non è detto che name sia valido

                // notare che original_class non viene modificato

                return (error);
            }

        }




        // 4) PARTE 'CUSTOM' DEI METODI

        [OnDeserialized]
        void on_deserialized(StreamingContext context)
        {

            // un on_deserialized può ritornare solo un void, quindi gli mdErrors vanno consumati tutti
            //    prima di uscirne!
            String errortype = "???";
            String message = "???";

            try
            {
                // Inserire qua la parte custom (controllo sintassi, campi 'automatici' ...), es.:

                // controllo dei campi 'locali'
                // lancio dei controlli sintattici delle sottoclassi (a cui si possono passare parametri,
                //   incluso un reference all'intera classe base)

            }
            catch
            {
                mdError error = new mdError();
                error.root(errortype, message);
                error.Display_and_Throw();
            }
        }


        private void effective_constructor(String user_files_directory)
        {
            this.user_files_directory = user_files_directory;
        }


    }
}
