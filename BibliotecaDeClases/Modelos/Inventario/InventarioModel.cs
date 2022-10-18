using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Inventario
{
 		public class InventarioModel
		{
		public int ID { get; set; }
		public string BarCod { get; set; }
		public string Descripcion { get; set; }
		public decimal PProduccion { get; set; }
		public decimal PVenta { get; set; }
		public string Unidad { get; set; }
		public int IDCategoria { get; set; }
		public int IDUbicacion { get; set; }
		public int IDProveedor { get; set; }
		public int IDCondicion { get; set; }
		public decimal VidaUtil { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }

        public int IDCotMaterial { get; set; }
        public int IDPT { get; set; }
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
