using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Hitos
{
	public class HitosModel
	{
		public int ID { get; set; }
		public string Nombre { get; set; }
		public DateTime Vencimiento { get; set; }
		public string Entregables { get; set; }
		public int IDEstado { get; set; }
		public int IDServicio { get; set; }

		public List<SelectList> Servicios {get; set;}
		public double HorasTotales { get; set; }
	}
}
