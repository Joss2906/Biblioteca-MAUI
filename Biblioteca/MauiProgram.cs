using Biblioteca.ViewModels;
using Biblioteca.Views;
using Microsoft.Extensions.Logging;

namespace Biblioteca
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<Splash>();
            builder.Services.AddTransient<AuthorsList>();
            builder.Services.AddTransient<AuthorForm>();
            builder.Services.AddTransient<BooksList>();
            builder.Services.AddTransient<BookForm>();
            builder.Services.AddTransient<LoansList>();
            builder.Services.AddTransient<LoanForm>();
            builder.Services.AddTransient<UserList>();
            builder.Services.AddTransient<UserForm>();

            builder.Services.AddTransient<AuthorListViewModel>();
            builder.Services.AddTransient<BooksListViewModel>();
            builder.Services.AddTransient<LoansListViewModel>();
            builder.Services.AddTransient<UserListViewModel>();
            builder.Services.AddTransient<AuthorFormViewModel>();
            builder.Services.AddTransient<BookFormViewModel>();
            builder.Services.AddTransient<LoanFormViewModel>();
            builder.Services.AddTransient<UserFormViewModel>();

            Routing.RegisterRoute(nameof(AuthorsList), typeof(AuthorsList));
            Routing.RegisterRoute(nameof(BooksList), typeof(BooksList));
            Routing.RegisterRoute(nameof(LoansList), typeof(LoansList));
            Routing.RegisterRoute(nameof(Splash), typeof(Splash));
            Routing.RegisterRoute(nameof(AuthorForm), typeof(AuthorForm));
            Routing.RegisterRoute(nameof(BookForm), typeof(BookForm));
            Routing.RegisterRoute(nameof(LoanForm), typeof(LoanForm));
            Routing.RegisterRoute(nameof(UserList), typeof(UserList));
            Routing.RegisterRoute(nameof(UserForm), typeof(UserForm));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
