using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
  public  class CECOsModel
    {
		public int ID { get; set; }
		public string Nombre { get; set; }
		public double Gasto { get; set; }
		public DateTime CreadoEn { get; set; }
		public string Descripcion { get; set; }
        public string  CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public int IDTipoCECO { get; set; }
		public string TipoCECO { get; set; }
		public int IDEstadoCECO { get; set; }
        public DateTime EditadoEn { get; set; }
		public string Estado { get; set; }
		public int IDEncargado { get; set; }
		public string Encargado { get; set; }
		public int IDTarea { get; set; }
        public List<SelectListItem> Estados { get; set; }
		public List<SelectListItem> Encargados { get; set; }
		
	}
}
