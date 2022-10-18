using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class InventarioEnTarea
    {
		public int ID { get; set; }
		public int IDItemInventario { get; set; }
		public string ItemSolicitud { get; set; }
		public decimal Cantidad { get; set; }
		public int IDTarea { get; set; }
		public string Tarea { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public int IDEstado { get; set; }
		public DateTime Inicio { get; set; }
		public DateTime Termino { get; set; }
        public string Responsable { get; set; }
        public int IDResponsable { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
	}
}