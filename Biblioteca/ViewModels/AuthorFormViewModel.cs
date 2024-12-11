using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Models;
using CommunityToolkit.Mvvm.Input;

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
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
            }
        }
        [RelayCommand]
        private async Task Save()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
