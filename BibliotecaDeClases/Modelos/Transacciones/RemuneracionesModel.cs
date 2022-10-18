using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
   public class RemuneracionesModel
    {
		public int ID { get; set; }
		public int IDEmpleado { get; set; }
		public decimal Monto { get; set; }
		public int Mes { get; set; }
		public int Año { get; set; }
		public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }
        public List<SelectListItem> Empleados { get; set; }
		public List<SelectListItem> Meses = new List<SelectListItem>
		{
			new SelectListItem{Text = "Enero",Value ="1" },
			new SelectListItem{Text = "Febrero",Value ="2" },
			new SelectListItem{Text = "Marzo",Value ="3" },
			new SelectListItem{Text = "Abril",Value ="4" },
			new SelectListItem{Text = "Mayo",Value ="5" },
			new SelectListItem{Text = "Junio",Value ="6" },
			new SelectListItem{Text = "Julio",Value ="7" },
			new SelectListItem{Text = "Agosto",Value ="8" },
			new SelectListItem{Text = "Septiembre",Value ="9" },
			new SelectListItem{Text = "Octubre",Value ="10" },
			new SelectListItem{Text = "Noviembre",Value ="11" },
			new SelectListItem{Text = "Diciembre",Value ="12" }
		};
    }
}
