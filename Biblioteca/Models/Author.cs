using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Biblioteca.Models
{
    public partial class Author : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string lastname;
        [ObservableProperty]
        public string address;
        [ObservableProperty]
        public string city;
    }
}
