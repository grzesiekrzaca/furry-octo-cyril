using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KruskallRSTP {
    class Kruskall {

        List<Vertex> vertices;
        List<Edge> edges;

        private static String TAG = "Kruskall";

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
                Tripple<Port, Vertex, bool> tripple3 = null;
                foreach (Tripple<Port, Vertex, bool> tripple2 in edgeGeneratorList) {
                    if (tripple2.First.Equals(tripple.First.destinationPort)) {
                        tripple3 = tripple2;
                        tripple2.Third = true;
                        break;
                    }
                }
                //to jest porzebne przy starcie bo może się okazać że krawędź idzie w wyłącozny bridge
                if (tripple3 == null) {
                    continue;
                }

                edges.Add(new Edge(tripple, tripple3, tripple.First.time));
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

            public override string ToString()
            {
                return bridge.bridgeId;
            }
        }

        private class Edge : IComparable{
            public Vertex v1 { get; private set; }
            public Vertex v2 { get; private set; }
            
            private Port p1;
            private Port p2;
            
            public int Time { get; private set; }
            
            public bool isEnabled{
                set {
                    p1.isEnabled = value;
                    p2.isEnabled = value; //just in case
                }
            }

            public Edge(Tripple<Port, Vertex, bool> t1, Tripple<Port, Vertex, bool> t2, int Time) {
                this.v1 = t1.Second;
                this.v2 = t2.Second;
                this.p1 = t1.First;
                this.p2 = t2.First;
                this.Time = Time;
            }

            public int CompareTo(object obj) {
                if (!(obj is Edge)) {
                    throw new ArgumentException(
                       "An Edge object is required for comparison.");
                }
                return Time - ((Edge)obj).Time;
            }

        }

        public int makeKruskall() {
            Log.i(TAG, "Start Kruskall algorithm");
            int totalTime = 0;
            List<Edge> treeEdges = makeKruskall(edges, out totalTime);
            foreach (Edge edge in edges) {
                if(treeEdges.Contains(edge)){
                    edge.isEnabled = true;
                } else {
                    edge.isEnabled = false;
                }
            }
            Log.i(TAG, "Finished Kruskall algorithm with total tree weight= "+totalTime.ToString());
            return totalTime;
        }

        private List<Edge> makeKruskall(List<Edge> edges, out int totalTime) {
            edges.Sort();
            List<Edge> forest = new List<Edge>();
            totalTime = 0;
            foreach (Edge edge in edges) {
                Vertex root1 = edge.v1.getRoot();
                Vertex root2 = edge.v2.getRoot();

                if (!root1.Equals(root2)) {
                    totalTime += edge.Time;
                    Vertex.JoinTree(root1, root2);
                    Log.i(TAG, "Joining " + root1.ToString() + " and " + root2.ToString());
                    forest.Add(edge);
                }
            }
            return forest;
        }
    }
}
