using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PetClinic
{
    [Table("clientes")]
    class Clientes
    {
        [Column("DNI")]
        [PrimaryKey, Unique]
        public string DNI { get; set; }
        [Column("nombre")]
        public string nombre { get; set; }
        [Column("apellidos")]
        public string apellidos { get; set; }
        [Column("direccion")]
        public string direccion { get; set; }
        [Column("telefono")]
        public string telefono { get; set; }
        [Column("email")]
        public string email { get; set; }



    }
}
