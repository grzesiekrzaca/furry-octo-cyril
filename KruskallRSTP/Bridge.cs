﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KruskallRSTP {
    class Bridge : INotifyPropertyChanged {
        public String bridgeId {get; private set;}
        public double xPosition { get; private set; }
        public double yPosition { get; private set; }
        public int priority { get; private set; }

        private int rootId;
        private int rootCost = 0;

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
        public List<Port> ports { get; set; }

        public Bridge(String bridgeId, double xPosition, double yPosition, List<Port> ports) {
            this.ports = ports;
            this.bridgeId = bridgeId;
            this.ports = ports;
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.isEnabled = false;
        }

        public Bridge(String bridgeId, int priority,double xPosition, double yPosition, List<Port> ports)
        {
            this.priority = priority;
            this.rootId = priority;
            this.bridgeId = bridgeId;
            this.ports = ports;
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.isEnabled = false;
        }

        public void readOnPorts()
        {
            foreach (Port port in ports)
            {
                BPDU bpdu = port.getBPDU();
                int distance = port.time;
                if (bpdu.RootIdentifier < priority)
                {
                    rootId = bpdu.RootIdentifier;
                    rootCost = bpdu.RootPathCost + port.time;
                    //znajdx stary root port i zmien go na blocking
                    foreach (Port oldport in ports)
                        if (oldport.state == Port.State.Root)
                            oldport.state = Port.State.Blocking;
                    //ustanw nowego rooat
                    port.state = Port.State.Root;
                } else if (bpdu.RootIdentifier == priority)
                {
                    if (bpdu.RootPathCost > rootCost)
                        port.state = Port.State.Blocking;
                    if (bpdu.RootPathCost < priority)
                    {
                        //znajdx stary root port i zmien go na blocking
                        foreach (Port oldport in ports)
                            if (oldport.state == Port.State.Root)
                                oldport.state = Port.State.Blocking;
                        //ustanw nowego rooat
                        port.state = Port.State.Root;
                    }
                }

            }
        }

        public void sendToPorts()
        {
            foreach (Port port in ports)
            {
                port.sendBPDU(new BPDU(0, 0, 0, 0, rootId, rootCost, priority, 0, 0, 0, 0, 0));
            }
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
