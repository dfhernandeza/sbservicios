using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.Empleados
{
   public class TipoLicenciaModel
    {
        public string ID { get; set; }
        public string TipoLicencia { get; set; }
    }
    public class PaisModel
    {
        public string ID { get; set; }
        public string Pais { get; set; }
    }
    public class TipoCuentaBancariaModel
    {
        public string ID { get; set; }
        public string Tipo { get; set; }
    }
    public class CuentaBancariaModel
    {
        public string ID { get; set; }
        public int IDEmpleado { get; set; }
        public string IDBanco { get; set; }
        public string IDTipoCuenta { get; set; }
        public string NumCuenta { get; set; }
        public int Cuenta { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public string Banco { get; set; }
        public string TipoCuenta { get; set; }
    }
    public class BancoModel
    {
        public string ID { get; set; }
        public string Banco { get; set; }
    }
}
