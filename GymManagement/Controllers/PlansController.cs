using GymManagement.DAL.DATA.DbContext;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;

        // private readonly IPlanRepository _planRepo;
        public PlansController(IGenericRepository<Plan> planRepository)
        {
            //_dbContext = new GymDbContext(); 1

            //_planRepo = planRepository;  2


            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct : ct);
            return View(plans);
        }

        public async Task<IActionResult> Details([FromRoute] int id, CancellationToken ct)
        {

            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(plan);

            }
        }

    }
}
