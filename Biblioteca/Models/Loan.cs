using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public partial class Loan : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public int book_id;
        [ObservableProperty]
        public int user_id;
        [ObservableProperty]
        public int status;

    }
}
