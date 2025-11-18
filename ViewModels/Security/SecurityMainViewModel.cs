using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutogestionSenaMaui.ViewModels.Security;

public class SecurityMainViewModel : INotifyPropertyChanged
{
    private string _selectedTab;
    private View _currentContent;

    public SecurityMainViewModel()
    {
        // Inicialización por defecto
        _selectedTab = "Resumen";
        UsersCount = 2;
        RolesCount = 2;
        ModulesCount = 2;
        FormsCount = 3;

        // Comando para cambiar de tab
        SelectTabCommand = new Command<string>(OnTabSelected);

        // Cargar contenido inicial
        LoadContent(_selectedTab);
    }

    #region Properties

    public int UsersCount { get; set; }
    public int RolesCount { get; set; }
    public int ModulesCount { get; set; }
    public int FormsCount { get; set; }

    public string SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab != value)
            {
                _selectedTab = value;
                OnPropertyChanged();
                LoadContent(value);
            }
        }
    }

    public View CurrentContent
    {
        get => _currentContent;
        set
        {
            if (_currentContent != value)
            {
                _currentContent = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Commands

    public ICommand SelectTabCommand { get; }

    #endregion

    #region Methods

    private void OnTabSelected(string tabName)
    {
        SelectedTab = tabName;
    }

    private void LoadContent(string tabName)
    {
        // Aquí cargaremos diferentes vistas según el tab seleccionado
        CurrentContent = tabName switch
        {
            "Resumen" => new Views.Security.SecuritySummaryView(),
            "Usuarios" => new Views.Security.SecurityUsersView(),
            "Roles" => new Views.Security.SecurityRolesView(),
            "Módulos" => new Views.Security.SecurityModulesView(),
            _ => new Views.Security.SecuritySummaryView()
        };
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
