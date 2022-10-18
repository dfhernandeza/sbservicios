using BibliotecaDeClases.Modelos.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos
{
  public  class CotizacionModel
    {
        public int ID { get; set; }
        public string IDCotizacion { get; set; }
        public string Servicio { get; set; }
        public DateTime Fecha { get; set; }
        public int IDCliente { get; set; }
        public int IDSupervisor { get; set; }
        public string Detalles { get; set; }
        public double TiempoEjecucion { get; set; }
        public string SolicitudPedido { get; set; }
        public string NLicitacion { get; set; }
        public int IDEstado { get; set; }
        public decimal Tasa { get; set; }
        public int IDTipoCotizacion { get; set; }
        public DateTime ValidezOferta { get; set; }
        public int IDCotizacionPrincipal { get; set; }
        public string CotizacionPrincipal { get; set; }
        public string Unidad { get; set; }
        public decimal TotalMateriales { get; set; }
        public decimal TotalPersonal { get; set; }
        public decimal TotalSeguridad { get; set; }
        public decimal TotalEquipos { get; set; }
        public decimal TotalSubCotizaciones { get; set; }
        public decimal TotalSubMateriales { get; set; }
        public decimal TotalSubPersonal { get; set; }
        public decimal TotalSubSeguridad { get; set; }
        public decimal TotalSubEquipos { get; set; }

        public decimal Total { get; set; }        
        public string Cliente { get; set; }     
        public string Supervisor { get; set; }       
        public string Estado { get; set; }
        public double Cantidad { get; set; }
        public List<SelectListItem> Clientes { get; set; }
        public List<SelectListItem> Contactos { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public SubCotizacionModel SubCoti { get; set; }
        public decimal Utilidad { get; set; }
        public decimal GastosGenerales { get; set; }
        public List<MaterialesModel> Materiales { get; set; }
        public List<CotizacionPersonalModel> Personal { get; set; }
        public List<CotizacionSeguridadModel> Seguridad { get; set; }
        public List<CotizacionEquiposModel> Equipos { get; set; }

    }
}
