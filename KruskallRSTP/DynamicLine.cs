using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows;

namespace KruskallRSTP {
    class DynamicLine {
        public Line line;

        DynamicEllipse ellipse1;
        DynamicEllipse ellipse2;

        public DynamicLine(DynamicEllipse ellipse1, DynamicEllipse ellipse2, bool isEnabled) {
            this.ellipse1 = ellipse1;
            this.ellipse2 = ellipse2;
            line = new Line();
            setEnabled(ellipse1.bridge.isEnabled && ellipse2.bridge.isEnabled);
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
            if (e.PropertyName.Equals("isEnabled")) {
                setEnabled(ellipse1.bridge.isEnabled && ellipse2.bridge.isEnabled);
            } else if (ellipse1.Equals(sender)) {
                if (e.PropertyName.Equals("X")) {
                    line.X1 = ((DynamicEllipse)sender).X;
                } else if (e.PropertyName.Equals("Y")) {
                    line.Y1 = ((DynamicEllipse)sender).Y;
                }
            } else if (ellipse2.Equals(sender)) {
                if (e.PropertyName.Equals("X")) {
                    line.X2 = ((DynamicEllipse)sender).X;
                } else if (e.PropertyName.Equals("Y")) {
                    line.Y2 = ((DynamicEllipse)sender).Y;
                }
            }
        }

        public void setEnabled(bool isEnabled) {
            if (isEnabled) {
                line.Stroke = System.Windows.Media.Brushes.Red;
                line.StrokeThickness = 3;
            } else {
                line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                line.StrokeThickness = 3;
            }
        }
    }
}
