using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class AuthorsList : ContentPage
{
	public AuthorsList(AuthorListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}