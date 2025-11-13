using System;
using Microsoft.Maui.Controls;

namespace Experiencias_Significativas_App.MAUI.ContentViews
{
    public partial class BottomMenu : ContentView
    {
        public BottomMenu()
        {
            InitializeComponent();
        }

        private async void AnimateButton(ImageButton button)
        {
            await button.ScaleTo(1.2, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }

        private async void OnHomeTapped(object sender, EventArgs e)
        {
            AnimateButton(HomeButton);
            await Shell.Current.GoToAsync($"//HomePage");
        }

        private async void OnFollowUpTapped(object sender, EventArgs e)
        {
            AnimateButton(FollowUpButton);
            await Shell.Current.GoToAsync($"//FollowUpPage");
        }

        private async void OnExperienceTapped(object sender, EventArgs e)
        {
            AnimateButton(ExperienceButton);
            await Shell.Current.GoToAsync($"//ExperiencePage");
        }

        private async void OnProfileTapped(object sender, EventArgs e)
        {
            AnimateButton(ProfileButton);
            await Shell.Current.GoToAsync($"//ProfilePage");
        }
    }
}
