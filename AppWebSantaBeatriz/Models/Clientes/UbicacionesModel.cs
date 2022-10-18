using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models
{
    public class UbicacionesModel
    {
        public int ID { get; set; }

        public string Nombre { get; set; }
        public int IDCliente { get; set; }

        public string Direccion { get; set; }

        public string Ciudad { get; set; }

        public string Alias { get; set; }
        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }


    }
}