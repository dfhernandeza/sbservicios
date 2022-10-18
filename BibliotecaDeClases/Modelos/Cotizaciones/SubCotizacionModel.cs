using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Cotizaciones
{
   public class SubCotizacionModel
    {
        public int ID { get; set; }  
        public string Servicio { get; set; }
        public string Detalles { get; set; }
        public DateTime Fecha { get; set; }
        public int IDEstado { get; set; }

        public double Utilidad { get; set; }
        public string Producto { get; set; }
        public int IDTipoCotizacion { get; set; }
        public int IDCotizacionPrincipal { get; set; }
        public string CotizacionPrincipal { get; set; }
        public double TotalMateriales { get; set; }
        public double TotalPersonal { get; set; }
        public double TotalSeguridad { get; set; }
        public double TotalEquipos { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; }
        public double Cantidad { get; set; }
        public string Unidad { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<MaterialesModel> ListaMateriales { get; set; }
        public List<CotizacionPersonalModel> ListaPersonal { get; set; }
        public List<CotizacionSeguridadModel> ListaSeguridad { get; set; }
        public List<CotizacionEquiposModel> ListaEquipos { get; set; }
        public List<EspecialidadModel> Especialidades { get; set; }
        public List<SelectListItem> CategoriasMRE { get; set; }

    }
}
