using System.Data.Entity;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
	public class EFDbContext : DbContext
	{
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<ProductCategory> ProductCategories { get; set; }
	}
}
