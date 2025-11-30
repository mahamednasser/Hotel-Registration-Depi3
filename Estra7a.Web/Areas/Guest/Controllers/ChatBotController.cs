using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class ChatBotController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl = "https://api.groq.com/openai/v1/chat/completions";

        public ChatBotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] string message)
        {
            try
            {
                var apiKey = _configuration["Groq_ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    return Json(new { success = false, error = "API key not configured" });
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    var requestBody = new
                    {
                        model = "llama-3.1-8b-instant",
                        messages = new[]
                        {
                            new { role = "system", content = "You are a helpful assistant." },
                            new { role = "user", content = message }
                        },
                        temperature = 0.7
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(responseString);
                        string botReply = result.choices[0].message.content;

                        return Json(new { success = true, reply = botReply });
                    }
                    else
                    {
                        return Json(new { success = false, error = "API request failed: " + responseString });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
