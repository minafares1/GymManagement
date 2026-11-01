using GymManagement.BLL.ViewModels.MemberviewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberVM>> GetAllMembersAsync(CancellationToken ct = default);

        Task<bool> CreateMemberAsync(CreateMemberViewModel memberViewModel, CancellationToken ct = default);

    }
}
