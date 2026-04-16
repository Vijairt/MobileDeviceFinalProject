using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Services
{
    /// <summary>
    /// Service for querying the Nutritionix API for food nutrition data.
    /// Register your own credentials at https://developer.nutritionix.com/
    /// </summary>
    public class NutritionixService
    {
        private static readonly HttpClient _httpClient = new();

        // Replace with your own Nutritionix API credentials
        private const string AppId = "YOUR_APP_ID";
        private const string AppKey = "YOUR_APP_KEY";
        private const string NutrientsEndpoint = "https://trackapi.nutritionix.com/v2/natural/nutrients";

        /// <summary>
        /// Searches Nutritionix for nutrition data matching <paramref name="query"/>.
        /// Returns an empty list when offline or when the API call fails.
        /// </summary>
        public async Task<List<NutritionResult>> SearchFoodAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return [];

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, NutrientsEndpoint);
                request.Headers.Add("x-app-id", AppId);
                request.Headers.Add("x-app-key", AppKey);
                request.Headers.Add("x-remote-user-id", "0");
                request.Content = JsonContent.Create(new { query });

                using var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return [];

                var json = await response.Content.ReadFromJsonAsync<NutritionixResponse>();
                if (json?.Foods == null)
                    return [];

                return json.Foods.Select(f => new NutritionResult
                {
                    FoodName = f.FoodName,
                    Calories = f.Calories,
                    ProteinG = f.ProteinG,
                    CarbsG = f.CarbsG,
                    FatG = f.FatG,
                    ServingQty = f.ServingQty,
                    ServingUnit = f.ServingUnit
                }).ToList();
            }
            catch
            {
                return [];
            }
        }

        private sealed class NutritionixResponse
        {
            [JsonPropertyName("foods")]
            public List<NutritionixFood>? Foods { get; set; }
        }

        private sealed class NutritionixFood
        {
            [JsonPropertyName("food_name")]
            public string FoodName { get; set; } = string.Empty;

            [JsonPropertyName("nf_calories")]
            public double Calories { get; set; }

            [JsonPropertyName("nf_protein")]
            public double ProteinG { get; set; }

            [JsonPropertyName("nf_total_carbohydrate")]
            public double CarbsG { get; set; }

            [JsonPropertyName("nf_total_fat")]
            public double FatG { get; set; }

            [JsonPropertyName("serving_qty")]
            public double ServingQty { get; set; }

            [JsonPropertyName("serving_unit")]
            public string ServingUnit { get; set; } = string.Empty;
        }
    }
}
