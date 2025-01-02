using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template
{

    // CONTROLLI IMPLEMENTATI:

    // 1) ComboBox
    // 2) PictureBox con barre di scroll
    // 3) PictureBox con eventi MouseHover






    // COMBO BOX

    // Cosa va definito all'esterno:

    //  Una ComboBox ComboBox_PROVACOMBOBOX, con associato l'evento _SelectedIndexChanged

    //  Un enum e una List<XComboBoxElement> che associa ad ogni valore dell'enum la stringa da visualizzare nella ComboBox. Notare che l'enum può avere dei "buchi",
    //      in modo da poter accomodare eventuali future expansions)

    /*     
          enum PROVACOMBOBOX { unplayable = 0, flat = 5, lowhills = 6 }

          List<XComboBoxElement> PROVACOMBOBOX_structure = new List<XComboBoxElement>()
          {
              new XComboBoxElement { enum_value = (int)PROVACOMBOBOX.unplayable, name = "Unplayable"},
              new XComboBoxElement { enum_value = (int)PROVACOMBOBOX.flat, name = "Flat"},
              new XComboBoxElement { enum_value = (int)PROVACOMBOBOX.lowhills, name = "Low Hills"}
          };         
      */

    //  Una variabile int che conterrà il valore corrente dell'enum sul quale è posizionata la ComboBox: public static int PROVACOMBOBOX_status



    // Gestioni:

    // La ComboBox va inizializzata (in initialize_controls) tramite XComboBox_initialize

    // PROVACOMBOBOX_status = (PROVACOMBOBOX)XComboBox_initialize(PROVACOMBOBOX_structure,ComboBox_PROVACOMBOBOX);

    // Nell'evento _SelectedIndexChanged si acquisisce il nuovo status, che può poi essere decodificato con uno switch (se tutto va bene VisualStudio
    //      inserisce automaticamente i valori dell'enum quando si crea lo switch):

    /* PROVACOMBOBOX_status = (PROVACOMBOBOX)PROVACOMBOBOX_structure[ComboBox_PROVACOMBOBOX.SelectedIndex].enum_value;

         switch (PROVACOMBOBOX_status)
        {
            case PROVACOMBOBOX.unplayable:
                break;
            case PROVACOMBOBOX.flat:
                break;
            case PROVACOMBOBOX.lowhills:
                break;
            default:
                break;
        }        
    */


    // Funzioni:

    // get_string(int enum_value, List<XComboBoxElement> combo_list): dato l'enum restituisce la stringa associata (cercandola nella combo_list)


    public class XComboBox
    {
        public static int XComboBox_initialize(List<XComboBoxElement> combo_list, ComboBox combo_box)
        {
            // Inizializzazione della ComboBox
            string[] combolist_array = new string[combo_list.Count];
            for (int i = 0; i < combo_list.Count; i++)   // questo for converte da List ad array (purtroppo AddRange accetta solo arrays, o comunque object[])
            {
                combolist_array[i] = combo_list[i].name;
            }

            combo_box.Text = combolist_array[0];
            combo_box.Items.Clear();
            combo_box.Items.AddRange(combolist_array);
            combo_box.Refresh();

            return combo_list[0].enum_value;
        }

        public static string get_string(int enum_value, List<XComboBoxElement> combo_list)
        {
            string out_string = "Software error in XComboBox.get_string";
            foreach (XComboBoxElement combo_element in combo_list)
            {
                if (enum_value == combo_element.enum_value)
                {
                    out_string = combo_element.name;
                    break;
                }
            }
            return out_string;
        }


    }

    public class XComboBoxElement
    {
        public int enum_value;
        public string name;
    }




    // PICTUREBOX CON SCROLLBARS (PICTUREBOX SCROLLABILE, SCROLLABLE PICTUREBOX)

    //  In effetti non c'è codice 'attivo' per le picturebox scrollabili, solo istruzioni e codice da copiare

    // 1) Il Panel dello splitContainer in cui è inserita pictureBox va settato con AutoScroll=true
    // 2) La pictureBox va settata con Dock=none, Anchor = top,left e SizeMode=autosize. Con Dock=fill o con Anchor != top,left le barre non funzionano più

    // 3) Dato che abbiamo dovuto mettere Dock=none sorge un difetto: se con un resize lo splitContainer.Panel diventa più grande dell'immagine
    //      che va visualizzata nella pictureBox l'effetto grafico è molto brutto. Per sistemarlo la routine che genera l'immagine deve ricevere la Size
    //      dello splitContainer.Panel e, se del caso, espandere l'immagine da visualizzare per ricoprirlo. Questo è un esempio del codice:

    //int image_width = 1000;                                  // Dimensioni minime dell'immagine che si desidera visualizzare (esempio)
    //int image_height = 50;
    //int width = Math.Max(min_image_width, panel_size.Width);     // Se sono inferiori a quelle del Panel espandi l'immagine
    //int height = Math.Max(min_image_height, panel_size.Height);
    //Bitmap image = new Bitmap(width, height);

    // Notare anche che non serve più gestire l'evento splitterMoved... ma perchè sia così, sinceramente, non l'ho capito


    // Nota: le scrollbars compaiono solo se almeno una dimensione dell'immagine è almeno 15 pixels più grande di quella della pictureBox
    //       Credo sia così perchè le barre sono larghe 15 pixels (btw compaiono sempre tutte e due anche se solo una dimensione sfora, non è elegante ma non ci si può far niente)





    // PICTUREBOX CON MOUSEHOVER

    // In effetti non c'è codice 'attivo' per la picturebox con MouseHover, solo istruzioni e codice da copiare

    // 1) Nella Form va definita una routine che gestisce l'evento TimerElapsed:
    /*
        private static void mousehover_Timer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (mainForm.pictureBox_NOME.InvokeRequired == true)
            {
                // se un eccezione viene alzata qua in realtà si è verificato qualche casino in ROUTINE_DI_VISUALIZZAZIONE,
                //   tipo oggetti non inizializzati o un loop infinito
                mainForm.Invoke((delegate_ROUTINE_DI_VISUALIZZAZIONE)ROUTINE_DI_VISUALIZZAZIONE);
            }
            else
            {
                ROUTINE_DI_VISUALIZZAZIONE();
            }
        }
    */

    // 2) Nelle variabili globali della Form vanno definiti un timer e una variabile Point (che conterrà le coordinate del mouse, relative alla pictureBox)
    //      e un delegate alla routine di gestione dell'evento:

    //private static System.Timers.Timer mousehover_Timer = new System.Timers.Timer();
    //private static Point mouse_position = new Point(0, 0); // coordinate correnti del mouse (da mouseHover event)

    //  (Nota: inutile aggiungere una "using System.Timers" per evitare di scriverlo nell'istruzione precedente, di Timer ce ne sono tanti e viene dato un'errore perchè "Timer" da solo è ambiguo)


    // 3) In initialize_controls va inserita l'inizializzazione del timer:

    /*
             // Timer che evita di lanciare la gestione del MouseHover ogni volta che si verifica un evento

            // Il timer è un monostabile che viene lanciato ad ogni evento mousehover, se arriva un altro mousehover
            //   prima che il timer scada il timer riparte e ritarda la gestione finchè il punto di hovering è 'stabile'            
            mousehover_Timer.Interval = 5;    // tempo in ms
            mousehover_Timer.Enabled = true;
            mousehover_Timer.AutoReset = false;
            mousehover_Timer.Stop();
            mousehover_Timer.Elapsed += new System.Timers.ElapsedEventHandler(mousehover_Timer_Elapsed);     
    */


    // 4) Alla pictureBox_NOME va associato l'evento MouseMove:

    /*
        private void pictureBox_NOME_MouseMove(object sender, MouseEventArgs e)
        {
            mousehover_Timer.Stop();    // spegni subito il timer, altrimenti si corre il rischio che
                                        //  scada mentre stiamo processando l'evento mousehover, con
                                        //  conseguenti casini

            mouse_position.X = e.X;     // Coordinate del mouse rispetto alla _Image_ della pictureBox, questo ANCHE nel caso la pictureBox non venga visualizzata
            mouse_position.Y = e.Y;     //    completamente (per esempio se ha le scrollbars: le coordinate sono sempre riferite all'immagine _completa_).

            mousehover_Timer.Start(); // va fatto partire ad OGNI mousevents
        }     
         
     */

    // La _MouseMove acquisisce costantemente le coordinate del mouse E quando il timer scade viene lanciata la routine di visualizzazione






}
