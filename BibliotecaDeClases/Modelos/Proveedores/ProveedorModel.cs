using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.Proveedores
{
  public class ProveedorModel
    {
		public int ID { get; set; }
		public string IDProveedor { get; set; }
		public string NombreProveedor { get; set; }
		public string EmailProveedor { get; set; }
		public string FonoProveedor { get; set; }
		public string OtrosDetalles { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }

		public string NombreFantasia { get; set; }


	}
}
