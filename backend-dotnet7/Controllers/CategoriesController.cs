using AutoMapper;
using backend_dotnet7.Core.Dtos.Category;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryReposatory _categoryReposatory;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryReposatory categoryReposatory, IMapper mapper)
        {
            _categoryReposatory = categoryReposatory;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<ICollection<CategoryDto>> GetCategories()
        {


            var categories = _categoryReposatory.GetAllCategories();


            var mappedCategory = _mapper.Map<ICollection<CategoryDto>>(categories);
            return Ok(mappedCategory);
        }

        [HttpGet("{id}", Name = "getCategory")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryReposatory.GetCategory(id);
            if (category == null) { return NotFound(); }

            var mappedCategory = _mapper.Map<CategoryDto>(category);
            return Ok(mappedCategory);
        }
        [HttpPost]
        public ActionResult<CategoryDto> CreateCategory(CreateCategoryDto category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            var newCategory = _categoryReposatory.AddCategory(categoryEntity);

            var categoryReturn = _mapper.Map<CategoryDto>(newCategory);
            return CreatedAtRoute("GetCategory", new { id = categoryReturn.IconwithTitle },
                categoryReturn);
        }
    }
}
