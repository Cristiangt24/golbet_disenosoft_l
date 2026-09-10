using GolBet.Entities.Common;

namespace GolBet.Repositories.Interface;

    /// <summary>
    /// Generic data-access contract for all domain entities.
    /// Specific queries live in entity-specific repositories.
    /// </summary>
    public interface IGenericRepository<T> where T : AuditableEntity
    {
        // ---- Queries ----
        Task<IEnumerable<T>> GetAllAsync(bool includeInactive = false);
        Task<T?> GetByIdAsync(int id);

        // ---- Commands ----
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeactivateAsync(int id);   // logical delete: IsActive = false
    }


