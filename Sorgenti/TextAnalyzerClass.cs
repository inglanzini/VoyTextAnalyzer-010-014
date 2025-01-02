using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace Template
{

    //  Questa classe è concepita per contenere (e poter salvare/caricare) i dati
    //   del programma, è la base di qualsiasi cosa. Ad essa si può associare un mdFile, ottenendo
    //   una classe facile da salvare e caricare da disco.

    // "Fully serializable class":

    //   Usa i DataContracts, coi quali si può serializzare di tutto incluse liste e dizionari  

    //      Nota: per far funzionare i DataContracts NON BASTA avere la "using System.Runtime.Serialization",
    //            BISOGNA ANCHE aggiungere una 'reference' a System.Runtime.Serialization nel progetto,
    //            tramite il menù Project/Add reference (lista Framework). Va a sapere perchè...

    //      Nota: per la serializzazione è INDISPENSABILE che la TUTTE LE SOTTOCLASSI abbiano il default
    //              constructor !! "Pare" (ma non ci giuro sopra) che funzioni così:

    //              - la classe che viene serializzata deve essere prefissata con [DataContract]
    //                  lei, e solo lei, può anche non avere il default constructor (ma lo metterei lo stesso..)
    //              - le sottoclassi devono essere prefissate da [DataMember], ma nella loro definizione NON
    //                   ci deve essere il prefisso [DataContract], mentre i loro campi interni possono essere
    //                   o non essere marcati da [DataMember]. MA il default constructor qua è indispensabile

    //          Dato che serve sempre il default constructor (che per definizione non ha parametri)
    //              qualsiasi altro constructor si scriva deve averne almeno uno!



    //    La classe, e ognuna delle sottoclassi interne, contengono il metodo on_deserialized che può essere
    //          usato per vari scopi:
    //          - verificare la correttezza ('sintassi') dei dati 
    //          - completare la classe calcolando gli eventuali campi che
    //              sono funzione di altri campi (dati 'automatici')
    //      Nota: a on_deserialized DEVE essere passato il parametro "StreamingContext context" (qualsiasi cosa
    //            sia), altrimenti viene generata un'eccezione (il bello è che viene generata anche
    //            durante la serializzazione, non solo quando si deserializza).

    //      In generale, più cose vengono gestite a livello 'locale' (cioè, p.es.: ogni sottoclasse controlla la 
    //         correttezza dei suoi dati) meglio le sottoclassi sono incapsulate e meno possibilità di errori ci sono.
    //         Però coi deserializzatori si può accedere solo ai dati locali e quindi se ci sono dipendenze con
    //         dati esterni alla sottoclasse non è possibile fare molto negli on_deserialized. L'unica possibilità
    //         è lanciare un controllo globale dall'on_deserialized della classe root (come veniva fatto in 
    //         Words Reborn). Lo svantaggio è che se tutto fosse gestito localmente una modifica alla classe
    //         base (p.es.: aggiungere una sottoclasse) non comporterebbe altre modifiche, ma con un controllo
    //         globale bisogna ricordarsi di modificare anche l'on_deserialized della classe base.


    // ATTENZIONE: quando si crea una nuova classe partendo da ClasseDiEsempio ricordarsi di cercare
    //              'ClasseDiEsempio' nel file e di sostituirlo col nome della nuova classe. Questo
    //               perchè il nome compare in vari punti ( typeof, boxing e unboxing..) dove il compilatore
    //               non segnala problemi, ma se si lascia il nome ClasseDiEsempio l'effetto non è piacevole :)


    [DataContract]
    public class TextAnalyzerClass
    {

        // 1) PARTE 'STANDARD' DELLA STRUTTURA DATI

        //     a) Definizione di alcune variabili indispensabili, che servono per il supporto di mdFile

        // is_saved: flag che dice se la classe è stata salvata su file e che viene poi testato
        //  quando si cerca di uscire dal programma

        // ATTENZIONE: VIENE MESSO A TRUE AUTOMATICAMENTE DA CONSTRUCTORS, SERIALIZZATORE E DESERIALIZZATORE,
        //  MA E' RESPONSABILITA' DELL'APPLICATIVO CUSTOM METTERLO A FALSE QUANDO I DATI VENGONO MODIFICATI !!!!

        // ATTENZIONE: BISOGNA ANCHE RICORDARSI DI TESTARE IL FLAG ALL'USCITA DEL PROGRAMMA (in Form1.FileClosing)
        //  SAREBBE STATO BELLO FARLO AUTOMATICAMENTE (OVVIAMENTE PER OGNI CLASSE DEFINITA COME CLASSEDIESEMPIO,
        //  MA NON ERA AFFATTO FACILE)

        public Boolean is_saved = true; //  Di default è true perchè una classe 'vuota' è automaticamente salvata

        [DataMember]
        public String file_extension = "txalysis";  // Estensione del file che verrà associato a questa classe



        //     b) Dati che male non fanno, anche se non venissero usati

        //      "name" è un esempio di un tipo semplice serializzato. Il suo scopo è essere usato nei messaggi
        //                (errori inclusi). Ha senso modificarlo solo con una compilazione, ma viene comunque serializzato.                      

        //      "version" è un esempio di un private serializzato. E' private e inaccessibile
        //                perchè ha senso modificarlo solo con una compilazione ed è fondamentale per poter
        //                gestire il caricamento di versioni obsolete della classe.
        //                Non è possibile definirci sopra un metodo get, altrimenti la serializzazione non
        //                funziona più: per leggerlo c'è il metodo get_version()

        //      "notes" è liberamente accessibile (e serializzato). Ci si può scrivere quello che si vuole

        //      "program_identifier" è un esempio di una sottoclasse serializzata. E' una classe del Template,
        //                      (definita in XStandards) completa di default constructor e on_deserialized
        //                      che contiene informazioni relative al programma che ha salvato la classe

        //      "save_date" è un'altra sottoclasse (in questo caso definita dal sistema) serializzata.
        //         Che possa essere utile avere la data di salvataggio scritta nel file mi sembra evidente.       


        [DataMember]
        public String name = "TextAnalyzer";

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
        public string user_notes;
        [DataMember]
        public string book_title;
        [DataMember]
        public string book_author;
        [DataMember]
        public string book_year;
        [DataMember]
        public string book_language;


        [DataMember]
        public List<LoadedFileRecord> loaded_files_list = new List<LoadedFileRecord>();   // lista dei files di testo che sono stati caricati, con info aggiuntive

        [DataMember]
        public String loaded_text;                                // Testo originale completo (tutti i files uno di seguito all'altro, con un '\r\n' aggiunto fra uno e l'altro

        [DataMember]
        public String raw_source_text;                            // Testo preprocessato eliminando i commenti e i comandi di pre-processing
        [DataMember]
        public String residual_remarks;                           // Quello che resta di loaded_text dopo aver tolto raw_source_text (commenti e comandi di preprocessing)
        [DataMember]
        public TextAnalysis_used_parameters used_parameters = new TextAnalysis_used_parameters();

        [DataMember]
        public String cleaned_text;                                // Testo dopo aver eseguito i comandi di pre-processing: è su questo che vengono eseguite tutte le analisi



        [DataMember]
        public TextAnalysis_results analysis_results = new TextAnalysis_results();




        // 3) PARTE 'STANDARD' DEI METODI

        // SEMPRE un default constructor, altrimenti è impossibile serializzare !!
        //  ma vedi commenti all'inizio: qua, e solo qua (nella classe 'top'), potrebbe anche non esserci
        public TextAnalyzerClass() { }

        // Constructor DEVE avere almeno un parametro per distinguerlo da quello di default (e, in generale,
        //   verrà passato al constructor effettivo per dimensionare gli arrays o altre strutture)!
        // Qua viene settato solo is_saved, il constructor effettivo (custom) è in una routine separata,
        //   per separare le parti standard da quelle custom
        public TextAnalyzerClass(int length)
        {

            this.is_saved = true;   // una classe appena costruita è automaticamente 'salvata'

            effective_constructor(length);
        }


        public int get_version()  { return (version); }


         


        // NOTARE che serializzatore e deserializzatore hanno come parametro un 'object': questo è
        //        indispensabile perchè i metodi save e load devono poter essere assegnati ai dei 
        //        delegates per poter essere usati con un mdFile e nella definizione del delegate
        //        NON deve comparire alcun nome esplicito (altrimenti li si potrebbe usare solo per
        //        classi con quel nome specifico)

        //  Ovviamente è anche pericoloso: il compilatore non ha modo di verificare che si stia passando il
        //      il tipo corretto, attenzione!!      

        // Serializzazione su file
        public mdError save(mdFile file, object source_class)
        {

            DataContractSerializer serializer;
            FileStream writer;

            // E qua facciamo l'unboxing da object alla nostra classe
            TextAnalyzerClass source = (TextAnalyzerClass)source_class;

            save_date = DateTime.Now;

            try
            { serializer = new DataContractSerializer(typeof(TextAnalyzerClass)); }
            catch
            {
                mdError error = new mdError();
                error.root("SOFTWARE ERROR", "Problems istancing serializer (save), class:' " + source.name + "'");
                return(error);
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
            TextAnalyzerClass current_class = (TextAnalyzerClass)source;
            current_class.is_saved = true;

            return (new mdError());
        }



        // Deserializzazione da file
        public mdError load(mdFile file, ref object original_class)
        {

            DataContractSerializer serializer;
            FileStream reader;

            // E qua facciamo l'unboxing da object alla nostra classe
            TextAnalyzerClass loaded_class = (TextAnalyzerClass)original_class;
            
            try
            { serializer = new DataContractSerializer(typeof(TextAnalyzerClass)); }
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
                loaded_class = (TextAnalyzerClass)serializer.ReadObject(reader);
                reader.Close();

                // assegna alla reference della classe (original_class) quella appena deserializzata
                original_class = loaded_class;


                TextAnalyzerClass current_class = (TextAnalyzerClass)original_class;
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


        // Routine che gestisce effettivamente il constructor di esempio: DEVE avere almeno un parametro per distinguerlo da quello di default
        //   (e, in generale, per dimensionare gli arrays o altre strutture) !
        private void effective_constructor(int length)
        {



        }


    }


    // Classe che riunisce tutte le opzioni e le regole usate nell'elaborazione
    public class TextAnalysis_used_parameters
    {

        public Boolean discard_all_arabic_numbers;
        public Boolean apostrophe_is_a_separator;
        public Boolean words_linked_by_an_apostrophe_are_separated;
        public Boolean words_linked_by_an_apostrophe_are_discarded;
        public Boolean words_linked_by_an_apostrophe_are_joined;
        public Boolean words_linked_by_a_dash_are_separated;
        public Boolean words_linked_by_a_dash_are_discarded;
        public Boolean words_linked_by_a_dash_are_joined;
        public Boolean keep_distinction_between_upper_and_lowercase;

        public List<RegexCommand> Regex_commands = new List<RegexCommand>(); // Lista dei comandi di preprocessing utilizzati

        public string charset_used_in_statistics;   // se 'null' defaulta a 'tutti i caratteri trovati'
        public string charset_reduced_chars;        // permette, a livello di statistiche, di riunire un gruppo di caratteri in un unico carattere


    }



    public class RegexCommand
    {
        public string search_string;
        public string replacement_string;
        public string user_remark;
    }


    public class LoadedFileRecord
    {
        public string file_name;

        public int file_length;
        public int file_charset_size;
        public int file_total_words;
        public int file_max_words_length;    
    }



}
