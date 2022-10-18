using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class TipoServicioModel
    {
		public int ID { get; set; }
		[Required]
		public string TipoServicio { get; set; }
		[Required]
		public string Descripcion { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
	}
}