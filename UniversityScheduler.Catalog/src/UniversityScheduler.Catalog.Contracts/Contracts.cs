namespace UniversityScheduler.Catalog.Contracts
{
    public record CatalogCourseCreated(Guid CourseId, string Name, string Lecturer, int Audience, DateTime Start, int Length);

    public record CatalogCourseUpdated(Guid CourseId, string Name, string Lecturer, int Audience, DateTime Start, int Length);

    public record CatalogCourseDeleted(Guid CourseId);
}