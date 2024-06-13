using UniversityScheduler.Inventory.Service;

namespace UniversityScheduler.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogCourseDTO>> GetCatalogCoursesAsync()
        {
            var courses = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogCourseDTO>>("/courses");
            return courses;
        }
    }
}