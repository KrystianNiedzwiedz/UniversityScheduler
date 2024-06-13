using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityScheduler.Catalog.Service
{
    public record CourseDTO(Guid Id, string Name, string Lecturer, int Audience, DateTime Start, int Length, DateTimeOffset CreatedDate);

    public record CreateCourseDTO([Required] string Name, string Lecturer, [Range(1,120)] int Audience, DateTime Start, [Range(1,3)] int Length);

    public record UpdateCourseDTO([Required] string Name, string Lecturer, [Range(1, 120)] int Audience, DateTime Start, [Range(1,3)] int Length);
}