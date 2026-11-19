using GymManagement.DAL.DATA.DbContext;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
            var query = _dbContext.Sessions.AsNoTracking()
                                           .Include(s => s.Trainer)
                                           .Include(s => s.Category);

            return await query.ToListAsync();

        }

        public async Task<int> GetCountOfBooksSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.CountAsync(b => b.SessionId == sessionId, ct);
        }
    }
}
