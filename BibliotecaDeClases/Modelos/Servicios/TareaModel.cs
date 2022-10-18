using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.Transacciones;

namespace BibliotecaDeClases.Modelos.Servicios
{
    public class TareaModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Descripcion { get; set; }
        public int IDEncargado { get; set; }
        public string Encargado { get; set; }
        public List<SelectList> Empleados { get; set; }
        public int IDServicio { get; set; }
        public List<SelectList> Servicios { get; set; }
        public decimal Progreso { get; set; }
        public List<EntregableModel> Entregables { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public int IDUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public int IDCECO { get; set; }
        public CECOsModel CECO { get; set; }
        public decimal Gasto { get; set; }
    }
    public class EntregableModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public string NombreTarea { get; set; }
        public int IDEstado { get; set; }
        public string Estado { get; set; }
        public bool ProductoTerminado { get; set; }
        public decimal Cantidad { get; set; }
        public int IDCotizacion { get; set; }
        public string Unidad { get; set; }
        public decimal PVenta { get; set; }
        public decimal PProduccion { get; set; }
        public string Entregable { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Comentarios { get; set; }
        public string Instrucciones { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public int IDEncargado { get; set; }
        public string Encargado { get; set; }
        public int IDCategoriaEntregable { get; set; }
        public string CategoriaEntregable { get; set; }
        public List<EntregableMaterialPT> MaterialesEntregable { get; set; }

    }
    public class EntregableMaterialPT
    {
        public int ID { get; set; }
        public int IDEntregable { get; set; }
        public int IDCotMaterial { get; set; }
        public int IDEntregablePT { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public string Item { get; set; }
        public decimal Cantidad { get; set; }
        public string Unidad { get; set; }
        public int Tipo { get; set; }
    }
    public class TipoTareaModel
    {
        public int ID { get; set; }
        public string TipoTarea { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class BitacoratareaModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }
        public decimal Progreso { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class SolicitudHerramientaModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public string Item { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int IDEstado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class SolicitudEppModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public string Item { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int IDEstado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class SolicitudDinero
    {
        public int ID { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public int IDTarea { get; set; }
        public string Empleado { get; set; }
        public int IDEmpleado { get; set; }
        public int IDEstado { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
    public class TareaEyS
    {
        public int ID { get; set; }
        public string Item { get; set; }
        public int IDEstado { get; set; }
        public int IDTarea { get; set; }
        public int IDCotEyS { get; set; }
        public decimal Cantidad { get; set; }
        public string Comentarios { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public string Unidad { get; set; }
        public string Estado { get; set; }
    }
    public class ActualizacionTareasModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Progreso { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }

       
    }

}
