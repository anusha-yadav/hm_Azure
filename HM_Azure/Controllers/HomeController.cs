using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HM_Azure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HM_Azure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            GetSecrets();
            ViewBag.Message = TempData["Message"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void GetSecrets()
        {
            // Key Vault URI (replace with your Key Vault's URI)
            string keyVaultUri = "https://dev-hm-1.vault.azure.net/";

            // Create a SecretClient using DefaultAzureCredential
            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

            try
            {
                // Retrieve a secret by name
                string secretName = "ConnectionStringDev"; // Replace with the name of your secret
                KeyVaultSecret secret = client.GetSecret(secretName);

                // Output the secret value
                Console.WriteLine($"Secret Name: {secretName}");
                Console.WriteLine($"Secret Value: {secret.Value}");
                TempData["Message"] = secret.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving secret: {ex.Message}");
            }
        }
    }
}
