using Google.Apis.MyBusiness.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.ServicioAutomotriz
{
    public class PagNeumaticosModel
    {
        public List<NeumaticosModel> Neumaticos { get; set; }
        public List<SelectListItem> Aros { get; set; }
        public List<SelectListItem> Perfiles { get; set; }
        public List<SelectListItem> Anchos { get; set; }
    }
    public class NeumaticosModel
    {
        public string ID { get; set; }
        public string Marca { get; set; }
        public string IDItem { get; set; }
        public string IDMarca { get; set; }
        public List<SelectListItem> Marcas { get; set; }
        public int IDAncho { get; set; }
        public string Ancho { get; set; }
        public List<SelectListItem> Anchos { get; set; }
        public int IDPerfil { get; set; }
        public string Perfil { get; set; }
        public List<SelectListItem> Perfiles { get; set; }
        public int IDAro { get; set; }
        public string Aro { get; set; }
        public List<SelectListItem> Aros { get; set; }
        public string IDModelo { get; set; }
        public string Modelo { get; set; }
        public List<SelectListItem> Modelos { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public decimal Stock { get; set; }
        public string IDFoto { get; set; }
        public string URL { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public decimal VMax { get; set; }
        public decimal Carga { get; set; }
        public string Origen { get; set; }
        public decimal PAire { get; set; }
        public string IndiceV { get; set; }
        public List<FotoModel> Fotos { get; set; }
        public string URLLogo { get; set; }
        public List<NeumaticoFotoModel> NeumaticoFotoList { get; set; }
        public ProductoModel Item { get; set; }
    }
    public class NeumaticoFotoModel
    {
        public string ID { get; set; }
        public string IDNeumatico { get; set; }
        public string IDFoto { get; set; }
    }
    public class MarcaModel
    {
        public int ID { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }

    }
    public class AnchoModel 
    {
        public int ID { get; set; }
        public string Ancho { get; set; }
    }
    public class PerfilModel
    {
        public int ID { get; set; }
        public string Ancho { get; set; }

    }
    public class AroModel
    {
        public int ID { get; set; }
        public string Aro { get; set; }
    }
    public class ModeloModel
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string IDMarca { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
    }
    public class CategoriaModel
    {
        public int ID { get; set; }
        public string Categoria { get; set; }
    }
    public class FotoNeumaticoModel
    {
        public string ID { get; set; }
        public string IDMarca { get; set; }
        public string Marca { get; set; }
        public string IDModelo { get; set; }
        public string Modelo { get; set; }
        public string URL { get; set; }
    }
    public class FotoLogoModel
    {
        public string ID { get; set; }
        public string IDMarca { get; set; }
        public string Marca { get; set; }
        public List<SelectListItem> Marcas { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public string URL { get; set; }
    }
    public class FotoModel
    {
        public string ID { get; set; }
        public string URL { get; set; }
        public DateTime FechaSubida { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public string Nombre { get; set; }
        public List<SelectListItem> CategoriasFotos { get; set; }
        public string IDCategoria { get; set; }
        public string IDSeccion { get; set; }
        public List<SelectListItem> Secciones { get; set; }
        public string Categoria { get; set; }
        public string Seccion { get; set; }
        public bool NombreDisponible { get; set; }
        public string Descripcion { get; set; }
    }
    public class PromocionModel
    {
        public int ID { get; set; }
        public string IDFoto { get; set; }
        public string Titulo { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
        public string URL { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public List<FotoModel> Fotos { get; set; }
    }
    public class FrontPageModel
    {
       public List<FotoModel> Fotos { get; set; }
       public IEnumerable<Review> Reviews { get; set; }
        public string Ranking { get; set; }
        public List<ProductoModel> Productos { get; set; }
    }
    public class ContactoCorreoModel
    {
       
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Fono { get; set; }
        public string Marca { get; set; }
        public string ModeloAuto { get; set; }
        public string Ano { get; set; }
        public string Cilindrada { get; set; }
        public bool Respondido { get; set; }
    }
    public class ProductoModel
    {
        public string ID { get; set; }
        public string IDItem { get; set; }
        public string Nombre { get; set; }
        public decimal Stock { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCompra { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FotoModel> Fotos { get; set; }
        public List<FotoModel> FotosProducto { get; set; }
        public List<SelectListItem> CatsFotos { get; set; }
        public List<FotoProductoModel> FotoProductoList { get; set; }
        public List<SelectListItem> Marcas { get; set; }
        public string IDMarca { get; set; }
        public string Marca { get; set; }
        public int CantidadFotos { get; set; }
        public string Categoria { get; set; }
        public string Logo { get; set; }
    }
   public class InventarioModel
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string Categoria { get; set; }
        public decimal PVENTA { get; set; }
        public decimal PCompra { get; set; }
        public decimal STOCK { get; set; }
        public double StockInicial { get; set; }
        public int Adiciones { get; set; }
        public DateTime FechaControl { get; set; }
        public double StockFinal { get; set; }
        public int Tiempo { get; set; }
        public bool Product { get; set; }
        public List<CategoriaFotoModel> CategoriasFotos { get; set; }
        public List<SelectListItem> Categorias { get; set; }
    }
   public class MarcaProductoModel
    {        
        public string ID { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public List<FotoModel> Fotos { get; set; }
        public string IDCategoria { get; set; }
        public string Categoria { get; set; }
        public List<SelectListItem> Categorias { get; set; }
        public List<ModeloModel> Modelos { get; set; }
    }
    public class CategoriaFotoModel
    {
        public string ID { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
    }
    public class FotoProductoModel
    {
        public string ID { get; set; }
        public string IDFoto { get; set; }
        public string IDProducto { get; set; }
    }
    public class SeccionesModel
    {
        public string ID { get; set; }
        public string Seccion { get; set; }
        public string Descripcion { get; set; }
    }
    public class CategoriaMarcaModel
    {
        public string ID { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
    }
    public class TiendaModel
    {
        public List<ProductoModel> Productos { get; set; }
        public List<FotoModel> FotosOpciones { get; set; }
    }
}
