
using System;
using System.Diagnostics;
using System.Windows;

using System.Windows.Input;
using SQLite;
using SQLitePCL;



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

        public void btn_register_Click(object sender, RoutedEventArgs e)
        {
            string dbname = "vetsys.db";
            if (!System.IO.File.Exists(dbname)) {

                Console.Write("Generando " + dbname);


                var db_create = new SQLiteConnection(dbname, SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite);
                db_create.CreateTable<Clientes>();
                db_create.CreateTable<Mascota>();
                db_create.Close();

            }
            int edadconvertida;
            int pesoconvertido;

            var cliente = new Clientes()
            {
                DNI = dnitext.Text,
                nombre = nombretext.Text,
                apellidos = apellidotext.Text,
                direccion = direcciontext.Text,
                telefono = telefonotext.Text,
                email = emailtext.Text

            };

            bool success1 = int.TryParse(edadmascotatext.Text.ToString(), out edadconvertida);
            bool success2 = int.TryParse(pesomascotatext.Text.ToString(), out pesoconvertido);


            var mascota = new Mascota()
            {
                nombre = nombremascotatext.Text,
                raza = razamascotatext.Text,

                edad = edadconvertida,
                DNI_Cliente = dnitext.Text,

                peso = pesoconvertido
            };

            try
            {
                var db = new SQLiteConnection("vetsys.db");

                db.Tracer = new Action<string>(q => Debug.WriteLine(q));
                db.Trace = true;
                int rowcliente = db.Insert(cliente);
                int rowMascota = db.Insert(mascota);
                db.Close();
            }
            catch (SQLiteException sqle)
            {
                Console.WriteLine(sqle);
            }

            //if (rowcliente > 0 && rowMascota > 0)
            //{
            //  MessageBox.Show("Registro realizado con éxito" + cliente.ToString() + " " + mascota.ToString() + cliente.DNI.ToString());
            //}



        }
    }

}
