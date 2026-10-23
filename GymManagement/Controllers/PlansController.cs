using GymManagement.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext _dbContext;
        public PlansController()
        {
            _dbContext = new GymDbContext();
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _dbContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details([FromRoute] int id)
        {

            var plan = await _dbContext.Plans.FindAsync(id);

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
