using System.Linq.Expressions;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace InkSpace.DataAccess.Repository;

public class Repository<T>() : IRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> DbSet;

    public Repository(ApplicationDbContext db) : this() {
        _db = db;
        DbSet = _db.Set<T>();
    }

    public IEnumerable<T> GetAll() {
        IQueryable<T> query = DbSet;
        return query.ToList();
    }

    public T Get(Expression<Func<T, bool>> filter) {
        IQueryable<T> query = DbSet;
        query = query.Where(filter);
        return query.FirstOrDefault()!;
    }

    public void Add(T entity) {
        DbSet.Add(entity);
    }

    public void Remove(T entity) {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities) {
        DbSet.RemoveRange(entities);
    }
}