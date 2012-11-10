using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class BPDU {
        public int ProtocolIdentifier { get; private set; }
        public int ProtocolVersionIdentifier { get; private set; }
        public int BPDUType { get; private set; }
        public int Flags { get; private set; }
        public int RootIdentifier { get; private set; }
        public int RootPathCost { get; private set; }
        public int BridgeIdentifier { get; private set; }
        public int PortIdentifier { get; private set; }
        public int MessageAge { get; private set; }
        public int MaxAge { get; private set; }
        public int HelloTime { get; private set; }
        public int ForwardDelay { get; private set; }

        public BPDU(int ProtocolIdentifier,
                    int ProtocolVersionIdentifier,
                    int BPDUType,
                    int Flags,
                    int RootIdentifier,
                    int RootPathCost,
                    int BridgeIdentifier,
                    int PortIdentifier,
                    int MessageAge,
                    int MaxAge,
                    int HelloTime,
                    int ForwardDelay) {
            this.ProtocolIdentifier = ProtocolIdentifier;
            this.ProtocolVersionIdentifier = ProtocolVersionIdentifier;
            this.BPDUType = BPDUType;
            this.Flags = Flags;
            this.RootIdentifier = RootIdentifier;
            this.RootPathCost = RootPathCost;
            this.BridgeIdentifier = BridgeIdentifier;
            this.PortIdentifier = PortIdentifier;
            this.MessageAge = MessageAge;
            this.MaxAge = MaxAge;
            this.HelloTime = HelloTime;
            this.ForwardDelay = ForwardDelay;
        }
    }
}
