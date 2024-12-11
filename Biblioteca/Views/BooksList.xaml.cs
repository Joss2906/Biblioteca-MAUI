using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class BooksList : ContentPage
{
	public BooksList(BooksListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}