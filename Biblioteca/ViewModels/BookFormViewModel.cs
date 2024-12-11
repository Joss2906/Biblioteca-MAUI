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
    public partial class BookFormViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private Book book = new Book();

        [ObservableProperty]
        private List<Author> authors = new List<Author>();

        [ObservableProperty]
        private Author? selectedAuthor;

        [ObservableProperty]
        private string? title;

        private int? bookId;

        [ObservableProperty]
        private bool loadingIsVisible = false;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.8:8000/api/")
        };

        public BookFormViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await GetAuthors());
        }

        public async Task GetAuthors()
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

            if (!string.IsNullOrEmpty(json))
            {
                var jsonObject = JObject.Parse(json);
                var authorsArray = jsonObject["authors"].ToString();
                var list = JsonConvert.DeserializeObject<List<Author>>(authorsArray);

                Authors = list;
            } else
            {
                Authors = new List<Author>();
            }
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            bookId = id;

            if (bookId == 0)
            {
                Title = "Crear libro";
            }
            else
            {
                Title = "Editar libro";
                LoadingIsVisible = true;
                await Task.Run(async () =>
                {
                    var json = "";
                    try
                    {
                        var response = await client.GetAsync("books/edit/" + bookId);
                        response.EnsureSuccessStatusCode();

                        json = await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    var jsonObject = JObject.Parse(json).ToString();
                    //var authorsArray = jsonObject["authors"].ToString();
                    var item = JsonConvert.DeserializeObject<Book>(jsonObject);
                   Console.WriteLine(item);
                    if (item != null)
                    {
                        Book = item;

                        SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.Author_id);
                        SelectedAuthor.Fullname = $"{SelectedAuthor.Name} {SelectedAuthor.Lastname}";
                    }
                    LoadingIsVisible = false;
                });
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            Book.Author_id = SelectedAuthor.Id;
            var json = JsonConvert.SerializeObject(Book);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                if (bookId == 0)
                {
                    var response = await client.PostAsync("books/create", content);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    var response = await client.PutAsync("books/update/" + bookId, content);
                    response.EnsureSuccessStatusCode();
                }

                WeakReferenceMessenger.Default.Send("ReloadBooks");

            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            await Shell.Current.Navigation.PopAsync();
        }   


    }
}
