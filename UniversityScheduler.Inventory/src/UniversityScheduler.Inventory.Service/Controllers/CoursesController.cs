using Microsoft.AspNetCore.Mvc;
using UniversityScheduler.Common;
using UniversityScheduler.Inventory.Service.Clients;
using UniversityScheduler.Inventory.Service.Entities;
using UnviversityScheduler.Inventory.Service;

namespace UniversityScheduler.Inventory.Service.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IRepository<InventoryCourse> inventoryCoursesRepository;
        private readonly IRepository<CatalogCourse> catalogCoursesRepository;

        public CoursesController(IRepository<InventoryCourse> inventoryCoursesRepository, IRepository<CatalogCourse> catalogCoursesRepository)
        {
            this.inventoryCoursesRepository = inventoryCoursesRepository;
            this.catalogCoursesRepository = catalogCoursesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable <InventoryCourseDTO>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            var inventoryCourseEntities = await inventoryCoursesRepository.GetAllAsync(course => course.UserId == userId);
            var courseIds = inventoryCourseEntities.Select(course => course.CatalogCourseId);
            var catalogCourseEntities = await catalogCoursesRepository.GetAllAsync(course => courseIds.Contains(course.Id));

            var InventoryCourseDTOS = inventoryCourseEntities.Select(inventoryCourse => 
            {
                var catalogCourse = catalogCourseEntities.Single(catalogCourse => catalogCourse.Id == inventoryCourse.CatalogCourseId);
                return inventoryCourse.AsDTO(catalogCourse.Name, catalogCourse.Lecturer);
            });
            return Ok(InventoryCourseDTOS);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantCoursesDTO grantCoursesDTO)
        {
            var inventoryCourse = await inventoryCoursesRepository.GetAsync(
                course => course.UserId == grantCoursesDTO.UserId && course.CatalogCourseId == grantCoursesDTO.CatalogCourseId);

            if(inventoryCourse == null)
            {
                inventoryCourse = new InventoryCourse
                {
                    CatalogCourseId = grantCoursesDTO.CatalogCourseId,
                    UserId = grantCoursesDTO.UserId,
                    Quantity = grantCoursesDTO.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await inventoryCoursesRepository.CreateAsync(inventoryCourse);
            }
            else
            {
                inventoryCourse.Quantity += grantCoursesDTO.Quantity;
                await inventoryCoursesRepository.UpdateAsync(inventoryCourse);
            }

            return Ok();
        }
    }
}