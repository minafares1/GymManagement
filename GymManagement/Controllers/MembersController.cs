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


        #region Create
        //Get create
        [HttpGet]
        public IActionResult Create() => View();
        //Post create
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var res = await _memberService.CreateMemberAsync(vm, ct);


            if (res)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion

        //Get Edit
        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var memberToUpdate = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (memberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(memberToUpdate);
        }
        //Post Edit
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var res = await _memberService.UpdateMemberAsync(id, vm, ct);
            if (res)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Update Member";


            return RedirectToAction(nameof(Index));


        }
        #endregion

        //Get Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        //Post Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken ct)
        {
            var res = await _memberService.RemoveMemberAsync(id, ct);
            if (res)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Delete Member";

            return RedirectToAction(nameof(Index));
        }

    } 
}
