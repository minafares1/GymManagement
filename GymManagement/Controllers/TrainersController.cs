using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        #region Get All
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerService.GetAllTrainersAsync();
            return View(trainers);
        }
        #endregion


        #region Create Trainer
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }
            var result = await _trainerService.CreateTrainerAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("DataMissed", "Trainer with this email or phone number already exists.");
                return View(nameof(Create), model);
            }
        }
        #endregion

        #region Trainer Details
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View("Details", trainer);
        }
        #endregion

        #region Edit Trainer

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _trainerService.UpdateTrainerAsync(id, model);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
               
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update trainer.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = trainer.Id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _trainerService.RemoveTrainerAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete trainer.";
            }
            return RedirectToAction(nameof(Index));
        }




        #endregion


    }
}
