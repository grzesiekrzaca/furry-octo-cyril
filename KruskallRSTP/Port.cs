﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KruskallRSTP {
    class Port : INotifyPropertyChanged {
        public MAC mac { get; private set; }

        private Port _destinationPort;
        public Port destinationPort{
            get {
                return _destinationPort;
            }
            set {
                if (_destinationPort == value) {
                    return;
                }
                if (_destinationPort != null) {
                    _destinationPort._destinationPort = null;
                    _destinationPort.time = int.MaxValue;
                }
                _destinationPort = value;
                if (_destinationPort != null) {
                    if (_destinationPort._destinationPort != null) {
                        _destinationPort._destinationPort._destinationPort = null;
                        _destinationPort._destinationPort.time = int.MaxValue;
                    }
                    _destinationPort._destinationPort = this;
                }
            }
        }

        public int time { get; private set; }

        private bool _isEnabled;
        public bool isEnabled {
            get {
                return _isEnabled;
            }
            set {
                if (_isEnabled == value) {
                    return;
                }

                _isEnabled = value;
                if (destinationPort != null) {
                    destinationPort.isEnabled = value;
                }
                SendPropertyChanged("isEnabled");
            }
        }
        
        private Queue<BPDU> bpdus;

        public Port(MAC mac, Port destinationPort, int time) {
            this.mac = mac;
            this.destinationPort = destinationPort;
            if (destinationPort != null) {
                this.time = time;
                destinationPort.destinationPort = this;
                destinationPort.time = time;
                isEnabled = true;
            } else {
                this.time = int.MaxValue;
                isEnabled = false;
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
