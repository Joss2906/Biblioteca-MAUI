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
    public partial class BooksListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Book> bookList = new ObservableCollection<Book>();

        

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.34.36:8000/api/")
        };

        public BooksListViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await Get());

            WeakReferenceMessenger.Default.Register<string>(this, async (r, message) =>
            {
                if (message == "ReloadBooks")
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
                var response = await client.GetAsync("books/");
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
                BookList = new ObservableCollection<Book>();
            }
            else
            {
                var jsonObject = JObject.Parse(json);
                var booksArray = jsonObject["books"].ToString();
                var list = JsonConvert.DeserializeObject<Book[]>(booksArray);

                BookList = new ObservableCollection<Book>(list);
            }
        }

        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(BookForm)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Update(Book book)
        {
            var uri = $"{nameof(BookForm)}?id={book.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Delete(Book book)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el libro?", "Si", "No");

            if (answer)
            {
                try
                {
                    var response = await client.DeleteAsync("books/delete/" + book.Id);
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
