

using APICatalogo.Models;

namespace APICatalogo_Repositorio.Repository {

    public interface ICategoriaRepository : IRepository<Categoria> {
        
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}