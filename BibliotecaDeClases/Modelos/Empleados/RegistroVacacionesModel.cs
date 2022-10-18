using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Empleados
{
    public class RegistroVacacionesModel
    {
        public int ID { get; set; }
        public int IDEmpleado { get; set; }
        public string Empleado { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal Dias { get; set; }
        public decimal DiasDisponibles { get; set; }
        public DateTime FechaContratacion { get; set; }
        public string Comentarios { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<RegistroVacacionesModel> Registro { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoTermino { get; set; }
        public List<PeriodosModel> Periodos { get; set; }
        public string Periodo { get; set; }
        public decimal Inhabiles { get; set; }
    }
   public class PeriodosModel
    {
        public int ID { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoTermino { get; set; }
    }
}
