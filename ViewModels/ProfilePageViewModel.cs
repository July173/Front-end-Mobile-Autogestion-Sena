using System.Windows.Input;
using Microsoft.Maui.Storage;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSenaMaui.ViewModels;

public class ProfilePageViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private string _userName = string.Empty;
    private string _userEmail = string.Empty;
    private string _roleName = string.Empty;
    private string _userInitials = string.Empty;

    public string UserName
    {
        get => _userName;
        set { _userName = value; OnPropertyChanged(); }
    }
    public string UserEmail
    {
        get => _userEmail;
        set { _userEmail = value; OnPropertyChanged(); }
    }
    public string RoleName
    {
        get => _roleName;
        set { _roleName = value; OnPropertyChanged(); }
    }
    public string UserInitials
    {
        get => _userInitials;
        set { _userInitials = value; OnPropertyChanged(); }
    }

    public ICommand LogoutCommand { get; }
    public ICommand ChangeRoleCommand { get; }
    public ICommand ViewProfileCommand { get; }
    public ICommand EditProfileCommand { get; }
    public ICommand ChangePhotoCommand { get; }

    public ProfilePageViewModel()
    {
        LogoutCommand = new Command(async () => await LogoutAsync());
        ChangeRoleCommand = new Command(async () => await OnChangeRoleAsync());
        ViewProfileCommand = new Command(async () => await OnViewProfileAsync());
        EditProfileCommand = new Command(async () => await OnEditProfileAsync());
        ChangePhotoCommand = new Command(async () => await OnChangePhotoAsync());
    }

    public void LoadUserData()
    {
        try
        {
            var userJson = Preferences.Get("user_data", string.Empty);
            if (string.IsNullOrEmpty(userJson))
            {
                var sec = SecureStorage.GetAsync("user_data").Result;
                userJson = sec ?? string.Empty;
            }

            if (!string.IsNullOrEmpty(userJson))
            {
                var user = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(userJson);
                if (user.TryGetProperty("firstName", out var firstName))
                {
                    var first = firstName.GetString() ?? string.Empty;
                    if (user.TryGetProperty("lastName", out var lastName))
                    {
                        var last = lastName.GetString() ?? string.Empty;
                        UserName = $"{first} {last}".Trim();
                    }
                    else
                    {
                        UserName = first;
                    }
                }
                if (user.TryGetProperty("email", out var email))
                {
                    UserEmail = email.GetString() ?? string.Empty;
                }
                if (user.TryGetProperty("role", out var role))
                {
                    RoleName = role.GetString() ?? string.Empty;
                }

                // Compute initials
                if (!string.IsNullOrEmpty(UserName))
                {
                    var parts = UserName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                        UserInitials = $"{parts[0][0]}{parts[1][0]}".ToUpper();
                    else if (parts.Length == 1)
                        UserInitials = parts[0].Length >= 2 ? parts[0].Substring(0, 2).ToUpper() : parts[0].ToUpper();
                    else
                        UserInitials = "US";
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PROFILE] Error loading user data: {ex}");
        }
    }

    private async Task OnViewProfileAsync()
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Perfil", "Se mostrará el perfil completo (placeholder).", "Aceptar");
    }

    private async Task OnChangeRoleAsync()
    {
        try
        {
            string result = string.Empty;
            if (Application.Current?.MainPage != null)
            {
                result = await Application.Current.MainPage.DisplayActionSheet("Cambiar rol", "Cancelar", null, "Apprentice", "Instructor", "Coordinator", "SofiaOperator");
            }
            if (!string.IsNullOrEmpty(result) && result != "Cancelar")
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Cambiar rol", $"Rol seleccionado: {result} (placeholder)", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PROFILE] Error change role: {ex}");
        }
    }

    private async Task LogoutAsync()
    {
        try
        {
            await NavigationHelper.LogoutAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PROFILE] Error logout: {ex}");
        }
    }

    private async Task OnEditProfileAsync()
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Editar perfil", "Editar perfil (placeholder).", "Aceptar");
    }

    private async Task OnChangePhotoAsync()
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Cambiar foto", "Cambiar foto (placeholder).", "Aceptar");
    }
    #region INotifyPropertyChanged
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
    #endregion
}