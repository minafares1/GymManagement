using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModel;
using GymManagement.DAL.DATA.DbContext;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace GymManagement.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planRepository;

        public PlansController(IPlanService planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllPlansAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details([FromRoute] int id, CancellationToken ct)
        {

            var plan = await _planRepository.GetPlanByIdAsync(id, ct);

            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(plan);

            }
        }

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(plan);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var res = await _planRepository.UpdatePlanAsync(id, vm, ct);
            if (res)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Update Plan";
            return RedirectToAction(nameof(Index));
        }

        #endregion


        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var res = await _planRepository.ToggleActivationAsync(id, ct);
            if (res)
                TempData["SuccessMessage"] = "Plan Activation Toggled Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Toggle Plan Activation";
            return RedirectToAction(nameof(Index));
        }



    }
}
