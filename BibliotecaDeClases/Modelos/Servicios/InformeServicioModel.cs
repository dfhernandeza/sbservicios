using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.Servicios
{
   public class ResumenPresupuestoModel
    {
        public int ID { get; set; }
        public int IDServicio { get; set; }
        public string NombreServicio { get; set; }
        public decimal  PresupuestoMateriales { get; set; }
        public decimal  PresupuestoPersonal { get; set; }
        public decimal  PresupuestoEquiposServicios { get; set; }
        public decimal  CostoMaterial { get; set; }
        public decimal  CostoPersonal { get; set; }
        public decimal  CostoEquiposServicios { get; set; }
        public decimal  GastosGenerales { get; set; }
        public decimal  CostoGastosGenerales { get; set; }
        public decimal  Utilidad { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaInicio { get; set; }



    }

    public class ResumenDiaModel
    {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasTotales { get; set; }
        public decimal ValorHH { get; set; }
        public decimal CostoColaciones { get; set; }
        public decimal CantidadColaciones { get; set; }
        public decimal CostoPeajes { get; set; }
        public decimal CostoCombustible { get; set; }
        public decimal CostoMateriales { get; set; }
        public decimal ValorHHAcumulado { get; set; }
        public decimal CostoColacionesAcumulado { get; set; }
        public decimal CantidadColacionesAcumulado { get; set; }
        public decimal CostoPeajesAcumulado { get; set; }
        public decimal CostoCombustibleAcumulado { get; set; }
        public decimal CostoMaterialesAcumulado { get; set; }
        public decimal PresupuestoPersonal { get; set; }
    }




}
