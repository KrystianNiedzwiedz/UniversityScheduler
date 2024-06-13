namespace UniversityScheduler.Inventory.Service
{
    public record GrantCoursesDTO(Guid UserId, Guid CatalogCourseId, int Quantity);

    public record InventoryCourseDTO(Guid CatalogCourseId, string Name, string Lecturer, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogCourseDTO(Guid Id, string Name, string Lecturer);

}