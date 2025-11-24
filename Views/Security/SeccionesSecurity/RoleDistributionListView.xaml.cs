using System.Collections;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views.Security.SeccionesSecurity
{
    public partial class RoleDistributionListView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RoleDistributionListView));

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public RoleDistributionListView()
        {
            InitializeComponent();
        }
    }
}
