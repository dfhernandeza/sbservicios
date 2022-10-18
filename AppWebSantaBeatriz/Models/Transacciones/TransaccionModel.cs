using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Costos
{
    public class TransaccionModel
    {
		public int ID { get; set; }
		public int IDDocumento { get; set; }
		public DateTime Fecha { get; set; }

		public string Item { get; set; }
		public decimal Cantidad { get; set; }

		public decimal Monto { get; set; }
		public int IDCuentaCR { get; set; }
		public string CuentaCR { get; set; }
		public int IDCuentaDB { get; set; }
		public string CuentaDB { get; set; }
		public int IDCECO { get; set; }
        public string CECO { get; set; }
        public string IDEmpleado { get; set; }
		public string Descripcion { get; set; }
		public decimal Total { get; set; }
		public List<SelectListItem> CECOs { get; set; }
		public List<SelectListItem> Cuentas { get; set; }

		public int Mes { get; set; }
		public int Año { get; set; }

		public DateTime FechaFiltroInicial { get; set; }

		public DateTime FechaFiltroFinal { get; set; }

	}
}