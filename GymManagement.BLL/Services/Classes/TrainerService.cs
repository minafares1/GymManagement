using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
     
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
           
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialities = t.Speciality.ToString(),
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct: ct);
            if (trainer is null) return null;

            return new TrainerViewModel()
            {
               
                Name = trainer.Name,
                Specialities =$"{trainer.Speciality} Trainer",
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.city}"

            };
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
           var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.city,
                Specialities = trainer.Speciality
            };
        }


        public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct))
                return false;
            if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct))
                return false;


            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.city = model.City;
            trainer.Speciality = model.Specialities;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer, ct);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }


        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel CreateTrainerVM, CancellationToken ct = default)
        {
           try
            {
                if (await IsEmailExists(CreateTrainerVM.Email, ct) || await IsPhoneExists(CreateTrainerVM.Phone, ct)) return false; 

                var Trainer = new Trainer()
                {
                    Name = CreateTrainerVM.Name,
                    Email = CreateTrainerVM.Email,
                    Phone = CreateTrainerVM.Phone,
                    DateOfBirth = CreateTrainerVM.DateOfBirth,
                    Gender = CreateTrainerVM.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = CreateTrainerVM.BuildingNumber,
                        Street = CreateTrainerVM.Street,
                        city = CreateTrainerVM.City
                    },
                    Speciality = CreateTrainerVM.Specialities,
                    CreatedAt = DateTime.Now
                };

                _unitOfWork.GetRepository<Trainer>().Add(Trainer, ct);
                var res = await _unitOfWork.SaveChangesAsync(ct);
                return res > 0;
            }
            catch (Exception)
            { 
                return false;
            }
        }

       
        

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainerToRemove = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainerToRemove is null || await HasActiveSessions(trainerId, ct)) return false;
            if (await HasActiveSessions(trainerId, ct)) return false;
            _unitOfWork.GetRepository<Trainer>().Delete(trainerToRemove, ct);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;

        }

        #region Helper Method
        private async Task<bool> HasActiveSessions(int trainerId, CancellationToken ct)
        {
            var existing = await _unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            return existing;
        }

        private async Task<bool> IsPhoneExists(string phone, CancellationToken ct)
        {
            var existing = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == phone, ct);
            return existing;
        }

        private async Task<bool> IsEmailExists(string email, CancellationToken ct)
        {
            var existing = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == email, ct);
            return existing;
        } 
        #endregion




    }
}
