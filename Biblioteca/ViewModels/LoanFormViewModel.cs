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
using System.Net;

namespace Biblioteca.ViewModels
{
    public partial class LoanFormViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private Loan loan = new Loan();

        [ObservableProperty]
        private List<Book> books = new List<Book>();

        [ObservableProperty]
        private Book? selectedBook;

        [ObservableProperty]
        private List<User> users = new List<User>();

        [ObservableProperty]
        private User? selectedUser;

        [ObservableProperty]
        private string? title;

        private int? loanId;

        [ObservableProperty]
        private bool loadingIsVisible = false;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.8:8000/api/")
        };

        public LoanFormViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await GetBooks());
            MainThread.BeginInvokeOnMainThread(async () => await GetUsers());
        }

        public async Task GetBooks()
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

            if (!string.IsNullOrEmpty(json))
            {
                var jsonObject = JObject.Parse(json);
                var booksArray = jsonObject["books"].ToString();
                var list = JsonConvert.DeserializeObject<List<Book>>(booksArray);

                Books = list;
            } else
            {
                Books = new List<Book>();
            }
        }

        public async Task GetUsers()
        {
            var json = "";
            try
            {
                var response = await client.GetAsync("users/");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(json);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(json))
            {
                var jsonObject = JObject.Parse(json);
                var booksArray = jsonObject["users"].ToString();
                var list = JsonConvert.DeserializeObject<List<User>>(booksArray);

                Users = list;
            }
            else
            {
                Users = new List<User>();
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            loanId = int.Parse(query["id"].ToString());

            if (loanId == 0)
            {
                Title = "Crear préstamo";
            }
            else
            {
                Title = "Editar préstamo";
                LoadingIsVisible = true;
                await Task.Run(async () =>
                {
                    var json = "";
                    try
                    {
                        var response = await client.GetAsync("loans/edit/" + loanId);
                        response.EnsureSuccessStatusCode();

                        json = await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    var jsonObject = JObject.Parse(json).ToString();
                    var item = JsonConvert.DeserializeObject<Loan>(jsonObject);

                    if (item != null)
                    {
                        Loan = item;
                        SelectedBook = Books.FirstOrDefault(b => b.Id == item.Book_id);
                        SelectedUser = Users.FirstOrDefault(u => u.Id == item.User_id);
                    }
                    LoadingIsVisible = false;
                }); 
            }
        }
        [RelayCommand]
        private async Task Save()
        {
            Loan.Book_id = SelectedBook.Id;
            Loan.User_id = SelectedUser.Id;

            var json = JsonConvert.SerializeObject(Loan);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                if (loanId == 0)
                {
                    var response = await client.PostAsync("loans/create", content);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    var response = await client.PutAsync("loans/update/" + loanId, content);
                    response.EnsureSuccessStatusCode();
                }

                WeakReferenceMessenger.Default.Send("ReloadLoans");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
