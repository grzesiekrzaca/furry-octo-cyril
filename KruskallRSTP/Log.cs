using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace KruskallRSTP
{
    static class Log
    {
        public static TextBox destinationTextBox;
        public static ScrollViewer scrollViewer;
        private static int counter = 1;

        public static void i(String TAG, String msg)
        {
            destinationTextBox.Text += counter.ToString() + "\t"+
                                         DateTime.Now.ToString() + "\t"+
                                         TAG + "\t\t" +
                                         msg + "\n";
            scrollViewer.ScrollToEnd();
            counter++;
        }

        public static void dump(string filename)
        {
            System.IO.File.WriteAllText(filename, destinationTextBox.Text);
        }
    }
}
