using BellaNapoli.Models;
using Microsoft.EntityFrameworkCore;

namespace BellaNapoli.Services
{
    public class ProductService : IProductService
    {

        private readonly TestDbventa1Context _context;

        public ProductService(TestDbventa1Context context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerProductos(string searchString)
        {
            var productos = from p in _context.Productos.Include(p => p.IdCategoriaNavigation)
                            select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString));
            }

            return await productos.ToListAsync();
        }

        public async Task<Producto?> ObtenerProductoPorId(int id)
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarProducto(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public bool ProductoExiste(int id)
        {
            return _context.Productos.Any(p => p.IdProducto == id);
        }

    }
}
