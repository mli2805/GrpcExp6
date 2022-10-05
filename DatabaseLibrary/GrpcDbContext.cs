using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatabaseLibrary;

public class GrpcDbContext : DbContext
{
    private static string MySqlConnectionString => "server=192.168.96.135;port=3306;user id=root;password=root;database=linuxexp";

    private static DbContextOptions<GrpcDbContext> Options =>
        new DbContextOptionsBuilder<GrpcDbContext>()
            .UseMySql(
                MySqlConnectionString,
                new MySqlServerVersion(new Version(5, 7, 39))
            ).Options;

    public GrpcDbContext() : base(Options)
    {
    }

    public DbSet<Product>? Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}

public class ProductRepository
{
    private readonly ILogger _logger;

    public ProductRepository(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        try
        {
            await using var dbContext = new GrpcDbContext();
            var dbContextProducts = dbContext.Products;
                
            return dbContextProducts != null ? await dbContextProducts.ToListAsync() : new List<Product>();
        }
        catch (Exception e)
        {
            _logger.LogError($"{e}");
            return new List<Product>();
        }
    }
}