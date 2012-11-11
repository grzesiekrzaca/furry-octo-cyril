using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KruskallRSTP {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        private static int CIRCLE_MARGIN = 30;
        private List<DynamicEllipse> ellipses;

        Net net = null;
        public Window1() {
            InitializeComponent();
            net = new Net();
            ellipses = new List<DynamicEllipse>();
            bridgesListView.ItemsSource = net.Bridges;
            int count = net.Bridges.Count;
            for (int i = 0; i < count; i++) {
                int r = 200;
                double basePhi = 2*Math.PI/count;
                drawCircle((int)(r*Math.Sin(basePhi*i)+r+CIRCLE_MARGIN), (int)(r*Math.Cos(basePhi*i)+r+CIRCLE_MARGIN), net.Bridges[i]);
            }
            foreach (DynamicEllipse ellipse1 in ellipses) {
                foreach (DynamicEllipse ellipse2 in ellipses) {
                    if (ellipse1 != ellipse2) {
                        drawLine(ellipse1, ellipse2, false);
                    }
                }
            }
        }

        private void drawCircle(int positionX, int positionY, Bridge bridge) {
            // Create a red Ellipse.
            DynamicEllipse ellipse = new DynamicEllipse(bridge);
            ellipses.Add(ellipse);

            // Add the Ellipse to the StackPanel.
            Canvas.SetLeft(ellipse.ellipse, ellipse.X = positionX);
            Canvas.SetTop(ellipse.ellipse, ellipse.Y = positionY);
            Canvas.SetZIndex(ellipse.ellipse, 1);
            drawCanvas.Children.Add(ellipse.ellipse);
        }

        private void drawLine(DynamicEllipse ellipse1, DynamicEllipse ellipse2, bool isEnabled) {
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            line.X1 = ellipse1.X + (int)(DynamicEllipse.ELLIPSE_DIMM/2);
            line.X2 = ellipse2.X + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.Y1 = ellipse1.Y + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.Y2 = ellipse2.Y + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = 2;
            Canvas.SetZIndex(line, 0);
            drawCanvas.Children.Add(line);
        }
    }
}
