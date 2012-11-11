using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace KruskallRSTP {
    class DynamicLine {
        public Line line;

        public DynamicLine(DynamicEllipse ellipse1, DynamicEllipse ellipse2, bool isEnabled) {
            line = new Line();
            setEnabled(isEnabled);
            line.X1 = ellipse1.X + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.X2 = ellipse2.X + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.Y1 = ellipse1.Y + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.Y2 = ellipse2.Y + (int)(DynamicEllipse.ELLIPSE_DIMM / 2);
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;
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
