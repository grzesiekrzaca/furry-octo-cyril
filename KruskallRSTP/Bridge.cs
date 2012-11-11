using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KruskallRSTP {
    class Bridge : INotifyPropertyChanged {
        public int bridgeId {get; private set;}
        private bool _isEnabled;
        public bool isEnabled {
            get{
                return _isEnabled;
            }
            set{
                if (_isEnabled == value) {
                    return;
                }

                _isEnabled = value;
                SendPropertyChanged("isEnabled"); 
            } 
        }
        public List<Port> ports { get; private set; }

        public Bridge(int bridgeId, List<Port> ports) {
            this.ports = ports;
            this.bridgeId = bridgeId;
            this.ports = ports;
            this.isEnabled = false;
        }

        private class TreeVertex {
            Bridge bridge;
            TreeVertex parent;

            public TreeVertex(Bridge bridge) {
                this.bridge = bridge;
                parent = null;
            }

            public TreeVertex getRoot() {
                if (parent != null) {
                    return parent.getRoot();
                }
                return this;
            }

            public static void JoinTree(TreeVertex root1, TreeVertex root2) {
                root1.parent = root2;
            }
        }

        private class Edge {
            public TreeVertex v1 { get; private set; }
            public TreeVertex v2 { get; private set; }
            public int Time { get; private set; }

            public Edge(TreeVertex v1, TreeVertex v2, int Time) {
                this.v1 = v1;
                this.v2 = v2;
                this.Time = Time;
            }
        }

        List<Edge> Kruskall(List<Edge> edges, out int totalTime) {
            edges.Sort();
            List<Edge> forest = new List<Edge>();
            totalTime = 0;
            foreach (Edge edge in edges) {
                TreeVertex root1 = edge.v1.getRoot();
                TreeVertex root2 = edge.v2.getRoot();

                if (!root1.Equals(root2)) {
                    totalTime += edge.Time;
                    TreeVertex.JoinTree(root1, root2);
                    forest.Add(edge);
                }
            }
            return forest;
        }

        private void SendPropertyChanged(string property) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
