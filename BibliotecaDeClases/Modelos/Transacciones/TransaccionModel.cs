using BibliotecaDeClases.Modelos.Empleados;
using BibliotecaDeClases.Modelos.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.ProcesadorEmpleados;
namespace BibliotecaDeClases.Modelos.Transacciones
{
    public class TransaccionModel
    {
		public int ID { get; set; }
		public int IDDocumento { get; set; }
		public DateTime Fecha { get; set; }
		public string Item { get; set; }
		public decimal Cantidad { get; set; }		
		public decimal Monto { get; set; }
		public int IDCuentaCR { get; set; }

        public string CuentaCR { get; set; }
        public int IDCuentaDB { get; set; }
        public string CuentaDB { get; set; }
        public int IDCECO { get; set; }
		public string CECO { get; set; }
		public string IDEmpleado { get; set; }
		public string Descripcion { get; set; }
		public decimal Total { get; set; }
		public List<SelectListItem> CECOs { get; set; }
		public List<SelectListItem> Cuentas { get; set; }
        public int Mes { get; set; }
		public int Año { get; set; }
        public DateTime FechaFiltroInicial { get; set; }
		public DateTime FechaFiltroFinal { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public ChequeModel Cheque { get; set; }
        public OpTCreditoModel OperacionTC { get; set; }
        public OpTElectronica OperacionTE { get; set; }
        public OpTDebito OperacionTD { get; set; }
        public int IDProveedor { get; set; }
        public string Nombre { get; set; }

    }
    public class AutoCompleteModel
    {
        public string Item { get; set; }
        public decimal Precio { get; set; }
    }
    public class TimeSheetModel
    {
        public List<EmpleadoModel> Empleados { get; set; }
        public int ID { get; set; }
        public DateTime Fecha { get; set; }

        public int IDCECO { get; set; }

        public string CECO { get; set; }
        public List<CECOsModel> CECOs { get; set; }
        public List<SelectListItem> CECOsList { get; set; }
        public int IDEmpleado { get; set; }
        public string Empleado { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public string Comentarios { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public int IDDocumento { get; set; }
        public DateTime FechaDocumento { get; set; }
        public decimal ValorHH { get; set; }
        public decimal HorasTotales { get; set; }
        public int IDCuentaEmpleado { get; set; }
        public int IDTransaccion { get; set; }
        public int  IDServicio { get; set; }
    }
    public class RegistroHorasModel
    {
        public int ID { get; set; }
        public int IDCECO { get; set; }
        public string CECO { get; set; }
        public List<TimeSheetModel> Horas { get; set; }
        public List<BibliotecaDeClases.Modelos.Servicios.ProgramaModel> Empleados { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class ColacionesModel
    {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public int IDCECO { get; set; }
        public List<SelectListItem> CECOs { get; set; }
        public List<SelectListItem> MPagos { get; set; }
        public int IDProveedor { get; set; }
        public List<SelectListItem> Proveedores { get; set; }
        public string Comentarios { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PUnitario { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
   
        public DateTime EditadoEn { get; set; }
    }
    public class RendicionModel
    {
        public int ID { get; set; }
        public decimal Total { get; set; }
        public string Empleado { get; set; }
        public int IDCuentaEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaRendicion { get; set; }
        public string Descripcion { get; set; }
        public int IDDocumento { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public decimal MontoRendido { get; set; }
        public int CuentaTransacciones { get; set; }
        public List<TransaccionModel> Transacciones { get; set; }
    }
    public class ReporteGastosModel
    {
        public int ID { get; set; }
        public int IDTransaccion { get; set; }
        public int IDOperacionCR { get; set; }
        public DateTime FechaRendicion { get; set; }
        public DateTime FechaReporte { get; set; }
        public decimal MontoRendido { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<DocumentoModel> Documentos { get; set; }
        public TransaccionModel Devolucion { get; set; }
        public int IDDocumento { get; set; }
    }
    public class DocumentoReporteGastosModel
    {
        public int ID { get; set; }
        public int IDDocumento { get; set; }
        public int IDReporteGastos { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class HorasExtraModel
    {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public int IDEmpleado { get; set; }
        public string Empleado { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public string MesText { get
            {
                List<Mes> meses = new List<Mes>();
                meses.Add(new Mes { Nombre = "Enero", Numero = "1" });
                meses.Add(new Mes { Nombre = "Febrero", Numero = "2" });
                meses.Add(new Mes { Nombre = "Marzo", Numero = "3" });
                meses.Add(new Mes { Nombre = "Abril", Numero = "4" });
                meses.Add(new Mes { Nombre = "Mayo", Numero = "5" });
                meses.Add(new Mes { Nombre = "Junio", Numero = "6" });
                meses.Add(new Mes { Nombre = "Julio", Numero = "7" });
                meses.Add(new Mes { Nombre = "Agosto", Numero = "8" });
                meses.Add(new Mes { Nombre = "Septiembre", Numero = "9" });
                meses.Add(new Mes { Nombre = "Octubre", Numero = "10" });
                meses.Add(new Mes { Nombre = "Noviembre", Numero = "11" });
                meses.Add(new Mes { Nombre = "Diciembre", Numero = "12" });
                return meses.Where(x => x.Numero == Mes.ToString()).FirstOrDefault().Nombre; ; } }
        public List<EmpleadoModel> Empleados { get; set; }
        public List<SelectListItem> EmpleadosSL { get; set; }
        public List<SelectListItem> SupervisoresSL { get; set; }
        public decimal Horas { get; set; }
        public decimal Factor { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public int IDCECO { get; set; }
        public List<SelectListItem> CECOs { get; set; }
        public int IDTransaccion { get; set; }
        public int IDDocumento { get; set; }
        public string Comentarios { get; set; }
        public string CECO { get; set; }

    }
}