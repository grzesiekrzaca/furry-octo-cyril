using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media;

namespace KruskallRSTP {
    class DynamicEllipse{
        public static int ELLIPSE_DIMM = 20;
        public Ellipse ellipse{get; set;}
        public int X{get;set;}
        public int Y{get;set;}

        public DynamicEllipse(Bridge bridge) {
            ellipse = new Ellipse();

            // Set the width and height of the Ellipse.
            ellipse.Width = ELLIPSE_DIMM;
            ellipse.Height = ELLIPSE_DIMM;

            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();

            fillElipse(bridge.isEnabled);
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = Brushes.Black;

            bridge.PropertyChanged += new PropertyChangedEventHandler(sc_PropertyChanged);

        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            fillElipse(((Bridge)sender).isEnabled);
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
    }
}
