using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public partial class Role : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string rol;
    }
}
