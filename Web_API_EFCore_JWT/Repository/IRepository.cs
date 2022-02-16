

using System.Linq.Expressions;

namespace APICatalogo_Repositorio.Repository {

    public interface IRepository<T> {

        // Metodos

        IQueryable<T> Get();// Retornar uma lista de um tipo

        T GetById(Expression<Func<T, bool>> predicate);// Consultar por id

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}