using System.Globalization;
using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UniversityScheduler.Catalog.Service.Entities;
using UniversityScheduler.Common;
using UniversityScheduler.Catalog.Contracts;

namespace UniversityScheduler.Catalog.Service.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CoursesController : ControllerBase
    {       
        private readonly IRepository<Course> coursesRepository;
        private readonly IPublishEndpoint publishEndpoint;

       public CoursesController(IRepository<Course> coursesRepository, IPublishEndpoint publishEndpoint)
       {
        this.coursesRepository = coursesRepository;
        this.publishEndpoint = publishEndpoint;
       }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetAsync()    
        {
            var courses = (await coursesRepository.GetAllAsync()).Select(courses => courses.AsDTO());
      
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetByIDAsync(Guid id)
        {
            var course = await coursesRepository.GetAsync(id);

            if(course == null)
            {
                return NotFound();
            }

            return course.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> PostAsync(CreateCourseDTO createCourseDTO)
        {   
            var course = new Course
            {
                Name = createCourseDTO.Name,
                Lecturer = createCourseDTO.Lecturer,
                Audience = createCourseDTO.Audience,
                Start = createCourseDTO.Start,
                Length = createCourseDTO.Length,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await coursesRepository.CreateAsync(course);          
            
            await publishEndpoint.Publish(new CatalogCourseCreated(course.Id, course.Name, course.Lecturer, course.Audience, course.Start, course.Length));
            return CreatedAtAction(nameof(GetByIDAsync), new {id = course.Id}, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCourseDTO updateCourseDTO)
        {
            var existingCourse = await coursesRepository.GetAsync(id);

            if(existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Name = updateCourseDTO.Name;
            existingCourse.Lecturer = updateCourseDTO.Lecturer;
            existingCourse.Audience = updateCourseDTO.Audience;
            existingCourse.Start = updateCourseDTO.Start;
            existingCourse.Length = updateCourseDTO.Length;

            await coursesRepository.UpdateAsync(existingCourse);

            await publishEndpoint.Publish(new CatalogCourseUpdated(existingCourse.Id, existingCourse.Name, existingCourse.Lecturer, existingCourse.Audience, existingCourse.Start, existingCourse.Length));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var course = await coursesRepository.GetAsync(id);
            
            if(course == null)
            {
                return NotFound();
            }
            
            await coursesRepository.RemoveAsync(course.Id);
            await publishEndpoint.Publish(new CatalogCourseDeleted(id));

            return NoContent();
        }
    }
}