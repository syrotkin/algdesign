using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HW4
{
    public class SCCFinder
    {
        String filePathSmall = @"C:\dev\algdesign\hw4\SCCSmall.txt";
        String filePathSmaller = @"C:\dev\algdesign\hw4\SCCSmaller.txt";
        String filePathBig = @"C:\dev\algdesign\hw4\SCC.txt";
        String filePathMedium = @"C:\dev\algdesign\hw4\SCCMedium.txt";

        Dictionary<int, Vertex> idToVertex;
        int numNodes = 0;

        private List<int[]> ReadFile(String path)
        {
            numNodes = 0; // #vertices
            //int counter = 0;
            String line = null;
            string[] splitter = new string[] { " "};
            List<int[]> edges = new List<int[]>();
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] parts = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                        int[] edge = new int[2];
                        edge[0] = int.Parse(parts[0]);
                        edge[1] = int.Parse(parts[1]);
                        if (edge[0] > numNodes) {
                            numNodes = edge[0];
                        }
                        if (edge[1] > numNodes) {
                            numNodes = edge[1];
                        }
                        edges.Add(edge);
                    }
                }
            }
            Console.WriteLine("#nodes: {0}", numNodes);
            return edges;
        }

        public void Run()
        {
            idToVertex = new Dictionary<int, Vertex>();

            List<int[]> edges = ReadFile(filePathBig); // CHANGE FILE PATH HERE
            //DebugPrint(edges);
            BuildGraph(edges);
            //PrintGraph(false, true);
            finishingTimes = new int[numNodes + 1];
            finishingTimes[0] = -1;
            leaders = new int[numNodes + 1];
            leaders[0] = -1;

            //FindSingleVertices();
            DFSLoop(1);
            //Console.WriteLine("finishing times:");
            //PrintArray(finishingTimes);
            DFSLoop(2);
            //Console.WriteLine("leaders:");
            //PrintArray(leaders);
            PrintSCC(leaders, true);
            Console.WriteLine("done");
        }

        private void FindSingleVertices()
        {
            Console.WriteLine("Single vertices:");
            for (int i = 1; i <= numNodes; i++) {
                if (idToVertex[i].Edges.Count == 0 && idToVertex[i].ReverseEdges.Count == 0) {
                    Console.Write(i + " ");
                }
            }
        }

        private void PrintSCC(int[] leaders, bool onlyCounts)
        {
            int[] maxes = new int[5];
            for (int i = 0; i < 5; i++) {
                maxes[i] = 0;
            }
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            for (int i = 0; i < leaders.Length; i++)
            {
                if (!dict.ContainsKey(leaders[i]))
                {
                    dict[leaders[i]] = new List<int>();
                }
                dict[leaders[i]].Add(i);
            }
            foreach (int key in dict.Keys)
            {
                int count = 0;
                if (!onlyCounts)
                {
                    Console.Write(key + ": ");
                }
                foreach (int value in dict[key])
                {
                    count++;
                    if (!onlyCounts)
                    {
                        Console.Write(value + " ");
                    }
                }
                //PutInArray(count, maxes);

                if (onlyCounts)
                {
                    if (count > 200)
                    {
                        Console.Write(key + ": ");
                        Console.WriteLine("count = {0}", count);
                    }
                }
                if (!onlyCounts)
                {
                    Console.WriteLine();
                }
            }
        }
                

        private void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write("{0}: {1}  ", i, array[i]);
            }
            Console.WriteLine();
        }

        private int t = 0; // finishing times in the 1st pass
        private int s = 0; // leaders in the second pass; instead of keeping reference to vertex, just keep vertex ID

        private int[] finishingTimes;  // the INDEX is the finishing time; the value is the vertex ID
        private int[] leaders; // the index is the vertex ID, the value is the ID of the vertex that is the leader of the current vertex
        private Dictionary<int, bool> explored;
                
        private void DFSLoop(int passNumber) {
            explored = new Dictionary<int,bool>();
            Stack<int> stack = new Stack<int>();
            t = 0;
            s = -1; // 0 is an invalid ID anyway
            for (int i = numNodes; i >= 1; i--) {
                int vertexID = -1;
                if (passNumber == 1)
                {
                    vertexID = i;
                }
                else {
                    vertexID = finishingTimes[i];
                }
                //Console.WriteLine("pass = {0}, exploring {1}", passNumber, vertexID);
                if (!explored.ContainsKey(vertexID)) {
                    s = vertexID;
                    //DFS(vertexID, passNumber, stack);
                    DFSRec(vertexID, passNumber);
                }
            }
        }

        private void DFSRec(int vertexID, int passNumber)
        {
            explored[vertexID] = true;
            //stack.Push(i);
            
            //Console.WriteLine("i = {0}", i);

            if (passNumber == 2)
            {
                leaders[vertexID] = s;
            }
            List<Edge> edges;
            if (passNumber == 1)
            {
                edges = idToVertex[vertexID].ReverseEdges;
            }
            else
            {
                edges = idToVertex[vertexID].Edges;
            }

            foreach (Edge edge in edges)
            {
                if (!explored.ContainsKey(edge.ID2))
                {
                    DFSRec(edge.ID2, passNumber);
                    //DFS(edge.ID2, passNumber);
                    //Console.WriteLine("push {0}", edge.ID2);
                    //stack.Push(edge.ID2);
                    //explored[edge.ID2] = true;
                    //i = edge.ID2;
                }
            }


            //Console.WriteLine("popped: {0}, t = {1}", i, t + 1);
            if (passNumber == 1)
            {
                t++;
                finishingTimes[t] = vertexID; // index is the finishing time. The value is the vertex
            }


        }

        private void DFS(int i, int passNumber, Stack<int> stack)
        {
            explored[i] = true;
            stack.Push(i);
            
            while (stack.Count != 0)
            {
                //Console.WriteLine("i = {0}", i);

                if (passNumber == 2)
                {
                    leaders[i] = s;
                }
                List<Edge> edges;
                if (passNumber == 1)
                {
                    edges = idToVertex[i].ReverseEdges;
                }
                else
                {
                    edges = idToVertex[i].Edges;
                }
                int num = 0;
                foreach (Edge edge in edges)
                {
                    if (!explored.ContainsKey(edge.ID2))
                    {
                        num++;
                        //Console.WriteLine("push {0}", edge.ID2);
                        stack.Push(edge.ID2);
                        explored[edge.ID2] = true;
                        i = edge.ID2;
                    }
                }
                if (num == 0) // NOTE: we are popping only if there are no unexplored nodes left // crazy heuristic
                {
                    i = stack.Pop();
                    //Console.WriteLine("popped: {0}, t = {1}", i, t + 1);
                    if (passNumber == 1)
                    {
                        t++;
                        finishingTimes[t] = i; // index is the finishing time. The value is the vertex
                    }
                }
            }
        }

        private void PrintGraph(bool printEdges, bool printReverseEdges)
        {
            // we know numNodes and we know idToVertex
            for (int i = 1; i <= numNodes; i++) {
                Vertex v = idToVertex[i];
                Console.WriteLine(v.ID);
                if (printEdges)
                {
                    Console.WriteLine("Edges:");
                    PrintEdges(v.Edges);
                }
                if (printReverseEdges)
                {
                    Console.WriteLine("Reverse Edges:");
                    PrintEdges(v.ReverseEdges);
                }
            }
        }

        private void PrintEdges(List<Edge> list)
        {
            for (int j = 0; j < list.Count; j++)
            {
                Edge edge = list[j];
                Console.WriteLine("\t{0} -> {1}", edge.ID1, edge.ID2);
            }
        }

        

        private void BuildGraph(List<int[]> edges)
        {
            for (int i = 0; i < edges.Count; ++i)
            {
                int[] edge = edges[i];
                Edge e1 = new Edge();
                e1.ID1 = edge[0];
                e1.ID2 = edge[1];
                // reverse edge
                Edge e2 = new Edge();
                e2.ID1 = edge[1];
                e2.ID2 = edge[0];

                Vertex v1 = GetVertex(edge[0]);
                v1.Edges.Add(e1);
                Vertex v2 = GetVertex(edge[1]);
                v2.ReverseEdges.Add(e2);
            }
        }

        private Vertex GetVertex(int id)
        {
            Vertex v1;
            if (idToVertex.ContainsKey(id))
            {
                v1 = idToVertex[id];
            }
            else
            {
                v1 = new Vertex();
                v1.ID = id;
                idToVertex[id] = v1;
            }
            return v1;
        }

        // Prints what we read from the file
        private void DebugPrint(List<int[]> edges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                int[] edge = edges[i];
                Console.WriteLine(String.Format("{0} -> {1}", edge[0], edge[1]));
            }        
        }
    }


    public class Vertex {
        public int ID { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Edge> ReverseEdges { get; set; }

        public Vertex() {
            Edges = new List<Edge>();
            ReverseEdges = new List<Edge>();
        }
    }

    public class Edge {
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        // we can store vertices here, but we can also get them from the idToVertex dictionary
    }
}
