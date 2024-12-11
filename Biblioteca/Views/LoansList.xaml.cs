using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class LoansList : ContentPage
{
	public LoansList(LoansListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}