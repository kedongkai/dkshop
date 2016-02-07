using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Can_Paginate()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"}
			});
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			// Act
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			// Assert
			Product[] prodArray = result.Products.ToArray();
			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].Name, "P4");
			Assert.AreEqual(prodArray[1].Name, "P5");
		}

		[TestMethod]
		public void Can_Filter_Products()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = new ProductCategory { CategoryID = 1, Name = "Cat1"} },
				new Product {ProductID = 2, Name = "P2", Category = new ProductCategory { CategoryID = 2, Name = "Cat2"} },
				new Product {ProductID = 3, Name = "P3", Category = new ProductCategory { CategoryID = 1, Name = "Cat1"} },
				new Product {ProductID = 4, Name = "P4", Category = new ProductCategory { CategoryID = 2, Name = "Cat2"} },
				new Product {ProductID = 5, Name = "P5", Category = new ProductCategory { CategoryID = 3, Name = "Cat3"} }
			});
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			// Act
			Product[] result = ((ProductsListViewModel)controller.List(2, 1).Model).Products.ToArray();

			// Assert
			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].Name == "P2" && result[0].Category.Name == "Cat2");
			Assert.IsTrue(result[1].Name == "P4" && result[1].Category.Name == "Cat2");
		}

		// I would usually create a common setup method, given the degree of duplication between these two test methods.
		[TestMethod]
		public void Can_Send_Pagination_View_Model()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"}
			});
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			// Act
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			// Assert
			PagingInfo pageInfo = result.PagingInfo;
			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Generate_Page_Links()
		{
			// Arrange - define an HTML helper - we need to do this in order to apply the extension method
			HtmlHelper myHelper = null;

			// Arrange - create PagingInfo data
			PagingInfo pagingInfo = new PagingInfo
			{
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};

			// Arrange - set up the delegate using a lambda expression
			Func<int, string> pageUrlDelegate = i => "Page" + i;

			// Act
			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

			// Assert
			Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
				+ @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
				+ @"<a class=""btn btn-default"" href=""Page3"">3</a>",
				result.ToString());
		}

		[TestMethod]
		public void Can_Create_Categories()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.ProductCategories).Returns(new ProductCategory[]
			{
				new ProductCategory { CategoryID = 1, Name = "Apples" },
				new ProductCategory { CategoryID = 2, Name = "Plums" },
				new ProductCategory { CategoryID = 3, Name = "Oranges" }
			});
			NavController target = new NavController(mock.Object);

			// Act
			ProductCategory[] results = ((IEnumerable<ProductCategory>)target.Menu().Model).ToArray();

			// Assert
			Assert.AreEqual(results.Length, 3);
			Assert.AreEqual(results[0].Name, "Apples");
			Assert.AreEqual(results[1].Name, "Oranges");
			Assert.AreEqual(results[2].Name, "Plums");
		}

		[TestMethod]
		public void Indicates_Selected_Category()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.ProductCategories).Returns(new ProductCategory[]
			{
				new ProductCategory { CategoryID = 1, Name = "Apples" },
				new ProductCategory { CategoryID = 2, Name = "Oranges" }
			});
			NavController target = new NavController(mock.Object);
			int categoryToSelect = 1;

			// Act
			int? result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

			// Assert
			Assert.AreEqual(categoryToSelect, result);
		}

		[TestMethod]
		public void Generate_Category_Specific_Product_Count()
		{
			// Arrange
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = new ProductCategory { CategoryID = 1, Name = "Cat1"} },
				new Product {ProductID = 2, Name = "P2", Category = new ProductCategory { CategoryID = 2, Name = "Cat2"} },
				new Product {ProductID = 3, Name = "P3", Category = new ProductCategory { CategoryID = 1, Name = "Cat1"} },
				new Product {ProductID = 4, Name = "P4", Category = new ProductCategory { CategoryID = 2, Name = "Cat2"} },
				new Product {ProductID = 5, Name = "P5", Category = new ProductCategory { CategoryID = 3, Name = "Cat3"} }
			});
			ProductController target = new ProductController(mock.Object);
			target.PageSize = 3;

			// Act
			int res1 = ((ProductsListViewModel)target.List(1).Model).PagingInfo.TotalItems;
			int res2 = ((ProductsListViewModel)target.List(2).Model).PagingInfo.TotalItems;
			int res3 = ((ProductsListViewModel)target.List(3).Model).PagingInfo.TotalItems;
			int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

			// Assert
			Assert.AreEqual(res1, 2);
			Assert.AreEqual(res2, 2);
			Assert.AreEqual(res3, 1);
			Assert.AreEqual(resAll, 5);
		}
	}
}
