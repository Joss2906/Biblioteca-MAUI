using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Models;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommunityToolkit.Mvvm.Messaging;

namespace Biblioteca.ViewModels
{
    public partial class AuthorFormViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private Author author = new Author();

        [ObservableProperty]
        private string? title;

        private int? authorId;

        [ObservableProperty]
        private bool loadingIsVisible = false;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.8:8000/api/")
        };

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            authorId = id;

            if (authorId == 0)
            {
                Title = "Crear autor";
            }
            else
            {
                Title = "Editar autor";
                LoadingIsVisible = true;
                await Task.Run(async () =>
                {
                    var json = "";
                    try
                    {
                        var response = await client.GetAsync("authors/edit/" + authorId);
                        response.EnsureSuccessStatusCode();

                        json = await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    var jsonObject = JObject.Parse(json).ToString();
                    //var authorsArray = jsonObject["authors"].ToString();
                    var item = JsonConvert.DeserializeObject<Author>(jsonObject);
                    if (item != null)
                    {
                        Author = item;
                    }
                    LoadingIsVisible = false;

                });
            }
        }
        [RelayCommand]
        private async Task Save()
        {
            //var id = authorId;
            var json = JsonConvert.SerializeObject(Author);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {  
                if (authorId == 0)
                {
                    var response = await client.PostAsync("authors/create", content);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    var response = await client.PutAsync("authors/update/" + authorId, content);
                    response.EnsureSuccessStatusCode();
                }
                //MessagingCenter.Send(this, "reload");

                WeakReferenceMessenger.Default.Send("ReloadAuthors");

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            await Shell.Current.Navigation.PopAsync();
        }
    }
}
