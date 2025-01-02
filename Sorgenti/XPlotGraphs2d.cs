using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Template
{
    // Visualizzatore di grafici 2D a dispersione, output con scala colori

    //  Questi grafici prevedono di avere in ingresso una List<List<List<>>> di dimensione N x M x M, cioè N serie di MxM dati
    //     Dividono l'area disponibile in N sottografici ognuno composto da MxM caselle a cui viene assegnato un colore proporzionale al valore dei dati

    // Gli assi vengono sempre marcati con le labels prese dalla lista passata come parametro a Display_Graph, non è supportata una modalità
    //   analoga a quella di XPlotGraphs1d in cui gli assi vengono marcati con valori numerici

    public class XPlotGraphs2d
    {


        public Color background_color = Color.Black;

        // Parametri del titolo globale del grafico
        public string title = "Title";

        public int title_upper_margin = 2;                  // in pixels
        public int title_left_margin = 2;                   // in pixels

        public string title_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float title_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color title_color = Color.White;


        // Parametri che determinano la posizione dell'area in cui vengono scritti i sottografici
        public int title_to_graphs_upper_margin = 4;
        public int graphs_left_margin = 4;


        // Parametri dei sottografici
        // Nome del sottografico (nome della serie): viene visualizzato solo se c'è più di 1 sottografico
        public string series_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float series_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color series_name_color = Color.White;

        // Labels (orizzontali e verticali)
        public string labels_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float labels_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color labels_color = Color.White;

        public bool double_labels = false;                                   // se true le labels vengono visualizzate sia in alto che in basso, e sia a sinistra che a destra
                                                                             //  In effetti DOVREBBE venire riservato lo spazio, ma la gestione che scrive le lables non c'è   

        public int series_name_to_hor_labels_upper_margin = 2;
        public int hor_labels_to_graph_upper_margin = 2;

        public int ver_labels_to_graph_left_margin = 2;
        public int ver_labels_maximum_length = 12;



        // Parametri grafici della scala colori
        public int color_scale_heigth = 24;           // altezza riservata per scala + numeri
        public float scale_percentage = 0.6f;         // quanto la scala dei colori è rimpicciolita rispetto all'area del grafico

        public int max_color_scale_line_height = 8;   // max altezza della barra colorata  

        public string color_scale_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float color_scale_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color color_scale_numbers_color = Color.White;


        // Definizione dei colori della scala, che è a tre colori (estermo negativo, centro, estremo positivo)
        public Color graph_color_low_extreme = Color.Blue;
        public Color graph_color_center = Color.White;
        public Color graph_color_high_extreme = Color.Red;


        // Gestione offscale low e offscale high
        public bool put_extremes_in_evidence = true;                    // si potrebbe anche eliminare, lasciandolo sempre 'true' basta settare high/low_extreme
                                                                        //   per abilitare o meno la gestione
        public float high_extreme = float.MaxValue;                     // tutti i valori <= sono 'offscale high'
        public Color graph_color_high_extreme_in_evidence = Color.Red;
        public float low_extreme = float.MinValue;                      // tutti i valori <= sono 'offscale low'
        public Color graph_color_low_extreme_in_evidence = Color.Navy;

        // Flag concepito per evidenziare il dato 'zero'. Utile in casi in cui alcuci valori sono a zero (p.es. valori 'proibiti', ma tutti gli altri sono compresi fra un minimo e un massimo che non include lo zero
        public bool put_zero_in_evidence = false;
        public Color graph_color_zero_in_evidence = Color.Navy;


        
        // Parametro interno private
        private readonly int minimum_square_size = 4;                                // quadratino 2x2 colorato più due righe per lato, NOTARE che è private (non è modificabile!)



        // Dati ricavati durante la scrittura dei grafici (tutti private!)
        //   OCCHIO A NON UTILIZZARLI PRIMA DI AVERLI GESTITI..... E' UN'IMPOSTAZIONE RISCHIOSA (SAREBBERO DOVUTI ESSERE TUTTI 'NULLABLE' PER SICUREZZA MI SA)
        //   vengono comodi perche' così si può passare tutto alle sottoroutines tramite graphsplotters, ma dato che sono pericolosi ne ho usati meno che potevo
        //   (anche se mi è toccato passare più parametri alle sottoroutines)

        private bool put_zero_in_evidence_is_active;

        private Rectangle graphs_area = new Rectangle();  // area in cui vengono scritti i sottografici (area totale depurata dallo spazio per il titolo e per la scala colori)
        private Size subgraphs_size = new Size();         // dimensioni di ogni sottografico  
        private int square_size;                          // dimensione di ogni quadratino dei grafici (inclusiva di un pixel di spazio per lato)


        // Inizializzazioni dummy di fonts, brushes etc.
        private Font default_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush default_brush = new SolidBrush(Color.White);
        private Font title_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush title_brush = new SolidBrush(Color.White);
        private int title_height;
        private Font labels_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush labels_brush = new SolidBrush(Color.White);
        private Font series_names_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush series_names_brush = new SolidBrush(Color.White);


        public static Bitmap DisplayGraph(Size image_size, List<List<List<float>>> values_list, List<string> series_names_list, List<List<string>> labels_list, XPlotGraphs2d graphsplotter)
        {
            Bitmap image = new Bitmap(image_size.Width, image_size.Height);

            Graphics g = Graphics.FromImage(image);
            g.PageUnit = GraphicsUnit.Pixel;        // Mah.. di default PageUnit è settata a Display, sembra non cambi niente se si toglie questa istruzione, ma va a sapere..

            g.Clear(graphsplotter.background_color);

            // Il default font assume le stesse caratteristiche del titolo, viene usato per visualizzare i meassagi di errore nel rafico (es. DrawNoDataGraph)
            graphsplotter.default_font = new Font(graphsplotter.title_font_name, graphsplotter.title_font_size);
            graphsplotter.default_brush = new SolidBrush(graphsplotter.title_color);

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


            // Un grafico è composto da:

            //      Titolo
            //      N sottografici (in funzione del numero delle serie, la prima dimensione della values list)
            //      La scala colori (ha senso solo metterla in orizzontale, questo perchè così è facile riservare l'area in cui scrivere la scale e
            //          soprattutto i numeri (che possono essere anche molto lunghi)


            // Parametri del titolo
            graphsplotter.title_font = new Font(graphsplotter.title_font_name, graphsplotter.title_font_size);
            graphsplotter.title_brush = new SolidBrush(graphsplotter.title_color);
            graphsplotter.title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.title_font).Height);



            // Troviamo l'area in cui verranno scritti i sottografici
            //   Iniziamo settando il punto di inizio dell'area, riservando lo spazio per il titolo e per il margine sinistro
            graphsplotter.graphs_area = new Rectangle(graphsplotter.graphs_left_margin, graphsplotter.title_upper_margin + graphsplotter.title_height + graphsplotter.title_to_graphs_upper_margin, 1, 1);
            //   Adesso troviamo le dimensioni dell'area, riservando lo spazio per la scala colori
            graphsplotter.graphs_area.Width = image_size.Width - graphsplotter.graphs_area.X;
            graphsplotter.graphs_area.Height = image_size.Height - graphsplotter.graphs_area.Y - graphsplotter.color_scale_heigth;

            if (graphsplotter.graphs_area.Width <= 0 || graphsplotter.graphs_area.Height <= 0)
            {
                DrawNoSpaceGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }




            // Suddivisione dell'area disponibile in modo che possa ospitare N sottografici
            int num_graphs_to_display = values_list.Count;
            int num_hor_divisions = 1;
            int num_ver_divisions = 1;
            while (num_hor_divisions * num_ver_divisions < num_graphs_to_display)
            {
                if ((graphsplotter.graphs_area.Width / (num_hor_divisions + 1)) > (graphsplotter.graphs_area.Height / (num_ver_divisions + 1)))
                {
                    num_hor_divisions += 1;
                }
                else
                {
                    num_ver_divisions += 1;
                }
            }

            graphsplotter.subgraphs_size = new Size(graphsplotter.graphs_area.Width / num_hor_divisions, graphsplotter.graphs_area.Height / num_ver_divisions);



            // Ora vogliamo controllare che lo spazio sia sufficiente: per farlo dobbiamo determinare la dimensione minima dei sottografici, dobbiamo considerare:
            //      - Eventuale intestazione (nome della serie)
            //      - Labels orizzontali e verticali e scacchiera coi dati

            // Come minimo, un quadratino coi dati ha dimensione 4x4 (un quadratino colorato 2x2 più una riga di un pixel per lato) [parametro non modificabile minimum_square_size]
            //   Le labels orizzontali si adattano automaticamente alla larghezza del quadratino (se non c'è spazio non vengono scritte)
            //   Le labels verticali vengono scritte se sono abbastanza corte [parametro ver_labels_maximum_length]

            // Determiniamo lunghezza e altezza massima delle labels
            graphsplotter.labels_font = new Font(graphsplotter.labels_font_name, graphsplotter.labels_font_size);
            int max_labels_width = 0;
            int max_labels_height = 0;
            foreach (List<string> labels in labels_list)
            {
                foreach (string label in labels)
                {
                    int labels_width = (int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Width);
                    if (labels_width > max_labels_width) max_labels_width = labels_width;
                    int labels_height = (int)Math.Ceiling(g.MeasureString(label, graphsplotter.labels_font).Height);
                    if (labels_height > max_labels_height) max_labels_height = labels_height;
                }
            }
            if (max_labels_width > graphsplotter.ver_labels_maximum_length) max_labels_width = graphsplotter.ver_labels_maximum_length;

            // E determiniamo la quantità massima di dati da visualizzare
            int max_data_length = 0;
            foreach (List<List<float>> data in values_list)
            {
                if (data.Count > max_data_length) max_data_length = data.Count;
            }
            if (max_data_length == 0)
            {
                DrawNoDataGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }


            // Richiedo per ogni grafico le dimensioni che servono per visualizzare la massima quantità di dati associata alla massima lunghezza delle labels
            //   (non è detto che il grafico con le labels più lunghe abbia anche la massima quantità di dati, ma importa poco o nulla)
            int min_subgraph_size_X = max_labels_width + graphsplotter.ver_labels_to_graph_left_margin + max_data_length * graphsplotter.minimum_square_size;
            int min_subgraph_size_Y = max_labels_height + graphsplotter.hor_labels_to_graph_upper_margin + max_data_length * graphsplotter.minimum_square_size;

            // Spazio per le doppie labels
            if (graphsplotter.double_labels == true)
            {
                min_subgraph_size_X += max_labels_width + graphsplotter.ver_labels_to_graph_left_margin;
                min_subgraph_size_Y += max_labels_height + graphsplotter.hor_labels_to_graph_upper_margin;
            }

            // Spazio per il nome della serie (se ce n'è più di una)
            int series_name_height = 0;
            graphsplotter.series_names_font = new Font(graphsplotter.series_font_name, graphsplotter.series_font_size);
            if (series_names_list != null && series_names_list.Count != 0)
            {
                series_name_height = (int)Math.Ceiling(g.MeasureString(series_names_list[0], graphsplotter.series_names_font).Height);
                series_name_height += graphsplotter.series_name_to_hor_labels_upper_margin;
            }
            min_subgraph_size_Y += series_name_height;


            // Check che lo spazio sia sufficiente
            if (graphsplotter.subgraphs_size.Width < min_subgraph_size_X || graphsplotter.subgraphs_size.Height < min_subgraph_size_Y)
            {
                DrawNoSpaceGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }



            // Crea la lista che contiene i punti di inizio di ognuno dei sottografici
            List<Point> subgraphs_start_points = new List<Point>();
            int hor_counter = 0;
            int ver_counter = 0;
            for (int i = 0; i < num_graphs_to_display; i++)
            {
                Point new_point = new Point(graphsplotter.graphs_area.X + hor_counter * graphsplotter.subgraphs_size.Width, graphsplotter.graphs_area.Y + ver_counter * graphsplotter.subgraphs_size.Height);
                subgraphs_start_points.Add(new_point);

                hor_counter++;
                if (hor_counter >= num_hor_divisions)
                {
                    hor_counter = 0;
                    ver_counter++;
                }
            }



            // Adesso dobbiamo ricavare quanto spazio effettivo abbiamo a disposizione per scrivere i quadratini, e da lì determinarne la dimensione
            int square_array_size_X = graphsplotter.subgraphs_size.Width - max_labels_width - graphsplotter.ver_labels_to_graph_left_margin; // spazio per le labels verticali a sinistra
            if (graphsplotter.double_labels == true)
            {
                square_array_size_X = square_array_size_X - max_labels_width - graphsplotter.ver_labels_to_graph_left_margin; // spazio per le labels verticali a destra
            }

            int square_array_size_Y = graphsplotter.subgraphs_size.Height - series_name_height - max_labels_height - graphsplotter.hor_labels_to_graph_upper_margin;
            if (graphsplotter.double_labels == true)
            {
                square_array_size_Y = square_array_size_Y - max_labels_height - graphsplotter.hor_labels_to_graph_upper_margin; // spazio per le labels orizzontali in basso
            }

            int square_array_size = square_array_size_X;
            if (square_array_size_Y < square_array_size) square_array_size = square_array_size_Y;

            graphsplotter.square_size = square_array_size / max_data_length;
            if (graphsplotter.square_size < graphsplotter.minimum_square_size) // 4 = quadratino colorato 2x2 + 2 bordi neri (safety)
            {
                DrawNoSpaceGraph(g, graphsplotter);
                g.Dispose();
                return (image);
            }




            // Calcolo dei parametri che servono per la scala sull'asse Z (scala dei colori)

            // Il funzionamento dell'autoscaling dipende:
            //      1) Dai valori max e min effettivamente contenuti nelle tabelle
            //      2) Dai parametri high_extreme e low_extreme, che considerano tutti i valori maggiori di high_extreme o minori di low_extreme
            //          come 'offscale' (a questi valori viene assegnato un colore particolare)
            //      3) Dal flag put_zero_in_evidence. Questo flag è concepito per dati con una forma particolare: includono il valore zero, ma tutti gli altri dati
            //            sono separati da zero. Per esempio: 0 0 5 6 8 0 8 0 5, dove i dati sono o zero o compresi fra 5 e 8. In questo caso allo zero viene
            //            assegnato un colore particolare, mentre la scala sull'asse Z viene calcolata fra 5 e 8


            // Troviamo il numero minimo e massimo all'interno della lista dei valori, che serve poi per dimensionare la scala sull'asse z (i colori)
            //   Lo zero ha una gestione speciale per poi poter considerare il flag put_zero_in_evidence
            float min_value = float.MaxValue;
            float max_value = float.MinValue;
            bool contains_zero = false;
            foreach (List<List<float>> values_table in values_list)
            {
                foreach (List<float> row in values_table)
                {
                    foreach (float value in row)
                    {
                        if (value == 0)
                        {
                            contains_zero = true;
                        }
                        else
                        {
                            if (value < min_value) min_value = value;
                            if (value > max_value) max_value = value;
                        }
                    }
                }
            }

            // put_zero_in_evidence: ha senso solo se i dati sono tutti negativi o tutti positivi, e contengono lo zero
            graphsplotter.put_zero_in_evidence_is_active = graphsplotter.put_zero_in_evidence;
            if (contains_zero == false || (min_value < 0 && max_value > 0))
            {
                graphsplotter.put_zero_in_evidence_is_active = false;
            }

            // high_extreme e low_extreme
            float z_scale_effective_min_value = min_value;
            float z_scale_effective_max_value = max_value;
            if (min_value < graphsplotter.low_extreme) z_scale_effective_min_value = graphsplotter.low_extreme;
            if (max_value > graphsplotter.high_extreme) z_scale_effective_max_value = graphsplotter.high_extreme;

            // E finalmente calcoliamo la scala sull'asse Z, cioè lungo la sequenza dei colori. La scala viene calcolata in modo tale che DUE
            //    sequenze di colori coprano l'intero range max_value - min_value (che può essere eventualmente limitato da low_extreme e high_extreme)
            float z_scale = 1;
            float absolute_range = z_scale_effective_max_value - z_scale_effective_min_value;
            // Absolute_range = 0 causerebbe, ovviamente, un problema di divisione per zero nel calcolo della scala Z
            //  Ma anche valori troppo piccoli di absolute_range causano (credo) problemi di precisione nei calcoli che alla fine possono portare
            //     ad un'eccezione in get_color_gradient perchè i parametri dei colori vanno fuori range (l'eccezione viene poi trappata nel try/catch
            //     che lancia draw_subgraphs)
            float min_allowed_absolute_value = 1E-6f;
            if (absolute_range <= min_allowed_absolute_value)
            {
                if (contains_zero == false)
                {
                    // Non c'è niente da fare, il range dei dati è zero
                    DrawAllDataEqualsGraph(g, graphsplotter);
                    g.Dispose();
                    return (image);
                }
                else
                {
                    // Else cerchiamo di recuperare la situazione: reinseriamo lo zero fra i dati validi
                    //   Ricordare che se arriviamo qua z_scale_max e z_scale_min sono uguali o vicinissimi (ma potrebbero essere zero o vicinissimi a zero!)
                    if (z_scale_effective_max_value > 0)
                    {
                        if (z_scale_effective_max_value > min_allowed_absolute_value)
                        {
                            z_scale_effective_min_value = 0;
                            absolute_range = z_scale_effective_max_value;
                        }
                        else
                        {
                            // Non c'è niente da fare, il range dei dati è zero
                            DrawAllDataEqualsGraph(g, graphsplotter);
                            g.Dispose();
                            return (image);
                        }
                    }
                    else
                    {
                        if (z_scale_effective_min_value < - 1* min_allowed_absolute_value)
                        {
                            z_scale_effective_max_value = 0;
                            absolute_range = -1 * z_scale_effective_min_value;
                        }
                        else
                        {
                            // Non c'è niente da fare, il range dei dati è zero
                            DrawAllDataEqualsGraph(g, graphsplotter);
                            g.Dispose();
                            return (image);
                        }
                    }
                    graphsplotter.put_zero_in_evidence_is_active = false;  // E va tolto l'eventuale 'zero in evidence' per non fare casini
                }
            }
            // Notare il DUE (due sequenze di colori)
            z_scale = 2 * Math.Abs(1 / absolute_range); // l'Abs è una sicurezza, absolute_range dovrebbe sempre essere positivo


            // E finalmente iniziamo a scrivere la bitmap, iniziando dal titolo
            g.DrawString(graphsplotter.title, graphsplotter.title_font, graphsplotter.title_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);



            // Scrittura dei sottografici
            for (int i = 0; i < num_graphs_to_display; i++)
            {
                try
                {
                    Point start_point = new Point(subgraphs_start_points[i].X, subgraphs_start_points[i].Y);
                    draw_subgraph(start_point, series_names_list[i], labels_list[i], values_list[i], graphsplotter.square_size, z_scale, z_scale_effective_min_value, z_scale_effective_max_value, graphsplotter, g);
                }
                catch
                {
                    DrawWrongDataGraph(g, graphsplotter);
                    g.Dispose();
                    return (image);
                }
            }



            // E ancora più alla fine scriviamo la scala dei colori
            // diciamo che voglio una scala larga l'x% del grafico complessivo, calcoliamo quanti pixels sono
            int color_scale_numpixels = (int)(graphsplotter.scale_percentage * graphsplotter.subgraphs_size.Width);
            Point color_scale_start_point = new Point((image.Width - color_scale_numpixels)/2, image.Height - graphsplotter.color_scale_heigth);

            draw_color_scale(color_scale_start_point, z_scale, color_scale_numpixels, graphsplotter.square_size, z_scale_effective_min_value, z_scale_effective_max_value, graphsplotter, g);


            g.Dispose();

            return (image);
        }


        // Overload di DisplayGraph con 1 sola serie di dati
        public static Bitmap DisplayGraph(Size image_size, List<List<float>> values_list, List<string> labels_list, XPlotGraphs2d graphsplotter)
        {
            List<List<List<float>>> extended_list = new List<List<List<float>>>();
            extended_list.Add(values_list);

            List<List<string>> extended_labels = new List<List<string>>();
            extended_labels.Add(labels_list);

            List<string> dummy_names = new List<string>();
            dummy_names.Add("");

            return ( DisplayGraph(image_size, extended_list, dummy_names, extended_labels, graphsplotter) );
        }



        private static void draw_subgraph(Point start_point, string name, List<string> labels, List<List<float>> values_table, int square_size, float z_scale, float min_value, float max_value,  XPlotGraphs2d graphsplotter, Graphics g)
        {
            // Scrittura del nome (se esiste): esattamente nell'angolo in alto a sinistra del sottografico
            int name_height = 0;
            if (name != "")
            {
                Font name_font = new Font(graphsplotter.series_font_name, graphsplotter.series_font_size);
                Brush name_brush = new SolidBrush(graphsplotter.series_name_color);
                name_height = (int)Math.Ceiling(g.MeasureString(name, name_font).Height);
                g.DrawString(name, name_font, name_brush, start_point.X, start_point.Y);
            }


            // Scrittura delle labels orizzontali: devo sapere quanto sono lunghe quelle che verranno scritte in verticale per sapere dove posizionare la prima
            Font labels_font = new Font(graphsplotter.labels_font_name, graphsplotter.labels_font_size);
            Brush labels_brush = new SolidBrush(graphsplotter.labels_color);
            int max_labels_width = 0;
            int max_labels_height = 0;
            foreach (string label in labels)
            {
                int scratch = (int)Math.Ceiling(g.MeasureString(label, labels_font).Width);
                if (scratch > max_labels_width) max_labels_width = scratch;
                scratch = (int)Math.Ceiling(g.MeasureString(label, labels_font).Height);
                if (scratch > max_labels_height) max_labels_height = scratch;
            }
            if (max_labels_width > graphsplotter.ver_labels_maximum_length) max_labels_width = graphsplotter.ver_labels_maximum_length;


            int hor_labels_start_X = start_point.X + max_labels_width;
            int hor_labels_start_Y = start_point.Y + name_height + graphsplotter.hor_labels_to_graph_upper_margin;
            for (int i = 0; i < labels.Count; i++)
            {
                string x_label = labels[i];
                int x_label_length = (int)Math.Ceiling(g.MeasureString(x_label, labels_font).Width);

                int x_label_X = 0; // don't care

                // Scrivi la label orizzontale
                if (x_label_length <= square_size)
                {
                    x_label_X = hor_labels_start_X + i * square_size + (square_size / 2) - (x_label_length / 2);
                    g.DrawString(x_label, labels_font, labels_brush, x_label_X, hor_labels_start_Y);
                }
                else
                {
                    x_label_X = hor_labels_start_X + i * square_size;

                    // Questo overload di DrawString _dovrebbe_ troncare la label se sfora dal rettangolo lungo square_size
                    int label_height = (int)Math.Ceiling(g.MeasureString(x_label, labels_font).Height);
                    RectangleF label_rectangle = new RectangleF(x_label_X, hor_labels_start_Y, square_size, label_height);
                    g.DrawString(x_label, labels_font, labels_brush, label_rectangle);
                }
            }


            // E, rullo di tamburi, possiamo scrivere il grafico (e le labels verticali)
            for (int row_index = 0; row_index < values_table.Count; row_index++)
            {
                int point_Y = hor_labels_start_Y + max_labels_height + row_index * square_size;

                // scrittura colonna labels
                string label = labels[row_index];
                int label_height = (int)Math.Ceiling(g.MeasureString(label, labels_font).Height);

                // le labels verticali vengono scritte anche se non c'è altezza sufficiente (stà meglio così)
                int label_start_Y = point_Y + (square_size / 2) - (label_height / 2);
                // Questo overload di DrawString _dovrebbe_ troncare la label se sfora dal rettangolo lungo ver_labels_maximum_length                
                RectangleF label_rectangle = new RectangleF(start_point.X, label_start_Y, graphsplotter.ver_labels_maximum_length, label_height);
                g.DrawString(label, labels_font, labels_brush, label_rectangle);


                // Scrittura quadratini coi dati
                for (int column_index = 0; column_index < values_table[row_index].Count; column_index++)
                {
                    int point_X = start_point.X + max_labels_width + graphsplotter.ver_labels_to_graph_left_margin + column_index * square_size;

                    Brush brush = new SolidBrush(graphsplotter.graph_color_center); // dummy
                    float value = values_table[row_index][column_index];
                    brush = new SolidBrush(get_color_gradient(value, z_scale, min_value, max_value, graphsplotter));

                    // lascia 1 pixel di spazio da ogni lato del quadratino
                    g.FillRectangle(brush, point_X + 1, point_Y + 1, square_size - 1, square_size - 1);

                }
            }
        }

        static Color get_color_gradient(float value, float z_scale, float min_data, float max_data, XPlotGraphs2d gplot)
        {

            if (value == 0 && gplot.put_zero_in_evidence_is_active == true) return gplot.graph_color_zero_in_evidence;

            if (gplot.put_extremes_in_evidence == true)
            {
                if (value >= gplot.high_extreme) return gplot.graph_color_high_extreme_in_evidence;
                if (value <= gplot.low_extreme) return gplot.graph_color_low_extreme_in_evidence;
            }

            Color color_low_scale = gplot.graph_color_low_extreme;
            Color color_high_scale = gplot.graph_color_center;

            if (value < min_data) value = min_data;  // safeties
            if (value > max_data) value = max_data;

            // ci sono tre casi: solo dati positivi, solo dati negativi, dati sia positivi che negativi
            float color_scale_factor = 1;
            if (min_data >= 0)
            {
                // dati solo positivi
                float midpoint = min_data + (max_data - min_data) / 2;
                if (value >= midpoint )
                {
                    color_low_scale = gplot.graph_color_center;
                    color_high_scale = gplot.graph_color_high_extreme;
                    color_scale_factor = (value - midpoint) * z_scale;
                }
                else
                {
                    color_low_scale = gplot.graph_color_low_extreme;
                    color_high_scale = gplot.graph_color_center;
                    color_scale_factor = (value - min_data) * z_scale;
                }
            }
            else
            {
                if (max_data <= 0)
                {
                    // dati solo negativi
                    float midpoint = max_data + (min_data - max_data) / 2;
                    if (value <= midpoint)
                    {
                        color_low_scale = gplot.graph_color_low_extreme;
                        color_high_scale = gplot.graph_color_center;
                        color_scale_factor = 1 - (Math.Abs(value) - Math.Abs(midpoint)) * z_scale;
                    }
                    else
                    {
                        color_low_scale = gplot.graph_color_center;
                        color_high_scale = gplot.graph_color_high_extreme;
                        color_scale_factor = 1 - (Math.Abs(value) - Math.Abs(max_data)) * z_scale;
                    }
                }
                else
                {
                    // dati sia positivi che negativi
                    float midpoint = min_data + (max_data - min_data) / 2;
                    if (value >= midpoint)
                    {
                        color_low_scale = gplot.graph_color_center;
                        color_high_scale = gplot.graph_color_high_extreme;
                        color_scale_factor = (value - midpoint) * z_scale;
                    }
                    else
                    {
                        color_low_scale = gplot.graph_color_low_extreme;
                        color_high_scale = gplot.graph_color_center;
                        color_scale_factor = 1 - Math.Abs(value - midpoint) * z_scale;
                    }
                }

            }

            if (color_scale_factor < 0) color_scale_factor = 0;  // Valori negativi possono causare eccezioni quando si crea l'out_color

            int deltacolor_R = color_high_scale.R - color_low_scale.R;
            int deltacolor_G = color_high_scale.G - color_low_scale.G;
            int deltacolor_B = color_high_scale.B - color_low_scale.B;

            Color out_color = Color.FromArgb(255, color_low_scale.R + (int)((float)deltacolor_R * color_scale_factor), color_low_scale.G + (int)((float)deltacolor_G * color_scale_factor), color_low_scale.B + (int)((float)deltacolor_B * color_scale_factor));

            return out_color;
        }

        private static void draw_color_scale(Point start_point, float z_scale, int color_scale_numpixels, int square_size, float min_value, float max_value, XPlotGraphs2d graphsplotter, Graphics g)
        {

            Font font = new Font(graphsplotter.color_scale_font_name, graphsplotter.color_scale_font_size);
            Brush brush = new SolidBrush(graphsplotter.color_scale_numbers_color);

            int numbers_height = (int)Math.Ceiling(g.MeasureString("999", font).Height);

            int lines_size = square_size;
            if (lines_size > graphsplotter.max_color_scale_line_height) lines_size = graphsplotter.max_color_scale_line_height;


            int color_scale_X = start_point.X;
            int color_scale_Y = start_point.Y;

            // Eventuale quadratino per 'zero in evidence', nel caso tutti i valori siano positivi
            if (graphsplotter.put_zero_in_evidence_is_active == true && min_value > 0)
            {
                Pen pen = new Pen(graphsplotter.graph_color_zero_in_evidence);
                for (int i = 0; i < lines_size; i++)
                {
                    g.DrawLine(pen, color_scale_X, i + color_scale_Y, color_scale_X + lines_size, i + color_scale_Y);
                }
                g.DrawString("0", font, brush, color_scale_X, color_scale_Y + lines_size);

                color_scale_X = color_scale_X + 2 * lines_size;
            }

            // Eventuale quadratino per 'offscale low'
            if (graphsplotter.put_extremes_in_evidence == true && min_value == graphsplotter.low_extreme)
            {
                Pen pen = new Pen(graphsplotter.graph_color_low_extreme_in_evidence);
                for (int i = 0; i < lines_size; i++)
                {
                    g.DrawLine(pen, color_scale_X, i + color_scale_Y, color_scale_X + lines_size, i + color_scale_Y);
                }
                g.DrawString("<= " + graphsplotter.low_extreme.ToString(), font, brush, color_scale_X, color_scale_Y + lines_size);

                int next_offset_X = (int)Math.Ceiling(g.MeasureString("<= " + graphsplotter.low_extreme.ToString(), font).Width);
                if ( 2 * lines_size > next_offset_X) next_offset_X = 2 * lines_size;

                color_scale_X += next_offset_X;
            }


            // Scala dei colori
            g.DrawString(min_value.ToString(), font, brush, color_scale_X, color_scale_Y + lines_size);  // valore minimo

            float base_value = 0;
            for (int i = 0; i < color_scale_numpixels; i++)
            {
                // base_value varia fra min_value e max_value
                base_value = ((max_value - min_value) / (float)color_scale_numpixels) * i + min_value;
                Pen pen = new Pen(get_color_gradient(base_value, z_scale, min_value, max_value, graphsplotter));
                g.DrawLine(pen, color_scale_X + i, color_scale_Y, color_scale_X + i, color_scale_Y + lines_size);
            }
            color_scale_X += color_scale_numpixels;


            // per sapere dove scrivere il valore massimo dobbiamo prima calcolarne la lunghezza, in modo che termini dove termina la scala colori
            int max_value_lenght = (int)Math.Ceiling(g.MeasureString(max_value.ToString(), font).Width);
            g.DrawString(max_value.ToString(), font, brush, color_scale_X - max_value_lenght, color_scale_Y + lines_size);
            color_scale_X += lines_size;


            // Eventuale quadratino per 'offscale high'
            if (graphsplotter.put_extremes_in_evidence == true && max_value == graphsplotter.high_extreme)
            {
                Pen pen = new Pen(graphsplotter.graph_color_high_extreme_in_evidence);
                for (int i = 0; i < lines_size; i++)
                {
                    g.DrawLine(pen, color_scale_X, i + color_scale_Y, color_scale_X + lines_size, i + color_scale_Y);
                }
                g.DrawString(">= " + graphsplotter.high_extreme.ToString(), font, brush, color_scale_X, color_scale_Y + lines_size);

                int next_offset_X = (int)Math.Ceiling(g.MeasureString(">= " + graphsplotter.high_extreme.ToString(), font).Width);
                if (2 * lines_size > next_offset_X) next_offset_X = 2 * lines_size;

                color_scale_X += next_offset_X;
            }

            // Eventuale quadratino per 'zero in evidence', nel caso tutti i valori siano negativi
            if (graphsplotter.put_zero_in_evidence_is_active == true && max_value < 0)
            {
                Pen pen = new Pen(graphsplotter.graph_color_zero_in_evidence);
                for (int i = 0; i < lines_size; i++)
                {
                    g.DrawLine(pen, color_scale_X, i + color_scale_Y, color_scale_X + lines_size, i + color_scale_Y);
                }
                g.DrawString("0", font, brush, color_scale_X, color_scale_Y + lines_size);

                color_scale_X = color_scale_X + 2 * lines_size;
            }

        }




        static void DrawNoDataGraph(Graphics g, XPlotGraphs2d graphsplotter)
        {
            int title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.default_font).Height);
            g.DrawString(graphsplotter.title, graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
            g.DrawString("No data to show", graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin + title_height + graphsplotter.title_upper_margin);
        }

        static void DrawWrongDataGraph(Graphics g, XPlotGraphs2d graphsplotter)
        {
            int title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.default_font).Height);
            g.DrawString(graphsplotter.title, graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
            g.DrawString("SOFTWARE ERROR: wrong input data", graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin + title_height + graphsplotter.title_upper_margin);
        }

        static void DrawNoSpaceGraph(Graphics g, XPlotGraphs2d graphsplotter)
        {
            int title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.default_font).Height);
            g.DrawString(graphsplotter.title, graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
            g.DrawString("Not enough space to draw graph", graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin + title_height + graphsplotter.title_upper_margin);
        }

        static void DrawAllDataEqualsGraph(Graphics g, XPlotGraphs2d graphsplotter)
        {
            int title_height = (int)Math.Ceiling(g.MeasureString(graphsplotter.title, graphsplotter.default_font).Height);
            g.DrawString(graphsplotter.title, graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin);
            g.DrawString("No graph to draw: in at least one graph all data have the same value", graphsplotter.default_font, graphsplotter.default_brush, graphsplotter.title_left_margin, graphsplotter.title_upper_margin + title_height + graphsplotter.title_upper_margin);
        }
    }
}

