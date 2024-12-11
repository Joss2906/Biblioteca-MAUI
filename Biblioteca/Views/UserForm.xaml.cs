using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class UserForm : ContentPage
{
	public UserForm(UserFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}