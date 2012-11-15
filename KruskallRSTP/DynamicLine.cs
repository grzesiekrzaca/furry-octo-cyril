using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace KruskallRSTP {
    class DynamicLine {
        public Line line;

        Port port1;
        Port port2;

        DynamicEllipse ellipse1;
        DynamicEllipse ellipse2;

        public DynamicLine(DynamicEllipse ellipse1, Port port1, DynamicEllipse ellipse2, Port port2, bool isEnabled) {
            this.ellipse1 = ellipse1;
            this.ellipse2 = ellipse2;
            this.port1 = port1;
            this.port2 = port2;
            line = new Line();
            line.X1 = ellipse1.X;
            line.X2 = ellipse2.X;
            line.Y1 = ellipse1.Y;
            line.Y2 = ellipse2.Y;
            setEnabled(port1.isEnabled, port2.isEnabled);
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;

            ellipse1.PropertyChanged += sc_PropertyChanged;
            ellipse2.PropertyChanged += sc_PropertyChanged;
            port1.PropertyChanged += sc_PropertyChanged;
            port2.PropertyChanged += sc_PropertyChanged;
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("portIsEnabled")) {
                setEnabled(port1.isEnabled, port2.isEnabled);
            } else {
                if (ellipse1.Equals(sender)) {
                    if (e.PropertyName.Equals("X")) {
                        line.X1 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        line.Y1 = ((DynamicEllipse)sender).Y;
                    }
                }
                if (ellipse2.Equals(sender)) {
                    if (e.PropertyName.Equals("X")) {
                        line.X2 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        line.Y2 = ((DynamicEllipse)sender).Y;
                    }
                }
            }
            setEnabled(port1.isEnabled, port2.isEnabled);
        }

        public void setEnabled(bool isEnabled1, bool isEnabled2) {
            double x = line.X2 - line.X1;
            double y = line.Y2 - line.Y1;
            double normaliser = Math.Max(Math.Abs(x), Math.Abs(y));
            x /= normaliser;
            y /= normaliser;
            double a = x < 0 ? -x : 0;
            double b = x >= 0 ? x : 0;
            double c = y < 0 ? -y : 0;
            double d = y >= 0 ? y : 0;
            Color c1 = isEnabled1 ? Color.FromArgb(255, 255, 70, 70) : Color.FromArgb(255, 0, 0, 255);
            Color c2 = isEnabled2 ? Color.FromArgb(255, 255, 70, 70) : Color.FromArgb(255, 0, 0, 255);
            Brush aGradientBrush = new LinearGradientBrush(c1, c2, new Point(a, c), new Point(b, d));
            line.Stroke = aGradientBrush;//.Windows.Media.Brushes.Red;
            line.StrokeThickness = 5;
        }
    }
}
