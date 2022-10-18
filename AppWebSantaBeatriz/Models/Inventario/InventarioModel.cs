using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Inventario
{
    public class InventarioModel
    {
		public int ID { get; set; }
		public string BarCod { get; set; }
		public string Descripcion { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string UnidadMedida { get; set; }
		public decimal Cantidad { get; set; }
		public int IDCategoria { get; set; }
		public int IDUbicacion { get; set; }
		public int IDProveedor { get; set; }
		public int IDCondicion { get; set; }
        public List<SelectListItem> Categorias { get; set; }
        public DateTime FechaReposicion { get; set; }
		public DateTime FechaControl { get; set; }
		public decimal VidaUtil { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
	}

	public class CatInventarioModel
    {
		public int ID { get; set; }
		public string Categoria { get; set; }
		public string Descripcion { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
	}






}