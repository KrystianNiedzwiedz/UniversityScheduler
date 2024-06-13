using System.Runtime.CompilerServices;
using UniversityScheduler.Catalog.Service;
using UniversityScheduler.Catalog.Service.Entities;


namespace UniversityScheduler.Catalog.Service
{
    public static class Extensions
    {
        public static CourseDTO AsDTO(this Course course)
        {
            return new CourseDTO(course.Id, course.Name, course.Lecturer, course.Audience, course.Start, course.Length, course.CreatedDate);
        }
    }
}
