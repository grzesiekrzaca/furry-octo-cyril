using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace KruskallRSTP {
    class Net {
        private readonly int NUMBER_OF_BRIDGES = 10;
        public List<Bridge> bridges { get; private set; }

        /**
         * 
         * make random net
         * don't do it at home!
         * 
         */
        public Net() {
            Random random = new Random();
            bridges = new List<Bridge>();
            Port port = null;
            for (int i = 0; i < NUMBER_OF_BRIDGES; i++) {
                List<Port> ports = new List<Port>();
                for (int j = 0; j < 10; j++) {
                    if (j == 0) {
                        port = new Port(new MAC(0, i, j), port, 0);
                    } else {
                        port = new Port(new MAC(0, i, j), null, 0);
                    }
                    bool kozaa =  (random.Next(0,2) == 1);
                    port.isEnabled = kozaa;
                    ports.Add(port);
                }
                Bridge bridge = new Bridge("Andzia"+i.ToString(), i, i, ports);
                bridges.Add(bridge);
                bridge.PropertyChanged += sc_PropertyChanged;
            }
        }

        public Net(XmlDocument xmlDocument) {
            bridges = new List<Bridge>();
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("isEnabled")) {
                //ponów kruskala;
            }
        }
    }
}
