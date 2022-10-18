using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Transacciones
{
    public class EstadoCECOModel
    {
        public int ID { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public string ActualizadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }

    }
}