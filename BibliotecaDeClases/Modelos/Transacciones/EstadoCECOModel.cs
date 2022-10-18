using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.Transacciones
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
