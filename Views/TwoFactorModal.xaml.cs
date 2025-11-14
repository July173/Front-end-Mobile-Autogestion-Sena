using System;
using Microsoft.Maui.Controls;

namespace AutogestionSena.MAUI.Views
{
    public partial class TwoFactorModal : ContentView
    {
        private bool _isModalVisible;
        private string _email = string.Empty;
        
        public event EventHandler<string>? CodeVerified;
        public event EventHandler? Cancelled;

        public new bool IsVisible
        {
            get => _isModalVisible;
            set
            {
                _isModalVisible = value;
                ModalContainer.IsVisible = value;
                if (value)
                {
                    ClearCode();
                    Code1Entry.Focus();
                }
            }
        }

        public TwoFactorModal()
        {
            InitializeComponent();
            ModalContainer.IsVisible = false;
        }

        public void Show(string email)
        {
            _email = email;
            MessageLabel.Text = $"Código enviado a: {email}";
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        private void ClearCode()
        {
            Code1Entry.Text = string.Empty;
            Code2Entry.Text = string.Empty;
            Code3Entry.Text = string.Empty;
            Code4Entry.Text = string.Empty;
            Code5Entry.Text = string.Empty;
            Code6Entry.Text = string.Empty;
        }

        private void OnCodeEntryChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
            {
                // Auto-avanzar al siguiente campo
                if (entry == Code1Entry && entry.Text.Length == 1)
                    Code2Entry.Focus();
                else if (entry == Code2Entry && entry.Text.Length == 1)
                    Code3Entry.Focus();
                else if (entry == Code3Entry && entry.Text.Length == 1)
                    Code4Entry.Focus();
                else if (entry == Code4Entry && entry.Text.Length == 1)
                    Code5Entry.Focus();
                else if (entry == Code5Entry && entry.Text.Length == 1)
                    Code6Entry.Focus();
            }
        }

        private void OnVerifyClicked(object? sender, EventArgs e)
        {
            var code = $"{Code1Entry.Text}{Code2Entry.Text}{Code3Entry.Text}{Code4Entry.Text}{Code5Entry.Text}{Code6Entry.Text}";
            
            if (code.Length != 6)
            {
                Application.Current?.MainPage?.DisplayAlert("Error", "Por favor ingresa los 6 dígitos del código", "Aceptar");
                return;
            }

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            VerifyButton.IsEnabled = false;

            CodeVerified?.Invoke(this, code);
        }

        public void ShowError(string message)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            VerifyButton.IsEnabled = true;
            
            Application.Current?.MainPage?.DisplayAlert("Error", message, "Aceptar");
        }

        public void ShowSuccess()
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            VerifyButton.IsEnabled = true;
            Hide();
        }

        private void OnCancelClicked(object? sender, EventArgs e)
        {
            Hide();
            Cancelled?.Invoke(this, EventArgs.Empty);
        }
    }
}
