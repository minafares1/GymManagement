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
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel memberVM, CancellationToken ct = default)
        {
            //check email
            var emailExist = await _memberRepository.AnyAsync(x => x.Email == memberVM.Email, ct);

            //check phone
            var phoneExist = await _memberRepository.AnyAsync(x => x.Phone == memberVM.Phone, ct);

            if(emailExist || phoneExist) return false;

            var member = new Member
            {
                Name = memberVM.Name,
                Email = memberVM.Email,
                Phone = memberVM.Phone,
                Gender = memberVM.Gender,
                //Photo = memberVM.Photo,
                DateOfBirth = memberVM.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = memberVM.BuildingNumber,
                    Street = memberVM.Street,
                    city = memberVM.City,
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = memberVM.HealthRecordViewModel.BloodType,
                    Height = memberVM.HealthRecordViewModel.Height,
                    Weight = memberVM.HealthRecordViewModel.Weight,
                    Note = memberVM.HealthRecordViewModel.Note,

                }

            };
             var res = await _memberRepository.AddAsync(member, ct);
            return res > 0;

        }

        public async Task<IEnumerable<MemberVM>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync();

            if (members.Any()) return [];


            //Map members to MemberVM
            //Manually map

            //List<MemberVM> membersToReturn = new List<MemberVM>();
            //foreach (var member in members)
            //{
            //    var memberVM = new MemberVM()
            //    {
            //        Id = member.Id,
            //        Photo = member.Photo,
            //        Name = member.Name,
            //        Email = member.Email,
            //        //Phone = member.Phone,
            //        Gender = member.Gender.ToString()
            //    };
            //    membersToReturn.Add(memberVM);
            //}   

            //return membersToReturn;

            //or

            var membersToReturn = members.Select(member => new MemberVM
            {
                Id = member.Id,
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString()
            });
            return membersToReturn;
        }
    }
}
