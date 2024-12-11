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

namespace Biblioteca.ViewModels
{
    public partial class AuthorListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Author> authorList = new ObservableCollection<Author>();

        

        public AuthorListViewModel()
        {
            //provisional mientras se hace el get de la api
            AuthorList.Add(new Author { Id = 1, Name = "Juan", Lastname = "Perez", Address = "Calle 1", City = "Lima" });
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
                AuthorList.Remove(author);
            }
        }
    }
}


