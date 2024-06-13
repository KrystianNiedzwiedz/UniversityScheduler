using UniversityScheduler.Common;

namespace UniversityScheduler.Inventory.Service.Entities
{
    public class InventoryCourse : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogCourseId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}