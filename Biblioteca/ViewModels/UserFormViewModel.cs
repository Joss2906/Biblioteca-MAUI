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
using static System.Reflection.Metadata.BlobBuilder;

namespace Biblioteca.ViewModels
{
    public partial class UserFormViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private User user = new User();

        [ObservableProperty]
        private List<Role> roles = new List<Role>();

        [ObservableProperty]
        private Role? selectedRole;

        [ObservableProperty]
        private string? title;

        private int? userId;

        [ObservableProperty]
        private bool loadingIsVisible = false;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.34.36:8000/api/")
        };

        public UserFormViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await GetRoles());
        }

        public async Task GetRoles()
        {
            var json = "";
            try
            {
                var response = await client.GetAsync("users/roles/");
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
                var rolesArray = jsonObject["roles"].ToString();
                var list = JsonConvert.DeserializeObject<List<Role>>(rolesArray);

                Roles = list;
            }
            else
            {
                Roles = new List<Role>();
            }

        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            userId = int.Parse(query["id"].ToString());

            if (userId == 0)
            {
                Title = "Crear usuario";
            }
            else
            {
                Title = "Editar usuario";
                LoadingIsVisible = true;
                await Task.Run(async () =>
                {
                    var json = "";
                    try
                    {
                        var response = await client.GetAsync("users/edit/" + userId);
                        response.EnsureSuccessStatusCode();

                        json = await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    var jsonObject = JObject.Parse(json).ToString();
                    var item = JsonConvert.DeserializeObject<User>(jsonObject);

                    if (item != null)
                    {
                        User = item;
                        SelectedRole = Roles.FirstOrDefault(r => r.Id == item.Rol_id);
                    }
                    LoadingIsVisible = false;
                });
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            User.Rol_id = SelectedRole.Id;

            var json = JsonConvert.SerializeObject(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                if (userId == 0)
                {
                    var response = await client.PostAsync("users/create", content);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    var response = await client.PutAsync("users/update/" + userId, content);
                    response.EnsureSuccessStatusCode();
                }

                WeakReferenceMessenger.Default.Send("ReloadUsers");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
