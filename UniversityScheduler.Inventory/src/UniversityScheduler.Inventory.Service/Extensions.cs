using UniversityScheduler.Inventory.Service;
using UniversityScheduler.Inventory.Service.Entities;

namespace UnviversityScheduler.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryCourseDTO AsDTO(this InventoryCourse course, string name, string audience)
        {
            return new InventoryCourseDTO(course.CatalogCourseId, name, audience, course.Quantity, course.AcquiredDate);
        }
    }
}