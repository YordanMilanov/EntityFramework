using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SimplePagesASPNET.ViewModels.Product;
using System.Text;
using System.Text.Json;

namespace SimplePagesASPNET.Controllers
{
    using static SimplePagesASPNET.Seeding.ProductsData;
    public class ProductController : Controller
    {

        public IActionResult ById(string id)
        {
            ProductViewModel product = Products
                .FirstOrDefault(p => p.Id.ToString().Equals(id));

            if(product == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(product);
        }

        public IActionResult AllAsJson()
        {

            return Json(Products, new JsonSerializerOptions()
            {
               WriteIndented = true,
            });
        }

        public IActionResult DownloadProductsInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var product in Products)
            {
                sb.AppendLine($"Product Id: {product.Id}");
                sb.AppendLine($"Product Name: {product.Name}");
                sb.AppendLine($"Product Price: {product.Price:f2}$");
                sb.AppendLine("------------------------------");
            }
            Response
                .Headers
                .Add(HeaderNames.ContentDisposition, "attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain");
        }

        public IActionResult All(string keyword)
        {
            if(String.IsNullOrWhiteSpace(keyword))
            {
                return View(Products);
            } else
            {
                IEnumerable<ProductViewModel> productsAfterSearch =
                    Products
                    .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
                    .ToArray();
                return View(productsAfterSearch);
            }
        }
    }
}
