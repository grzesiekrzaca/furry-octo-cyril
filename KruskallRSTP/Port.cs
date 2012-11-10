using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class Port {
        public MAC mac { get; private set; }
        public Port destinationPort { get; private set; }
        public int Time { get; private set; }

        private Queue<BPDU> bpdus;

        public Port(MAC mac, Port destinationPort, int time) {
            this.mac = mac;
            this.destinationPort = destinationPort;
            if (destinationPort != null) {
                this.Time = time;
            } else {
                this.Time = int.MaxValue;
            }

            bpdus = new Queue<BPDU>();
        }

        private void receiveBPDU(BPDU bpdu) {
            bpdus.Enqueue(bpdu);
        }

        public void sendBPDU(BPDU bpdu) {
            if (destinationPort != null) {
                destinationPort.receiveBPDU(bpdu);
            }
        }

        public BPDU getBPDU() {
            return bpdus.Dequeue();
        }

    }
}
