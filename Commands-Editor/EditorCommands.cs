using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Commands_Editor
{
    public class EditorCommands
    {
        private static RoutedUICommand zoomIn;
        private static RoutedUICommand zoomOut;

        static EditorCommands() // static para que se ejecute en la primera utilizacion de la clase
        {
            zoomIn = new RoutedUICommand(
                "Zoom In", "ZoomIn", typeof(EditorCommands));
            zoomOut = new RoutedUICommand(
                "Zoom Out", "ZoomOut", typeof(EditorCommands));
        }

        public static RoutedUICommand ZoomIn { get { return zoomIn; } } // no lo usamos, lo hacemos desde XAML
        public static RoutedUICommand ZoomOut { get { return zoomOut; } } // el que usamos desde codigo
    }
}
