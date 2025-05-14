using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AgriEnergyConnectMVC.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AgriEnergyConnectMVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var claims = User.Claims.ToList();  // Log the claims to see if they are correct
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim type: {claim.Type}, value: {claim.Value}");
            }

            var client = _clientFactory.CreateClient("ProductService");

            // Get the JWT from the current cookie
            var token = User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;

            if (token == null)
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            List<ProductViewModel> products = new();

            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            HttpResponseMessage response;

            if (userRole == "Employee")
            {
                // Get all products
                response = await client.GetAsync("api/product/all");
            }
            else
            {
                // Get products for current farmer
                response = await client.GetAsync($"api/product/my-products");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductViewModel>>(json);
            }
            else
            {
                ModelState.AddModelError("", "Could not retrieve products.");
            }

            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            Console.WriteLine("AddProduct called");
            Console.WriteLine($"UserId from token: {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Validation error on '{state.Key}': {error.ErrorMessage}");
                    }
                }
                return View(model);
            }
            Console.WriteLine("Model State Passed!");

            // Save the image if it exists
            string imagePath = null;
            if (model.Image != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                var extension = Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);

                // Create "images/products" directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imagePath = "/images/products/" + Path.GetFileName(filePath); // Path stored for product
            }
            Console.WriteLine("Image Stored!");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var productDto = new
            {
                Name = model.Name,
                Category = model.Category,
                ProductionDate = model.ProductionDate,
                UserId = userId,
                Image = imagePath,
                Price = model.Price,
                Description = model.Description
            };
            Console.WriteLine(productDto.Name);
            Console.WriteLine(productDto.Category);
            Console.WriteLine(productDto.ProductionDate);
            Console.WriteLine(productDto.UserId);
            Console.WriteLine(productDto.Image);


            var json = JsonConvert.SerializeObject(productDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("JSON Encoded");

            var client = _clientFactory.CreateClient("ProductService");
            Console.WriteLine("Client created.");

            
            var response = await client.PostAsync("/api/product/add", content);
            Console.WriteLine($"The response is: {response}");
            Console.WriteLine("Waiting on response from ProductService.");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response Failed.");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");

                ModelState.AddModelError("", "Failed to add product.");
                return View(model);
            }

            return RedirectToAction("Index");
        }




        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> Filter()
        {
            Console.WriteLine("Filter(GET) was called");
            var client = _clientFactory.CreateClient("UserService");
            

            var response = await client.GetAsync("api/auth/farmers");
            Console.WriteLine($"Response: {response}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"json: {json}");
                var farmers = JsonConvert.DeserializeObject<List<FarmerViewModel>>(json);
                var model = new ProductFilterViewModel { Farmers = farmers };
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Failed to fetch list of farmers.");
                return View(new ProductFilterViewModel());
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> Filter(ProductFilterViewModel model)
        {
            var client = _clientFactory.CreateClient("ProductService");

            var query = new List<string>();
            if (!string.IsNullOrEmpty(model.Category))
                query.Add($"category={model.Category}");
            if (model.FromDate.HasValue)
                query.Add($"fromDate={model.FromDate.Value:yyyy-MM-dd}");
            if (model.ToDate.HasValue)
                query.Add($"toDate={model.ToDate.Value:yyyy-MM-dd}");
            if (!string.IsNullOrEmpty(model.FarmerId))
                query.Add($"farmerId={model.FarmerId}");

            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";

            var response = await client.GetAsync($"/api/product/all{queryString}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to fetch products.");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            model.Results = JsonConvert.DeserializeObject<List<ProductViewModel>>(json);

            // You might want to reload the farmers list here again to populate the dropdown on the returned view
            var userServiceClient = _clientFactory.CreateClient("UserService");
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                userServiceClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            var farmersResponse = await userServiceClient.GetAsync("api/auth/farmers");
            if (farmersResponse.IsSuccessStatusCode)
            {
                var farmersJson = await farmersResponse.Content.ReadAsStringAsync();
                model.Farmers = JsonConvert.DeserializeObject<List<FarmerViewModel>>(farmersJson);
            }
            else
            {
                // Handle error fetching farmers again
            }

            return View(model);
        }




    }
}
