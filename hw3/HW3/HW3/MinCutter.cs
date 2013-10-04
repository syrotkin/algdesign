using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3
{
    public class MinCutter
    {
        String filePathSmall = @"C:\dev\algdesign\hw3\small.txt"; // answer: 2
        String filePath = @"C:\dev\algdesign\hw3\kargerMinCut.txt"; // answer: 17

        private Dictionary<int, Vertex> idToVertex;
        private List<Edge> allEdges; 

        public void Run()
        {
            List<String[]> lines = ReadFile(filePath);
            int n = lines.Count;
            int numTimes = (int)Math.Floor(n * n * Math.Log(n));
            Console.WriteLine("total number of iterations: {0}", numTimes);
            int min = Int32.MaxValue;

            for (int i = 0; i < numTimes; i++)
            {
                BuildGraph(lines);
                int newMin = DoRandomContraction();
                if (newMin < min) {
                    min = newMin;
                }
                if (i % 1000 == 0)
                {
                    Console.WriteLine("Iteration {0}", i);
                    Console.WriteLine("current total min: " + min);
                }
            }
            Console.WriteLine("Total min: " + min);
        }

        private List<String[]> ReadFile(String path)
        {
            List<String[]> lines = new List<String[]>();
            String line;
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(parts);
                    }
                }
            }
            return lines;
        }

        private void BuildGraph(List<String[]> lines)
        {
            idToVertex = new Dictionary<int, Vertex>();
            allEdges = new List<Edge>();

            foreach (String[] parts in lines)
            {
                Vertex vertex = new Vertex();
                vertex.ID = int.Parse(parts[0]);
                vertex.StringID = vertex.ID.ToString();
                idToVertex.Add(vertex.ID, vertex);
                for (int i = 1; i < parts.Length; i++)
                {
                    int number = int.Parse(parts[i]);
                    if (number > vertex.ID)
                    {  // NOTE: Assumption: all numbers are distinct
                        Edge edge = new Edge();
                        edge.ID1 = vertex.ID;
                        edge.ID2 = number;
                        edge.End1 = vertex;
                        allEdges.Add(edge);
                        vertex.Edges.Add(edge);
                    }
                    else
                    {
                        // ignore the numbers smaller than the given vertex
                    }
                }
            }

            // Some edges are missing a reference to an end because at the time the edge was created, that vertex still did not exist
            foreach (Edge edge in allEdges)
            {
                Vertex vertex;
                if (edge.End1 == null)
                {
                    vertex = idToVertex[edge.ID1];
                    edge.End1 = vertex;
                    vertex.Edges.Add(edge);
                }
                else if (edge.End2 == null)
                {
                    vertex = idToVertex[edge.ID2];
                    edge.End2 = vertex;
                    vertex.Edges.Add(edge);
                }
            }
            //PrintAdjacencyList();
        }

        private int DoRandomContraction()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            while (idToVertex.Count > 2)
            {
                int nextIndex = random.Next(0, allEdges.Count);
                Edge edge = allEdges[nextIndex];
                allEdges.Remove(edge);
                //Console.WriteLine("removing edge {0},{1}", edge.ID1, edge.ID2);

                Vertex vertex1 = edge.End1;
                Vertex vertex2 = edge.End2;
                idToVertex.Remove(vertex1.ID);
                idToVertex.Remove(vertex2.ID);
                Vertex newVertex = new Vertex();
                newVertex.ID = vertex1.ID;
                newVertex.StringID = String.Format("{0},{1}", vertex1.StringID, vertex2.StringID);

                RearrangeEdges(vertex1, edge, newVertex);
                RearrangeEdges(vertex2, edge, newVertex);
                RemoveLoops(vertex1);
                RemoveLoops(vertex2);


                //Console.WriteLine(String.Format("vertex {0}: {1}", vertex1.ID, vertex1.StringID));
                //PrintEdges(vertex1.Edges);
                //Console.WriteLine(String.Format("vertex {0}: {1}", vertex2.ID, vertex2.StringID));
                //PrintEdges(vertex2.Edges);

                newVertex.Edges.AddRange(vertex1.Edges);
                newVertex.Edges.AddRange(vertex2.Edges);
                idToVertex.Add(newVertex.ID, newVertex);

                //Console.WriteLine("--------------------------------------");
                //PrintAdjacencyList();
            }
            return allEdges.Count;

            //PrintAdjacencyList();
        }

        private void RemoveLoops(Vertex vertex)
        {
            //Console.WriteLine("removing loops");
            //Console.WriteLine(String.Format("vertex {0}: {1}", vertex.ID, vertex.StringID));
            for (int i = vertex.Edges.Count - 1; i >= 0; i--)
            {
                Edge vertexEdge = vertex.Edges[i];
                if (vertexEdge.ID1 == vertexEdge.ID2)
                {
                    //Console.WriteLine("REMOVING edge: {0},{1}", vertexEdge.ID1, vertexEdge.ID2);
                    vertex.Edges.Remove(vertexEdge);
                    allEdges.Remove(vertexEdge);
                }
            }
        }

        private void PrintEdges(List<Edge> list)
        {
            foreach (Edge item in list)
            {
                Console.WriteLine("\t{0},{1}", item.ID1, item.ID2);
            }
        }

        // Deletes the edge in question from the vertex's edge list
        // Repoints the other edges to the newly created vertex
        // If both of the edge's endpoints point to the same vertex (have the same id), delete such an edge
        private void RearrangeEdges(Vertex vertex, Edge edge, Vertex newVertex)
        {
            //Console.WriteLine(String.Format("vertex {0}: {1}", vertex.ID, vertex.StringID));
            for (int i = vertex.Edges.Count - 1; i >= 0; i--)
            {
                Edge vertexEdge = vertex.Edges[i];
                //Console.WriteLine("checking edge: {0},{1}", vertexEdge.ID1, vertexEdge.ID2);

                if (vertexEdge == edge)
                {
                    //Console.WriteLine("\tremoving this edge as part of this round");
                    vertex.Edges.Remove(vertexEdge);
                    continue;
                }
                if (vertexEdge.End1 == vertex)
                {
                    vertexEdge.End1 = newVertex;
                    vertexEdge.ID1 = newVertex.ID;
                }
                else if (vertexEdge.End2 == vertex)
                {
                    vertexEdge.End2 = newVertex;
                    vertexEdge.ID2 = newVertex.ID;
                }
                //Console.WriteLine("edge became: {0},{1}", vertexEdge.ID1, vertexEdge.ID2);
            }
        }
        
        private void PrintAdjacencyList()
        {
            Console.WriteLine("Vertices:");
            foreach (KeyValuePair<int, Vertex> item in idToVertex)
            {
                Console.WriteLine(String.Format("{0}: {1}", item.Key, item.Value.StringID));
                foreach (Edge edge in item.Value.Edges)
                {
                    Console.WriteLine(String.Format("\t{0},{1} ", edge.ID1, edge.ID2));
                }
            }

            Console.WriteLine("Edges:");
            foreach (Edge edge in allEdges)
            {
                Console.WriteLine(String.Format("{0},{1}", edge.ID1, edge.ID2));
                Console.WriteLine("\t{0}", edge.End1.ID);
                Console.WriteLine("\t{0}", edge.End2.ID);
            }
        }

    }

    public class Vertex
    {
        public int ID { get; set; }
        public String StringID { get; set;}
        public List<Edge> Edges { get; set; }
        public Vertex()
        {
            Edges = new List<Edge>();
        }

    }

    public class Edge
    {
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        public Vertex End1 { get; set; }
        public Vertex End2 { get; set; }
    }
}
