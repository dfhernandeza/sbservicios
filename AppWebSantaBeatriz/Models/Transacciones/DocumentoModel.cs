using AppWebSantaBeatriz.Models.Costos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Transacciones
{
    public class DocumentoModel
    {
		[Required]

		public string IDDocumento { get; set; }
		[DataType(DataType.DateTime)]
		[Display(Name ="Fecha")]
		public DateTime? FechaDocumento { get; set; }
		public string Descripcion { get; set; }
		public string Comentarios { get; set; }
        public List<SelectListItem> TiposDocumentos { get; set; }
        public int IDTipo { get; set; }

		public string Tipo { get; set; }

		public List<SelectListItem> Emisores { get; set; }

		public List<TransaccionModel> ListaItems { get; set; }
        public List<TimeSheetModel> ListaTimeSheet { get; set; }
		public List<CECOsModel> ListaCECOs { get; set; }
		public List<EmpleadoModel> ListaEmpleados { get; set; }
        public string IDEmisor { get; set; }

		public string Emisor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string CreadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
		public string EditadoPor { get; set; }

		public double Total { get; set; }
		public int IDCECO { get; set; }

		public DateTime FechaFiltroInicial { get; set; }

		public DateTime FechaFiltroFinal { get; set; }

		public int Mes { get; set; }

		public int Año { get; set; }
		public DateTime FechaFiltro { get; set; }

        public int ID { get; set; }

        public string MedioPago { get; set; }
    }
}