using Biblioteca.ViewModels;
namespace Biblioteca.Views;

public partial class AuthorForm : ContentPage
{
	public AuthorForm(AuthorFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}