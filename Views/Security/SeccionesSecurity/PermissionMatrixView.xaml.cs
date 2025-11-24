using System.Collections;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views.Security.SeccionesSecurity
{
    public partial class PermissionMatrixView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(PermissionMatrixView));

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public PermissionMatrixView()
        {
            InitializeComponent();
        }
    }
}
