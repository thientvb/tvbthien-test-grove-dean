using GroveDeanAPI.Configurations;
using GroveDeanAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace GroveDeanAPI.Controllers
{
    [Route("api/cat")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string? apiUrl;

        public CatController(CatApiSettings catApiSettings)
        {
            apiUrl = catApiSettings.ApiUrl;
            httpClient = new HttpClient();
        }

        [HttpGet("{endpoint}")]
        public async Task<ActionResult<CatInfo>> GetCatData(string endpoint)
        {
            if (apiUrl != null)
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/{endpoint}");

                    if (response.IsSuccessStatusCode)
                    {
                        CatInfo? catInfo = await response.Content.ReadFromJsonAsync<CatInfo>();
                        return Ok(catInfo);
                    }
                    else
                    {
                        return BadRequest("Failed to fetch data from The Cat API.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("catApiSettings.ApiUrl is null");
            }
        }
    }
}
