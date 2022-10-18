using AppWebSantaBeatriz.Models.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Cotizaciones
{
    public class CotizacionAraucoModel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "N° Solicitud Pedido")]
        public string SolicitudPedido { get; set; }
        [Required]
        [Display(Name = "N° Cotización")]
        public string IDCotizacion { get; set; }
        [Display(Name ="N° Licitación")]
        public string NLicitacion { get; set; }
        [Required]
        [Display(Name ="Nombre Ingeniero de Contratos")]
        public int IDSupervisor { get; set; }
        public int IDUbicacion { get; set; }

        public string Ubicacion { get; set; }
        public int IDCliente { get; set; }
        public string Cliente { get; set; }
        public string IngenieroContratos { get; set; }
        [Required]
        [Display(Name ="Fecha Oferta")]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Validez Oferta")]
        public DateTime ValidezOferta { get; set; }

        [Required]
        [Display(Name = "Ejecución (Días Hábiles")]
        public string TiempoEjecucion { get; set; }
        [Required]
        [Display(Name ="Descripción")]
        public string Detalles { get; set; }
        public decimal CostoInsumosMaterialesTotal { get; set; }
        public decimal CostoPersonalTotal { get; set; }
        public decimal CostoEquiposyServiciosTotal { get; set; }
        [Required]
        [Display(Name = "Utilidad")]
        public decimal Utilidad { get; set; }
        [Required]
        [Display(Name = "Gastos Generales")]
        public decimal GastosGenerales { get; set; }
        public decimal TotalOferta { get; set; }
        public string Notas { get; set; }
        public string Comentarios { get; set; }
        public string Estado { get; set; }
        public int IDEstado { get; set; }
        public List<SelectListItem> Estados { get; set; }
        public List<UbicacionesModel> Ubicaciones { get; set; }
        public List<CotizacionMaterialesModel> Materiales { get; set; }
        public List<CotizacionPersonalModel> Personal { get; set; }
        public List<CotizacionEquiposModel> Equipos { get; set; }
        public List<ContactoModel> Supervisores { get; set; }
        public List<EspecialidadModel> Especialidades { get; set; }
        public List<SelectListItem> CategoriasMRE { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CradoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public string DATOE { get; set; }


    }
}