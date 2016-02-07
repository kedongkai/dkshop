using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
	public class Product
	{
		[Key]
		[HiddenInput(DisplayValue = false)]
		public int ProductID { get; set; }

		[Required(ErrorMessage = "Please enter a product name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter a description")]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
		public decimal Price { get; set; }

		[Column("Category")]
		public int CategoryId { get; set; }

		//[Required(ErrorMessage = "Please specify a category")]
		//[ForeignKey("CategoryId")]
		public virtual ProductCategory Category { get; set; }
	}
}
