using CustomersUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;



namespace CustomersUI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlAPI = "https://localhost:7093/api/Customers";

        public CustomersController(HttpClient httpClient)
        { 
            _httpClient = httpClient;
        }

        public List<Customer> Customers { get; set; } = new List<Customer>();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_urlAPI);

            if (response.IsSuccessStatusCode)
            {
                var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
                return View(customers);
            }

            else
            {
                // Log or handle API error
                return View("Error", new ErrorViewModel { RequestId = "API Error" });
            }


        }

     
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync(_urlAPI, customer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(customer); // Return the form if the customer creation fails
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int customerID)
        {
            var response = await _httpClient.GetAsync($"{_urlAPI}/{customerID}");
            if (!response.IsSuccessStatusCode)
            {
                // Redirect to an error page
                return View("Error", new ErrorViewModel { RequestId = "API Error" });
            }
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_urlAPI}/{customer.CustomerID}", customer);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    // Logging the error
                    ModelState.AddModelError("", "Customer update failed, please try again.");
                }
            }

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int customerID)
        {
            var response = await _httpClient.GetAsync($"{_urlAPI}/{customerID}");
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int customerID)
        {
            var response = await _httpClient.DeleteAsync($"{_urlAPI}/{customerID}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(); // Return to the delete view in case of failure
        }


    }
}
