using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace KruskallRSTP {
    class RSTPMarker {
        private static int SIZE = 10;
        private static int OFFSET = 10;
        private DynamicEllipse ellipse1;
        private Port port1;
        private DynamicEllipse ellipse2;
        private Port port2;
        private bool isEnabled;
        public Ellipse marker1 { get; set; }
        public Ellipse marker2 { get; set; }
        private double _X1;
        private double _Y1;
        private double _X2;
        private double _Y2;

        public double X1 {
            get {
                return _X1;
            }
            set {
                _X1 = value > 0 ? value : 0;
                Canvas.SetLeft(marker1, getPosition(true).X);
                Canvas.SetLeft(marker2, getPosition(false).X);
            }
        }

        public double Y1 {
            get {
                return _Y1;
            }
            set {
                _Y1 = value > 0 ? value : 0;
                Canvas.SetTop(marker1, getPosition(true).Y);
                Canvas.SetTop(marker2, getPosition(false).Y);
            }
        }

        public double X2 {
            get {
                return _X2;
            }
            set {
                _X2 = value > 0 ? value : 0;
                Canvas.SetLeft(marker1, getPosition(true).X);
                Canvas.SetLeft(marker2, getPosition(false).X);
            }
        }

        public double Y2 {
            get {
                return _Y2;
            }
            set {
                _Y2 = value > 0 ? value : 0;
                Canvas.SetTop(marker1, getPosition(true).Y);
                Canvas.SetTop(marker2, getPosition(false).Y);
            }
        }

        public RSTPMarker(DynamicEllipse ellipse1, Port port1, DynamicEllipse ellipse2, Port port2, bool isEnabled) {
            // TODO: Complete member initialization
            this.ellipse1 = ellipse1;
            this.port1 = port1;
            this.ellipse2 = ellipse2;
            this.port2 = port2;
            this.isEnabled = isEnabled;

            marker1 = new Ellipse();
            marker2 = new Ellipse();
            marker1.Width = SIZE;
            marker1.Height = SIZE;
            marker2.Width = SIZE;
            marker2.Height = SIZE;
            marker1.Fill = getColor(port1);
            marker2.Fill = getColor(port2);

            //podwójnie i tak ma być!
            X1 = ellipse1.X;
            X2 = ellipse2.X;
            Y1 = ellipse1.Y;
            Y2 = ellipse2.Y;
            X1 = ellipse1.X;
            X2 = ellipse2.X;
            Y1 = ellipse1.Y;
            Y2 = ellipse2.Y;

            ellipse1.PropertyChanged += sc_PropertyChanged;
            ellipse2.PropertyChanged += sc_PropertyChanged;
            port1.PropertyChanged += sc_PropertyChanged;
            port2.PropertyChanged += sc_PropertyChanged;
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("portIsEnabled")) {
                //setEnabled(port1.isEnabled, port2.isEnabled);
            } else {
                if (ellipse1.Equals(sender)) {
                    if (e.PropertyName.Equals("X")) {
                        X1 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        Y1 = ((DynamicEllipse)sender).Y;
                    }
                }
                if (ellipse2.Equals(sender)) {
                    if (e.PropertyName.Equals("X")) {
                        X2 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        Y2 = ((DynamicEllipse)sender).Y;
                    }
                }
            }
            if (e.PropertyName.Equals("state"))
            {
                //zmienił się kolorek
                //Queue<BPDU> bpdus;
                //bpdus = new Queue<BPDU>();
                //bpdus.Dequeue();
                marker1.Fill = getColor(port1);
                marker2.Fill = getColor(port2);
            }
            //setEnabled(port1.isEnabled, port2.isEnabled);
        }

        private SolidColorBrush getColor(Port port) {
            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            switch (port.state) {
                case Port.State.Root: solidColorBrush.Color = Color.FromArgb(255, 0, 0, 0); break;
                case Port.State.Blocking: solidColorBrush.Color = Color.FromArgb(255, 180, 0, 0); break;
                case Port.State.Designated: solidColorBrush.Color = Color.FromArgb(255, 0, 180, 0); break;

            }
            return solidColorBrush;
        }

        private Point getPosition(bool first) {
            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            Point pos = new Point();
            double dx = X2 - X1;
            double dy = Y2 - Y1;
            double length = Math.Sqrt(dx * dx + dy * dy);
            dx /= length;
            dy /= length;
            if (first) {
                pos.X = X1 + dx * (DynamicEllipse.ELLIPSE_DIMM + OFFSET) / 2 - SIZE / 2;
                pos.Y = Y1 + dy * (DynamicEllipse.ELLIPSE_DIMM + OFFSET) / 2 - SIZE / 2;
            } else {
                pos.X = X2 - dx * ((DynamicEllipse.ELLIPSE_DIMM + OFFSET) / 2) - SIZE / 2;
                pos.Y = Y2 - dy * ((DynamicEllipse.ELLIPSE_DIMM + OFFSET) / 2) - SIZE / 2;
            }

            return pos;
        }
    }
}
