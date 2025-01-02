using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{

    public class ClusterNew
    {
        public float max_intra_cluster_distance;

        public List<int> numbers_list;

        public List<ClusterNew> child_clusters;

        // Dictionary di tutti i numeri assegnati a questo cluster: necessario per poter effettuare le gestioni in modo umano
        public Dictionary<int, int> assigned_numbers;

        // Dati accessori (usati poi dalla gestione MouseHover di XPlotClusters per visualizzare informazioni aggiuntive)
        public int link_number_1st;
        public int link_number_2nd;
        public float link_distance;

    }




    public class ClusteringResults
    {
        public float limit_distance;    // Distanza limite sulla base della quale è stato calcolato questo clustering

        public List<ClusterGroup> clusters = new List<ClusterGroup>();      // Lista contenente la struttura di tutti i clusters

        public Dictionary<int, AssignedText> texts_assignements = new Dictionary<int, AssignedText>();  // Struttura duale della precedente: indica a quali cluster sono assegnati i testi
                                                                                                        //  La chiave è il numero del testo
    }


    public class ClusterGroup
    {
        public Dictionary<int, int> group_members = new Dictionary<int, int>();  // meglio un Dictionary che una list, per velocità di esecuzione
        public float max_intra_cluster_distance;

        public List<int> overlapping_clusters = new List<int>();       
    }

    public class AssignedText
    {
        public List<int> assigned_clusters_list = new List<int>();
    }


    // Questa classe è simile a EValue_2d, ma gli 'elements' sono i numeri dei testi, non delle stringhe
    public class RelativeDistance
    {
        public float distance;

        public int text_1st;
        public int text_2nd; 
    
    }

}
