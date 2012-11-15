using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;

namespace KruskallRSTP {
    class Net {
        private readonly int NUMBER_OF_BRIDGES = 10;
        public List<Bridge> Bridges { get; private set; }

        public Net() {
            Bridges = new List<Bridge>();
            Port port = null;
            for (int i = 0; i < NUMBER_OF_BRIDGES; i++) {
                List<Port> ports = new List<Port>();
                for (int j = 0; j < 10; j++) {
                    if (j == 0) {
                        port = new Port(new MAC(0, i, j), port, 0);
                    } else {
                        port = new Port(new MAC(0, i, j), null, 0);
                    }
                    ports.Add(port);
                }
                Bridge bridge = new Bridge(i, ports);
                Bridges.Add(bridge);
                bridge.PropertyChanged += sc_PropertyChanged;
            }
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("isEnabled")) {
                //ponów kruskala;
            }
        }
    }
}
