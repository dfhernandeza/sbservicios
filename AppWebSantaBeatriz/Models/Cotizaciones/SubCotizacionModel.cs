using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Cotizaciones
{
    public class SubCotizacionModel
    {
        public int ID { get; set; }
       
        [Required]
        public string Servicio { get; set; }

        public string Producto { get; set; }

        public DateTime Fecha { get; set; }

        [Required]
        public string Detalles { get; set; }

        public double Utilidad { get; set; }
        public int IDEstado { get; set; }

        public int IDTipoCotizacion { get; set; }
        
        public int IDCotizacionPrincipal { get; set; }
        public string CotizacionPrincipal { get; set; }

        [DataType(DataType.Currency)]
        public double TotalMateriales { get; set; }
        [DataType(DataType.Currency)]
        public double TotalPersonal { get; set; }
        [DataType(DataType.Currency)]
        public double TotalSeguridad { get; set; }
        [DataType(DataType.Currency)]
        public double TotalEquipos { get; set; }
        [DataType(DataType.Currency)]
        public double Total { get; set; }
       
        public string Estado { get; set; }
        public double? Cantidad { get; set; }
        public string Origen { get; set; }
        public string Unidad { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<CotizacionMaterialesModel> ListaMateriales { get; set; }
        public List<CotizacionPersonalModel> ListaPersonal { get; set; }
        public List<CotizacionSeguridadModel> ListaSeguridad { get; set; }
        public List<CotizacionEquiposModel> ListaEquipos { get; set; }
        public List<EspecialidadModel> Especialidades { get; set; }

        public List<SelectListItem> CategoriasMRE { get; set; }
    }
}