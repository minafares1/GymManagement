using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberviewModels;
using GymManagement.DAL.DATA.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        //index
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        //MemberDetails
        //HealthRecordDetails

        //Get create
        [HttpGet]
        public IActionResult Create() => View();
        //Post create
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var res = await _memberService.CreateMemberAsync(vm, ct);
            return RedirectToAction(nameof(Index));

        }

        //Get Edit
        //Post Edit

        //Get Delete
        //Post Delete
    } 
}
