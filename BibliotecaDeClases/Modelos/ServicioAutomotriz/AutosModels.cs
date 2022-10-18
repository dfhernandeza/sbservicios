using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos.ServicioAutomotriz
{
    public class MarcasAutosModel
    {
        public string ID { get; set; }
        public string MarcaAuto { get; set; }
    }
    public class ModelosAutosModel
    {
        public string ID { get; set; }
        public string ModeloAuto { get; set; }
        public string IDMarcaAuto { get; set; }
    }
    public class AñosAutosModel
    {
        public string ID { get; set; }
        public int AñoAutos { get; set; }
        public bool Selected { get; set; }
    }
    public class ModeloAutoAñoModel
    {
        public string ID { get; set; }
        public string IDModelo { get; set; }
        public string IDAño { get; set; }

    }
    public class AñadirAñosPaginaModel
    {
        public List<AñosAutosModel> Años { get; set; }
        public string MarcaAuto { get; set; }
        public string ModeloAuto { get; set; }
        public string IDMarcaAuto { get; set; }
        public string IDModeloAuto { get; set; }
        public List<string> AñosSeleccionados { get; set; }
    }
    public class PaginaModelosAuto
    {
        public string MarcaAuto { get; set; }
        public string IDMarca { get; set; }
        public List<ModelosAutosModel> Modelos { get; set; }
    }
}
