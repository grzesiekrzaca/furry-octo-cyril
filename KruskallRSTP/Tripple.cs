using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KruskallRSTP {
    class Tripple<T, U, V> {
        private Tripple() {
        }

        public Tripple(T first, U second, V third) {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        public T First { get; set; }
        public U Second { get; set; }
        public V Third { get; set; }
    }
}
