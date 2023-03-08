
using System.Windows.Media;
using System.Xml;

namespace PetClinic
{
    class Configuracion
    {

        public static Brush DefaultForeground { get; set; } = Brushes.White;

        public Configuracion() { }

        public Configuracion(Brush Foreground)
        {
            DefaultForeground = Foreground;
        }

        public static void LeerXML()
        {
            if (System.IO.File.Exists("config.xml"))
            {
                // Cargar el archivo XML
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("config.xml");

                // Obtener los valores de color de la fuente del archivo XML
                XmlNode colorNode = xmlDocument.SelectSingleNode("/configuracion/Foreground");

                if (colorNode.InnerText.Contains("Orange"))
                {
                    DefaultForeground = Brushes.Orange;
                }

                if (colorNode.InnerText.Contains("Red"))
                {
                    DefaultForeground = Brushes.Red;
                }

                if (colorNode.InnerText.Contains("Brown"))
                {
                    DefaultForeground = Brushes.Brown;
                }

                if (colorNode.InnerText.Contains("AliceBlue"))
                {
                    DefaultForeground = Brushes.AliceBlue;
                }

            }
        }
    }
}
