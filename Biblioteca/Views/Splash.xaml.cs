using System.Diagnostics;

namespace Biblioteca.Views;

public partial class Splash : ContentPage
{
	public Splash()
	{
		InitializeComponent();
	}

    private void OnGetStartedClicked(object sender, EventArgs e)
    {
        // Navegar a la p�gina principal
        //await Shell.Current.GoToAsync("//LoansList");
        //Debug.WriteLine("OnGetStartedClicked");
        Application.Current.MainPage = new AppShell();

    }
}