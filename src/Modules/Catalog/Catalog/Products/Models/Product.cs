


namespace Catalog.Products.Models
{
    public class Product : Aggregrate<Guid>
    {
        public string? Name { get; private set; } = default!;
        public List<string> Category { get; private set; } = new();
        public string Description { get; private set; } = default!;
        public string ImageFile { get; private set; } = default!;
        public decimal Price { get; private set; }

        public static Product Create(string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Category = category,
                Description = description,
                ImageFile = imageFile,
                Price = price,
                CreatedAt = DateTime.UtcNow
            };

            product.AddDomainEvent(new ProductCreatedEvent(product));

            return product;
        }
        public void Update(string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));
            Name = name;
            Category = category;
            Description = description;
            ImageFile = imageFile;
            Price = price;
            LastModified = DateTime.UtcNow;

            if (Price != price)
            {
                Price = price;
                AddDomainEvent(new ProductPriceChangeEvent(this));
            }
        }
    }
}
