using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberviewModels;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Member> _memberRepository;
       
        public MemberService(IUnitOfWork unitOfWork,
                             IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memberRepository = _unitOfWork.GetRepository<Member>();
            
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel memberVM, CancellationToken ct = default)
        {
            //check email
            var emailExist = await _memberRepository.AnyAsync(x => x.Email == memberVM.Email, ct);

            //check phone
            var phoneExist = await _memberRepository.AnyAsync(x => x.Phone == memberVM.Phone, ct);

            if (emailExist || phoneExist) return false;

            var member = _mapper.Map<Member>(memberVM);

            _memberRepository.Add(member, ct);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;

        }

        public async Task<IEnumerable<MemberVM>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync();

            if (!members.Any()) return [];


            var membersToReturn = _mapper.Map<IEnumerable<MemberVM>>(members);
            return membersToReturn;
        }

        public async Task<MemberVM?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, ct);
            if (member == null) return null;

            var memberToReturn = _mapper.Map<Member, MemberVM>(member);

            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == member.Id && m.EndDate > DateTime.Now);
            if (activeMembership is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId);
                memberToReturn.PlanName = activePlan?.Name;
                memberToReturn.MembershipStartDate = activeMembership.CreatedAt.ToString();
                memberToReturn.MembershipEndDate = activeMembership.EndDate.ToString();
            }
            return memberToReturn;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct: ct);
            if (record == null) return null;

            else return _mapper.Map<HealthRecordViewModel>(record);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            else return _mapper.Map<MemberToUpdateViewModel>(member);

        }


        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel memberVM, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member == null) return false;

            //check email
            var emailExist = await _memberRepository.AnyAsync(x => x.Email == memberVM.Email && x.Id != memberId, ct);

            //check phone
            var phoneExist = await _memberRepository.AnyAsync(x => x.Phone == memberVM.Phone && x.Id != memberId, ct);

            if (emailExist || phoneExist) return false;

            //member.Email = memberVM.Email; //Still My Email Or New Unique Email
            //member.Phone = memberVM.Phone; // Still My Phone Or New Unique Phone

            //member.Address.city = memberVM.City;
            //member.Address.Street = memberVM.Street;
            //member.Address.BuildingNumber = memberVM.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            _mapper.Map(memberVM, member);

           _memberRepository.Update(member, ct);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;



        }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member == null) return false;

            var hasFutureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b=> b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);
            if (hasFutureBookings) return false;

           _memberRepository.Delete(member, ct);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }
    }

}
