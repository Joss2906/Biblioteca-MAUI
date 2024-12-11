using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Models;
using Biblioteca.Views;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CommunityToolkit.Mvvm.Messaging;


namespace Biblioteca.ViewModels
{
    public partial class AuthorListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Author> authorList = new ObservableCollection<Author>();
                
        //json_object json_object = new json_object();
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.8:8000/api/")
        };

        public AuthorListViewModel()
        {
            //provisional mientras se hace el get de la api
            //AuthorList.Add(new Author { Id = 1, Name = "Juan", Lastname = "Perez", Address = "Calle 1", City = "Lima" });

            MainThread.BeginInvokeOnMainThread(async () => await Get());

            WeakReferenceMessenger.Default.Register<string>(this, async (r, message) =>
            {
                if (message == "ReloadAuthors")
                {
                    await Get();
                }
            });
        }

        public async Task Get()
        {
            var json = "";
            try
            {
                var response = await client.GetAsync("authors/");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            if (string.IsNullOrEmpty(json))
            {
                //return Array.Empty<Author>();
                AuthorList = new ObservableCollection<Author>();
            } else
            {
                var jsonObject = JObject.Parse(json);
                var authorsArray = jsonObject["authors"].ToString();
                var list = JsonConvert.DeserializeObject<Author[]>(authorsArray);

                AuthorList = new ObservableCollection<Author>(list);
            }
        }

        
        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(AuthorForm)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Update(Author author)
        {
            var uri = $"{nameof(AuthorForm)}?id={author.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Delete(Author author)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el autor?", "Si", "No");

            if (answer)
            {
                try
                {
                    var response = await client.DeleteAsync("authors/delete/" + author.Id);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                await Get();
            }
        }
    }
}


