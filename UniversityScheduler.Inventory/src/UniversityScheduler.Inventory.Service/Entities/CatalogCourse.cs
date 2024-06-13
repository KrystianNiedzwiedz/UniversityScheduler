using Microsoft.Extensions.Diagnostics.HealthChecks;
using UniversityScheduler.Common;

namespace UniversityScheduler.Inventory.Service.Entities
{
    public class CatalogCourse : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
        public int Audience { get; set; }
        public DateTime Start { get; set; }
        public int Length { get; set; }
    }
}