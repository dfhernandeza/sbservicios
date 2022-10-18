using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
	public class ChequeModel
	{
		public int ID { get; set; }
		public int Serie { get; set; }
		public DateTime Fecha { get; set; }
		public int IDReceptor { get; set; }
		public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }
		public int IDTransaccion { get; set; }
        public decimal Monto { get; set; }
		public int IDCuentaCR { get; set; }

	}
	public class OpTCreditoModel
	{
		public int ID { get; set; }
		public int IDTransaccion { get; set; }
		public int Cuotas { get; set; }
		public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }
		public decimal Monto { get; set; }
        public string Voucher { get; set; }
		public int IDCuentaCR { get; set; }

	}
	public class CuotaModel
	{
		public int ID { get; set; }
		public int IDOperacion { get; set; }
		public int Cuota { get; set; }
        public DateTime Vencimiento { get; set; }
        public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }


	}
	public class MediosPagoModel
	{
		public int ID { get; set; }
		[Required]
		public string MedioPago { get; set; }
		[Required]
		public int IDCuenta { get; set; }
		public string CreadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public string EditadoPor { get; set; }
		public DateTime EditadoEn { get; set; }
        public List<SelectListItem> Cuentas { get; set; }
        public string Descripcion { get; set; }
        public string CuentaContable { get; set; }


    }
	public class OpTElectronica
	{
		public int ID { get; set; }
		public int IDTransaccion { get; set; }
		public string IDOperacion { get; set; }
		public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }
		public decimal Monto { get; set; }
		public int IDCuentaCR { get; set; }

	}
	public class OpTDebito
	{
		public int ID { get; set; }
		public int IDTransaccion { get; set; }
		public string Voucher { get; set; }
		public string CreadoPor { get; set; }
		public string EditadoPor { get; set; }
		public DateTime CreadoEn { get; set; }
		public DateTime EditadoEn { get; set; }
		public decimal Monto { get; set; }
        public int IDCuentaCR { get; set; }
    }
    public class PagoEfectivo 
	{
        public int ID { get; set; }
        public decimal Monto { get; set; }
		public int IDCuentaCR { get; set; }
	}
}
