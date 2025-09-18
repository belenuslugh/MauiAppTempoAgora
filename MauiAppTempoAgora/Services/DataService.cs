using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<(Tempo? tempo, string? errorMessage)> GetPrevisao(string cidade)
        {
            Tempo? t = null;
            string? errorMessage = null;
            string chave = "503dc14357ea7c6b58102d03af9c73c9";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        DateTime sunrise = unixEpoch.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = unixEpoch.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString("HH:mm:ss"),
                            sunset = sunset.ToString("HH:mm:ss"),
                        };
                    }
                    else
                    {
                        switch (resp.StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                errorMessage = "Cidade não encontrada. Verifique o nome e tente novamente.";
                                break;
                            default:
                                errorMessage = $"Erro na requisição: {resp.StatusCode}";
                                break;
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                errorMessage = "Sem conexão com a internet. Verifique sua conexão e tente novamente.";
            }
            catch (TaskCanceledException)
            {
                errorMessage = "Na requisição. Verifique sua conexão e tente novamente.";
            }
            catch (Exception ex)
            {
                errorMessage = $"Erro inesperado: {ex.Message}";
            }

            return (t, errorMessage);
        }
    }
}
