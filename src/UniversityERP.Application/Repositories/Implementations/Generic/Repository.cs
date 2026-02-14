using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Application.Repositories.Implementations.Generic;

internal class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> GetAll(bool ignoreQueryFilter = false)
    {
        var query = _dbSet.AsQueryable();

        if (ignoreQueryFilter)
            query = query.IgnoreQueryFilters();

        return query;
    }

    public Task<T?> GetByIdAsync(Guid id)
        => _dbSet.FindAsync(id).AsTask();

    public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        => _dbSet.FirstOrDefaultAsync(expression);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        => _dbSet.AnyAsync(expression);

    public Task AddAsync(T entity)
        => _dbSet.AddAsync(entity).AsTask();

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity); // interceptor will turn this into soft delete

    public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();
}