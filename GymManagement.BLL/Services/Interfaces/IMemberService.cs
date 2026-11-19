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

        Task<MemberVM?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);

        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);

        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel memberToUpdateViewModel, CancellationToken ct = default);

        Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default);
    }
}
