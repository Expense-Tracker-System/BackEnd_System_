using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_dotnet7.Core.Dtos.Category;
using AutoMapper;

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
        public ActionResult<ICollection<CategoryDto>> GetCategories()
        {

            var CategoriesDto = new List<CategoryDto>();

            foreach (var category in categories) {
                CategoriesDto.Add(new CategoryDto { 
                    Id = category.Id,
                    TitleIcon = $"{category.Title},{category.Icon}"
                });
            }
           
           // var mappedCategories = _mapper.Map<ICollection<CategoryDto>>(categories);

            return Ok(CategoriesDto) ;
        }
        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategory(int id)
        {
            var category = _service.GetCategory(id);
            if(category == null) { return NotFound(); }

            var categoryDto = new CategoryDto();

            categoryDto.Id = category.Id;
            categoryDto.TitleIcon = $"{category.Title},{category.Icon}";
           
            return Ok(categoryDto);
        }
    }
}
