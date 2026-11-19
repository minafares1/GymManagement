using AutoMapper;
using GymManagement.BLL.ViewModels.MemberviewModels;
using GymManagement.DAL.DATA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberVM>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.city}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => $"{src.DateOfBirth:yyyy-MM-dd}"));

            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.city));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                //.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src));
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.city = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    city = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));




            //var member = new Member
            //{
            // 
            //    Address = new Address()
            //    {
            //        BuildingNumber = memberVM.BuildingNumber,
            //        Street = memberVM.Street,
            //        city = memberVM.City,
            //    },
            //    HealthRecord = new HealthRecord()
            //    {
            //        BloodType = memberVM.HealthRecordViewModel.BloodType,
            //        Height = memberVM.HealthRecordViewModel.Height,
            //        Weight = memberVM.HealthRecordViewModel.Weight,
            //        Note = memberVM.HealthRecordViewModel.Note,

            //    }

            }
    }
}
