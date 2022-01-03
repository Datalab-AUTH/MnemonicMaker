using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core
public class BFS
{
    class Graph<T>
    {
        public Graph() { }
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }
    }

    class Algorithms
    {
        public HashSet<T> BFS<T>(Graph<T> graph, T start, Action<T> preVisit = null)
        {
            var visited = new HashSet<T>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                if (preVisit != null)
                    preVisit(vertex);

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return visited;
        }

        public Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v => {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }

    Graph<int> graph;
    Algorithms algorithms;

    public BFS(IEnumerable<int> vertices, IEnumerable<Tuple<int, int>> edges)
    {
        graph = new Graph<int>(vertices, edges);
        algorithms = new Algorithms();
    }

    public List<int> findPath(int startVertex, int endVertex)
    {
        var shortestPath = algorithms.ShortestPathFunction(graph, startVertex);
        return (List<int>)shortestPath(endVertex);
    }

}
