using Biblioteca.Views;
namespace Biblioteca
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new Views.Splash();

            //Routing.RegisterRoute(nameof(AuthorsList), typeof(AuthorsList));
            //Routing.RegisterRoute(nameof(BooksList), typeof(BooksList));
            //Routing.RegisterRoute(nameof(LoansList), typeof(LoansList));
            //Routing.RegisterRoute(nameof(Splash), typeof(Splash));

            //Shell.Current.GoToAsync("//Splash");
        }
    }
}
