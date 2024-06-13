using MassTransit;
using UniversityScheduler.Catalog.Contracts;
using UniversityScheduler.Common;
using UniversityScheduler.Inventory.Service.Entities;

namespace UniversityScheduler.Inventory.Consumers
{
    public class CatalogCourseDeletedConsumer : IConsumer<CatalogCourseDeleted>
    {
        private readonly IRepository<CatalogCourse> repository;

        public CatalogCourseDeletedConsumer(IRepository<CatalogCourse> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogCourseDeleted> context)
        {
            var message = context.Message;

            var course = await repository.GetAsync(message.CourseId);

            if(course == null)
            {         
               return;
            };

            await repository.RemoveAsync(message.CourseId);                 
        }
    }
}