using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Clientes
{
    public class ContactoModel
    {
		public int ID { get; set; }
		public string IDContacto { get; set; }
		public string Alias { get; set; }
		public string Cargo { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string NombreApellido { get; set; }
		public string Email { get; set; }
		public string Ocupacion { get; set; }
		public string Fono { get; set; }
		public string Notas { get; set; }
		public int IDCliente { get; set; }

		public string Cliente { get; set; }
		public List<SelectListItem> Clientes { get; set; }
		public string Area { get; set; }

		public string CreadoPor { get; set; }

		public DateTime CreadoEn { get; set; }

		public string EditadoPor { get; set; }

		public DateTime EditadoEn { get; set; }
        public string Origen { get; set; }
    }
}