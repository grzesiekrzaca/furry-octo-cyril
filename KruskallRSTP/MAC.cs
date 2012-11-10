using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class MAC {
        private long first;
        private long second;
        private long third;
        public MAC(long first, long second, long third) {
            this.first = first;
            this.second = second;
            this.third = third;
        }

        public override string ToString() {
            return first.ToString("0000") + ":" + second.ToString("0000") + ":" + third.ToString("0000");
        }
    }
}
