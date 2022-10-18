using BibliotecaDeClases.Modelos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Cotizaciones
{
 public  class CotizacionAraucoModel
    {
        public int ID { get; set; }
        public string SolicitudPedido { get; set; }
        public string IDCotizacion { get; set; }
        public string NLicitacion { get; set; }
        public int IDSupervisor { get; set; }
        public int IDCliente { get; set; }
        public int IDUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string Cliente { get; set; }
        public string IngenieroContratos { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime ValidezOferta { get; set; }
        public string TiempoEjecucion { get; set; }
        public string Detalles { get; set; }
        public decimal CostoInsumosMaterialesTotal { get; set; }
        public decimal CostoPersonalTotal { get; set; }
        public decimal CostoEquiposyServiciosTotal { get; set; }
        public string Notas { get; set; }
        public string Comentarios { get; set; }
        public decimal Utilidad { get; set; }
        public decimal GastosGenerales { get; set; }
        public decimal TotalOferta { get; set; }
        public int IDEstado { get; set; }
        public string Estado { get; set; }
        public int IDTipoCotizacion { get; set; }
        public string TipoCotizacion { get; set; }
        public List<MaterialesModel> Materiales { get; set; }
        public List<CotizacionPersonalModel> Personal { get; set; }
        public List<CotizacionEquiposModel> Equipos { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public string DATOE { get; set; }
        public List<ContactoModel> Supervisores { get; set; }
        public List<MaterialForExcelModel> MaterialesExc { get; set; }
        public List<SelectListItem> Estados { get; set; }
        public List<EspecialidadModel> Especialidades { get; set; }
        public List<SelectListItem> CategoriasMRE { get; set; }
        public string NRFPAriba { get; set; }
    }

    public class CotizacionAraucoFoxExcel
    {
        public int ID { get; set; }
        public int IDEstado { get; set; }
        public string Estado { get; set; }
        public string SolicitudPedido { get; set; }
        public string IDCotizacion { get; set; }
        public string NLicitacion { get; set; }
        public int IDSupervisor { get; set; }
        public int IDCliente { get; set; }
        public string Cliente { get; set; }
        public string IngenieroContratos { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime ValidezOferta { get; set; }
        public string TiempoEjecucion { get; set; }
        public string Detalles { get; set; }
        public decimal CostoInsumosMaterialesTotal { get; set; }
        public decimal CostoPersonalTotal { get; set; }
        public decimal CostoEquiposyServiciosTotal { get; set; }
        public string Notas { get; set; }
        public string Comentarios { get; set; }
        public decimal Utilidad { get; set; }
        public decimal GastosGenerales { get; set; }
        public decimal TotalOferta { get; set; }
        public string DATOE { get; set; }
        public List<MaterialForExcelModel> Materiales { get; set; }
        public List<CotizacionPersonalModel> Personal { get; set; }
        public List<CotizacionEquiposModel> Equipos { get; set; }
        public string NRFPAriba { get; set; }

    }

}
