using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;
using System.Linq.Expressions;

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

    // Get all query with the posibiltiy to add incldues 
    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.ToList();
    }

    public T GetById(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
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
