using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
		private IProductRepository repository;

		public NavController(IProductRepository repo)
		{
			repository = repo;
		}

		public PartialViewResult Menu(int? category = null)
		{
			IEnumerable<ProductCategory> categories = repository.ProductCategories.OrderBy(x => x.Name);

			ViewBag.SelectedCategory = category;

			return PartialView(categories);
		}

		// GET: Nav
		public ActionResult Index()
        {
            return View();
        }
    }
}