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
