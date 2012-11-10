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
        public static int CIRCLE_MARGIN = 30;

        Net net = null;
        public Window1() {
            InitializeComponent();
            net = new Net();
            bridgesListView.ItemsSource = net.Bridges;
            int count = net.Bridges.Count;
            for (int i = 0; i < count; i++) {
                int r = 200;
                double basePhi = 2*Math.PI/count;
                drawCircle((int)(r*Math.Sin(basePhi*i)+r+CIRCLE_MARGIN), (int)(r*Math.Cos(basePhi*i)+r+CIRCLE_MARGIN), false);
            }
        }

        public void drawCircle(int positionX, int positionY, bool isActiove) {
            // Create a red Ellipse.
            Ellipse ellipse = new Ellipse();

            // Set the width and height of the Ellipse.
            ellipse.Width = 20;
            ellipse.Height = 20;

            //// Create a SolidColorBrush with a red color to fill the  // Ellipse with.
            SolidColorBrush solidColorBrush = new SolidColorBrush();

            //// Describes the brush's color using RGB values.  // Each value has a range of 0-255.
            if (isActiove) {
                solidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            } else {
                solidColorBrush.Color = Color.FromArgb(255, 180, 180, 180);
            }
            ellipse.Fill = solidColorBrush;
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = Brushes.Black;

            // Add the Ellipse to the StackPanel.
            Canvas.SetLeft(ellipse, positionX);
            Canvas.SetTop(ellipse, positionY);
            drawCanvas.Children.Add(ellipse);
        }
    }
}
