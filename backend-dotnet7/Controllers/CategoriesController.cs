using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryReposatory _service;
        public CategoriesController(ICategoryReposatory reposatory)
        {
            _service = reposatory;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _service.GetAllCategory();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _service.GetCategory(id);
            if(category == null) { return NotFound(); }
            return Ok(category);
        }
    }
}
