

using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo_Repositorio.Repository {

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository {

        
        public CategoriaRepository(AppDbContext context) : base(context) {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos() {
           
           return Get().Include(x => x.Produtos);
        }
    }
}