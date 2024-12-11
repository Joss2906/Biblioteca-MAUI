using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class UserList : ContentPage
{
	public UserList(UserListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}