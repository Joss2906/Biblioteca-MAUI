using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class BookForm : ContentPage
{
	public BookForm(BookFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}