using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Template
{
    // Visualizzatore di grafici 1D a linee o istogrammi

    //  Questi grafici prevedono di avere in ingresso una List<List<>> di dimensione N x M, cioè N serie di M dati
    //     Le N serie sono visualizzate tutte contemporaneamente, con colori diversi

    // Gli assi vengono marcati con dei valori numerici, ma l'asse X può (in modalità 'istogramma') essere marcato con delle labels

    public class XPlotGraphs1d
    {

        public Color background_color = Color.Black;


        public string x_units = " X units";
        public string y_units = " Y units";


        // Parametri del titolo del grafico
        public string title = "Title";

        public int title_upper_margin = 2;                  // in pixels
        public int title_left_margin = 2;                   // in pixels


        // Parametri che determinano la posizione dell'area in cui viene scritto il grafico
        public int title_to_graphs_upper_margin = 4;
        public int graph_left_margin = 2;
        public int graph_to_series_legend_margin = 4;

        public int y_labels_horizontal_offset_from_vertical_axis = 2;

        public int x_labels_vertical_offset_from_horizontal_axis = 2;

        public bool histogram = false;           // se false: asse X normale, con labels numeriche che variano da 0 a quanti sono i dati
                                                 //   se true: le labels vengono prese dalla lista di stringhe passata come parametro a Display_Graph, inoltre,
                                                 //      se c'è spazio, i dati vengono scritti come rettangoli e non come trattini
        public int histogram_min_rectangle_width = 3;  // larghezza minima a cui può essere ristretto ogni rettangolino: se non c'è spazio si passa alla modalità 'non-istogramma'
        public int histogram_min_margin = 2;           // spazio vuoto minimo (sia a sinistra che a destra) che viene lasciato  ai lati di un blocco di rettangolini



        public bool autoscale_y_is_oscilloscope_like = true;   // Sceglie valori come 0.1 0.2 0.5 1 2 5 10 20 etc.
        public bool autoscale_x_is_oscilloscope_like = true;

        public bool join_points = false;        // usato solo quando il grafico viene visualizzato come punti (o trattini), non quandoi viene visualizzato come istogramma
                                                //   se true i punti/trattini vengono uniti fra di loro in una riga continua


        // Parametri della legenda (nomi delle serie)
        public int legend_left_margin = 30;
        public int legend_lower_margin = 8;

        public int legend_marker_to_name_margin = 2;  // separazione fra il marker colorato e il nome
        public int legend_max_displayable_name_length = 200;
        public int legend_horizontal_space_between_legend_voices = 12;
        public int legend_vertical_space_between_legend_voices = 2;





        public int x_maximum_width = 100;       // in pixels: usato solo quando ci sono più pixels a disposizione che dati da visualizzare. Ogni dato può essere espanso al max
                                                //  fino a questa larghezza (è un parametro poco importante, serve solo in casi particolari in cui ci sono pochissimi dati da visualizzare rispetto all'area disponibile)

        public int x_minimum_division_length = 100;     // x_minimum_division_length è una lunghezza in pixels ed è usato solo se ci sono meno pixels a disposizione
                                                        //   che dati da visualizzare. E' abbastanza arbitrario come valore: più è basso più il grafico può essere compresso
                                                        //   in poco spazio (p.es. se si stringe la finestra), prima che scompaia


        public Color lines_color = Color.White;

        public Color plot_color_series_0 = Color.Blue;  // 16 colori predefiniti per le serie, oltre questi si riinizia da _0
        public Color plot_color_series_1 = Color.Green;
        public Color plot_color_series_2 = Color.Orange;
        public Color plot_color_series_3 = Color.Red;
        public Color plot_color_series_4 = Color.DarkGray;
        public Color plot_color_series_5 = Color.Gold;
        public Color plot_color_series_6 = Color.LightBlue;
        public Color plot_color_series_7 = Color.LightGreen;
        public Color plot_color_series_8 = Color.LightSalmon;
        public Color plot_color_series_9 = Color.LightCyan;
        public Color plot_color_series_10 = Color.LightGray;
        public Color plot_color_series_11 = Color.Magenta;
        public Color plot_color_series_12 = Color.Aquamarine;
        public Color plot_color_series_13 = Color.Pink;
        public Color plot_color_series_14 = Color.Brown;
        public Color plot_color_series_15 = Color.Coral;


        public string title_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float title_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color title_color = Color.White;

        public string labels_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float labels_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color labels_color = Color.White;

        public string series_names_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float series_names_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color series_names_color = Color.White;



        // Dati ricavati durante la scrittura dei grafici (tutti private!)
        //   OCCHIO A NON UTILIZZARLI PRIMA DI AVERLI GESTITI..... E' UN'IMPOSTAZIONE RISCHIOSA (SAREBBERO DOVUTI ESSERE TUTTI 'NULLABLE' PER SICUREZZA MI SA)
        //   vengono comodi perche' così si può passare tutto alle sottoroutines tramite graphsplotters, ma dato che sono pericolosi ne ho usati meno che potevo
        //   (anche se mi è toccato passare più parametri alle sottoroutines)

        private Rectangle graph_area = new Rectangle();  // area in cui viene scritto il grafico (area totale depurata dallo spazio per il titolo e per l'eventuale legenda)
        private Size subgraph_size = new Size();         // dimensione utile in cui scrivere il grafico (rispetto a graph_area lascia lo spazio per scrivere le labels dell'asse X)

        private List<Color> series_colors = new List<Color>();

        private Font title_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush title_brush = new SolidBrush(Color.White);
        private int title_height;

        private Font labels_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush labels_brush = new SolidBrush(Color.White);
        private int labels_height;

        private Font series_names_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush series_names_brush = new SolidBrush(Color.White);

        private int legend_num_hor_divisions;
        private int legend_num_ver_divisions;
        private int legend_max_names_width;
        private int legend_max_names_height;
        private int legend_marker_length;
        private int legend_voice_length;



        // parametri per Marginvalue
        private static readonly bool _direction_is_up = true;
        private static readonly bool _direction_is_down = false;



        public static Bitmap DisplayGraph(Size image_size, List<List<float>> values_list,List<string> series_names_list, List<string> x_labels_list, XPlotGraphs1d graphsplotter)
        {
            Bitmap image = new Bitmap(image_size.Width, image_size.Height);

            Graphics g = Graphics.FromImage(image);
            g.PageUnit = GraphicsUnit.Pixel;        // Mah.. di default PageUnit è settata a Display, sembra non cambi niente se si toglie questa istruzione, ma va a sapere..

            g.Clear(graphsplotter.background_color);

            if (values_list == null)
            {
                DrawNoDataGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }
            if (values_list.Count == 0)
            {
                DrawNoDataGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }
            if (values_list[0].Count == 0)
            {
                DrawNoDataGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }

            int num_series = values_list.Count;

            // Inizializzazione lista colori delle serie
            graphsplotter.series_colors = new List<Color>();
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_0);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_1);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_2);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_3);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_4);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_5);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_6);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_7);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_8);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_9);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_10);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_11);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_12);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_13);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_14);
            graphsplotter.series_colors.Add(graphsplotter.plot_color_series_15);

            graphsplotter.series_names_font = new Font(graphsplotter.series_names_font_name, graphsplotter.series_names_font_size);
            graphsplotter.series_names_brush = new SolidBrush(graphsplotter.series_names_color);



            // Un grafico è composto da:

            //      Titolo
            //      Un grafico vero e proprio

            // Parametri del titolo
            graphsplotter.title_font = new Font(graphsplotter.title_font_name, graphsplotter.title_font_size);
            graphsplotter.title_brush = new SolidBrush(graphsplotter.title_color);
            graphsplotter.title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.title_font).Height);


            // Troviamo l'area in cui verrà scritto il grafico
            //   Iniziamo settando il punto di inizio dell'area, riservando lo spazio per il titolo e per il margine sinistro
            graphsplotter.graph_area = new Rectangle(graphsplotter.graph_left_margin, graphsplotter.title_upper_margin + graphsplotter.title_height + graphsplotter.title_to_graphs_upper_margin, 1, 1);
            //   Adesso troviamo le dimensioni dell'area, riservando anche lo spazio per la legenda
            graphsplotter.graph_area.Width = image_size.Width - graphsplotter.graph_area.X;
            if (num_series == 1)
            {
                graphsplotter.graph_area.Height = image_size.Height - graphsplotter.graph_area.Y;
            }
            else
            {
                graphsplotter.graph_area.Height = image_size.Height - graphsplotter.graph_area.Y - get_series_legend_height(series_names_list, graphsplotter, g);
            }

            if (graphsplotter.graph_area.Width <= 0 || graphsplotter.graph_area.Height <= 0)
            {
                DrawNoSpaceGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }



            // Determiniamo le dimensioni _utili_ dell'area del grafico. In basso va lasciato lo spazio per scrivere le labels dell'asse X
            //   (gestione fatta allo stesso modo di XGraphs2d, anche se qua si poteva semplificare)
            graphsplotter.labels_font = new Font(graphsplotter.labels_font_name, graphsplotter.labels_font_size);
            graphsplotter.labels_brush = new SolidBrush(graphsplotter.labels_color);
            graphsplotter.labels_height = (int)Math.Ceiling(g.MeasureString("999" + graphsplotter.x_units, graphsplotter.labels_font).Height);

            graphsplotter.subgraph_size = new Size(graphsplotter.graph_area.Width, graphsplotter.graph_area.Height - graphsplotter.labels_height - graphsplotter.graph_to_series_legend_margin);
            if (graphsplotter.subgraph_size.Width <= 0 || graphsplotter.subgraph_size.Height <= 0)
            {
                DrawNoSpaceGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }



            // Adesso troviamo il numero minimo e massimo all'interno della lista dei valori a marginiamoli a valori 'ragionevoli' se l'opzione 'oscilloscope' è settata
            float min_value = float.MaxValue;
            float max_value = float.MinValue;
            foreach (List<float> series in values_list)
            {
                if (series.Min() < min_value) min_value = series.Min();
                if (series.Max() > max_value) max_value = series.Max();
            }

            if (graphsplotter.autoscale_y_is_oscilloscope_like == true)
            {
                min_value = MarginValue(min_value, _direction_is_down);
                max_value = MarginValue(max_value, _direction_is_up);
            }

            if (max_value < 0) max_value = 0;  // trappa il caso in cui ci sono solo valori negativi (l'asse y=0 viene visualizzato in ogni caso)
            if (min_value > 0) min_value = 0;  // trappa il caso in cui ci sono solo valori positivi (l'asse y=0 viene visualizzato in ogni caso)

            // Con min e max, e l'altezza utile del grafico, calcoliamo la scala sull'asse Y
            float y_scale = Math.Abs((float)(max_value - min_value) / (float)graphsplotter.subgraph_size.Height);


           

            Pen solidpen = new Pen(graphsplotter.lines_color);                  // penne che verranno usate dopo per scrivere gli assi (non spostare da qua)
            Pen dashedpen = new Pen(graphsplotter.lines_color);
            dashedpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Pen axis_y0_pen = solidpen;

            // Ci sono 4 casi nella visualizzazione:

            //      - tutti i valori sono zero, viene scritto solo l'asse 0 dashed al centro dell'area utile del grafico, con la sua label, e due assi  solid, senza labels,
            //              agli estremi inferiore e superiore che servono solo per delimitare il grafico
            //      - tutti i valori sono negativi, viene scritto l'asse 0 solid al margine superiore del grafico, e l'asse y = ymin al margine inferiore
            //      - tutti i valori sono positivi, viene scritto l'asse 0 solid al margine inferiore del grafico, e l'asse y = ymax al margine superiore
            //      - ci sono valori si positivi che negativi: gli assi y=ymax e y=ymin (solid) vanno all'estremo del grafico, mentre la posizion dell'asse 0 (dashed) viene calcolata
            //           all'interno. CHIARO CHE NON SAREBBE ACCETTABILE IN UN OSCILLOSCOPIO, DOVE L'ASSE ZERO DOVREBBE ESSERE IN POSIZIONE FISSA PER MOTIVI DI
            //           PRECISIONE DI LETTURA, MA SI TRATTA ANCHE DI UNA FILOSOFIA DI VISUALIZZAZIONE COMPLETAMENTE DIVERSA DA QUELLA DEL GRAFICO

            bool write_axis_ymax = false;
            bool write_axis_ymin = false;
            // L'asse y0 viene scritto sempre e non c'è un flag

            string axis_ymax_label = "";
            string axis_y0_label = "0" + graphsplotter.y_units;
            string axis_ymin_label = "";

            int axis_ymax_Y = 1;    // don't care
            int axis_y0_Y = 1;
            int axis_ymin_Y = 1;

            if (min_value == 0 && max_value == 0)
            {
                // Tutti i values sono zero
                write_axis_ymax = true;
                axis_ymax_Y = graphsplotter.graph_area.Y;

                axis_y0_Y = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height / 2;

                write_axis_ymin = true;
                axis_ymin_Y = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;
            }
            else
            {
                if (max_value == 0)
                {
                    // Vanno scritti gli assi y=0 e y=min
                    axis_y0_Y = graphsplotter.graph_area.Y;

                    write_axis_ymin = true;
                    axis_ymin_Y = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;
                    axis_ymin_label = min_value.ToString("G") + graphsplotter.y_units;

                }
                else
                {
                    if (min_value == 0)
                    {
                        // Vanno scritti gli assi y=max e y=0
                        write_axis_ymax = true;
                        axis_ymax_Y = graphsplotter.graph_area.Y;
                        axis_ymax_label = max_value.ToString("G") + graphsplotter.y_units;

                        axis_y0_Y = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;
                    }
                    else
                    {
                        // Vanno scritti tutti e tre gli assi,ma potrebbe succedere che l'asse y0 vada a coincidere con ymax o ymin, p.es. se c'è un grafico con un
                        //   piccolo numero negativo assieme a numeri positivi molto grandi. Se y0 coincide uso la penna solid, altriment la dashed
                        //   (notare anche che se y0 coincide con ymax sovrascrive ymax, mentre se y0 coincide con ymin viene sovrascritto da ymin, ma non ha importanza,
                        //   si tratta sempre di righe uguali in posizione uguale)

                        write_axis_ymax = true;
                        axis_ymax_Y = graphsplotter.graph_area.Y;
                        axis_ymax_label = max_value.ToString("G") + graphsplotter.y_units;

                        axis_y0_Y = graphsplotter.graph_area.Y + (int)Math.Round(max_value / y_scale);

                        write_axis_ymin = true;
                        axis_ymin_Y = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;
                        axis_ymin_label = min_value.ToString("G") + graphsplotter.y_units;

                        if (axis_y0_Y != axis_ymax_Y || axis_y0_Y != axis_ymin_Y)
                        {
                            axis_y0_pen = dashedpen;
                        }

                    }
                }
            }


            // Calcolate le posizioni Y degli assi pensiamo alle posizioni X di labels e assi. Per poter tenere allineate le labels a destra bisogna prima trovare
            //   la loro lunghezza massima
            int axis_ymin_label_width = (int)g.MeasureString(axis_ymin_label, graphsplotter.labels_font).Width;
            int axis_ymax_label_width = (int)g.MeasureString(axis_ymax_label, graphsplotter.labels_font).Width;
            int axis_y0_label_width = (int)g.MeasureString(axis_y0_label, graphsplotter.labels_font).Width;
            int y_axis_labels_max_length = (int)Math.Max(axis_ymin_label_width, axis_ymax_label_width);
            y_axis_labels_max_length = (int)Math.Max(y_axis_labels_max_length, axis_y0_label_width);


            // E adesso è abbastanza facile calcolare il resto
            int axis_ymax_label_Y = axis_ymax_Y - graphsplotter.labels_height / 2;
            int axis_y0_label_Y = axis_y0_Y - graphsplotter.labels_height / 2;
            int axis_ymin_label_Y = axis_ymin_Y - graphsplotter.labels_height / 2;

            int axis_ymax_label_X = graphsplotter.graph_left_margin + y_axis_labels_max_length - axis_ymax_label_width;
            int axis_y0_label_X = graphsplotter.graph_left_margin + y_axis_labels_max_length - axis_y0_label_width;
            int axis_ymin_label_X = graphsplotter.graph_left_margin + y_axis_labels_max_length - axis_ymin_label_width;

            int axis_ymax_X = graphsplotter.graph_left_margin + y_axis_labels_max_length + graphsplotter.y_labels_horizontal_offset_from_vertical_axis;
            int axis_y0_X = graphsplotter.graph_left_margin + y_axis_labels_max_length + graphsplotter.y_labels_horizontal_offset_from_vertical_axis;
            int axis_ymin_X = graphsplotter.graph_left_margin + y_axis_labels_max_length + graphsplotter.y_labels_horizontal_offset_from_vertical_axis;

            int axis_y_X_end = graphsplotter.subgraph_size.Width;




            // Scrittura del titolo, allineato a sinistra dell'asse Y del grafico
            g.DrawString(graphsplotter.title, graphsplotter.title_font, graphsplotter.title_brush, axis_y0_X, graphsplotter.title_upper_margin);


            // Scrittura degli assi orizzontali e della loro label (la label è il numero che identifica la coordinata y dell'asse: max_value, 0 , min_value)

            // Asse y = max_value
            if (write_axis_ymax == true)
            {
                //  Sopprimi la label di ymax se va a interferire con quella dell'asse y=0
                if (axis_ymax_label_Y + graphsplotter.labels_height < axis_y0_label_Y)
                {
                    g.DrawString(axis_ymax_label, graphsplotter.labels_font, graphsplotter.labels_brush, axis_ymax_label_X, axis_ymax_label_Y);
                }
                g.DrawLine(solidpen, axis_ymax_X, axis_ymax_Y, axis_y_X_end, axis_ymax_Y);
            }

            // Asse y = 0
            g.DrawString(axis_y0_label, graphsplotter.labels_font, graphsplotter.labels_brush, axis_y0_label_X, axis_y0_label_Y);
            g.DrawLine(axis_y0_pen, axis_y0_X, axis_y0_Y, axis_y_X_end, axis_y0_Y);

            // asse y = min_value
            if (write_axis_ymin == true)
            {
                //  Sopprimi la label di ymin se va a interferire con quella dell'asse y=0
                if (axis_y0_label_Y + graphsplotter.labels_height < axis_ymin_label_Y)
                {
                    g.DrawString(axis_ymin_label, graphsplotter.labels_font, graphsplotter.labels_brush, axis_ymin_label_X, axis_ymin_label_Y);
                }
                g.DrawLine(solidpen, axis_ymin_X, axis_ymin_Y, axis_y_X_end, axis_ymin_Y);
            }





            // Adesso determiniamo la 'scala dei tempi' orizzontale
            int x_total_ticks = 1;  // don't care
            foreach (List<float> single_linear_data in values_list)
            {
                // Dobbiamo basarci sulla lunghezza massima delle serie
                if (single_linear_data.Count > x_total_ticks) x_total_ticks = single_linear_data.Count;
            }


            int x_ticks_per_division = 1; // don't care
            int num_x_divisions = 1;      // don't care

            // ATTENZIONE!! qua bisogna considerare che le labels con le coordinate Y degli assi (appena scritte) riducono l'area a disposizione per visualizzare la traccia
            int plot_width = graphsplotter.subgraph_size.Width - (graphsplotter.graph_left_margin + y_axis_labels_max_length + graphsplotter.y_labels_horizontal_offset_from_vertical_axis);

            // Qua abbiamo due casi:
            //      1) Abbiamo più pixels a disposizione che dati da rappresentare: ogni punto del grafico viene 'espanso' in un trattino. Solo in questo
            //          caso, se la modalità 'istogramma' è attiva, la scala dell'asse X viene sostituita dalle labels dell'asse X. Se la modalità
            //          'istogramma' non è attiva la scala dell'asse X viene visualizzata con dei numeri.
            //      2) Abbiamo meno pixels che dati: la scala orizzontale viene calcolata in modo tale da comprimere i dati nei pixels disponibili,
            //          la scala dell'asse X viene visualizzata con dei numeri.

            if (x_total_ticks <= plot_width)
            {
                // Abbiamo più pixels a disposizione che dati da rappresentare (o almeno altrettanti), ma se siamo in modalità istogramma dobbiamo
                //    verificare se c'è spazio sufficiente per scrivere tutti i rettangolini

                // Vediamo se c'è una larghezza sufficiente per rappresentare il grafico come istogramma (se l'opzione è abilitata)
                //      Ogni 'punto' di un istogramma è lungo come minimo 2 separatori + num_series rettangoli di larghezza minima)
                int single_histogram_block_min_width = 2 * graphsplotter.histogram_min_margin + num_series * graphsplotter.histogram_min_rectangle_width;
                bool histogram_is_active = false;
                if (graphsplotter.histogram == true && plot_width >= single_histogram_block_min_width * x_total_ticks) histogram_is_active = true;

                // Vediamo di quanto possiamo 'espandere' effettivamente ogni tick
                int x_expanded_scale = plot_width / x_total_ticks;
                if (x_expanded_scale > graphsplotter.x_maximum_width) x_expanded_scale = graphsplotter.x_maximum_width;

                // Vediamo se c'è una larghezza sufficiente per scrivere almeno la label più corta
                bool draw_xlabels_from_list_is_active = true;
                int min_labels_length = int.MaxValue;
                foreach (string label in x_labels_list)
                {
                    if ((int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Height) < min_labels_length) min_labels_length = (int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Height);
                }
                if (min_labels_length > x_expanded_scale) draw_xlabels_from_list_is_active = false;



                // Scrittura delle labels orizzontali (numeriche, o dalla lista, a seconda dei casi)
                if (histogram_is_active == false || draw_xlabels_from_list_is_active == false)
                {
                    // adesso bisogna ricalcolare la larghezza in pixels di una suddivisione in modo tale che termini su un 'bel' numero di ticks
                    x_ticks_per_division = (int)MarginValue(x_total_ticks, _direction_is_down);

                    num_x_divisions = plot_width / x_ticks_per_division;

                    // Abbiamo abbastanza dati per scrivere le labels numeriche e le barre verticali
                    draw_numeric_labels_and_bars(x_ticks_per_division, x_ticks_per_division, num_x_divisions, x_expanded_scale, axis_y0_X, graphsplotter, solidpen, g);
                }
                else
                {
                    draw_xlabels_from_list(x_labels_list, x_expanded_scale, axis_y0_X, graphsplotter, solidpen, g);
                }


                // E, rullo di tamburi, possiamo scrivere il grafico
                int series_counter = 0;
                foreach (List<float> series in values_list)
                {
                    // Per ogni serie di dati
                    int last_written_point_X = 0;
                    int last_written_point_Y = 0;
                    for (int i = 0; i < series.Count; i++)
                    {

                        int point_Y = axis_y0_Y;
                        // OCCHIO CHE LA Y IN PIXEL-COORDINATES VA VERSO IL BASSO
                        if (y_scale != 0) point_Y -= (int)(series[i] / y_scale);


                        int point_X = 0; ;
                        if (histogram_is_active == false)
                        {
                            // Grafico normale: ogni tick è un trattino orizzontale largo come tutto lo spazio disponibile
                            // posizione verticale del punto: oltre che dal valore dipende da dove è posizionato l'asse y=0                    
                            //  scrittura (con espansione orizzontale) del punto
                            point_X = axis_y0_X + i * x_expanded_scale;

                            Pen pen = new Pen(graphsplotter.series_colors[series_counter%graphsplotter.series_colors.Count]);

                            if (num_series > 1 && x_expanded_scale >= 3)
                            {
                                // Se c'è più di una serie (e abbastanza spazio!) lascia almeno un pixel di pendenza, altrimenti il grafico è troppo brutto
                                g.DrawLine(pen, point_X + 1, point_Y, point_X + x_expanded_scale - 2, point_Y);

                                if (graphsplotter.join_points == true && last_written_point_X != 0 && last_written_point_Y != 0)
                                {
                                    g.DrawLine(pen, last_written_point_X, last_written_point_Y, point_X + 1, point_Y);
                                }
                                last_written_point_X = point_X + x_expanded_scale - 2;
                            }
                            else
                            {
                                g.DrawLine(pen, point_X, point_Y, point_X + x_expanded_scale, point_Y);

                                if (graphsplotter.join_points == true && last_written_point_X != 0 && last_written_point_Y != 0)
                                {
                                    g.DrawLine(pen, last_written_point_X, last_written_point_Y, point_X, point_Y);
                                }
                                last_written_point_X = point_X + x_expanded_scale;
                            }



                            last_written_point_Y = point_Y;
                        }
                        else
                        {
                            // Se l'opzione histogram è vera il grafico viene disegnato con dei rettangoli pieni
                            //     larghi rectangles width ed offsettati ad ogni serie per non sovrapporli
                            //   Il -2 nel calcolo della larghezza dei rettangoli serve per tener conto dei pixels di spazio (sia a sinistra che a destra)
                            int rectangles_width = (x_expanded_scale - 2 * graphsplotter.histogram_min_margin) / num_series;
                            point_X = axis_y0_X + i * x_expanded_scale + series_counter * rectangles_width;

                            Brush brush = new SolidBrush(graphsplotter.series_colors[series_counter % graphsplotter.series_colors.Count]);
                            if (point_Y <= axis_y0_Y)
                            {
                                g.FillRectangle(brush, point_X + graphsplotter.histogram_min_margin, point_Y, rectangles_width, axis_y0_Y - point_Y);
                            }
                            else
                            {
                                g.FillRectangle(brush, point_X + graphsplotter.histogram_min_margin, axis_y0_Y, rectangles_width, point_Y - axis_y0_Y);
                            }
                        }

                    }
                    series_counter++;

                }
            }
            else
            {
                // Qua invece abbiamo più dati che pixels

                // La scala dei tempi è composta da un certo numero (num_x_divisions) di divisioni, ognuna di lunghezza x_ticks_per_division ticks,
                //      con x_ticks_per_division che alla fine deve essere un 'bel' numero (10 - 20 - 50 - 100 etc.)

                // x_division_length è una lunghezza in pixels ed è abbastanza arbitrario come valore: più è corto più il grafico può essere compresso
                //    in poco spazio, p.es. stringendo la finestra, ma col difetto che il grafico può diventare illeggible. Se x_division_length
                //    è lungo il grafico resta leggibile, ma stringendo la finestra scompare.
                num_x_divisions = plot_width / graphsplotter.x_minimum_division_length;
                if (num_x_divisions <= 0) { g.Dispose(); return (image); }

                float x_ticks_per_division_float = (float)x_total_ticks / (float)num_x_divisions;
                if (x_ticks_per_division_float <= 0) { g.Dispose(); return (image); }                  // safety
                if (graphsplotter.autoscale_x_is_oscilloscope_like == true)
                {
                    x_ticks_per_division_float = MarginValue(x_ticks_per_division_float, true);
                }
                if (x_ticks_per_division_float <= 0) { g.Dispose(); return (image); };                 // safety
                x_ticks_per_division = (int)(x_ticks_per_division_float);

                // L' "1" è x_expanded_scale (che qua non può essere altro che 1)
                draw_numeric_labels_and_bars(x_ticks_per_division, graphsplotter.x_minimum_division_length, num_x_divisions, 1, axis_y0_X, graphsplotter, solidpen, g);

                // calcolo della scala sull'asse X
                float x_scale = (float)graphsplotter.x_minimum_division_length / (float)x_ticks_per_division;

                // E, rullo di tamburi, possiamo scrivere il grafico
                int series_counter = 0;
                foreach (List<float> series in values_list)
                {
                    int last_written_point_X = 0;
                    int last_written_point_Y = 0;
                    for (int i = 0; i < series.Count; i++)
                    {
                        // posizione verticale del punto: oltre che dal valore dipende da dove è posizionato l'asse y=0                    
                        int point_Y = axis_y0_Y;
                        // OCCHIO CHE LA Y IN PIXEL-COORDINATES VA VERSO IL BASSO E VA INVERTITA, PER QUESTO C'E' IL SEGNO -!
                        if (y_scale != 0) point_Y -= (int)(series[i] / y_scale);   // else il punto resta a zero

                        // posizione orizzontale del punto
                        int point_X = axis_y0_X + (int)(i * x_scale);

                        // scrittura del punto
                        Pen pen = new Pen(graphsplotter.series_colors[series_counter % graphsplotter.series_colors.Count]);
                        g.DrawEllipse(pen, point_X, point_Y, 1, 1); // il metodo più semplice che ho trovato per disegnare un solo punto...
                        if (graphsplotter.join_points == true && last_written_point_X != 0 && last_written_point_Y != 0)
                        {
                            g.DrawLine(pen, last_written_point_X, last_written_point_Y, point_X, point_Y);
                        }
                        last_written_point_X = point_X;
                        last_written_point_Y = point_Y;
                    }
                    series_counter++;
                }
            }


            // Scrittura della legenda
            draw_series_names(graphsplotter.graph_area.Y + graphsplotter.graph_area.Height, series_names_list, graphsplotter, g);

            g.Dispose();

            return (image);
        }

        // Overload di DisplayGraph con 1 sola serie di dati
        public static Bitmap DisplayGraph(Size image_size, List<float> values_list, List<string> x_labels_list, XPlotGraphs1d graphsplotter)
        {
            List<List<float>> extended_list = new List<List<float>>();
            extended_list.Add(values_list);

            List<string> dummy_names = new List<string>();
            dummy_names.Add("");

            return DisplayGraph(image_size, extended_list, dummy_names, x_labels_list, graphsplotter);
        }




        static float MarginValue(float value, bool direction_is_up)
        {
            // Margina un numero ad un valore superiore in valore assoluto che abbia la forma  ... 0.01 0.02 0.05 1 2 5 10 20 50 100 200 500...
            //      (oppure ...-500 -200 -100 -50 -20 -10 -5 -2 -1 -0.5 -0.2 -0.1... se il numero è negativo)

            if (value == 0) return (0);

            double out_value = 0;
            double margined_value = 0;

            double absvalue = Math.Abs(value);

            double log_value = Math.Log10(absvalue);

            //  Vediamo se si può migliorare il range, facendo come sugli oscilloscopi: 1x 2x 5x 10x etc.
            // Margined_value è la potenza di 10 immediatamente superiore (o inferiore) a value
            if (direction_is_up == true)
            {
                if (Math.Sign(value) != -1)
                {
                    margined_value = Math.Abs(Math.Pow(10, Math.Ceiling(log_value)));
                    if (margined_value / 5 > absvalue) out_value = margined_value / 5;  // scala 5x
                    else
                    {
                        if (margined_value / 2 > absvalue) out_value = margined_value / 2;  // scala 2x
                        else
                        {
                            out_value = margined_value;
                        }
                    }
                }
                else
                {
                    margined_value = Math.Abs(Math.Pow(10, Math.Floor(log_value)));
                    if (margined_value * 5 < absvalue) out_value = margined_value * 5;  // scala 5x
                    else
                    {
                        if (margined_value * 2 < absvalue) out_value = margined_value * 2;  // scala 2x
                        else
                        {
                            out_value = margined_value;
                        }
                    }
                }
            }
            else
            {
                if (Math.Sign(value) != -1)
                {
                    margined_value = Math.Abs(Math.Pow(10, Math.Floor(log_value)));
                    if (margined_value * 5 < absvalue) out_value = margined_value * 5;  // scala 5x
                    else
                    {
                        if (margined_value * 2 < absvalue) out_value = margined_value * 2;  // scala 2x
                        else
                        {
                            out_value = margined_value;
                        }
                    }
                }
                else
                {
                    margined_value = Math.Abs(Math.Pow(10, Math.Ceiling(log_value)));
                    if (margined_value / 5 > absvalue) out_value = margined_value / 5;  // scala 5x
                    else
                    {
                        if (margined_value / 2 > absvalue) out_value = margined_value / 2;  // scala 2x
                        else
                        {
                            out_value = margined_value;
                        }
                    }
                }
            }

            return ((float)out_value * Math.Sign(value));
        }


        static void draw_numeric_labels_and_bars(int x_ticks_per_division, int x_pixels_per_division, int num_x_divisions, int x_expanded_scale, int axis_y0_X, XPlotGraphs1d graphsplotter, Pen solidpen, Graphics g)
        {
            int labels_base_Y_coord = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;

            int x_labels_Y = labels_base_Y_coord + graphsplotter.x_labels_vertical_offset_from_horizontal_axis;
            int x_axis_X = 1;      // don't care
            string label = "";    // don't care
            int x_label_length = 1; // don't care
            int x_label_X = 1;      // don't care
            for (int i = 0; i < (num_x_divisions + 1); i++) // +1 perchè la prima label è quella 'zero'
            {
                label = (i * x_ticks_per_division).ToString("G") + graphsplotter.x_units;
                x_label_length = (int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Width);
                x_axis_X = axis_y0_X + (i * x_pixels_per_division * x_expanded_scale);
                x_label_X = x_axis_X - x_label_length / 2;
                g.DrawString(label, graphsplotter.labels_font, graphsplotter.labels_brush, x_label_X, x_labels_Y);
                g.DrawLine(solidpen, x_axis_X, graphsplotter.graph_area.Y, x_axis_X, labels_base_Y_coord);   // scrittura barra verticale
            }

            // aggiungiamo una linea verticale alla fine del grafico
            g.DrawLine(solidpen, graphsplotter.subgraph_size.Width, graphsplotter.graph_area.Y, graphsplotter.subgraph_size.Width, labels_base_Y_coord);
        }

        static void draw_xlabels_from_list(List<string> x_labels, int x_expanded_scale, int axis_y0_X, XPlotGraphs1d graphsplotter, Pen solidpen, Graphics g)
        {
            int labels_base_Y_coord = graphsplotter.graph_area.Y + graphsplotter.subgraph_size.Height;

            int x_labels_Y = labels_base_Y_coord + graphsplotter.x_labels_vertical_offset_from_horizontal_axis;
            int x_axis_X = 1;      // don't care
            string label = "";    // don't care
            int x_label_length = 1; // don't care
            int x_label_X = 1;      // don't care
            for (int i = 0; i < x_labels.Count; i++)
            {
                label = x_labels[i];
                x_label_length = (int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Width);
                x_axis_X = axis_y0_X + (i * x_expanded_scale);
                x_label_X = x_axis_X + (x_expanded_scale /2) - (x_label_length / 2);
                if (x_label_length < x_expanded_scale + 2)
                {
                    // Scrivi la label solo se ci sta
                    g.DrawString(label, graphsplotter.labels_font, graphsplotter.labels_brush, x_label_X, x_labels_Y);
                }
                // Scrivi la riga verticale
                g.DrawLine(solidpen, x_axis_X, graphsplotter.graph_area.Y, x_axis_X, labels_base_Y_coord);
            }

            // aggiungiamo una linea verticale alla fine del grafico
            g.DrawLine(solidpen, graphsplotter.subgraph_size.Width, graphsplotter.graph_area.Y, graphsplotter.subgraph_size.Width, labels_base_Y_coord);
        }


        private static int get_series_legend_height(List<string> series_names_list, XPlotGraphs1d graphsplotter, Graphics g)
        {
            int legend_height = 0;
       
            graphsplotter.legend_max_names_width = 0;
            graphsplotter.legend_max_names_height = 0;
            foreach (string name in series_names_list)
            {
                if ((int)Math.Ceiling(g.MeasureString(name, graphsplotter.series_names_font).Width) > graphsplotter.legend_max_names_width) graphsplotter.legend_max_names_width = (int)Math.Ceiling(g.MeasureString(name, graphsplotter.series_names_font).Width);
                if ((int)Math.Ceiling(g.MeasureString(name, graphsplotter.series_names_font).Height) > graphsplotter.legend_max_names_height) graphsplotter.legend_max_names_height = (int)Math.Ceiling(g.MeasureString(name, graphsplotter.series_names_font).Height);
            }
            if (graphsplotter.legend_max_names_width > graphsplotter.legend_max_displayable_name_length) graphsplotter.legend_max_names_width = graphsplotter.legend_max_displayable_name_length;

            // Ogni voce della legenda è formata dal marker colorato + separatore + nome + separatore
            //      Il marker colorato è un quadratino alto come un nome, meno due pixels
            //     
            graphsplotter.legend_marker_length = graphsplotter.legend_max_names_height - 2;
            graphsplotter.legend_voice_length = graphsplotter.legend_marker_length + graphsplotter.legend_marker_to_name_margin + graphsplotter.legend_max_names_width + graphsplotter.legend_horizontal_space_between_legend_voices;

            // Suddivisione dell'area disponibile in modo che possa ospitare N voci di legenda
            int num_names_to_display = series_names_list.Count;           
            graphsplotter.legend_num_hor_divisions = (graphsplotter.graph_area.Width - graphsplotter.legend_left_margin) / graphsplotter.legend_voice_length;
            graphsplotter.legend_num_ver_divisions = (int)Math.Ceiling((double)num_names_to_display/(double)graphsplotter.legend_num_hor_divisions);

            legend_height = graphsplotter.legend_num_ver_divisions * graphsplotter.legend_max_names_height + graphsplotter.legend_lower_margin;

            return legend_height;
        }

        private static void draw_series_names(int legend_Y_coord, List<string> series_names_list, XPlotGraphs1d graphsplotter, Graphics g)
        {
            int legend_X_coord = graphsplotter.legend_left_margin;

            int column_counter = 0;
            int row_counter = 0;
            for (int i = 0; i < series_names_list.Count; i++)
            {
                draw_legend_voice(legend_X_coord + column_counter * graphsplotter.legend_voice_length, legend_Y_coord + row_counter * (graphsplotter.legend_max_names_height + graphsplotter.legend_vertical_space_between_legend_voices),
                                       series_names_list[i], graphsplotter.series_colors[i % graphsplotter.series_colors.Count], graphsplotter, g);
                column_counter++;
                if (column_counter >= graphsplotter.legend_num_hor_divisions)
                {
                    column_counter = 0;
                    row_counter++;
                }
            }


        }
        private static void draw_legend_voice(int voice_X_coord, int voice_Y_coord, string name, Color name_color, XPlotGraphs1d graphsplotter, Graphics g)
        {
            Brush marker_brush = new SolidBrush(name_color);
            int marker_size = graphsplotter.legend_max_names_height;
            g.FillRectangle(marker_brush, voice_X_coord, voice_Y_coord, marker_size, marker_size);

            // Overload di DrawString che limita la lunghezza di quello che viene scritto
            RectangleF name_rectangle = new RectangleF(voice_X_coord + marker_size + graphsplotter.legend_marker_to_name_margin, voice_Y_coord, graphsplotter.legend_max_names_width, (int)Math.Ceiling(g.MeasureString(name, graphsplotter.series_names_font).Height));
            g.DrawString(name, graphsplotter.series_names_font, graphsplotter.series_names_brush, name_rectangle);
        }






        private static void DrawNoDataGraph(Graphics g, XPlotGraphs1d graphsplotter)
        {
            g.DrawString(graphsplotter.title + " No data to show", graphsplotter.title_font, graphsplotter.labels_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
        }

        private static void DrawNoSpaceGraph(Graphics g, XPlotGraphs1d graphsplotter)
        {
            g.DrawString(graphsplotter.title + " Not enough space to draw graph", graphsplotter.title_font, graphsplotter.labels_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
        }

    }
}

