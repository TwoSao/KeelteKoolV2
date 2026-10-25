using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Models.Students;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentsServices _studentsServices;

    public StudentsController(IStudentsServices studentsServices) => _studentsServices = studentsServices;

    public async Task<IActionResult> Index()
    {
        var students = await _studentsServices.GetAll();
        var model = students.Select(student => new StudentViewModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            UserID = student.UserID
        });
        return View(model);
    }

    [HttpGet]
    public IActionResult Create() => View("CreateUpdate", new StudentCreateUpdateViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("CreateUpdate", vm);

        var result = await _studentsServices.Create(ToDTO(vm));
        if (result is null)
            return View("CreateUpdate", vm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var student = await _studentsServices.DetailAsync(id);
        if (student is null)
            return NotFound();

        ViewData["ViewType"] = "details";
        return View("DetailsDelete", ToViewModel(student));
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var student = await _studentsServices.DetailAsync(id);
        if (student is null)
            return NotFound();

        return View("CreateUpdate", new StudentCreateUpdateViewModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            UserID = student.UserID
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(StudentCreateUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("CreateUpdate", vm);

        var student = await _studentsServices.Update(ToDTO(vm));
        if (student is null)
            return NotFound();

        return RedirectToAction(nameof(Details), new { id = student.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var student = await _studentsServices.DetailAsync(id);
        if (student is null)
            return NotFound();

        ViewData["ViewType"] = "delete";
        return View("DetailsDelete", ToViewModel(student));
    }

    [HttpPost, ActionName("DeleteConfirmed"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var student = await _studentsServices.Delete(id);
        return student is null ? NotFound() : RedirectToAction(nameof(Index));
    }

    private static StudentDTO ToDTO(StudentCreateUpdateViewModel vm) => new()
    {
        Id = vm.Id ?? Guid.Empty,
        FirstName = vm.FirstName,
        LastName = vm.LastName,
        UserID = vm.UserID
    };

    private static StudentViewModel ToViewModel(Core.Domain.Student student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        UserID = student.UserID
    };
}
