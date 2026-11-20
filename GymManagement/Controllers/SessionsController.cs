using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }
        #region create session

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownsAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                await PopulateDropDownsAsync();
                return View(model);
            }

        }

        
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);
            if (result.Success)
            {
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var res = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (res.Success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDownAsync(), "Id", "Name");
                return View(res.Value);
            }
            else
            {
                TempData["ErrorMessage"] = res.error;
                return RedirectToAction(nameof(Index));

            }

        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDownAsync(), "Id", "Name");
                return View(model);
            }


            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDownAsync(), "Id", "Name");
                return View(model);
            }

        }




        #endregion




        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var res = await _sessionService.GetSessionByIdAsync(id, ct);
            if (res.Success) return View(res.Value);
            else
            {
                TempData["ErrorMessage"] = res.error;
                return RedirectToAction(nameof(Index));

            }
      
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id, ct);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Session deleted successfully." : result.error;
            return RedirectToAction(nameof(Index));
        }
        #endregion





        private async Task PopulateDropDownsAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryForDropDownAsync(), "Id", "Name");
        }

    }
     
}
  
