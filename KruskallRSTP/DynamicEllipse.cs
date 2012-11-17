using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace KruskallRSTP {
    class DynamicEllipse : INotifyPropertyChanged {
        public static int ELLIPSE_DIMM = 20;
        public Ellipse ellipse { get; set; }
        public TextBlock textBlock { get; set; }
        public Bridge bridge { get; private set; }
        private double _X;
        public double X {
            get{
                return _X;
            }
            set{
                _X = value > 0 ? value : 0 ;
                Canvas.SetLeft(ellipse, _X - ELLIPSE_DIMM / 2);
                Canvas.SetLeft(textBlock, _X - textBlock.Text.Length*textBlock.FontSize/4);
                SendPropertyChanged("X");
            }
        }
        private double _Y;
        public double Y {
            get {
                return _Y;
            }
            set {
                _Y = value > 0 ? value : 0;
                Canvas.SetTop(ellipse, _Y - ELLIPSE_DIMM / 2);
                Canvas.SetTop(textBlock, _Y - ELLIPSE_DIMM - 2);
                SendPropertyChanged("Y");
            }
        }
        
        private Canvas canvas;

        public DynamicEllipse(Bridge bridge, Canvas canvas) {
            this.canvas = canvas;
            ellipse = new Ellipse();
            textBlock = new TextBlock();
            textBlock.FontSize = 12;
            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.bridge = bridge;

            // Set the width and height of the Ellipse.
            ellipse.Width = ELLIPSE_DIMM;
            ellipse.Height = ELLIPSE_DIMM;

            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();

            fillElipse(bridge.isEnabled);
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = Brushes.Black;

            ellipse.MouseLeftButtonDown += switchEnability;
            
            ellipse.MouseRightButtonDown += startMoving;
            ellipse.MouseRightButtonUp += endMoving;

            bridge.PropertyChanged += new PropertyChangedEventHandler(sc_PropertyChanged);
            Canvas.SetZIndex(ellipse, 1);

            textBlock.Text = bridge.bridgeId;
            Canvas.SetZIndex(textBlock, 1);
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            fillElipse(((Bridge)sender).isEnabled);
            SendPropertyChanged("isEnabled");
        }

        private void switchEnability(object sender, MouseButtonEventArgs e) {
            bridge.isEnabled = !bridge.isEnabled;
        }

        private void startMoving(object sender, MouseButtonEventArgs e) {
            ellipse.CaptureMouse();
            ellipse.MouseMove += moveEllipse;
        }

        private void endMoving(object sender, MouseButtonEventArgs e) {
            ellipse.ReleaseMouseCapture();
            ellipse.MouseMove -= moveEllipse;
        }

        private void moveEllipse(object sender, MouseEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                System.Windows.Point p = e.GetPosition(canvas);
                X = (int)p.X;
                Y = (int)p.Y;
            }

        }

        private void fillElipse(bool isEnabled) {
            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            if (isEnabled) {
                solidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            } else {
                solidColorBrush.Color = Color.FromArgb(255, 180, 180, 180);
            }
            ellipse.Fill = solidColorBrush;
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
