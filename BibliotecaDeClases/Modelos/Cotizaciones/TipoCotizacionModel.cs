using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.Cotizaciones
{
   public class TipoCotizacionModel
    {

		public int ID { get; set; }
		public string TipoCotizacion { get; set; }
		public string Descripcion { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
		public string CreadoPor { get; set; }
	}
}
