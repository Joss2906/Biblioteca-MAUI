using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public partial class Book : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string title;
        [ObservableProperty]
        public string description;
        [ObservableProperty]
        public string isbn;
        [ObservableProperty]
        public string gender;
        [ObservableProperty]
        public int author_id;
        [ObservableProperty]
        public int status;
    }
}
