using GymManagement.DAL.DbContext;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
           

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking ? _dbContext.Plans : _dbContext.Plans.AsNoTracking();
            return await query.ToListAsync();

                    //Or

            //if (tracking)
            //    return await _dbContext.Plans.ToListAsync();
            //else
            //    return await _dbContext.Plans.AsNoTracking().ToListAsync();
        }
        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
           return await _dbContext.Plans.FindAsync(id);
        }


        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Add(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Remove(plan);
            return await _dbContext.SaveChangesAsync(   );
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Update(plan);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
