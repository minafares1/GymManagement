using GymManagement.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        //Plan CRUD operations
        Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default);

        Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> AddAsync(Plan plan, CancellationToken ct = default);

        Task<int> UpdateAsync(Plan plan, CancellationToken ct = default);

        Task<int> DeleteAsync(Plan plan, CancellationToken ct = default);

    }
}
