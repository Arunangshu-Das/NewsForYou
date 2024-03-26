using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsForYou.Business;
using NewsForYou.Models;
using System.Collections.Generic;

namespace NewsForYou.Controllers
{
    [Route("api")]
    [ApiController]
    public class NewsForYouController : ControllerBase
    {
        private IService _service;

        public NewsForYouController(IService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            bool flag = await _service.SignUp(model);
            return Ok(new { flag});
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            (bool, string)  result=await _service.Login(model);
            return Ok(new { authenticate = result.Item1, jwtToken = result.Item2 }); ;
        }


        [HttpPost]
        [Route("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryModel model)
        {
            bool flag = await _service.AddCategory(model);
            return Ok(new { flag });
        }

        [HttpPost]
        [Route("addagency")]
        public async Task<IActionResult> AddAgency(AgencyModel model)
        {
            bool flag = await _service.AddAgency(model);
            return Ok(new { flag});
        }

        [HttpPost]
        [Route("addagencyfeed")]
        public async Task<IActionResult> AddAgencyFeed(AgencyFeedModel model)
        {
            bool flag = await _service.AddAgencyFeed(model);
            return Ok(new { flag });
        }

        [HttpGet]
        [Route("getcategory")]
        public async Task<IActionResult> GetCategory()
        {
            List<CategoryModel> result = await _service.GetCategory();
            return Ok(new { result });
        }

        [HttpGet]
        [Route("getagency")]
        public async Task<IActionResult> GetAgency()
        {
            List<AgencyModel> result = await _service.GetAgency();
            return Ok( new {result});
        }

        [HttpDelete]
        [Route("deleteall")]
        public async Task<IActionResult> DeleteAllNews()
        {
            bool flag= await _service.DeleteAllNews();
            return Ok(new {flag});
        }

        [HttpGet]
        [Route("getcategoriesfromagencyid")]
        public async Task<IActionResult> GetCategoriesFromAgencyId(int id)
        {
            List < CategoryModel > category= await _service.GetCategoriesFromAgencyId(id); ;
            return Ok(new{ category});
        }

        [HttpGet]
        [Route("getallnews")]
        public async Task<IActionResult> GetAllNews(int id)
        {
            List < NewsModel > allnews= await _service.GetAllNews(id);
            return Ok(new { allnews });
        }

        [HttpPost]
        [Route("generatepdf")]
        public async Task<IActionResult> GeneratePdf(ExportModel export)
        {
            List < ReportModel > report= await _service.GeneratePdf(export.Start, export.End); 
            return Ok(new { report });
        }

        [HttpPost]
        [Route("getnewsbycategories")]
        public async Task<IActionResult> GetNewsByCategories(GetNewsByCategoriesModel model)
        {
            List < NewsModel > getnesfromcategory= await _service.GetNewsByCategories(model.categories, model.id); ;
            return Ok(new { getnesfromcategory });
        }

        [HttpGet]
        [Route("incrementnewsclickcount")]
        public async Task<IActionResult> IncrementNewsClickCount(int id)
        {
            bool flag = await _service.IncrementNewsClickCount(id);
            return Ok(new { flag });
        }

        [HttpGet]
        [Route("checkemail")]
        public async Task<IActionResult> FindEmail(string id)
        {
            bool flag = await _service.FindEmail(id);
            return Ok(new { flag });
        }
    }
}
