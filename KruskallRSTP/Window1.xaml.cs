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
                        drawLine(ellipse1, ellipse2, ellipse1.GetHashCode() < ellipse2.GetHashCode());
                    }
                }
            }
        }

        private void drawCircle(int positionX, int positionY, Bridge bridge) {
            // Create a red Ellipse.
            DynamicEllipse ellipse = new DynamicEllipse(bridge, drawCanvas);
            ellipses.Add(ellipse);

            // Add the Ellipse to the StackPanel.
            ellipse.X = positionX;
            ellipse.Y = positionY;
            
            drawCanvas.Children.Add(ellipse.ellipse);
        }

        private void drawLine(DynamicEllipse ellipse1, DynamicEllipse ellipse2, bool isEnabled) {
            DynamicLine line = new DynamicLine(ellipse1,ellipse2, isEnabled);
            Canvas.SetZIndex(line.line, 0);
            drawCanvas.Children.Add(line.line);
        }
    }
}
