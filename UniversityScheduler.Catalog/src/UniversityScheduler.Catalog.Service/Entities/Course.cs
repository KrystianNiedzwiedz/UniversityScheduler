using UniversityScheduler.Common;

namespace UniversityScheduler.Catalog.Service.Entities
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
        public int Audience { get; set; }
        public DateTime Start { get; set; }
        public int Length { get; set; }
        public DateTimeOffset CreatedDate { get; set; }


    }
}
