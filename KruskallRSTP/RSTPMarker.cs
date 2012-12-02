using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace KruskallRSTP
{
    class RSTPMarker
    {
        private static int size = 10;
        private DynamicEllipse ellipse1;
        private Port port1;
        private DynamicEllipse ellipse2;
        private Port port2;
        private bool isEnabled;
        public Ellipse marker1 { get; set; } 
        public Ellipse marker2 { get; set; }
        private double x1;
        private double x2;
        private double y1;
        private double y2;

        public RSTPMarker(DynamicEllipse ellipse1, Port port1, DynamicEllipse ellipse2, Port port2, bool isEnabled)
        {
            // TODO: Complete member initialization
            this.ellipse1 = ellipse1;
            this.port1 = port1;
            this.ellipse2 = ellipse2;
            this.port2 = port2;
            this.isEnabled = isEnabled;
            x1 = ellipse1.X;
            x2 = ellipse2.X;
            y1 = ellipse1.Y;
            y2 = ellipse2.Y;
            marker1 = new Ellipse();
            marker2 = new Ellipse();
            marker1.Width = size;
            marker1.Height = size;
            marker2.Width = size;
            marker2.Height = size;
            marker1.Fill = getColor(port1);
            marker2.Fill = getColor(port2);
            Point marker1point = getPosition(true);
            Canvas.SetLeft(marker1, marker1point.X - size / 2);
            Canvas.SetTop(marker1, marker1point.Y - size / 2);
            Point marker2point = getPosition(false);
            Canvas.SetLeft(marker2, marker1point.X - size / 2);
            Canvas.SetTop(marker2, marker1point.Y - size / 2);

        }

        private SolidColorBrush getColor(Port port)
        {
            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            switch (port.state)
            {
                case Port.State.Root: solidColorBrush.Color = Color.FromArgb(255, 0, 0, 0); break;
                case Port.State.Blocking: solidColorBrush.Color = Color.FromArgb(255, 180, 0, 0); break;
                case Port.State.Designated: solidColorBrush.Color = Color.FromArgb(255, 0, 180, 0); break;

            }
            return solidColorBrush;
        }

        private Point getPosition(bool first)
        {
            // Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            Point pos = new Point();
            if (first)
            {
                Point vec = new Point(x1 - x2, y1 - y2);
                pos.X = x1 + vec.X / 3.0;
                pos.Y = y1 + vec.Y / 3.0;
            }
            else
            {
                Point vec = new Point(x2 - x1, y2 - y1);
                pos.X = x2 + vec.X / 3.0;
                pos.Y = y2 + vec.Y / 3.0;
            }
           

            return pos;
        }
    }
}
