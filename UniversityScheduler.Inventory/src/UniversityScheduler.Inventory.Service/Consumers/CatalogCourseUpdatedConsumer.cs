using MassTransit;
using UniversityScheduler.Catalog.Contracts;
using UniversityScheduler.Common;
using UniversityScheduler.Inventory.Service.Entities;

namespace UniversityScheduler.Inventory.Consumers
{
    public class CatalogCourseUpdatedConsumer : IConsumer<CatalogCourseUpdated>
    {
        private readonly IRepository<CatalogCourse> repository;

        public CatalogCourseUpdatedConsumer(IRepository<CatalogCourse> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogCourseUpdated> context)
        {
            var message = context.Message;

            var course = await repository.GetAsync(message.CourseId);

            if(course == null)
            {
            course = new CatalogCourse
            {
                Id = message.CourseId,
                Name = message.Name,
                Lecturer = message.Lecturer,
                Audience = message.Audience,
                Start = message.Start,
                Length = message.Length
            };

            await repository.CreateAsync(course);
            }
            else
            {
                course.Name = message.Name;
                course.Lecturer = message.Lecturer;
                course.Audience = message.Audience;
                course.Start = message.Start;
                course.Length = message.Length;

                await repository.UpdateAsync(course);
            }
        }
    }
}