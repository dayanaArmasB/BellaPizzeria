using BellaNapoli.Models;

namespace BellaNapoli.Services
{
    public interface IProductService
    {
        Task<List<Producto>> ObtenerProductos(string searchString);
        Task<Producto?> ObtenerProductoPorId(int id);
        Task CrearProducto(Producto producto);
        Task ActualizarProducto(Producto producto);
        Task EliminarProducto(int id);
        bool ProductoExiste(int id);

    }
}
