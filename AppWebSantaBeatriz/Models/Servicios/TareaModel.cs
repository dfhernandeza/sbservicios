using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class TareaModel
    {
		public int ID { get; set; }
		public string Nombre { get; set; }
		public DateTime FechaInicial { get; set; }
		public DateTime FechaFinal { get; set; }
		public string Descripcion { get; set; }
		public string IDEncargado { get; set; }
        public List<SelectList> Empleados { get; set; }
        public int IDServicio { get; set; }
		public List<SelectList> Servicios { get; set; }
        public List<EntregableModel> Entregables { get; set; }
        public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
        public string Encargado { get; set; }
        public decimal Progreso { get; set; }
    }
	public class EntregableModel
	{
		public int ID { get; set; }	
		public int IDTarea { get; set; }
		public int IDEstado { get; set; }
		public bool ProductoTerminado { get; set; }
		public decimal Cantidad { get; set; }
		public int IDCotizacion { get; set; }
		public string Unidad { get; set; }
		public decimal PVenta { get; set; }
		public decimal PProduccion { get; set; }
		public string Entregable { get; set; }
		public DateTime FechaInicial { get; set; }
		public DateTime FechaEntrega { get; set; }
		public string Comentarios { get; set; }
		public string Instrucciones { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
        public int IDEncargado { get; set; }
		public int IDCategoriaEntregable { get; set; }
        public List<EntregableMaterialPT> MaterialesEntregable { get; set; }
    }

	public class EntregableMaterialPT
    {
		public int ID_EntregableMaterialPT { get; set; }
		public int IDEntregable_EntregableMaterialPT { get; set; }
		public int IDCotMaterial_EntregableMaterialPT { get; set; }
		public int IDEntregablePT_EntregableMaterialPT { get; set; }
		public string CreadoPor_EntregableMaterialPT { get; set; }
		public DateTime CreadoEn_EntregableMaterialPT { get; set; }
		public string EditadoPor_EntregableMaterialPT { get; set; }
		public DateTime EditadoEn_EntregableMaterialPT { get; set; }
		public string Item_EntregableMaterialPT { get; set; }
		public decimal Cantidad_EntregableMaterialPT { get; set; }
		public string Unidad_EntregableMaterialPT { get; set; }
		public int Tipo_EntregableMaterialPT { get; set; }

	}

	public class TipoTareaModel
    {
		public int ID { get; set; }
		public string TipoTarea { get; set; }
		public string Descripcion { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
	}





}