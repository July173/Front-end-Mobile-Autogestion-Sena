using AutogestionSena.MAUI.Views;

namespace AutogestionSena.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Iniciamos la app en la pantalla de Login
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
