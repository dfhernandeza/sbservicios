using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
  public class DocumentoModel
    {
		public string IDDocumento { get; set; }

        public string IDDocumentoEditada { get; set; }
        public DateTime FechaDocumento { get; set; }
		public string Descripcion { get; set; }
		public string Comentarios { get; set; }
		public int IDTipo { get; set; }
		public string Tipo { get; set; }
        public List<EmpleadoModel> ListaEmpleados { get; set; }
        public List<CECOsModel> ListaCECOs { get; set; }
        public List<SelectListItem> ListaCECOsItems { get; set; }
        public string IDEmisor { get; set; }
		public string Emisor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string CreadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
		public string EditadoPor { get; set; }
        public int IDCECO { get; set; }
        public double Total { get; set; }

		public DateTime FechaFiltroInicial { get; set; }

		public DateTime FechaFiltroFinal { get; set; }

		public int Mes { get; set; }

		public int Year { get; set; }

        public DateTime FechaFiltro { get; set; }

		public int ID { get; set; }

        public string MedioPago { get; set; }

        public string IDEmpleadoEmisor { get; set; }
        public List<TransaccionModel> Transacciones { get; set; }
        public List<HorasExtraModel> HorasExtra { get; set; }
        public List<OpTCreditoModel> TCreditos { get; set; }
        public List<OpTDebito> TDebitos { get; set; }
        public List<ChequeModel> Cheques { get; set; }
        public List<PagoEfectivo> Efectivos { get; set; }
        public List<OpTElectronica> OpTElectronicas { get; set; }
        public List<TimeSheetModel> HH { get; set; }
        public List<TimeSheetModel> HHDistinct { get; set; }
        public List<SelectListItem> Supervisores { get; set; }
        public int IDSupervisor { get; set; }
        public List<SelectListItem> CuentasCR { get; set; }
        public int IDReporteGastos { get; set; }
        public bool Rendicion { get; set; }
        public int IDDocRendicion { get; set; }
        public int IDReporteRendicion { get; set; }
        public string Supervisor { get; set; }
        public string Empleado { get; set; }
    }
}
