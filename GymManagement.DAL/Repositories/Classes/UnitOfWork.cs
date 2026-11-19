using GymManagement.DAL.DATA.DbContext;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;

        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository)
        {
           _dbContext = dbContext;
           SessionRepository = sessionRepository;   
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity>)value;
            else
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories[typeName] = repo;
                return repo;

            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
          => await _dbContext.SaveChangesAsync(ct);
    }
    
}
