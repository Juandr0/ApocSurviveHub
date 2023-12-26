using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Repository;

public class CrudRepository<T> : ICrud<T> where T : class
{

    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;


    public CrudRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public void Create(T entity)
    {
        _dbContext.Add(entity);
        _dbContext.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
    }
}
