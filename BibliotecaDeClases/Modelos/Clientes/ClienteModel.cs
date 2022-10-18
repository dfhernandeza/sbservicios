using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos
{
  public  class ClienteModel
    {
		public int ID { get; set; }
		public string Alias { get; set; }
		public string IDCliente { get; set; }
		public string RazonSocial { get; set; }
		public string Giro { get; set; }
        public bool Arauco { get; set; }
        public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }

		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }


	}
}
