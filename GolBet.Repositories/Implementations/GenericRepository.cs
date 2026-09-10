using GolBet.Entities.Common;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : AuditableEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();   // resolves the DbSet for T at runtime
    }

    // ---- Queries ----

    public virtual async Task<IEnumerable<T>> GetAllAsync(bool includeInactive = false)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (!includeInactive)
            query = query.Where(e => e.IsActive);

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    // ---- Commands ----

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();   // audit stamps applied here (Module 2)
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return;

        entity.IsActive = false;             // logical delete
        await _context.SaveChangesAsync();   // tracked as Modified -> gets ModifiedDate
    }
}

