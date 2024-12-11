using Biblioteca.ViewModels;

namespace Biblioteca.Views;

public partial class LoanForm : ContentPage
{
	public LoanForm(LoanFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}