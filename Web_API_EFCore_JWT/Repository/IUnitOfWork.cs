

namespace APICatalogo_Repositorio.Repository {

    public interface IUnitOfWork {

        IProdutoRepository ProdutoRepository { get; }
        
        ICategoriaRepository CategoriaRepository { get; }

        void Commit(); // (Commit) Confirma a transação do banco de dados.
    }
}