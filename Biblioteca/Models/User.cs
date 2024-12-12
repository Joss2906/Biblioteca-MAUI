using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public int rol_id;
        [ObservableProperty]
        public int status;

        [ObservableProperty]
        public string roles;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string rol;
    }
}
