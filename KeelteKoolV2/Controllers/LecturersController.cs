using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using KeelteKoolV2.Models.Lecturers;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers
{
    public class LecturersController : Controller
    {
        private readonly KeelteKoolV2Context _context;
        private readonly ILecturersServices _lecturersServices;
        private readonly IFilesServices _filesServices;

        public LecturersController(
            KeelteKoolV2Context context,
            ILecturersServices lecturersServices,
            IFilesServices filesServices)
        {
            _context = context;
            _lecturersServices = lecturersServices;
            _filesServices = filesServices;
        }

        public IActionResult Index()
        {
            var result = _context.Lecturers
                .Select(x => new LecturerViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Qualifications = x.Qualifications,
                }).Take(20).OrderBy(x => x.LastName);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LecturerCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LecturerCreateUpdateViewModel vm)
        {
            if (vm == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var dto = new LecturerDTO()
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Qualifications = vm.Qualifications,
                UserID = vm.UserID
            };

            var result = await _lecturersServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var vm = new LecturerViewModel() { };
            vm.Id = lecturer.Id;
            vm.FirstName = lecturer.FirstName;
            vm.LastName = lecturer.LastName;
            vm.Qualifications = lecturer.Qualifications;
            vm.UserID = lecturer.UserID;

            ViewData["ViewType"] = "details";

            return View("DetailsDelete", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var vm = new LecturerCreateUpdateViewModel() { };
            vm.Id = lecturer.Id;
            vm.FirstName = lecturer.FirstName;
            vm.LastName = lecturer.LastName;
            vm.Qualifications = lecturer.Qualifications;
            vm.UserID = lecturer.UserID;
            vm.CreatedAt = lecturer.CreatedAt;
            vm.ModifiedAt = lecturer.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LecturerCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new LecturerDTO()
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Qualifications = vm.Qualifications,
                UserID = vm.UserID,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _lecturersServices.Update(dto);
            var resultId = result.Id;

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Update), new { id = resultId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var lecturer = await _lecturersServices.DetailAsync((Guid)id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var vm = new LecturerViewModel() { };
            vm.Id = lecturer.Id;
            vm.FirstName = lecturer.FirstName;
            vm.LastName = lecturer.LastName;
            vm.Qualifications = lecturer.Qualifications;
            vm.UserID = lecturer.UserID;

            ViewData["ViewType"] = "delete";

            return View("DetailsDelete", vm);
        }

        [HttpPost]
        [ActionName("DeleteConfirmed")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var lecturer = await _lecturersServices.Delete(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
