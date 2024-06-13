using MassTransit;
using UniversityScheduler.Catalog.Contracts;
using UniversityScheduler.Common;
using UniversityScheduler.Inventory.Service.Entities;

namespace UniversityScheduler.Inventory.Consumers
{
    public class CatalogCourseCreatedConsumer : IConsumer<CatalogCourseCreated>
    {
        private readonly IRepository<CatalogCourse> repository;

        public CatalogCourseCreatedConsumer(IRepository<CatalogCourse> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogCourseCreated> context)
        {
            var message = context.Message;

            var course = await repository.GetAsync(message.CourseId);

            if(course != null)
            {
                return;
            }

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
    }
}