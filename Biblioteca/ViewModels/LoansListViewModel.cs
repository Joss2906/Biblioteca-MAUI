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
    public partial class LoansListViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Loan> loanList = new ObservableCollection<Loan>();

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.8:8000/api/")
        };

        public LoansListViewModel()
        {
            MainThread.BeginInvokeOnMainThread(async () => await Get());

            WeakReferenceMessenger.Default.Register<string>(this, async (r, message) =>
            {
                if (message == "ReloadLoans")
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
                var response = await client.GetAsync("loans/");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();

            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (string.IsNullOrEmpty(json))
            {
                LoanList = new ObservableCollection<Loan>();
            } else
            {
                var jsonObject = JObject.Parse(json);
                var loansArray = jsonObject.GetValue("loans").ToString();
                var list = JsonConvert.DeserializeObject<Loan[]>(loansArray);

                LoanList = new ObservableCollection<Loan>(list);
            }
        }

        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(LoanForm)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Update(Loan loan)
        {
            var uri = $"{nameof(LoanForm)}?id={loan.id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Delete(Loan loan)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el prestamo?", "Si", "No");

            if (answer)
            {
                try
                {
                    var response = await client.DeleteAsync("loans/delete/" + loan.Id);
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
