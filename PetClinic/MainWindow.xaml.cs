
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
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
                Server = "vps.azuredragoon.com",
                UserID = "dam2",
                Password = "1234",
                Database = "veterinaria",
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

        public void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "vps.azuredragoon.com",
                UserID = "dam2",
                Password = "1234",
                Database = "veterinaria",
            };

            var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT mascota.nombre, mascota.raza, mascota.edad, mascota.peso, clientes.nombre as cliente from mascota
INNER JOIN clientes ON DNI_Cliente = @dni";

            
            command.Parameters.AddWithValue("@dni", dnisearchtext.Text);

            Console.WriteLine(command.ToString());

            
            int row = command.ExecuteNonQuery();

            if(row > 0)
            {
                MySqlDataAdapter sda = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                mascotaGrid.ItemsSource = dt.DefaultView;

            }
            else
            {
                MessageBox.Show("No existen coincidencias");
            }


        }
    }

}
