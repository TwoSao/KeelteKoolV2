using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
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
            return View();
        }
    }
}
