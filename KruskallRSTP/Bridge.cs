using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KruskallRSTP {
    class Bridge : INotifyPropertyChanged {
        public String bridgeId {get; private set;}
        public double xPosition { get; private set; }
        public double yPosition { get; private set; }
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

        public Bridge(String bridgeId, double xPosition, double yPostition, List<Port> ports) {
            this.ports = ports;
            this.bridgeId = bridgeId;
            this.ports = ports;
            this.xPosition = xPosition;
            this.yPosition = yPosition;
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
