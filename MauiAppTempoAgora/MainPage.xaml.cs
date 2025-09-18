using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    var (tempo, errorMessage) = await DataService.GetPrevisao(txt_cidade.Text.Trim());

                    if (tempo != null)
                    {
                        string labelResultado = $"Latitude: {tempo.lat}\n" +
                                              $"Longitude: {tempo.lon}\n" +
                                              $"Clima: {tempo.main} - {tempo.description}\n" +
                                              $"Temp. Min: {tempo.temp_min}°C - Temp. Max: {tempo.temp_max}°C\n" +
                                              $"Velocidade do Vento: {tempo.speed} m/s\n" +
                                              $"Visibilidade: {tempo.visibility} m\n" +
                                              $"Nascer do Sol: {tempo.sunrise}\n" +
                                              $"Pôr do Sol: {tempo.sunset}\n";
                        lbl_res.Text = labelResultado;
                    }
                    else if (!string.IsNullOrEmpty(errorMessage))
                    {
                        await DisplayAlert("Erro", errorMessage, "OK");
                        lbl_res.Text = "";
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Não foi possível obter os dados meteorológicos.", "OK");
                        lbl_res.Text = "";
                    }
                }
                else
                {
                    await DisplayAlert("Atenção", "Por favor, insira o nome de uma cidade.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro inesperado: {ex.Message}", "OK");
                lbl_res.Text = "";
            }
        }
    }
}
