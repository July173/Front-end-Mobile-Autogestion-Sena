using AutogestionSenaMaui.ViewModels;

namespace AutogestionSenaMaui.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageViewModel _vm;

    public ProfilePage()
    {
        InitializeComponent();
        _vm = new ProfilePageViewModel();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadUserData();
    }
}