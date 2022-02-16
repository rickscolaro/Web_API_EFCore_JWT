

using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo_Repositorio.Repository {

    public class ProdutoRepository : Repository<Produto>, IProdutoRepository {

        public ProdutoRepository(AppDbContext contexto) : base(contexto) {

        }

        // Método especifico
        public IEnumerable<Produto> GetProdutosPorPreco() {

            return Get().OrderBy(c => c.Preco).ToList();
        }


    }
}