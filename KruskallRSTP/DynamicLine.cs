using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace KruskallRSTP {
    class DynamicLine {
        public Line line;
        public TextBlock textBlock { get; set; }

        Port port1;
        Port port2;

        DynamicEllipse ellipse1;
        DynamicEllipse ellipse2;

        private double _x1;
        private double _x2;
        private double _y1;
        private double _y2;

        public double x1 {
            get {
                return _x1;
            }
            set {
                _x1 = value;
                line.X1 = value;
                Canvas.SetLeft(textBlock, (x2 + value) / 2);
            }
        }

        public double x2 {
            get {
                return _x2;
            }
            set {
                _x2 = value;
                line.X2 = value;
                Canvas.SetLeft(textBlock, (x1 + value) / 2);
            }
        }

        public double y1 {
            get {
                return _y1;
            }
            set {
                _y1 = value;
                line.Y1 = value;
                Canvas.SetTop(textBlock, (y2 + value) / 2);
            }
        }

        public double y2 {
            get {
                return _y2;
            }
            set {
                _y2 = value;
                line.Y2 = value;
                Canvas.SetTop(textBlock, (y1 + value) / 2);
            }
        }

        public DynamicLine(DynamicEllipse ellipse1, Port port1, DynamicEllipse ellipse2, Port port2, bool isEnabled) {
            this.ellipse1 = ellipse1;
            this.ellipse2 = ellipse2;
            this.port1 = port1;
            this.port2 = port2;
            line = new Line();
            textBlock = new TextBlock();
            textBlock.FontWeight = FontWeights.UltraBold;
            textBlock.FontSize = 12;
            textBlock.Text = port1.time.ToString();
            x1 = ellipse1.X;
            x2 = ellipse2.X;
            y1 = ellipse1.Y;
            y2 = ellipse2.Y;

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
                        x1 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        y1 = ((DynamicEllipse)sender).Y;
                    }
                }
                if (ellipse2.Equals(sender)) {
                    if (e.PropertyName.Equals("X")) {
                        x2 = ((DynamicEllipse)sender).X;
                    } else if (e.PropertyName.Equals("Y")) {
                        y2 = ((DynamicEllipse)sender).Y;
                    }
                }
            }
            setEnabled(port1.isEnabled, port2.isEnabled);
        }

        public void setEnabled(bool isEnabled1, bool isEnabled2) {
            double x = x2 - x1;
            double y = y2 - y1;
            double normaliser = Math.Max(Math.Abs(x), Math.Abs(y));
            x /= normaliser;
            y /= normaliser;
            double a = x < 0 ? -x : 0;
            double b = x >= 0 ? x : 0;
            double c = y < 0 ? -y : 0;
            double d = y >= 0 ? y : 0;
            Color c1 = isEnabled1 ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 255, 70, 70);
            Color c2 = isEnabled2 ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 255, 70, 70);
            Brush aGradientBrush = new LinearGradientBrush(c1, c2, new Point(a, c), new Point(b, d));
            line.Stroke = aGradientBrush;
            line.StrokeThickness = 5;
        }
    }
}
