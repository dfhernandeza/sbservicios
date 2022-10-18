
using AppWebSantaBeatriz.Models.Transacciones;
using BibliotecaDeClases.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebSantaBeatriz.Models.Cotizaciones;
using BibliotecaDeClases.Modelos.Servicios;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class ServicioModel
    {
		public int ID { get; set; }
		public int IDEncargado { get; set; }
		public string Encargado { get; set; }

		public List<SelectListItem> Empleados { get; set; }
		public string NombreServicio { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaTermino { get; set; }
		public string Descripcion { get; set; }
		public string NumPedido { get; set; }
		public int IDCotizacion { get; set; }
        public List<SelectListItem> UbisTareas { get; set; }
        public List<SelectListItem> Cotizaciones { get; set; }
		public decimal HorasTotales { get; set; }
		public decimal CostoPersonal { get; set; }
		public decimal CostoMaterial { get; set; }
		public decimal CostoEquiposServicios { get; set; }
		public decimal CostoSeguridad { get; set; }
		public int IDEstado { get; set; }
		public string Estado { get; set; }
		public int IDCliente { get; set; }

        public int IDTipoServicio { get; set; }
        public string TipoServicio { get; set; }
        public string CLiente { get; set; }
		public List<SelectListItem> Clientes { get; set; }
		public int IDContacto { get; set; }

		public string Contacto { get; set; }
		public List<SelectListItem> Contactos { get; set; }
        public List<SelectListItem> TiposServicios { get; set; }
        public List<ClienteModel> ListaClientes { get; set; }
        public decimal? PresupuestoMateriales { get; set; }
	
		public decimal? PresupuestoPersonal { get; set; }
	
		public decimal? PresupuestoSeguridad { get; set; }
		
		public decimal? PresupuestoEquiposServicios { get; set; }

		public int IDCeco { get; set; }

		public string Ceco { get; set; }

        public string Ubicacion { get; set; }
        public int IDUbicacion { get; set; }

        public List<SelectListItem> Ubicaciones { get; set; }

        public CECOsModel CECOModel { get; set; }
        public List<EntregableModel> Entregables { get; set; }
        public List<BibliotecaDeClases.Modelos.Transacciones.TimeSheetModel> ResumenTimeSheet { get; set; }
        public List<SelectListItem> CategoriasEntregables { get; set; }
		public List<BibliotecaDeClases.Modelos.CotizacionEquiposModel> EySList { get; set; }
        public List<ResumenDiaModel> ResumenDiaList { get; set; }
    }
}