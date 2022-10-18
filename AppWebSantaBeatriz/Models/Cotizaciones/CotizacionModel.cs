using AppWebSantaBeatriz.Models.Cotizaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models
{
    public class CotizacionModel
    {
        
        public int ID { get; set; }
        [Required]
        public string IDCotizacion { get; set; }
        [Required]
        public string Servicio { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Fecha { get; set; }
        [Required]
        public int IDCliente { get; set; }
        [Required]
        public int IDSupervisor { get; set; }
        [Required]
        public string Detalles { get; set; }

        [Display(Name ="T. Ejec.")]
       
        public Double TiempoEjecucion { get; set; }
        [Display(Name = "SP")]
        public string SolicitudPedido { get; set; }

        public string NLicitacion { get; set; }
        public int IDEstado { get; set; }
        public decimal Tasa { get; set; }
        public int IDTipoCotizacion { get; set; }
        public DateTime ValidezOferta { get; set; }
        public int IDCotizacionPrincipal { get; set; }
        public string CotizacionPrincipal { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalMateriales { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPersonal { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalSeguridad { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalEquipos { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total  { get; set; }
        public decimal TotalSubCotizaciones { get; set; }
        public decimal TotalSubMateriales { get; set; }
        public decimal TotalSubPersonal { get; set; }
        public decimal TotalSubSeguridad { get; set; }
        public decimal TotalSubEquipos { get; set; }
        public string Cliente { get; set; }
        public string Supervisor { get; set; }
        public string Estado { get; set; }
        public decimal Cantidad { get; set; }

        public string Unidad { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<SelectListItem> Clientes { get; set; }
        public List<SelectListItem> Estados { get; set; }
        public List<SelectListItem> Contactos { get; set; }
    
        public List<CotizacionMaterialesModel> ListaMateriales { get; set; }
        public List<CotizacionPersonalModel> ListaPersonal{ get; set; }
        public List<CotizacionSeguridadModel> ListaSeguridad { get; set; }
        public List<CotizacionEquiposModel> ListaEquipos { get; set; }
        public List<EspecialidadModel> Especialidades { get; set; }
        public List<SubCotizacionModel> ListaProductosPropios { get; set; }

        public List<SelectListItem> CategoriasMRE { get; set; }
        public decimal GastosGenerales { get; set; }


    }
    
}