using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class EstadoServicioModel
    {
        public int ID { get; set; }
        public string Estado { get; set; }

        public string Descripcion { get; set; }

        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public string EditadoEn { get; set; }

    }
}