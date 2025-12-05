using System.Net.Http.Json;

namespace GoalFlow.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<(string Title, string Content)> GetDataForCategory(string category)
        {
            try
            {
                switch (category)
                {
                    case "Finance":
                        // API: Frankfurter (Free, No Key). Exchange rate USD to EUR.
                        var finData = await _client.GetFromJsonAsync<ExchangeRateResponse>("https://api.frankfurter.app/latest?from=USD&to=EUR");
                        if (finData != null && finData.rates.ContainsKey("EUR"))
                        {
                            return ("Exchange Rate", $"1 USD = {finData.rates["EUR"]} EUR");
                        }
                        return ("Exchange Rate", "Data unavailable");

                    case "Health":
                        // API: Open-Meteo (Free, No Key). Weather for Sofia (User context).
                        var weatherData = await _client.GetFromJsonAsync<WeatherResponse>("https://api.open-meteo.com/v1/forecast?latitude=42.6977&longitude=23.3219&current_weather=true");
                        return ("Weather in Sofia", $"{weatherData?.current_weather.temperature}°C, Wind: {weatherData?.current_weather.windspeed} km/h");

                    case "Personal":
                        // API: DummyJSON Quotes (Free, No Key). Motivational thought.
                        var quoteData = await _client.GetFromJsonAsync<QuoteResponse>("https://dummyjson.com/quotes/random");
                        return ("Daily Motivation", $"\"{quoteData?.quote}\"\n- {quoteData?.author}");

                    case "Education":
                        // API: UselessFacts (Free, No Key). Random fact.
                        var factData = await _client.GetFromJsonAsync<FactResponse>("https://uselessfacts.jsph.pl/random.json?language=en");
                        string text = factData?.text ?? "Data unavailable";
                        return ("Did you know?", text);

                    default:
                        return ("GoalFlow", "Keep pushing towards your goals!");
                }
            }
            catch (Exception)
            {
                return ("Offline", "Data unavailable. Keep going!");
            }
        }

        // Helper classes for JSON deserialization
        public class ExchangeRateResponse { public required Dictionary<string, double> rates { get; set; } }
        public class QuoteResponse { public required string quote { get; set; } public required string author { get; set; } }
        public class WeatherResponse { public required CurrentWeather current_weather { get; set; } }
        public class CurrentWeather { public double temperature { get; set; } public double windspeed { get; set; } }
        public class FactResponse { public required string text { get; set; } }
    }
}