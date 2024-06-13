// using MongoDB.Driver;
// using UniversityScheduler.Courses.Service.Entities;

// namespace UniversityScheduler.Courses.Service.Repositories
// {
//     public class CoursesRepository : ICoursesRepository
//     {
//         private const string collectionName = "items";
//         private readonly IMongoCollection<Course> dbCollection;
//         private readonly FilterDefinitionBuilder<Course> filterBuilder = Builders<Course>.Filter;

//         public CoursesRepository(IMongoDatabase database)
//         {
//             //var mongoClient = new MongoClient("mongodb://localhost:27017");
//             //var database = mongoClient.GetDatabase("Catalog");
//             dbCollection = database.GetCollection<Course>(collectionName);
//         }

//         public async Task<IReadOnlyCollection<Course>> GetAllAsync()
//         {
//             return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
//         }

//         public async Task<Course> GetAsync(Guid id)
//         {
//             FilterDefinition<Course> filter = filterBuilder.Eq(entity => entity.Id, id);
//             return await dbCollection.Find(filter).FirstOrDefaultAsync();
//         }

//         public async Task CreateAsync(Course entity)
//         {
//             if (entity == null)
//             {
//                 throw new ArgumentNullException(nameof(entity));
//             }
//             await dbCollection.InsertOneAsync(entity);
//         }

//         public async Task UpdateAsync(Course entity)
//         {
//             if (entity == null)
//             {
//                 throw new ArgumentNullException(nameof(entity));
//             }

//             FilterDefinition<Course> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
//             await dbCollection.ReplaceOneAsync(filter, entity);
//         }
//         public async Task RemoveAsync(Guid id)
//         {
//             FilterDefinition<Course> filter = filterBuilder.Eq(entity => entity.Id, id);
//             await dbCollection.DeleteOneAsync(filter);
//         }
//     }
// }
