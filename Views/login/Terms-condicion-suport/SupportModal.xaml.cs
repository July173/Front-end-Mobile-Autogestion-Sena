using System;
using System.Linq;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos.General;
using System.Collections.Generic;

namespace AutogestionSena.MAUI.Views
{
	public partial class SupportModalView : ContentView
	{
		private readonly GeneralService _generalService;
		public SupportModalView()
		{
			InitializeComponent();
			_generalService = new GeneralService();
		}

		public void Show()
		{
			ModalContainer.IsVisible = true;
			_ = ShowOverlayAsync();
			LoadData();
		}

		public void Hide()
		{
			_ = HideOverlayAsync();
		}

		private async System.Threading.Tasks.Task ShowOverlayAsync()
		{
			try
			{
				ModalContainer.Opacity =0;
				ModalFrame.Scale =0.97;
				ModalContainer.IsVisible = true;
				await ModalContainer.FadeTo(1,160);
				await ModalFrame.ScaleTo(1.0,180, Easing.CubicOut);
			}
			catch (Exception)
			{
				ModalContainer.IsVisible = true;
			}
		}

		private async System.Threading.Tasks.Task HideOverlayAsync()
		{
			try
			{
				await ModalFrame.ScaleTo(0.98,140, Easing.CubicIn);
				await ModalContainer.FadeTo(0,140);
				ModalContainer.IsVisible = false;
			}
			catch (Exception)
			{
				ModalContainer.IsVisible = false;
			}
		}

		private async void LoadData()
		{
			try
			{
				ContactGrid.Children.Clear();
				SchedulesList.Children.Clear();
				LinksList.Children.Clear();
				CategoryPicker.ItemsSource = null;
				var contacts = await _generalService.GetSupportContactsAsync();
				var schedules = await _general_service_get_schedules_async();
				var queries = await _general_service_get_queries_async();
				int col = 0;
				if (contacts != null && contacts.Any())
				{
					foreach (var c in contacts.Take(2))
					{
						var card = CreateContactCard(c);
						ContactGrid.Add(card, col, 0);
						col++;
					}
				}
				else
				{
					var emailCard = CreateContactCard(new SupportContactDto { Label = "Email", Type = "Soporte", Value = "servicio@sena.edu.co", ExtraInfo = "Respuesta en 24-48 horas" });
					var phoneCard = CreateContactCard(new SupportContactDto { Label = "Teléfono", Type = "Línea gratuita", Value = "01 8000 910 270", ExtraInfo = "Lunes a viernes: 7:00 AM - 7:00 PM" });
					ContactGrid.Add(emailCard, 0, 0);
					ContactGrid.Add(phoneCard, 1, 0);
				}
				if (schedules != null && schedules.Any())
				{
					foreach (var s in schedules)
					{
						var row = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
						row.Add(new Label { Text = s.DayRange, FontSize = 14, TextColor = Colors.Black }, 0, 0);
						row.Add(new Label { Text = s.Hours, FontSize = 14, TextColor = Color.FromArgb("#43A047") }, 1, 0);
						SchedulesList.Children.Add(row);
					}
				}
				else
				{
					var row1 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row1.Add(new Label { Text = "Lunes a Viernes", FontSize = 14, TextColor = Colors.Black }, 0, 0);
					row1.Add(new Label { Text = "7:00 AM - 7:00 PM", FontSize = 14, TextColor = Color.FromArgb("#43A047") }, 1, 0);
					SchedulesList.Children.Add(row1);
					var row2 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row2.Add(new Label { Text = "Sábados", FontSize = 14, TextColor = Colors.Black }, 0, 0);
					row2.Add(new Label { Text = "8:00 AM - 4:00 PM", FontSize = 14, TextColor = Color.FromArgb("#43A047") }, 1, 0);
					SchedulesList.Children.Add(row2);
					var row3 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row3.Add(new Label { Text = "Domingos y festivos", FontSize = 14, TextColor = Colors.Black }, 0, 0);
					row3.Add(new Label { Text = "Cerrado", FontSize = 14, TextColor = Colors.Gray }, 1, 0);
					SchedulesList.Children.Add(row3);
				}
				if (queries != null && queries.Any())
				{
					CategoryPicker.ItemsSource = queries;
					CategoryPicker.ItemDisplayBinding = new Binding("Name");
				}
				else
				{
					CategoryPicker.ItemsSource = new List<string> { "Soporte Técnico", "Consulta Académica", "Problemas con la plataforma", "Otros" };
				}
				var link = new Label { Text = "Sofia Plus - Oferta Educativa", TextColor = Color.FromArgb("#374151") };
				var tap = new TapGestureRecognizer();
				tap.Tapped += (s, e) => Launcher.OpenAsync(new Uri("https://betowa.sena.edu.co/"));
				link.GestureRecognizers.Add(tap);
				LinksList.Children.Add(link);
			}
			catch (Exception ex)
			{
				await Application.Current?.MainPage?.DisplayAlert("Error", ex.Message, "Aceptar");
			}
		}

		private Frame CreateContactCard(SupportContactDto c)
		{
			var frame = new Frame { CornerRadius =8, Padding =12, BackgroundColor = Colors.White, BorderColor = Color.FromArgb("#f0f0f0"), HasShadow = false };
			var vs = new VerticalStackLayout { Spacing =4 };
			vs.Children.Add(new Label { Text = c.Label, FontAttributes = FontAttributes.Bold, FontSize =14 });
			// Mostrar tipo o informaci�n extra como subt�tulo
			var subtitle = !string.IsNullOrEmpty(c.ExtraInfo) ? c.ExtraInfo : c.Type;
			if (!string.IsNullOrEmpty(subtitle)) vs.Children.Add(new Label { Text = subtitle, FontSize =12, TextColor = Color.FromArgb("#757575") });
			vs.Children.Add(new Label { Text = c.Value, FontSize =16, TextColor = Color.FromArgb("#f57c00") });
			frame.Content = vs;
			return frame;
		}

		private System.Threading.Tasks.Task<List<SupportScheduleDto>?> _general_service_get_schedules_async()
		{
			return _generalService.GetSupportSchedulesAsync();
		}

		private System.Threading.Tasks.Task<List<TypeOfQueryDto>?> _general_service_get_queries_async()
		{
			return _generalService.GetTypeOfQueriesAsync();
		}

		private void OnCloseClicked(object sender, EventArgs e)
		{
			Hide();
		}

		private void OnSubmitClicked(object sender, EventArgs e)
		{
			var name = NameEntry.Text;
			var email = EmailEntry.Text;
			var message = MessageEditor.Text;
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
			{
				Application.Current?.MainPage?.DisplayAlert("Error", "Completa todos los campos obligatorios", "Aceptar");
				return;
			}
			Application.Current?.MainPage?.DisplayAlert("Enviado", "Tu mensaje ha sido enviado", "Aceptar");
		}
	}
}
