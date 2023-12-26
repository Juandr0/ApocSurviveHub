namespace ApocSurviveHub.API.Interfaces;
public interface ICrud<T>

{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}