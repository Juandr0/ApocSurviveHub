using System.Linq.Expressions;

namespace ApocSurviveHub.API.Interfaces;
public interface ICrud<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}