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
    public partial class UserListViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<User> userList = new ObservableCollection<User>();

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.34.36:8000/api/")
        };

        public UserListViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await Get());

            WeakReferenceMessenger.Default.Register<string>(this, async (r, message) =>
            {
                if (message == "ReloadUsers")
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
                var response = await client.GetAsync("users/");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (string.IsNullOrEmpty(json))
            {
                UserList = new ObservableCollection<User>();
            } else
            {
                var jsonObject = JObject.Parse(json);
                var userArray = jsonObject.GetValue("users").ToString();
                var list = JsonConvert.DeserializeObject<User[]>(userArray);

                UserList = new ObservableCollection<User>(list);
            }
        }

        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(UserForm)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }
        [RelayCommand]
        private async Task Update(User user)
        {
            var uri = $"{nameof(UserForm)}?id={user.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Delete(User user)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el usuario?", "Si", "No");

            if (answer)
            {
                try
                {
                    var response = await client.DeleteAsync("users/delete/" + user.Id);
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
