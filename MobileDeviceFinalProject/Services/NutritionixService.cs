using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Services
{

    public class NutritionixService
    {
        private static readonly HttpClient _httpClient = new();

     
        private const string ApiKey = "mvRgTynkO20PpenVVgdsIefO88dfhgchdkVQp3ZD";
        private const string SearchEndpoint = "https://api.nal.usda.gov/fdc/v1/foods/search";

        // Nutrient IDs in USDA FoodData Central
        private const int NutrientIdCalories = 1008;
        private const int NutrientIdProtein  = 1003;
        private const int NutrientIdCarbs    = 1005;
        private const int NutrientIdFat      = 1004;

        /// <summary>
        /// Searches the USDA FoodData Central for foods matching <paramref name="query"/>.
        /// Returns up to 10 results. Throws on network or API errors.
        /// </summary>
        public async Task<List<NutritionResult>> SearchFoodAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return [];

            var url = $"{SearchEndpoint}?query={HttpUtility.UrlEncode(query)}&pageSize=10&api_key={ApiKey}";

            using var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"USDA FoodData Central error {(int)response.StatusCode}: {response.ReasonPhrase}. {body}");
            }

            var json = await response.Content.ReadFromJsonAsync<UsdaSearchResponse>();
            if (json?.Foods == null || json.Foods.Count == 0)
                return [];

            var results = new List<NutritionResult>();

            foreach (var food in json.Foods)
            {
                double GetNutrient(int id) =>
                    food.FoodNutrients?.FirstOrDefault(n => n.NutrientId == id)?.Value ?? 0;

                results.Add(new NutritionResult
                {
                    FoodName   = food.Description ?? string.Empty,
                    Calories   = GetNutrient(NutrientIdCalories),
                    ProteinG   = GetNutrient(NutrientIdProtein),
                    CarbsG     = GetNutrient(NutrientIdCarbs),
                    FatG       = GetNutrient(NutrientIdFat),
                    ServingQty = 100,
                    ServingUnit = "g"
                });
            }

            return results;
        }

        // ── USDA response shapes ──────────────────────────────────────────────

        private sealed class UsdaSearchResponse
        {
            [JsonPropertyName("foods")]
            public List<UsdaFood>? Foods { get; set; }
        }

        private sealed class UsdaFood
        {
            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("foodNutrients")]
            public List<UsdaNutrient>? FoodNutrients { get; set; }
        }

        private sealed class UsdaNutrient
        {
            [JsonPropertyName("nutrientId")]
            public int NutrientId { get; set; }

            [JsonPropertyName("value")]
            public double Value { get; set; }
        }
    }
}
