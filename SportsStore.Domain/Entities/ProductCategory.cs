using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
	public class ProductCategory
	{
		[Key]
		public int CategoryID { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
