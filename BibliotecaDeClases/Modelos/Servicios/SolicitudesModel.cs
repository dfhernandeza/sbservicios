namespace BibliotecaDeClases.Modelos.Servicios
{
  public  class SolicitudesModel
    {
        public int ID { get; set; }
        public int IDSolicitud { get; set; }
        public int IDCotMaterial { get; set; }
        public string NombreServicio { get; set; }
        public string NombreTarea { get; set; }
        public string Entregable { get; set; }
        public string Item { get; set; }
        public decimal Cantidad { get; set; }
        public string Estado { get; set; }
           
    }
}