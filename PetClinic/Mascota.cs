using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PetClinic
{
    [Table("mascota")]
    class Mascota
    {
        [Column("id_mascota")]
        [PrimaryKey, AutoIncrement]
        public int id_mascota { get; set; }

        [Column("DNI_Cliente")]
        public string DNI_Cliente { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }

        [Column("raza")]
        public string raza { get; set; }

        [Column("edad")]
        public int edad { get; set; }
        [Column("peso")]
        public int peso { get; set; }



    }
}
