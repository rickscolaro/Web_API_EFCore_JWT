
using APICatalogo.Context;

namespace APICatalogo_Repositorio.Repository {

    public class UnitOfWork : IUnitOfWork {

        private ProdutoRepository _produtoRepository;

        private CategoriaRepository _categoriaRepository;


        public AppDbContext _context;

        public UnitOfWork(AppDbContext context) {

            _context = context;
        }



        public IProdutoRepository ProdutoRepository {

            get {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository {

            get {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public void Commit() {

            _context.SaveChanges();
        }

        //  liberar recursos injetados não managedos
        public void Dispose() {

            _context.Dispose();
        }
    }
}