using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Template
{
    // VEDERE documento Word "Documentazione XPlotClusters.docx" in TextAnalyzer 010 015/Sorgenti/Bin/Debug per capire come funzionano i clusters!!!
    // VEDERE documento Word "Documentazione XPlotClusters.docx" in TextAnalyzer 010 015/Sorgenti/Bin/Debug per capire come funzionano i clusters!!!
    public class XPlotClusters
    {

        public Color background_color = Color.Black;

        // Parametri del titolo globale del grafico
        public string title = "Title";

        public int title_upper_margin = 2;                  // in pixels
        public int title_left_margin = 2;                   // in pixels

        public string title_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float title_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color title_color = Color.White;


        public bool optimize_graph_area = false;

        // Parametri che determinano la posizione dell'area in cui viene scritto il grafico dei clusters
        public int title_to_graphs_upper_margin = 4;
        public int graphs_left_margin = 4;



        // Parametri dei clusters

        public int cluster_border_line_witdh = 3;

        public int cluster_elements_upper_margin = 8;
        public int cluster_elements_left_margin = 8;

        public int hor_space_between_numbers = 5;

        public string numbers_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float numbers_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color numbers_color = Color.White;




        // Parametri della scala colori
        public int color_scale_heigth = 24;           // altezza riservata per scala + numeri
        public float scale_percentage = 0.6f;         // quanto la scala dei colori è rimpicciolita rispetto all'area del grafico

        public int max_color_scale_line_height = 8;   // max altezza della barra colorata  

        public string color_scale_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float color_scale_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color color_scale_numbers_color = Color.White;


        // Definizione dei colori della scala, che è a tre colori (estremo negativo, centro, estremo positivo)
        public Color graph_color_low_extreme = Color.Green;
        public Color graph_color_center = Color.Yellow;
        public Color graph_color_high_extreme = Color.Red;


        // Parametri della finestra dati MouseHover
        public int mousehover_data_upper_margin = 5;
        public int mousehover_data_left_margin = 5;

        public int mousehover_data_border_line_width = 3;

        public string mousehover_font_name = Form1.DefaultFont.Name;     // Esempio: "Microsoft Sans Serif"
        public float mousehover_font_size = 8.25F;                       // in 'punti tipografici', 8.25 è il valore di DefaultFont
        public Color mosehover_color = Color.White;




        public bool debug_show_duplicated_clusters = true; // VECCHIA GESTIONE DA ELIMINARE!!!


        // Dati ricavati durante la scrittura dei grafici (tutti private!)
        //   OCCHIO A NON UTILIZZARLI PRIMA DI AVERLI GESTITI..... E' UN'IMPOSTAZIONE RISCHIOSA (SAREBBERO DOVUTI ESSERE TUTTI 'NULLABLE' PER SICUREZZA MI SA)
        //   vengono comodi perche' così si può passare tutto alle sottoroutines tramite graphsplotters, ma dato che sono pericolosi ne ho usati meno che potevo
        //   (anche se mi è toccato passare più parametri alle sottoroutines)

        // Inizializzazioni dummy di fonts, brushes etc.
        private Font numbers_font = new Font(Form1.DefaultFont.Name, 8.25F);
        private Brush numbers_brush = new SolidBrush(Color.White);
        private Font mousehover_font = new Font(Form1.DefaultFont.Name, 8.25F);



        // Struttura principale in cui sono contenuti i dati del grafico
        private static List<VisualCluster> graph_data = new List<VisualCluster>();     

        // Dal punto di vista logico max_ e min_distance fanno parte di graph_data, ma per non complicarmi ulteriormente la vita le definisco qua
        private static float max_distance;
        private static float min_distance;


        // Punto di inizio del grafico dei clusters
        private static Point clusters_starting_point;

        public static Bitmap DisplayGraphNew(Size panel_size, List<ClusterNew> cluster_data, XPlotClusters graph)
        {

            graph_data = convert_clusters_to_graphNew(cluster_data, graph);
            
            // determiniamo le dimensioni dell'immagine finale
            clusters_starting_point = new Point(5, 5);      // PER ORA!!!!!!!!!!!!!!!!!!

            int min_image_width = clusters_starting_point.X + graph_data[0].bounding_box.Width + 5;   // PER ORA!!! NOTARE CHE EVENTUALI NODI ROOT USEREBBERO LA SIZE DEL PRIMO
            int min_image_height = clusters_starting_point.Y + graph_data[0].bounding_box.Height + 5; //   (E IN display_graph_clusters VENGONO SOVRAPPOSTI AL PRIMO)

            int width = Math.Max(min_image_width, panel_size.Width);     // Se sono inferiori a quelle del Panel espandi l'immagine
            int height = Math.Max(min_image_height, panel_size.Height);

            Bitmap image = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(image);
            g.PageUnit = GraphicsUnit.Pixel;        // Mah.. di default PageUnit è settata a Display, sembra non cambi niente se si toglie questa istruzione, ma va a sapere..

            g.Clear(graph.background_color);

            graph.numbers_font = new Font(graph.numbers_font_name, graph.numbers_font_size);
            graph.numbers_brush = new SolidBrush(graph.numbers_color);


            display_graph_clusters(graph_data, clusters_starting_point, graph, g);

            g.Dispose();
            return image;
        }




        public static Bitmap DisplayGraph(Size image_size, List<VisualCluster> graph_data, XPlotClusters graph)
        {
            Bitmap image = new Bitmap(image_size.Width, image_size.Height);

            Graphics g = Graphics.FromImage(image);
            g.PageUnit = GraphicsUnit.Pixel;        // Mah.. di default PageUnit è settata a Display, sembra non cambi niente se si toglie questa istruzione, ma va a sapere..

            g.Clear(graph.background_color);

            graph.numbers_font = new Font(graph.numbers_font_name, graph.numbers_font_size);
            graph.numbers_brush = new SolidBrush(graph.numbers_color);

            Point starting_point = new Point (5, 5);      // PER ORA!!!!!!!!!!!!!!!!!!
            display_graph_clusters(graph_data, starting_point, graph, g);

            g.Dispose();
            return image;
        }

        private static void display_graph_clusters(List<VisualCluster> graph_data, Point starting_point, XPlotClusters graph, Graphics g)
        {
            Point this_starting_point = new Point(starting_point.X, starting_point.Y);  // OCCHIO: E CI FOSSE PIU' DI UN CLUSTER ROOT VERREBBERO SOVRAPPOSTI TUTTI....
            foreach (VisualCluster root_cluster in graph_data)
            {
                display_cluster(root_cluster, this_starting_point, graph, g);
            }
        }

        private static void display_cluster(VisualCluster cluster, Point starting_point, XPlotClusters graph, Graphics g)
        {

            // Bordo colorato

            Color rectangle_color = get_color_gradient(cluster.distance, min_distance, max_distance, graph);
            Pen colored_rectangle_pen = new Pen(rectangle_color, (float)graph.cluster_border_line_witdh);
            g.DrawRectangle(colored_rectangle_pen, starting_point.X , starting_point.Y , cluster.bounding_box.Width, cluster.bounding_box.Height);

            // Numeri
            int number_counter = 0;
            foreach (int number in cluster.numbers_list)
            {
                g.DrawString((number+1).ToString(), graph.numbers_font, graph.numbers_brush, starting_point.X + cluster.numbers_positions[number_counter].X, starting_point.Y + cluster.numbers_positions[number_counter].Y);

                number_counter++;
            }



            int child_cluster_counter = 0;
            foreach (VisualCluster child_cluster in cluster.child_clusters)
            {
                Point this_starting_point = new Point(starting_point.X + cluster.child_clusters_positions[child_cluster_counter].X, starting_point.Y + cluster.child_clusters_positions[child_cluster_counter].Y);
                display_cluster(child_cluster, this_starting_point, graph, g);

                child_cluster_counter++;
            }
        }


        // Routine simile a quella di XPlotGraphs2d
        private static Color get_color_gradient(float value, float min_value, float max_value, XPlotClusters gplot)
        {

            if (max_value == min_value)
            {
                return gplot.graph_color_low_extreme;
            }

            // Calcoliamo la scala sull'asse Z, cioè lungo la sequenza dei colori. La scala viene calcolata in modo tale che DUE
            //    sequenze di colori coprano l'intero range max_value - min_value
            float z_scale = 1;
            float absolute_range = max_value - min_value;
            z_scale = 2 * Math.Abs(1 / absolute_range);

            Color color_low_scale = gplot.graph_color_low_extreme;
            Color color_high_scale = gplot.graph_color_center;

            if (value < min_value) value = min_value;  // safeties
            if (value > max_value) value = max_value;

            // qui i dati pssono essere solo positivi! (in XPlotGraphs2d ci sono tre casi differenti
            float color_scale_factor = 1;
            float midpoint = min_value + (max_value - min_value) / 2;
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
                color_scale_factor = (value - min_value) * z_scale;
            }

            if (color_scale_factor < 0) color_scale_factor = 0;  // Valori negativi possono causare eccezioni quando si crea l'out_color

            int deltacolor_R = color_high_scale.R - color_low_scale.R;
            int deltacolor_G = color_high_scale.G - color_low_scale.G;
            int deltacolor_B = color_high_scale.B - color_low_scale.B;

            Color out_color = Color.FromArgb(255, color_low_scale.R + (int)((float)deltacolor_R * color_scale_factor), color_low_scale.G + (int)((float)deltacolor_G * color_scale_factor), color_low_scale.B + (int)((float)deltacolor_B * color_scale_factor));

            return out_color;
        }





        // VEDERE documento Word "Documentazione XPlotClusters.docx" in TextAnalyzer 010 015/Sorgenti/Bin/Debug per capire come funzionano i clusters!!!
        private static List<VisualCluster> convert_clusters_to_graphNew(List<ClusterNew> source, XPlotClusters graph)
        {
            List<VisualCluster> result = new List<VisualCluster>();

            max_distance = float.MinValue;   // Serviranno molto più tardi... per la scala colori
            min_distance = float.MaxValue;

            foreach (ClusterNew root_cluster in source)
            {
                VisualCluster visual_cluster = get_visual_cluster(root_cluster, graph);
                result.Add(visual_cluster);
            }

            return result;
        }

        private static VisualCluster get_visual_cluster(ClusterNew cluster, XPlotClusters graph)
        {
            //  PARTE RICORSIVA
            List<VisualCluster> children_visual_clusters = new List<VisualCluster>();
            foreach (ClusterNew child_cluster in cluster.child_clusters)
            {
                VisualCluster child_visual_cluster = get_visual_cluster(child_cluster, graph);
                children_visual_clusters.Add(child_visual_cluster);
            }
            // FINE PARTE RICORSIVA!!



            VisualCluster result = new VisualCluster();

            result.distance = cluster.max_intra_cluster_distance;
            if (result.distance < min_distance) min_distance = result.distance;
            if (result.distance > max_distance) max_distance = result.distance;


            // Childrens
            result.child_clusters = new List<VisualCluster>();
            foreach (VisualCluster child_visual_cluster in children_visual_clusters)
            {
                result.child_clusters.Add(child_visual_cluster);
            }

            // Numeri
            result.numbers_list = new List<int>();
            foreach (int number in cluster.numbers_list)
            {
                result.numbers_list.Add(number);
            }

            // Dati aggiuntivi
            result.link_number_1st = cluster.link_number_1st;
            result.link_number_2nd = cluster.link_number_2nd;
            result.link_distance = cluster.link_distance;


            // Alla fine di tutto possiamo determinare la colored box E le posizioni in cui scrivere tutti i numeri E la posizione in cui scrivere il cluster child
            result.child_clusters_positions = new List<Point>();
            result.numbers_positions = new List<Point>();
            result.bounding_box = get_colored_box(result.numbers_list, ref result.numbers_positions,
                                                                        ref result.child_clusters, ref result.child_clusters_positions, graph);

            return result;
        }






        // VEDERE documento Word "Documentazione XPlotClusters.docx" in TextAnalyzer 010 015/Sorgenti/Bin/Debug per capire come funzionano i clusters!!!
        public static List<VisualCluster> convert_clusters_to_graph(List<ClusteringResults> source, XPlotClusters graph)
        {
            List<VisualCluster> result = new List<VisualCluster>();

            max_distance = float.MinValue;   // Serviranno molto più tardi... per la scala colori
            min_distance = float.MaxValue;

            int layer_counter = 0;      // Solo per visualizzazione nella status window!
            foreach (ClusteringResults layer in source)
            {
                string message = "Graph layer " + (layer_counter + 1) + "...";
                // Form1.mainForm.Invoke((Form1.delegate_newline_to_mainStatuswindow)Form1.newline_to_mainStatusWindow, new object[] { message });

                // In questo foreach 'results' viene passato a find_if_extension. Non sarebbe affatto prudente modificare results all'intero del foreach...
                //  Per questo motivo qua viene creata una lista delle modifiche da apportare a results, che verrà applicata subito dopo
                List<Modification> modifications = new List<Modification>();
                int cluster_counter = 0;
                foreach (ClusterGroup cluster in layer.clusters)
                {


                    //  find_if_estension può ritornare:

                    //          _new: il cluster è nuovo (va inserito nella lista dei clusters)
                    //        _equal: il cluster è identico a uno che c'è già, è ridondante e quindi viene scartato
                    //     _includes: il cluster estende (include) uno o più clusters già presenti, i cui indici vengono ritornati nella lista included_clusters_list

                    List<int> included_clusters_list = new List<int>();
                    ClusterIncludes modification_type = find_if_extension(cluster, result, ref included_clusters_list);

                    if (modification_type != ClusterIncludes._equal)
                    {
                        // casi _includes e _new
                        Modification modification = new Modification();
                        modification.modification_type = (int)modification_type; 
                        modification.cluster_number = cluster_counter;
                        modification.cluster = cluster;
                        modification.is_extension_of = included_clusters_list;
                        modifications.Add(modification);
                    }
                    
                    cluster_counter++;
                }

                result = apply_modifications(modifications, result, graph);

                layer_counter++;  // Solo per il messaggio nella status_window
            }
        
            return result;
        }


        private static ClusterIncludes find_if_extension(ClusterGroup cluster, List<VisualCluster> previous_result, ref List<int> clusters_index)
        {

            // Vediamo se i numeri in old_cluster sono contenuti in cluster

            //  Il caso _equal dovrebbe potersi verificare una volta sola (cioè: 'credo' che al massimo si possa trovare solo un cluster 'equal'),
            //      quindi si può uscire subito

            //  Invece posso avere _includes multipli: questi li devo inserire nella lista clusters_index in modo da poterli poi gestire tutti assieme

            int old_cluster_number = 0;
            bool includes_old_clusters = false;
            foreach (VisualCluster old_cluster in previous_result)
            {

                switch (check_if_contains(cluster, old_cluster))
                {
                    case ClusterIncludes._new:
                        break;
                    case ClusterIncludes._equal:
                        return ClusterIncludes._equal;
                    case ClusterIncludes._includes:
                        includes_old_clusters = true;
                        clusters_index.Add(old_cluster_number);
                        break;
                    default:
                        // Error
                        break;
                }

                old_cluster_number++;   
            }

            if (includes_old_clusters == true)
            {
                return ClusterIncludes._includes;
            }

            return ClusterIncludes._new;
        }

        // Routine che controlla se new_cluster include tutti i numeri di old_cluster
        public enum ClusterIncludes
        {
            _new,
            _equal,
            _includes        
        }
        private static ClusterIncludes check_if_contains(ClusterGroup new_cluster, VisualCluster old_cluster)
        {
            bool is_contained = true;
            foreach (int number in old_cluster.assigned_numbers.Values)
            {
                if (new_cluster.group_members.ContainsKey(number) == false)
                {
                    is_contained = false;
                    break;
                }
            }

            if (is_contained == true)
            {
                // Se arriviamo qua tutti i numeri di old_cluster sono contenuti in new_cluster

                // I clusters sono identici?
                if (new_cluster.group_members.Count == old_cluster.assigned_numbers.Count)
                {
                    return ClusterIncludes._equal;
                }
                else
                {
                    return ClusterIncludes._includes;
                }
            }
            return ClusterIncludes._new;
        }



        private static List<VisualCluster> apply_modifications(List<Modification> modifications, List<VisualCluster> old_result, XPlotClusters graph)
        {
            List<VisualCluster> result = new List<VisualCluster>();

            Dictionary<int, int> old_clusters_extended_list = new Dictionary<int, int>();

            foreach (Modification modification in modifications)
            {
                // Quando si arriva qua ci sono due casi:
                //   1) Il cluster è nuovo
                //   2) Il cluster è l'estensione di uno o più clusters precedenti
                if (modification.modification_type == (int)ClusterIncludes._new)
                {
                    // Nuovo cluster
                    create_new_cluster(modification, ref result, graph);
                }
                else
                {
                    // Estensione di un cluster precedente

                    // Qua la gestione dei layers può provocare un effetto collaterale: uno stesso cluster viene esteso da due (o più) clusters diversi al'interno dello
                    //    stesso layer, ma su distanze diverse: questo porterebbe alla duplicazione del cluster che viene esteso, che verrebbe messo come child
                    //    ad entrambi i clusters che lo estendono. Non è affatto facile spiegare la cosa, per questo motivo c'è un flag che controlla se i clusters
                    //    duplicati vengono accettati tutti, o se si visualizza solo quello a distanza inferiore (i clusters eliminati rientreranno comunque in gioco
                    //    nei layers successivi, quindi non si perdono informazioni).

                    // Noatre che i clusters sono già ordinati per distanza, quindi vengono scartati quelli con distanze maggiori
                    bool keep_this_cluster = true;
                    if (graph.debug_show_duplicated_clusters == false)
                    {
                        // Almeno uno dei clusters estesi è già stato esteso?
                        foreach (int cluster_number in modification.is_extension_of)
                        {
                            if (old_clusters_extended_list.ContainsKey(cluster_number) == true)
                            {
                                keep_this_cluster = false;
                            }
                        }

                    }

                    if (keep_this_cluster == true)
                    {

                        extend_cluster(modification, old_result, ref result, graph);

                        // Annotiamo nella lista quali clusters sono stati estesi (e quindi rimossi dal result, ma inseriti come children)
                        foreach (int extended_cluster_index in modification.is_extension_of)
                        {
                            if (old_clusters_extended_list.ContainsKey(extended_cluster_index) == false)
                            {
                                old_clusters_extended_list.Add(extended_cluster_index, extended_cluster_index);
                            }
                        }
                    }

                }
            }


            // Adesso dobbiamo trasferire in results tutti i clusters di old_results che NON sono stati processati da extend_cluster
            int cluster_counter = 0;
            foreach (VisualCluster old_cluster in old_result)
            {
                if (old_clusters_extended_list.ContainsKey(cluster_counter) == false)
                {
                    result.Add(old_cluster);
                }
                cluster_counter++;
            }

            return result;
        }

        private static void create_new_cluster(Modification modification, ref List<VisualCluster> result, XPlotClusters graph)
        {
            VisualCluster new_visual_cluster = new VisualCluster();

            new_visual_cluster.numbers_list = new List<int>();
            new_visual_cluster.assigned_numbers = new Dictionary<int, int>();
            foreach (int number in modification.cluster.group_members.Values)
            {
                new_visual_cluster.numbers_list.Add(number);
                new_visual_cluster.assigned_numbers.Add(number, number);
            }
            // ordinamento della lista dei numeri per migliorare la visualizzazione
            new_visual_cluster.numbers_list.Sort();


            // trova la colored box E le posizioni in cui scrivere tutti i numeri
            new_visual_cluster.numbers_positions = new List<Point>();
            new_visual_cluster.bounding_box = get_colored_box(new_visual_cluster.numbers_list, ref new_visual_cluster.numbers_positions, graph);

            new_visual_cluster.distance = modification.cluster.max_intra_cluster_distance;
            if (new_visual_cluster.distance < min_distance) min_distance = new_visual_cluster.distance;
            if (new_visual_cluster.distance > max_distance) max_distance = new_visual_cluster.distance;

            // tutte queste liste sono vuote con un nuovo cluster
            new_visual_cluster.child_clusters = new List<VisualCluster>();
            new_visual_cluster.child_clusters_positions = new List<Point>();
        
            result.Add(new_visual_cluster);
        }

        private static void extend_cluster(Modification modification, List<VisualCluster> old_result, ref List<VisualCluster> result, XPlotClusters graph)
        {

            // Creo il nuovo cluster, col cluster che viene esteso inserito come child
            VisualCluster new_visual_cluster = new VisualCluster();

            // nella number_list del cluster padre dobbiamo inserire solo i numeri che NON compaiono in ALCUNO dei clusters figli
            new_visual_cluster.numbers_list = new List<int>();
            new_visual_cluster.assigned_numbers = new Dictionary<int, int>();
            foreach (int number in modification.cluster.group_members.Values)
            {
                // per ogni numero del cluster attuale:

                bool already_in_children = false;
                foreach (int child_cluster_number in modification.is_extension_of)
                {
                    foreach (int child_number in old_result[child_cluster_number].assigned_numbers.Values)
                    {
                        if (child_number == number)
                        {
                            already_in_children = true;
                            break;
                        }
                    }
                    if (already_in_children == true) break;
                }
                if (already_in_children == false)
                {
                    new_visual_cluster.numbers_list.Add(number);
                }

                // In assigned_numbers, invece, vanno inseriti sempre tutti i numeri!
                new_visual_cluster.assigned_numbers.Add(number, number);
            }
            // ordinamento della lista dei numeri per migliorare la visualizzazione
            new_visual_cluster.numbers_list.Sort();

            new_visual_cluster.distance = modification.cluster.max_intra_cluster_distance;
            if (new_visual_cluster.distance < min_distance) min_distance = new_visual_cluster.distance;
            if (new_visual_cluster.distance > max_distance) max_distance = new_visual_cluster.distance;

            // Inserimento dei clusters che vengono estesi come children del cluster attuale
            new_visual_cluster.child_clusters = new List<VisualCluster>();
            foreach (int child_cluster_number in modification.is_extension_of)
            {
                VisualCluster extended_cluster = old_result[child_cluster_number];
                new_visual_cluster.child_clusters.Add(extended_cluster);
            }



            // Alla fine di tutto possiamo determinare la colored box E le posizioni in cui scrivere tutti i numeri E la posizione in cui scrivere il cluster child
            new_visual_cluster.child_clusters_positions = new List<Point>();
            new_visual_cluster.numbers_positions = new List<Point>();
            new_visual_cluster.bounding_box = get_colored_box(new_visual_cluster.numbers_list, ref new_visual_cluster.numbers_positions,
                                                                        ref new_visual_cluster.child_clusters, ref new_visual_cluster.child_clusters_positions, graph);

            result.Add(new_visual_cluster);
        }






        private static Size get_colored_box(List<int> numbers_list, ref List<Point> numbers_positions, XPlotClusters graph)
        {

            List<VisualCluster> dummy_child_clusters = new List<VisualCluster>();
            List<Point> dummy_child_clusters_positions = new List<Point>();

            return get_colored_box(numbers_list, ref numbers_positions, ref dummy_child_clusters, ref dummy_child_clusters_positions, graph);
        }

        private static Size get_colored_box(List<int> numbers_list, ref List<Point> numbers_positions, ref List<VisualCluster> child_clusters, ref List<Point> child_clusters_positions, XPlotClusters graph)
        {

            // 1) Creo una lista delle bounding boxes da piazzare (notare che parto da dati ordinati per distanza, quindi anche le bounding_boxes sono
            //      ordinate per distanza)

            // 2) Setto come 'punto disponibile per il piazzamento' l'angolino in alto a sinistra del cluster principale

            // 3) Per ogni bounding box in lista:
            //       3a) Cerco fra i vari 'punti disponibili' la posizione migliore in cui mettere la bounding box in modo da minimizzare le dimensioni totali
            //              E senza sovrapporla ad alcuna altra bounding box (il che è il casino principale, mi sa)
            //       3b) La piazzo e aggiorno la lista dei 'punti disponibili'

            // 4) Poi vedo dove scrivere i numeri cercando anche qua di minimizzare le dimensioni


            if (graph.optimize_graph_area == false)
            {
                // In questo caso è meglio ordinare i clusters fra di loro per dimensione [NOTA: gestione provata solo coi grafici che hanno al max due clusters children!]
                IComparer<VisualCluster> comparer = new CompareVisualCluster_bounding_box_SizeByWidth_descending();
                child_clusters.Sort(comparer);
            }



            //  1) Creazione lista delle bounding boxes dei clusters figli
            List<Size> children_bounding_boxes = new List<Size>();
            foreach (VisualCluster cluster in child_clusters)
            {
                children_bounding_boxes.Add(cluster.bounding_box);
            }


            // 2) settatura del primo 'punto disponibile'
            List<Point> available_points = new List<Point>();
            Point new_point = new Point();
            new_point.X = graph.cluster_border_line_witdh + graph.cluster_elements_left_margin;
            new_point.Y = graph.cluster_border_line_witdh + graph.cluster_elements_upper_margin;
            available_points.Add(new_point);

            Size parent_bounding_box = new Size(0, 0);
            foreach (Size bounding_box in children_bounding_boxes)
            {
                // 3) Per ogni bounding box in lista: posizionala e aggiorna mano a mano la parent_bounding_box
                parent_bounding_box = find_best_position(bounding_box, ref available_points, ref child_clusters_positions, parent_bounding_box, graph);
            }


            // 4) Inserimento dei numeri
            parent_bounding_box = find_numbers_positions(numbers_list, ref available_points, ref numbers_positions, parent_bounding_box, graph);


            return parent_bounding_box;
        }

        private static Size find_best_position(Size bounding_box, ref List<Point> available_points, ref List<Point> child_clusters_positions, Size parent_bounding_box, XPlotClusters graph)
        {
            Size result_bounding_box = new Size(parent_bounding_box.Width, parent_bounding_box.Height);

            // 3a) Proviamo tutti i piazzamenti e scegliamo il migliore
            int insertion_point_index = find_best_placement(bounding_box, available_points, parent_bounding_box, ref result_bounding_box, graph);


            // 3b) Piazziamo il child
            Point insertion_point = available_points[insertion_point_index];
            child_clusters_positions.Add(insertion_point);

            // 3b) Aggiornamento lista punti disponibili

            //  Rimozione di quello che è appena stato usato
            available_points.RemoveAt(insertion_point_index);

            //  Inserimento dei due nuovi points agli estremi della bounding_box appena inserita (a destra e in basso)
            Point new_point = new Point();
            new_point.X = insertion_point.X + bounding_box.Width + graph.cluster_elements_left_margin + graph.cluster_border_line_witdh;
            new_point.Y = insertion_point.Y;
            available_points.Add(new_point);

            new_point.X = insertion_point.X;
            new_point.Y = insertion_point.Y + bounding_box.Height + graph.cluster_elements_upper_margin + graph.cluster_border_line_witdh;
            available_points.Add(new_point);

            return result_bounding_box;
        }

        private static int find_best_placement(Size bounding_box, List<Point> available_points, Size parent_bounding_box, ref Size result_bounding_box, XPlotClusters graph)
        {
            // 3a) Proviamo tutti i piazzamenti
            List<Size> resulting_sizes = new List<Size>();
            foreach (Point placement_point in available_points)
            {
                Size this_size = new Size(parent_bounding_box.Width, parent_bounding_box.Height);

                int width_after_adding_box = placement_point.X + bounding_box.Width + graph.cluster_elements_left_margin + graph.cluster_border_line_witdh;
                if (width_after_adding_box > this_size.Width)
                {
                    this_size.Width = width_after_adding_box;
                }

                int height_after_adding_box = placement_point.Y + bounding_box.Height + graph.cluster_elements_upper_margin + graph.cluster_border_line_witdh;
                if (height_after_adding_box > this_size.Height)
                {
                    this_size.Height = height_after_adding_box;
                }

                resulting_sizes.Add(this_size);
            }

            // 3b) Scegliamo il piazzamento migliore
            //   ANCHE QUA CI SARA' IL PROBLEMA DI DOVER EVITARE SOVRAPPOSIZIONI DI BOUNDING BOXES!!!!
            int point_counter = 0;
            int insertion_point_index = 0; // dummy
            result_bounding_box = new Size(int.MaxValue, int.MaxValue);
            foreach (Size this_size in resulting_sizes)
            {
                if (graph.optimize_graph_area == false && point_counter != 0) break; // Viene scelto sempre il punto di indice zero (in alto a destra)

                int old_max_dimension = Math.Max(result_bounding_box.Width, result_bounding_box.Height);
                int this_max_dimension = Math.Max(this_size.Width, this_size.Height);
                if (this_max_dimension <= old_max_dimension)
                {
                    if (this_max_dimension == old_max_dimension)
                    {
                        // Caso più complicato (si verifica di rado): scegliamo la bounding box con somma dei lati minore
                        int old_semiperimeter = result_bounding_box.Width + result_bounding_box.Height;
                        int new_semiperimeter = this_size.Width + this_size.Height;
                        if (new_semiperimeter < old_semiperimeter)
                        {
                            insertion_point_index = point_counter;
                            result_bounding_box = this_size;
                        }
                    }
                    else
                    {
                        insertion_point_index = point_counter;
                        result_bounding_box = this_size;
                    }

                }
                point_counter++;
            }

            return insertion_point_index;
        }


        private static Size find_numbers_positions(List<int> numbers_list, ref List<Point> available_points, ref List<Point> numbers_positions, Size parent_bounding_box, XPlotClusters graph)
        {

            if (numbers_list.Count == 0)
            {
                return parent_bounding_box;
            }

            // Qua il problema è che posso disporre i numeri in tanti modi.. da N*1 fino a 1*N, cioè in N bounding boxes diverse in totale. Il max per ottimizzare il grafico
            //    sarebbe provarle tutte in tutti i possibili available_points... oh vabbè, proviamoci.

            // Misuriamo larghezza ed altezza dei numeri
            Bitmap dummy = new Bitmap(1,1);
            Graphics g = Graphics.FromImage(dummy);
            Font numbers_font = new Font(graph.numbers_font_name, graph.numbers_font_size);

            float numbers_height = g.MeasureString(numbers_list[0].ToString(), numbers_font).Height;
            float max_numbers_width = float.MinValue;
            foreach (int number in numbers_list)
            {
                float this_width = g.MeasureString((number + 1).ToString(), numbers_font).Width;  // il numero verrà scritto + 1 !
                if (this_width > max_numbers_width) max_numbers_width = this_width;
            }

            // Creazione della lista di bounding_boxes, da N*1 a 1*N
            List<Size> numbers_xy_boxes = new List<Size>();
            for (int x_size = 0; x_size < numbers_list.Count; x_size++)
            {
                int y_size = (int)Math.Ceiling((double)numbers_list.Count / (double)(x_size + 1));
                Size new_size = new Size((x_size + 1), y_size);
                numbers_xy_boxes.Add(new_size);
            }

            // Conversione delle box 'xy' in bounding boxes reali
            List<Size> numbers_bounding_boxes = new List<Size>();
            foreach (Size xy_box in numbers_xy_boxes)
            {
                Size new_bounding_box = new Size();
                new_bounding_box.Height = xy_box.Height * (int)Math.Ceiling((double)numbers_height);
                new_bounding_box.Width = xy_box.Width * (int)Math.Ceiling((double)max_numbers_width) + (xy_box.Width - 1 ) * graph.hor_space_between_numbers;

                numbers_bounding_boxes.Add(new_bounding_box);
            }


            // Scegliamo la migliore bounding_box, nella posizione migliore
            int bounding_box_index = 0;
            int chosen_bounding_box_index = 0; // dummy
            int chosen_bounding_box_placement_point_index = 0; // dummy
            Size result_bounding_box = new Size(int.MaxValue, int.MaxValue);
            foreach (Size bounding_box in numbers_bounding_boxes)
            {
                if (graph.optimize_graph_area == false && chosen_bounding_box_placement_point_index != 0) break;

                Size dummy_bounding_box = new Size();
                int this_box_best_placement_index = find_best_placement(bounding_box, available_points, parent_bounding_box, ref dummy_bounding_box, graph);

                Size this_box_result_bounding_box = get_result_bounding_box(parent_bounding_box, available_points, this_box_best_placement_index, numbers_bounding_boxes, bounding_box_index, graph);

                int old_max_dimension = Math.Max(result_bounding_box.Width, result_bounding_box.Height);
                int this_max_dimension = Math.Max(this_box_result_bounding_box.Width, this_box_result_bounding_box.Height);
                if (this_max_dimension <= old_max_dimension)
                {
                    if (this_max_dimension == old_max_dimension)
                    {
                        // Caso più complicato (si verifica di rado): scegliamo la bounding box con somma dei lati minore
                        int old_semiperimeter = result_bounding_box.Width + result_bounding_box.Height;
                        int new_semiperimeter = this_box_result_bounding_box.Width + this_box_result_bounding_box.Height;
                        if (new_semiperimeter < old_semiperimeter)
                        {
                            chosen_bounding_box_index = bounding_box_index;
                            chosen_bounding_box_placement_point_index = this_box_best_placement_index;
                            result_bounding_box = this_box_result_bounding_box;
                        }
                    }
                    else
                    {
                        // Caso semplice e più frequente: scegliamo direttamente la bounding box con dimensione max minore
                        chosen_bounding_box_index = bounding_box_index;
                        chosen_bounding_box_placement_point_index = this_box_best_placement_index;
                        result_bounding_box = this_box_result_bounding_box;
                    }
                }

                bounding_box_index++;
            }



            // QUA BISOGNEREBBE TOGLIERE L'AVAILABLE_POINT UTILIZZATO, ED AGGIUNGERE  I DUE POINTS SUCCESSIVI, QUESTO PER UNIFORMITA' COL RESTO DELLE GESTIONI
            //    E PER EVENTUALI ESPANSIONI FUTURE


            // E finalmente calcoliamo i punti in cui piazzare i numeri
            int current_x_position = available_points[chosen_bounding_box_placement_point_index].X;
            int x_counter = 0;
            int current_y_position = available_points[chosen_bounding_box_placement_point_index].Y;
            for (int i = 0; i < numbers_list.Count; i++)
            {
                Point this_number_position = new Point();
                this_number_position.X = current_x_position;
                this_number_position.Y = current_y_position;                
                numbers_positions.Add(this_number_position);

                x_counter++;
                if (x_counter >= numbers_xy_boxes[chosen_bounding_box_index].Width)
                {
                    x_counter = 0;
                    current_x_position = available_points[chosen_bounding_box_placement_point_index].X;
                    current_y_position += (int)Math.Ceiling((double)numbers_height);
                }
                else
                {
                    current_x_position +=  (int)Math.Ceiling((double)max_numbers_width) + graph.hor_space_between_numbers;
                }
            }


            return result_bounding_box;
        }

        private static Size get_result_bounding_box(Size parent_bounding_box, List<Point> available_points, int chosen_bounding_box_placement_point_index, List<Size> numbers_bounding_boxes, int chosen_bounding_box_index, XPlotClusters graph)
        {
            Size result_bounding_box = new Size(parent_bounding_box.Width, parent_bounding_box.Height);

            int width_after_adding_box = available_points[chosen_bounding_box_placement_point_index].X + numbers_bounding_boxes[chosen_bounding_box_index].Width + graph.cluster_elements_left_margin + graph.cluster_border_line_witdh;
            if (width_after_adding_box > result_bounding_box.Width)
            {
                result_bounding_box.Width = width_after_adding_box;
            }

            int height_after_adding_box = available_points[chosen_bounding_box_placement_point_index].Y + numbers_bounding_boxes[chosen_bounding_box_index].Height + graph.cluster_elements_upper_margin + graph.cluster_border_line_witdh;
            if (height_after_adding_box > result_bounding_box.Height)
            {
                result_bounding_box.Height = height_after_adding_box;
            }

            return result_bounding_box;
        }







        // Routine grafico interattivo (display in un'immagine separata del cluster puntato dal mouse (eventi MouseHover))
        public static Bitmap DisplayMouseCluster(Size image_size, Point mouse_position, XPlotClusters graph)
        {
            Bitmap image = new Bitmap(image_size.Width, image_size.Height);

            if (graph_data.Count == 0)
            {
                return image;
            }

            VisualCluster cluster = get_cluster_from_mouse(mouse_position);


            Graphics g = Graphics.FromImage(image);
            g.Clear(graph.background_color);


            if (cluster == null)
            {
                g.DrawString("No cluster to show", graph.numbers_font, graph.numbers_brush, graph.mousehover_data_left_margin, graph.mousehover_data_upper_margin);
                g.Dispose();
                return image;
            }

            graph.mousehover_font = new Font(graph.mousehover_font_name, graph.mousehover_font_size);

            // ATTENZIONE! La visualizzazione è concepita solo per i casi 1) due clusters children, 2) un child e un number, 3) due numbers
            if (cluster.child_clusters != null)
            {
                if (cluster.child_clusters.Count == 2 || cluster.child_clusters.Count == 1)
                {
                    display_two_or_one_children(cluster, graph, g);
                }
                if (cluster.child_clusters.Count == 0)             
                {
                    if (cluster.numbers_list != null)
                    {
                        display_two_numbers(cluster, graph, g);
                    }
                }
            }

            g.Dispose();
            return image;
        }

        private static VisualCluster get_cluster_from_mouse(Point mouse_position)
        {
            VisualCluster cluster = new VisualCluster();

            Point current_cluster_position = new Point(clusters_starting_point.X, clusters_starting_point.Y);
            foreach (VisualCluster root_cluster in graph_data)
            {
                get_cluster_from_coords(root_cluster, current_cluster_position, mouse_position, ref cluster);
            }

            return (cluster);
        }

        private static void get_cluster_from_coords(VisualCluster source_cluster, Point source_cluster_position, Point mouse_position, ref VisualCluster out_cluster)
        {

            if (mouse_position.X >= source_cluster_position.X && mouse_position.X <= (source_cluster_position.X + source_cluster.bounding_box.Width) &&
                mouse_position.Y >= source_cluster_position.Y && mouse_position.Y <= (source_cluster_position.Y + source_cluster.bounding_box.Height)    )
            {
                // mouse_position è all'interno della bounding box
                out_cluster = source_cluster;
            }
            else
            {
                return;
            }


            int child_clusters_counter = 0;
            foreach (VisualCluster child_cluster in source_cluster.child_clusters)
            {
                Point child_cluster_position = new Point(source_cluster_position.X + source_cluster.child_clusters_positions[child_clusters_counter].X,
                                                         source_cluster_position.Y + source_cluster.child_clusters_positions[child_clusters_counter].Y);

                get_cluster_from_coords(child_cluster, child_cluster_position, mouse_position, ref out_cluster);                

                child_clusters_counter++;
            }
        
        }




        private static void display_two_or_one_children(VisualCluster cluster, XPlotClusters graph, Graphics g)
        {
            // Devo visualizzare:
            //  Rettangolo esterno con la sua distanza intra-cluster nell'angolo in alto a sinistra
            //  I due rettangoli dei clusters figli, ognuno contenente la sua distanza intra_cluster, OPPURE un rettangolo + un numero
            //  Una riga che indica i due numbers che collegano i children, con la distanza fra questi due elementi

            int max_distance_width = (int)Math.Ceiling(g.MeasureString(cluster.child_clusters[0].distance.ToString(), graph.mousehover_font).Width);
            if (cluster.child_clusters.Count == 2)
            {
                int distance_child_2_length = (int)Math.Ceiling(g.MeasureString(cluster.child_clusters[1].distance.ToString(), graph.mousehover_font).Width);
                if (distance_child_2_length > max_distance_width) max_distance_width = distance_child_2_length;
            }
            int distance_height = (int)Math.Ceiling(g.MeasureString(cluster.child_clusters[0].distance.ToString(), graph.mousehover_font).Height);

            // Dimensioni dei rettangoli interni
            Size inner_rectangles_size = new Size(0, 0);
            inner_rectangles_size.Width = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_left_margin + max_distance_width;
            inner_rectangles_size.Height = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_upper_margin + distance_height;

            // Dimensione rettangolo esterno
            Size outer_rectangle_size = new Size(0, 0);
            outer_rectangle_size.Width = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_left_margin + 2 * inner_rectangles_size.Width + graph.mousehover_data_left_margin;
            outer_rectangle_size.Height = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_upper_margin + distance_height + inner_rectangles_size.Height
                                            + graph.mousehover_data_upper_margin + distance_height;

            // Scrittura rettangolo esterno e della sua distanza
            Point starting_point = new Point(graph.mousehover_data_left_margin, graph.mousehover_data_upper_margin);
            Color this_color = get_color_gradient(cluster.distance, min_distance, max_distance, graph);
            Pen this_pen = new Pen(this_color, graph.mousehover_data_border_line_width);
            Brush this_brush = new SolidBrush(this_color);
            g.DrawRectangle(this_pen, starting_point.X, starting_point.Y, outer_rectangle_size.Width, outer_rectangle_size.Height);
            starting_point.X += graph.mousehover_data_border_line_width;
            starting_point.Y += graph.mousehover_data_border_line_width;
            g.DrawString(cluster.distance.ToString(), graph.mousehover_font, this_brush, starting_point.X, starting_point.Y);


            // Scrittura primo rettangolo child con la sua distanza
            this_color = get_color_gradient(cluster.child_clusters[0].distance, min_distance, max_distance, graph);
            this_pen = new Pen(this_color, graph.mousehover_data_border_line_width);
            this_brush = new SolidBrush(this_color);
            starting_point.X += graph.mousehover_data_left_margin;
            starting_point.Y += graph.mousehover_data_upper_margin + distance_height;
            g.DrawRectangle(this_pen, starting_point.X, starting_point.Y, inner_rectangles_size.Width, inner_rectangles_size.Height);
            g.DrawString(cluster.child_clusters[0].distance.ToString(), graph.mousehover_font, this_brush, starting_point.X + graph.mousehover_data_border_line_width, starting_point.Y + graph.mousehover_data_border_line_width);

            if (cluster.child_clusters.Count == 2)
            {
                // Scrittura secondo rettangolo child con la sua distanza
                this_color = get_color_gradient(cluster.child_clusters[1].distance, min_distance, max_distance, graph);
                this_pen = new Pen(this_color, graph.mousehover_data_border_line_width);
                this_brush = new SolidBrush(this_color);
                starting_point.X += inner_rectangles_size.Width + graph.mousehover_data_left_margin;
                g.DrawRectangle(this_pen, starting_point.X, starting_point.Y, inner_rectangles_size.Width, inner_rectangles_size.Height);
                g.DrawString(cluster.child_clusters[1].distance.ToString(), graph.mousehover_font, this_brush, starting_point.X + graph.mousehover_data_border_line_width, starting_point.Y + graph.mousehover_data_border_line_width);
            }
            else
            {
                // Scrittura del numero isolato
                this_color = Color.White;
                this_brush = new SolidBrush(this_color);
                starting_point.X += inner_rectangles_size.Width + graph.mousehover_data_left_margin;
                g.DrawString((cluster.numbers_list[0] + 1).ToString(), graph.mousehover_font, this_brush, starting_point.X + graph.mousehover_data_border_line_width, starting_point.Y + graph.mousehover_data_border_line_width);
            }


            // Scrittura della stringa che indica quali elementi collegano i due children fra di loro
            this_color = get_color_gradient(cluster.link_distance, min_distance, max_distance, graph);
            this_brush = new SolidBrush(this_color);
            starting_point.X -= inner_rectangles_size.Width + graph.mousehover_data_left_margin;  // torniamo a 'inizio riga'
            starting_point.Y += inner_rectangles_size.Height + graph.mousehover_data_upper_margin;
            string link_message = (cluster.link_number_1st + 1) + " <-" + cluster.link_distance + "-> " + (cluster.link_number_2nd + 1);
            g.DrawString(link_message, graph.mousehover_font, this_brush, starting_point.X, starting_point.Y);

        }

        private static void display_two_numbers(VisualCluster cluster, XPlotClusters graph, Graphics g)
        {
            // Qua viene visualizzato solo il rettangolo esterno + la distanza intra-cluster
            int max_distance_width = (int)Math.Ceiling(g.MeasureString(cluster.distance.ToString(), graph.mousehover_font).Width);
            int distance_height = (int)Math.Ceiling(g.MeasureString(cluster.distance.ToString(), graph.mousehover_font).Height);

            // Dimensione rettangolo esterno
            Size outer_rectangle_size = new Size(0, 0);
            outer_rectangle_size.Width = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_left_margin + max_distance_width;
            outer_rectangle_size.Height = 2 * graph.mousehover_data_border_line_width + 2 * graph.mousehover_data_upper_margin + distance_height;

            // Scrittura rettangolo esterno e della sua distanza
            Point starting_point = new Point(graph.mousehover_data_left_margin, graph.mousehover_data_upper_margin);
            Color this_color = get_color_gradient(cluster.distance, min_distance, max_distance, graph);
            Pen this_pen = new Pen(this_color, graph.mousehover_data_border_line_width);
            Brush this_brush = new SolidBrush(this_color);
            g.DrawRectangle(this_pen, starting_point.X, starting_point.Y, outer_rectangle_size.Width, outer_rectangle_size.Height);
            starting_point.X += graph.mousehover_data_border_line_width;
            starting_point.Y += graph.mousehover_data_border_line_width;
            g.DrawString(cluster.distance.ToString(), graph.mousehover_font, this_brush, starting_point.X, starting_point.Y);
        }


        public class CompareVisualCluster_bounding_box_SizeByWidth_descending : IComparer<VisualCluster>
        {
            public int Compare(VisualCluster x, VisualCluster y)
            {
                if (x.bounding_box.Width == y.bounding_box.Width) return (0);

                if (x.bounding_box.Width < y.bounding_box.Width)
                {
                    return (1);
                }
                return (-1);
            }
        }




    }



    // VEDERE documento Word "Documentazione XPlotClusters.docx" in TextAnalyzer 010 015/Sorgenti/Bin/Debug per capire come funzionano i clusters!!!
    public class VisualCluster
    {
        public Size bounding_box;
        public float distance;

        public List<int> numbers_list;
        public List<Point> numbers_positions;
        public Dictionary<int, int> overlapping_clusters;

        public List<VisualCluster> child_clusters;
        public List<Point> child_clusters_positions;

        // Dictionary di tutti i numeri assegnati a questo cluster: necessario per poter gestire la conversione da clustering_results in un modo umano..
        public Dictionary<int, int> assigned_numbers;

        // Dati accessori (usati poi dalla gestione MouseHover per visualizzare informazioni aggiuntive)
        public int link_number_1st;
        public int link_number_2nd;
        public float link_distance;

    }



    // Classe 'di lavoro' all'interno di convert_clusters_to_graph
    public class Modification
    {
        public int modification_type;

        public int cluster_number;
        public ClusterGroup cluster;

        public List<int> is_extension_of;
    }


}
