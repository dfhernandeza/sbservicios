using BibliotecaDeClases.Modelos.Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BibliotecaDeClases
{
    public class EmpleadoModel
    {
        public int ID { get; set; }
        public string IDEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechadeNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Fono { get; set; }
        public string Cargo { get; set; }
        public List<SelectListItem> ListaCargos { get; set; }
        public string Especialidad { get; set; }
        public int IDEspecialidad { get; set; }
        public List<SelectListItem> ListaEspecialidades { get; set; }
        public string Supervisor { get; set; }
        public int IDSupervisor { get; set; }
        public List<SelectListItem> ListaSupervisores { get; set; }
        public string OtrosDetalles { get; set; }
        public string TallaZapatos { get; set; }
        public List<SelectListItem> ListaTallasZapatos { get; set; }
        public string TallaPantalon { get; set; }
        public List<SelectListItem> ListaTallaPantalones { get; set; }
        public List<SelectListItem> ListaTallasTop { get; set; }
        public string TallaTop { get; set; }
        public string NombreApellido { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public decimal ValorHH { get; set; }
        public string IDSinFormato { get; set; }
        public List<EmpModel> Liquidaciones { get; set; }
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int IDCuentaEmpleado { get; set; }
        public int CuentaGastos { get; set; }
        public int CuentaPagos { get; set; }
        public DateTime FechaContratacion { get; set; }
        public decimal SueldoBase { get; set; }
        public List<SelectListItem> Bancos { get; set; }
        public List<SelectListItem> TiposCuentaBancaria { get; set; }
        public List<SelectListItem> TiposLicenciaConducir { get; set; }
        public List<SelectListItem> TiposEstadoCivil { get; set; }
        public List<SelectListItem> Paises { get; set; }
        public string IDNacionalidad { get; set; }
        public string IDEstadoCivil { get; set; }
        public string IDTipoLicencia { get; set; }
        public CuentaBancariaModel CuentaBancaria { get; set; }
        public string Cod { get; set; }
        public bool Actualizado { get; set; }
        public DateTime? FechaCorreoAct { get; set; }
        public string EstadoCivil { get; set; }
        public string TipoLicencia { get; set; }
        public string Nacionalidad { get; set; }
        public string Banco { get; set; }
        public string TipoCuentaBancaria { get; set; }
        public string NumCuenta { get; set; }
        public string IDFiniquito { get; set; }
        public bool Planta { get; set; }
        public string IDEstadoContrato { get; set; }
        public List<SelectListItem> EstadosContrato { get; set; }
        public FiniquitoModel Finiquito { get; set; }
        
    }

    public class FirmaLiquidacionModel
    {
        public string ID { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public string IDUsuario { get; set; }
        public string Firma { get; set; }
        public DateTime Fecha { get; set; }
        public string Code { get; set; }
        public string Usuario { get; set; }
    }
    public class FiniquitoModel
    {
        public string ID { get; set; }
        public int IDEmpleado { get; set; }
        public string Empleado { get; set; }
        public DateTime Fecha { get; set; }
        public HttpPostedFileBase Finiquito { get; set; }
    }
}
