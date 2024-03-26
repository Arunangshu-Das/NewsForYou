using Microsoft.AspNetCore.Mvc;
using NewsForYou.Business;
using NewsForYou.Models;

namespace NewsForYou.Controllers
{
    public class SignUpController : Controller
    {
        private IService _service;

        public SignUpController(IService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            bool flag = await _service.SignUp(model);
            return Json(flag);
        }
    }
}
