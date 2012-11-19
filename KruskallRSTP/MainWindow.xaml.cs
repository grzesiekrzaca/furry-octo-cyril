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
using System.Xml;

namespace KruskallRSTP {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static int CIRCLE_MARGIN = 30;

        Net net = null;
        Kruskall kruskall = null;
        public MainWindow() {
            InitializeComponent();

            createRandomNet();
        }

        private DynamicEllipse drawCircle(int positionX, int positionY, Bridge bridge) {
            // Create a red Ellipse.
            DynamicEllipse ellipse = new DynamicEllipse(bridge, drawCanvas);
            ellipse.ellipse.MouseLeftButtonDown += onEllipseClick;
            // Add the Ellipse to the StackPanel.
            ellipse.X = positionX;
            ellipse.Y = positionY;

            drawCanvas.Children.Add(ellipse.ellipse);
            drawCanvas.Children.Add(ellipse.textBlock);

            return ellipse;
        }

        private void onEllipseClick(object sender, MouseButtonEventArgs e) {
            makeKruskall();
        }

        private void drawLine(DynamicEllipse ellipse1, Port port1, DynamicEllipse ellipse2, Port port2, bool isEnabled) {
            DynamicLine line = new DynamicLine(ellipse1, port1, ellipse2, port2, isEnabled);
            Canvas.SetZIndex(line.line, 0);
            Canvas.SetZIndex(line.textBlock, 0);
            drawCanvas.Children.Add(line.line);
            drawCanvas.Children.Add(line.textBlock);
        }

        private void reloadViewAfterNewNet() {
            //set left list
            bridgesListView.ItemsSource = net.bridges;
            //clear canvas
            drawCanvas.Children.Clear();

            //set verticesView
            List<Tripple<Port, DynamicEllipse, bool>> edgeGeneratorList = new List<Tripple<Port, DynamicEllipse, bool>>();
            List<DynamicEllipse> ellipses = new List<DynamicEllipse>();
            int count = net.bridges.Count;
            for (int i = 0; i < count; i++) {
                int r = 200;
                double basePhi = 2 * Math.PI / count;
                Bridge bridge = net.bridges[i];
                DynamicEllipse ellipse = drawCircle((int)(r * Math.Sin(basePhi * i) + r + CIRCLE_MARGIN),
                                             (int)(r * Math.Cos(basePhi * i) + r + CIRCLE_MARGIN),
                                             bridge);
                ellipses.Add(ellipse);
                foreach (Port port in bridge.ports) {
                    edgeGeneratorList.Add(new Tripple<Port, DynamicEllipse, bool>(port, ellipse, false));
                }
            }

            //set edgesView
            foreach (Tripple<Port, DynamicEllipse, bool> tripple in edgeGeneratorList) {
                if (tripple.Third) {
                    continue;
                }
                if (tripple.First.destinationPort == null) {
                    continue;
                }
                Tripple<Port, DynamicEllipse, bool> tripple3 = null;
                foreach (Tripple<Port, DynamicEllipse, bool> tripple2 in edgeGeneratorList) {
                    if (tripple2.First.Equals(tripple.First.destinationPort)) {
                        tripple3 = tripple2;
                        tripple2.Third = true;
                        break;
                    }
                }

                drawLine(tripple.Second, tripple.First, tripple3.Second, tripple3.First, false);
                //edges.Add(new Edge(tripple.Second, ellipse, tripple.First.time));
                tripple.Third = true;
            }
        }

        private void createRandomNet() {
            net = new Net();
            kruskall = new Kruskall(net.bridges);
            reloadViewAfterNewNet();
        }


        private void onLoadGraphButtonClick(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true) {
                string filename = dialog.FileName;
                XmlDocument xmlDocument = new XmlDocument();
                try {
                    xmlDocument.Load(filename);
                } catch (XmlException ex) {
                    MessageBox.Show("Invalid XML file!!!!\n\n\n" + ex.ToString());
                    return;
                }
                net = new Net(xmlDocument);
                reloadViewAfterNewNet();
            }
        }

        private void makeKruskall() {
            List<Bridge> enableBridges = new List<Bridge>();
            foreach (Bridge bridge in net.bridges) {
                foreach (Port port in bridge.ports) {
                    port.isEnabled = false;
                }
                if (bridge.isEnabled) {
                    enableBridges.Add(bridge);
                }
            }
            kruskall = new Kruskall(enableBridges);
            minimalCost.Header = "minimal cost = " + kruskall.makeKruskall().ToString();
        }

        private void onCloseButtonClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void onMakeKruskallButtonClick(object sender, RoutedEventArgs e) {

        }

        private void onAboutButtonClick(object sender, RoutedEventArgs e) {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}
