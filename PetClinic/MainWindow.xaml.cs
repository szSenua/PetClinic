
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using MySqlConnector;
using MessageBox = System.Windows.MessageBox;

namespace PetClinic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Configuracion.LeerXML();
            pet.Foreground = Configuracion.DefaultForeground;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }

        }

        bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;

                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    

                    IsMaximized = true;

                }
            }

        }

        //Botón registro
        public void btn_register_Click(object sender, RoutedEventArgs e)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "server",
                UserID = "user",
                Password = "pass",
                Database = "db",
            };

            var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO clientes(DNI, nombre, apellidos, direccion, telefono, email)
                                  VALUES (@dni,@nombre,@apellidos,@direccion,@telefono,@email)";
            command.Parameters.AddWithValue("@dni", dnitext.Text);
            command.Parameters.AddWithValue("@nombre", nombretext.Text);
            command.Parameters.AddWithValue("@apellidos", apellidotext.Text);
            command.Parameters.AddWithValue("@direccion", direcciontext.Text);
            command.Parameters.AddWithValue("@telefono", telefonotext.Text);
            command.Parameters.AddWithValue("@email", emailtext.Text);

            var command2 = connection.CreateCommand();
            command2.CommandText = @"INSERT INTO mascota(DNI_Cliente, nombre, raza, edad, peso)
                                    VALUES (@dni_cliente, @nombre, @raza, @edad, @peso)";

            command2.Parameters.AddWithValue("@dni_cliente", dnitext.Text);
            command2.Parameters.AddWithValue("@nombre", nombremascotatext.Text);
            command2.Parameters.AddWithValue("@raza", razamascotatext.Text);
            command2.Parameters.AddWithValue("@edad", edadmascotatext.Text);
            command2.Parameters.AddWithValue("@peso", pesomascotatext.Text);

            

            int row = command.ExecuteNonQuery();
            int row2 = command2.ExecuteNonQuery();


            if(row > 0 && row2 > 0)
            {
                MessageBox.Show("Se ha realizado el registro correctamente.");
            }
            else
            {
                MessageBox.Show("Se ha producido un problema");
            }

            //Limpiar los textbox
            dnitext.Text = String.Empty;
            nombretext.Text = String.Empty;
            apellidotext.Text = String.Empty;
            direcciontext.Text = String.Empty;
            telefonotext.Text = String.Empty;
            emailtext.Text = String.Empty;
            nombremascotatext.Text = String.Empty;
            razamascotatext.Text = String.Empty;
            edadmascotatext.Text = String.Empty;
            pesomascotatext.Text = String.Empty;
            

        }

        //Función que busca en una bbdd un dni concreto y muestra el resultado en un DataGrid
        public void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "server",
                UserID = "user",
                Password = "pass",
                Database = "db",
            };

            var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();


            MySqlCommand command = new MySqlCommand(@"SELECT mascota.nombre, mascota.raza, mascota.edad, mascota.peso, clientes.nombre as cliente from mascota
            INNER JOIN clientes ON mascota.DNI_Cliente = clientes.DNI AND mascota.DNI_Cliente = @DNI");




            command.Parameters.AddWithValue("@DNI", dnisearchtext.Text);

            command.Connection = connection;
            command.CommandType = CommandType.Text;





            MySqlDataAdapter sda = new MySqlDataAdapter(command);
            DataSet ds = new DataSet();
            sda.Fill(ds, "gridPet");
            mascotaGrid.DataContext = ds;



        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            string selectedTheme = (colorComboBox.SelectedItem as ComboBoxItem)?.Content as string;
            System.Windows.Media.Brush brush = System.Windows.Media.Brushes.Black;
            if (!string.IsNullOrEmpty(selectedTheme))
            {
                brush = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(selectedTheme);
            }
            pet.Foreground = brush;

            using (XmlWriter writer = XmlWriter.Create("config.xml"))
            {
                // Escribir el encabezado del archivo XML
                writer.WriteStartDocument();

                // Escribir el elemento raíz
                writer.WriteStartElement("configuracion");

                // Escribir el elemento para el color
                writer.WriteStartElement("Foreground");
                writer.WriteString(selectedTheme);
                writer.WriteEndElement();

                writer.WriteEndElement();
                System.Windows.Forms.Application.Restart();
                MessageBox.Show("Cambios Aplicados", "Configuracion", (MessageBoxButton)MessageBoxButtons.OK);

                // Cerrar el archivo XML
                writer.WriteEndDocument();
            }
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el valor seleccionado en el ComboBox
            string selectedTheme = (colorComboBox.SelectedItem as ComboBoxItem)?.Content as string;

            // Asignar el color correspondiente al TextBlock
            switch (selectedTheme)
            {
                case "Red":
                    pet.Foreground = System.Windows.Media.Brushes.Red;
                    break;
                case "Orange":
                    pet.Foreground = System.Windows.Media.Brushes.Orange;
                    break;
                case "Brown":
                    pet.Foreground = System.Windows.Media.Brushes.Brown;
                    break;

                case "AliceBlue":
                    pet.Foreground = System.Windows.Media.Brushes.AliceBlue;
                    break;
                default:
                    pet.Foreground = System.Windows.Media.Brushes.White; // Valor predeterminado
                    break;
            }
        }



        //Función que cierra el programa
        public void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}





