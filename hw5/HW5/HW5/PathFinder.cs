using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HW5
{
    /// <summary>
    /// The assumption is that the graph is undirected. When creating the edges, create only the ones where the vertices are sorted in ascending order, 
    /// to prevent creating the same edge twice
    /// 
    /// The answer is: 2599,2610,2947,2052,2367,2399,2029,2442,2505,3068
    /// That is:
    /// 7: 2599
    ///37: 2610
    ///59: 2947
    ///82: 2052
    ///99: 2367
    ///115: 2399
    ///133: 2029
    ///165: 2442
    ///188: 2505
    ///197: 3068
    /// </summary>
    public class PathFinder
    {
        string filePath = @"C:\dev\algdesign\hw5\dijkstraData.txt"; // n = 200, m = 1867
        //string filePathSmall = @"C:\dev\algdesign\hw5\dijkstraSmall.txt";  // n = 4, m = 5;
        Dictionary<int, Vertex> allVertices;
        Dictionary<string, Edge> allEdges;
        
        public void Run()
        {
            ReadFile(filePath);
            //PrintGraph();
            Dictionary<int, int> distances = Dijkstra(1);
            // This was the question. Print distances for the vertices with the following indices
            PrintDistances(distances, new int[] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 });
        }

        // Run Dijkstra's algorithm starting with the vertex sourceID.
        private Dictionary<int, int> Dijkstra(int sourceID) {
            // X -- processed vertices
            Dictionary<int, bool> processedVertices = new Dictionary<int, bool>();
            processedVertices[sourceID] = true;
            // A -- distances for each vertex
            Dictionary<int, int> distances = new Dictionary<int, int>();
            distances[sourceID] = 0;
            // B --> actual paths --> will not use, it is not required
            while (processedVertices.Count != allVertices.Count) {
                int minGreedy = 1000 * 1000;
                Edge newEdge = null;
                int oldVertexID = -1;
                int newVertexID = -1;

                int numEdges = 0; // helper, just for statistics
                foreach (int processedVertexID in processedVertices.Keys)
                {
                    foreach (Edge edge in allVertices[processedVertexID].Edges)
                    {
                        ++numEdges;
                        if ((edge.ID1 == processedVertexID && !processedVertices.ContainsKey(edge.ID2)) ||   // edges where one end is processed and the other one is not
                            (edge.ID2 == processedVertexID && !processedVertices.ContainsKey(edge.ID1)))
                        {
                            if (distances[processedVertexID] + edge.Length < minGreedy)
                            {
                                minGreedy = distances[processedVertexID] + edge.Length;
                                newEdge = edge;
                                oldVertexID = processedVertexID;

                                if (processedVertexID == edge.ID1)
                                {
                                    newVertexID = edge.ID2;
                                }
                                else
                                {
                                    newVertexID = edge.ID1;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("{0} edges", numEdges);

                // 1) add unprocessed end of edge to processedVertices
                processedVertices[newVertexID] = true;
                // 2) A[w] = A[v] + l_vw
                distances[newVertexID] = distances[oldVertexID] + newEdge.Length;
            }
            return distances;
        }
               
        // Read file and build the graph
        private void ReadFile(String path)
        {
            allVertices = new Dictionary<int, Vertex>();
            allEdges = new Dictionary<string, Edge>();
            char[] splitter = new char[] {'\t'};
            String line = null;
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                        //Console.WriteLine("{0}: {1}", parts[0], parts.Length);

                        Vertex vertex = new Vertex();
                        int currentVertexID = int.Parse(parts[0]);                        
                        vertex.ID = currentVertexID;
                        allVertices[currentVertexID] = vertex;
                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] edgeParts = parts[i].Split(',');
                            int adjacentVertexID = int.Parse(edgeParts[0]);
                            int edgeLength = int.Parse(edgeParts[1]);
                            Edge edge = null;
                            if (currentVertexID < adjacentVertexID)
                            {
                                edge = new Edge();
                                edge.ID1 = currentVertexID;
                                edge.ID2 = adjacentVertexID;
                                edge.Length = edgeLength;
                                allEdges.Add(string.Format("{0}_{1}", currentVertexID, adjacentVertexID), edge);
                            }
                            else
                            {
                                string edgeID = string.Format("{0}_{1}", adjacentVertexID, currentVertexID);
                                edge = allEdges[edgeID];
                            }
                            vertex.Edges.Add(edge);
                        }
                    }
                }
            }
        }

        private void PrintGraph()
        {
            Console.WriteLine("Total vertices: {0}", allVertices.Count);
            Console.WriteLine("Total edges: {0}", allEdges.Count);

            foreach (int vertexID in allVertices.Keys)
            {
                Console.WriteLine("{0}: ", vertexID);
                foreach (Edge edge in allVertices[vertexID].Edges)
                {
                    Console.Write("\t{0},{1}:{2}", edge.ID1, edge.ID2, edge.Length);
                }
                Console.WriteLine();
            }
        }

        private void PrintDistances(Dictionary<int, int> distances)
        {
            foreach (KeyValuePair<int, int> pair in distances)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

        }

        private void PrintDistances(Dictionary<int, int> distances, int[] indices)
        {
            foreach (int index in indices)
            {
                //Console.WriteLine("{0}: {1}", index, distances[index]);
                Console.Write("{0},", distances[index]);
            }
            Console.WriteLine();
        }
    }

    public class Vertex
    {
        public Vertex() {
            Edges = new List<Edge>();
        }

        public int ID { get; set; }
        public List<Edge> Edges { get; set; }
    }

    public class Edge
    {
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        public int Length { get; set; }
    }

}
