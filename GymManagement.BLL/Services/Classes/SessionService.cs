using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.DATA.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<SessionViewModel?>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session =  await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);
            if (session is null) 
                return Result<SessionViewModel?>.NotFound("Session not found.");
            else
            {
               
                  var mappedSession =  _mapper.Map<SessionViewModel>(session);
                mappedSession.AvailableSlots = session.Capacity - (await _unitOfWork.SessionRepository.GetCountOfBooksSlotsAsync(session.Id, ct));
                return Result<SessionViewModel?>.Ok(mappedSession);
            }
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date.");
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future.");
            if (model.Capacity < 1 || model.Capacity > 25)
                return Result.Validation("Capacity must be between 1 and 25.");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null)
                return Result.NotFound("Trainer not found.");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category is null)
                return Result.NotFound("Category not found.");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true , out var categorySpeciality);
            if (!isValid || trainer.Speciality != categorySpeciality) 
                return Result.NotFound("Cannot create This session for this trainer.");

            var session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session, ct);

            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Failed to create session.");


        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (sessions is null || !sessions.Any()) return null;

            var mappedSessions = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.Category.CategoryName,
                TrainerName = s.Trainer.Name,
                Description = s.Description,
                EndDate = s.EndDate,
                StartDate = s.StartDate,

            });
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - ( await _unitOfWork.SessionRepository.GetCountOfBooksSlotsAsync(session.Id, ct));
            }
            return mappedSessions;

        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct = default)
        {
            var result = await  _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

       

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);

        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session is null)
                return Result<UpdateSessionViewModel>.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Cannot update session that has already started.");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBooksSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result<UpdateSessionViewModel>.Fail("Cannot update session that has already bookings.");

            else
            {

                var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);
                return Result<UpdateSessionViewModel>.Ok(mappedSession);
            }
        }



        public async Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session is null)
                return Result.NotFound("Session not found.");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date.");
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future.");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBooksSlotsAsync(sessionId, ct);
            if(bookingCount > 0)
                return Result.Fail("Cannot update session that has already bookings.");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null)
                return Result.NotFound("Trainer not found.");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
            if (category is null)
                return Result.NotFound("Category not found.");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var categorySpeciality);
            if (!isValid || trainer.Speciality != categorySpeciality)
                return Result.NotFound("Cannot Update This session With this trainer.");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
           
            _unitOfWork.SessionRepository.Update(session, ct);

            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Failed to update session.");

        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);

            if (session is null)
                return Result.NotFound("Session not found.");

            if(session.EndDate >= DateTime.Now)
                return Result.Validation("Cannot delete future session.");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBooksSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result.Fail("Cannot delete session that has already bookings.");

            _unitOfWork.SessionRepository.Delete(session, ct);

            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Failed to delete session.");
        }
    }
}
