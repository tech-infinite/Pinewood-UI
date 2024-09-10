using CustomersUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomersUI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly string _urlAPI = "";

        public CustomersController(ILogger<CustomersController> logger)
        {
            

        public async Task<IActionResult> Edit()
        {
            return View();
        }

        public async Task<IActionResult> Delete()
        {
            return View();
        }

        
    }
}
