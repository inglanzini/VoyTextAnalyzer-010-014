using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class ParseHuffmann
    {



        public static void get_Huffmann_codes(ref Dictionary<string, ParseVoynich.Chunk> chunks_dictionary)
        {
            // get_Huffmann_codes aggiunge i codici Huffman ai chunks in chunks_dictionary


            // 1) Viene prima generato l'Huffmann tree (https://en.wikipedia.org/wiki/Huffman_coding). L'algortimo in sè è semplice

            //      All'inizio tutti i chunks sono un nodo leaf dell'albero
            //          Finchè non si arriva alla root:
            //          I due nodi col minor numero di occorrenze vengono riuniti in un nodo dell'albero, che ha occorrenze = somma di quelle dei due precedenti

            // 2) Dopodichè dall'Huffmann tree vengono ricavati i codici


            List<HuffmannNode> Huffmann_queue = new List<HuffmannNode>(); // Lista di tutti i nodi ancora da esaminare

            List<HuffmannNode> Huffmann_tree = new List<HuffmannNode>();  //  Albero (output della parte (1) )



            // Inizializziamo Huffmann_queue
            foreach (ParseVoynich.Chunk chunk in chunks_dictionary.Values)
            {
                HuffmannNode new_node = new HuffmannNode();
                new_node.chunk = chunk.chunk;
                new_node.number_of_times_used = chunk.number_of_times_used;
                Huffmann_queue.Add(new_node);
            }
            // E ordiniamola per number_of_times_used
            IComparer<HuffmannNode> comparer = new CompareHuffmannNode_by_times_used();
            Huffmann_queue.Sort(comparer);


            // 1) generazione dell'Huffmann tree
            while (Huffmann_queue.Count > 1)
            {
                // Get i due elementi alla fine della coda
                HuffmannNode old_node_1st = get_node(Huffmann_queue, Huffmann_queue.Count - 1);
                HuffmannNode old_node_2nd = get_node(Huffmann_queue, Huffmann_queue.Count - 2);

                // Crea il nodo che li riunisce
                HuffmannNode new_node = new HuffmannNode();
                new_node.chunk = "";
                new_node.number_of_times_used = Huffmann_queue[Huffmann_queue.Count - 1].number_of_times_used + Huffmann_queue[Huffmann_queue.Count - 2].number_of_times_used;

                // I due old_nodes vengono poi inseriti in sequenza alla fine del tree
                new_node.child_node_0 = Huffmann_tree.Count;
                new_node.child_node_1 = Huffmann_tree.Count + 1;

                // Inserisci i due elementi appena compattati nell'albero finale. Nel farlo bisogna anche aggiornare il parent_node dei nodi figli
                //   (il puntatore al parent_node lo scopriamo solo qua!)
                Huffmann_tree.Add(old_node_1st);
                update_childrens(old_node_1st, Huffmann_tree.Count - 1, Huffmann_tree);
                Huffmann_tree.Add(old_node_2nd);
                update_childrens(old_node_2nd, Huffmann_tree.Count - 1, Huffmann_tree);

                // E rimuovili dalla coda
                Huffmann_queue.RemoveAt(Huffmann_queue.Count - 1);
                Huffmann_queue.RemoveAt(Huffmann_queue.Count - 1);

                // Adesso inserisci il nuovo nodo nella coda: va inserito in modo da rispettare l'ordinamento!!!!
                insert_in_Huffmann_queue(new_node, ref Huffmann_queue);
            }

            // Adesso nella queue è rimasto solo il nodo root, inseriamo anche quello nell'albero
            HuffmannNode root_node = get_node(Huffmann_queue, 0);
            Huffmann_tree.Add(root_node);
            update_childrens(root_node, Huffmann_tree.Count - 1, Huffmann_tree);


            // 2) Ricostruiamo il codice

            //  Per non ascire pazzo scrivendo anche una routine che attraversa l'albero dalla root... cerco le foglie e poi lo risalgo seguendo i parent_nodes
            List<ParseVoynich.Chunk> coded_chunks = new List<ParseVoynich.Chunk>();
            for (int i = 0; i < Huffmann_tree.Count; i++)
            {
                if (Huffmann_tree[i].chunk != "")
                {
                    // E' una foglia
                    string Huffman_code_reverse = "";
                    int current_node = i;
                    int? parent_node = Huffmann_tree[current_node].parent_node;
                    while (parent_node != null) // Se è null siamo arrivati alla root
                    {
                        if (Huffmann_tree[current_node].parent_is_1 == true)
                        {
                            Huffman_code_reverse += "1";
                        }
                        else
                        {
                            Huffman_code_reverse += "0";
                        }
                        current_node = (int)parent_node;
                        parent_node = Huffmann_tree[current_node].parent_node;
                    }
                    // Ora rovesciamo il codice ottenuto ed è come se lo avessimo letto dalla root
                    string Huffman_code = "";
                    for (int j = Huffman_code_reverse.Length - 1; j >= 0; j--)
                    {
                        Huffman_code += Huffman_code_reverse[j];
                    }
                    Huffmann_tree[i].Huffman_code = Huffman_code;

                    ParseVoynich.Chunk new_coded_chunk = new ParseVoynich.Chunk();
                    new_coded_chunk.chunk = Huffmann_tree[i].chunk;
                    new_coded_chunk.number_of_times_used = Huffmann_tree[i].number_of_times_used;
                    new_coded_chunk.Huffman_code = Huffmann_tree[i].Huffman_code;
                    coded_chunks.Add(new_coded_chunk);
                }
            }

            // Adesso devo prendere gli Huffmann codes dalle leaves del tree (lista coded_chunks) e trasferirli in chunks_dictionary
            foreach (ParseVoynich.Chunk chunk in coded_chunks)
            {

                ParseVoynich.Chunk old_chunk = new ParseVoynich.Chunk();
                if (chunks_dictionary.TryGetValue(chunk.chunk, out old_chunk) == true)
                {
                    old_chunk.Huffman_code = chunk.Huffman_code;
                    chunks_dictionary.Remove(chunk.chunk);
                    chunks_dictionary.Add(chunk.chunk, old_chunk);
                }
                else
                {
                    mdError error = new mdError();
                    error.root("","Problems transcribing Huffmann codes");
                    error.Display_and_Clear();
                    return;
                }

            }

        }


        private static HuffmannNode get_node(List<HuffmannNode> Huffmann_queue, int node_pointer)
        {
            HuffmannNode node = new HuffmannNode();
            // in effetti è un copy constructor...
            node.chunk = Huffmann_queue[node_pointer].chunk;
            node.number_of_times_used = Huffmann_queue[node_pointer].number_of_times_used;
            node.Huffman_code = Huffmann_queue[node_pointer].Huffman_code;
            node.parent_node = Huffmann_queue[node_pointer].parent_node;
            node.parent_is_1 = Huffmann_queue[node_pointer].parent_is_1;
            node.child_node_0 = Huffmann_queue[node_pointer].child_node_0;
            node.child_node_1 = Huffmann_queue[node_pointer].child_node_1;
            return node;
        }

        private static void update_childrens(HuffmannNode parent, int parent_pointer, List<HuffmannNode> Huffmann_tree)
        {
            if (parent.child_node_0 != null)
            {
                Huffmann_tree[(int)parent.child_node_0].parent_node = parent_pointer;
                Huffmann_tree[(int)parent.child_node_0].parent_is_1 = false;             // Questo flag mi semplifica la vita quando si tratta di determinare i codici dall'albero!
            }
            if (parent.child_node_1 != null)
            {
                Huffmann_tree[(int)parent.child_node_1].parent_node = parent_pointer;
                Huffmann_tree[(int)parent.child_node_1].parent_is_1 = true;
            }
        }

        private static void insert_in_Huffmann_queue(HuffmannNode new_node, ref List<HuffmannNode> Huffmann_queue)
        {
            // Inserimento nella queue rispettando l'ordinamento
            int insertion_point = Huffmann_queue.Count - 1;
            while (insertion_point >= 0)
            {
                if (Huffmann_queue[insertion_point].number_of_times_used >= new_node.number_of_times_used)
                {
                    break;
                }
                insertion_point--;
            }

            // La queue va shiftata in alto di 1 a partire da insertion_point + 1
            HuffmannNode dummy_node = new HuffmannNode(); // e per farlo ci serve un elemento in più!
            Huffmann_queue.Add(dummy_node);
            for (int i = Huffmann_queue.Count - 1; i > insertion_point + 1; i--)
            {
                HuffmannNode source_node = get_node(Huffmann_queue, i - 1);
                Huffmann_queue[i] = source_node;            
            }

            // E il nuovo elemento va in insertion_point + 1
            Huffmann_queue[insertion_point + 1] = new_node;        
        }



        public class CompareHuffmannNode_by_times_used : IComparer<HuffmannNode>
        {
            public int Compare(HuffmannNode x, HuffmannNode y)
            {
                if (x.number_of_times_used == y.number_of_times_used) return (0);

                if (x.number_of_times_used < y.number_of_times_used)
                {
                    return (1);
                }
                return (-1);
            }
        }






        public class HuffmannNode
        {
            public string chunk;                // Se non è null il nodo è una leaf
            public long number_of_times_used;

            // Variabili per codifica Huffman
            public string Huffman_code = "";    // Risultato

            public int? parent_node;             // Variabile gestionale per il calcolo dell'Huffman tree
            public bool parent_is_1;            // Mi evita di dover poi scandire l'albero partendo dalla root per trovar i codici

            public int? child_node_0;            // Variabili gestionali coi pointers ai nodi figli
            public int? child_node_1;

        }





    }
}
