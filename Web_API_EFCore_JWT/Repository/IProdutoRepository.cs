

using APICatalogo.Models;

namespace APICatalogo_Repositorio.Repository {

    public interface IProdutoRepository: IRepository<Produto> {

        IEnumerable<Produto> GetProdutosPorPreco();
    }
}