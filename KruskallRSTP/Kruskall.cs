using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class Kruskall {

        List<Vertex> vertices;
        List<Edge> edges;

        public Kruskall(List<Bridge> bridges) {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
            List<Tripple<Port, Vertex, bool>> edgeGeneratorList = new List<Tripple<Port, Vertex, bool>>();
            
            //generating vertices
            foreach (Bridge bridge in bridges) {
                Vertex vertex = new Vertex(bridge);
                vertices.Add(vertex);
                foreach (Port port in bridge.ports) {
                    edgeGeneratorList.Add(new Tripple<Port, Vertex, bool>(port,vertex,false));
                }
            }

            //generating edges
            foreach (Tripple<Port, Vertex, bool> tripple in edgeGeneratorList) {
                if (tripple.Third) {
                    continue;
                }
                if (tripple.First.destinationPort == null) {
                    continue;
                }
                Vertex vertex = null;
                foreach (Tripple<Port, Vertex, bool> tripple2 in edgeGeneratorList) {
                    if (tripple2.First.Equals(tripple.First.destinationPort)) {
                        vertex = tripple2.Second;
                        tripple2.Third = true;
                        break;
                    }
                }

                edges.Add(new Edge(tripple.Second, vertex, tripple.First.time));
                tripple.Third = true;
            }
        }

        private class Vertex {
            Bridge bridge;
            Vertex parent;

            public Vertex(Bridge bridge) {
                this.bridge = bridge;
                parent = null;
            }

            public Vertex getRoot() {
                if (parent != null) {
                    return parent.getRoot();
                }
                return this;
            }

            public static void JoinTree(Vertex root1, Vertex root2) {
                root1.parent = root2;
            }
        }

        private class Edge {
            public Vertex v1 { get; private set; }
            public Vertex v2 { get; private set; }
            public int Time { get; private set; }

            public Edge(Vertex v1, Vertex v2, int Time) {
                this.v1 = v1;
                this.v2 = v2;
                this.Time = Time;
            }
        }

        List<Edge> makeKruskall(List<Edge> edges, out int totalTime) {
            edges.Sort();
            List<Edge> forest = new List<Edge>();
            totalTime = 0;
            foreach (Edge edge in edges) {
                Vertex root1 = edge.v1.getRoot();
                Vertex root2 = edge.v2.getRoot();

                if (!root1.Equals(root2)) {
                    totalTime += edge.Time;
                    Vertex.JoinTree(root1, root2);
                    forest.Add(edge);
                }
            }
            return forest;
        }
    }
}
