using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class Net {
        private readonly int NUMBER_OF_BRIDGES = 20;
        public List<Bridge> Bridges { get; private set; }

        public Net(){
            Bridges = new List<Bridge>();
            for (int i = 0; i < NUMBER_OF_BRIDGES; i++) {
                List<Port> ports = new List<Port>();
                for (int j = 0; j < 10; j++) {
                    ports.Add(new Port(new MAC(0, i, j), null, 0));
                }
                Bridges.Add(new Bridge(i,ports));
                
            }
        }
    }
}
