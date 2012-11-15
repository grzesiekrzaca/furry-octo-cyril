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
            line = new Line();
            setEnabled(port1.isEnabled, port2.isEnabled);
            line.X1 = ellipse1.X;
            line.X2 = ellipse2.X;
            line.Y1 = ellipse1.Y;
            line.Y2 = ellipse2.Y;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;

            ellipse1.PropertyChanged += sc_PropertyChanged;
            ellipse2.PropertyChanged += sc_PropertyChanged;
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
        }

        public void setEnabled(bool isEnabled1, bool isEnabled2) {
            Brush aGradientBrush = new LinearGradientBrush(Color.FromArgb(255, 255, 0, 0),
                                                           Color.FromArgb(255, 0, 255, 255),
                                                           new Point(line.X1, line.Y1),
                                                           new Point(line.X2, line.Y2));
            //if (isEnabled) {
                line.Stroke = aGradientBrush;//.Windows.Media.Brushes.Red;
                line.StrokeThickness = 3;
            //} else {
            //    line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            //    line.StrokeThickness = 3;
            //}
        }
    }
}
