
namespace Catalog.Data.Seed
{
    public class CatalogDataSeeder(CatalogDbContext dbContext) : IDataSeeder
    {
        public async Task SeedAsync()
        {
            if (!await dbContext.Products.AnyAsync())
            {
                dbContext.Products.AddRange(InitialData.Products);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
