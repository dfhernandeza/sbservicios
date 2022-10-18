using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Cotizaciones
{
    public class TipoCotizacionModel
    {
		
			public int ID { get; set; }
			[Required]
			public string TipoCotizacion { get; set; }
			[Required]
			public string Descripcion { get; set; }
			public DateTime CreadoEn { get; set; }
			public string EditadoPor { get; set; }
			public DateTime EditadoEn { get; set; }
			public string CreadoPor { get; set; }

		
	}
}