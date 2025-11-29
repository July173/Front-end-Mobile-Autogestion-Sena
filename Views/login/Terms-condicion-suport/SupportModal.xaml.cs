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
			SizeChanged += OnSizeChanged;
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
				// Use the single ContentStack present in XAML as the container
				var content = FindByName("ContentStack") as VerticalStackLayout;
				if (content == null) return;
				content.Children.Clear();

				// Fetch data
				var contacts = await _generalService.GetSupportContactsAsync();
				var schedules = await _general_service_get_schedules_async();
				var queries = await _general_service_get_queries_async();

				// Debug/status label to help diagnose empty responses
				var statusLabel = new Label { FontSize =12, TextColor = Colors.Gray };
				statusLabel.Text = $"Debug: contacts={contacts?.Count ??0}, schedules={schedules?.Count ??0}, queries={queries?.Count ??0}";
				content.Children.Add(statusLabel);

				bool anyData = (contacts != null && contacts.Any()) || (schedules != null && schedules.Any()) || (queries != null && queries.Any());

				// Contacts section
				content.Children.Add(new Label { Text = "Contactos", FontSize =18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black });
				var contactGrid = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) }, ColumnSpacing =12 };
				int col =0;
				if (contacts != null && contacts.Any())
				{
					foreach (var c in contacts.Take(2))
					{
						var card = CreateContactCard(c);
						contactGrid.Add(card, col,0);
						col++;
					}
				}
				else
				{
					var emailCard = CreateContactCard(new SupportContactDto { Label = "Email", Type = "Soporte", Value = "servicio@sena.edu.co", ExtraInfo = "Respuesta en24-48 horas" });
					var phoneCard = CreateContactCard(new SupportContactDto { Label = "Teléfono", Type = "Línea gratuita", Value = "018000910270", ExtraInfo = "Lunes a viernes:7:00 AM -7:00 PM" });
					contactGrid.Add(emailCard,0,0);
					contactGrid.Add(phoneCard,1,0);
				}
				content.Children.Add(contactGrid);

				// Schedules section
				content.Children.Add(new Label { Text = "Horarios", FontSize =18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black });
				var schedulesList = new VerticalStackLayout { Spacing =6 };
				if (schedules != null && schedules.Any())
				{
					foreach (var s in schedules)
					{
						var row = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
						row.Add(new Label { Text = s.DayRange, FontSize =14, TextColor = Colors.Black },0,0);
						row.Add(new Label { Text = s.Hours, FontSize =14, TextColor = Colors.Black },1,0);
						schedulesList.Children.Add(row);
					}
				}
				else
				{
					var row1 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row1.Add(new Label { Text = "Lunes a Viernes", FontSize =14, TextColor = Colors.Black },0,0);
					row1.Add(new Label { Text = "7:00 AM -7:00 PM", FontSize =14, TextColor = Color.FromArgb("#43A047") },1,0);
					schedulesList.Children.Add(row1);
					var row2 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row2.Add(new Label { Text = "Sábados", FontSize =14, TextColor = Colors.Black },0,0);
					row2.Add(new Label { Text = "8:00 AM -4:00 PM", FontSize =14, TextColor = Color.FromArgb("#43A047") },1,0);
					schedulesList.Children.Add(row2);
					var row3 = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
					row3.Add(new Label { Text = "Domingos y festivos", FontSize =14, TextColor = Colors.Black },0,0);
					row3.Add(new Label { Text = "Cerrado", FontSize =14, TextColor = Colors.Gray },1,0);
					schedulesList.Children.Add(row3);
				}
				content.Children.Add(schedulesList);

				// Query category picker / list
				content.Children.Add(new Label { Text = "Tipo de consulta", FontSize =18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black });
				if (queries != null && queries.Any())
				{
					var picker = new Picker { Title = "Selecciona un tipo" };
					picker.ItemsSource = queries;
					picker.ItemDisplayBinding = new Binding("Name");
					content.Children.Add(picker);
				}
				else
				{
					var fallback = new VerticalStackLayout { Spacing =4 };
					fallback.Children.Add(new Label { Text = "Soporte Técnico", FontSize =14, TextColor = Colors.Black });
					fallback.Children.Add(new Label { Text = "Consulta Académica", FontSize =14, TextColor = Colors.Black });
					content.Children.Add(fallback);
				}

				// Links
				content.Children.Add(new Label { Text = "Enlaces útiles", FontSize =18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black });
				var link = new Label { Text = "Sofia Plus - Oferta Educativa", TextColor = Colors.Black };
				var tap = new TapGestureRecognizer();
				tap.Tapped += (s, e) => Launcher.OpenAsync(new Uri("https://betowa.sena.edu.co/"));
				link.GestureRecognizers.Add(tap);
				content.Children.Add(link);

				// If nothing returned from server show friendly message
				if (!anyData)
				{
					content.Children.Add(new Label { Text = "No hay datos disponibles desde el servidor.", FontSize =14, TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center });
				}
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
			vs.Children.Add(new Label { Text = c.Label, FontAttributes = FontAttributes.Bold, FontSize =14, TextColor = Colors.Black });
			// Mostrar tipo o información extra como subtítulo
			var subtitle = !string.IsNullOrEmpty(c.ExtraInfo) ? c.ExtraInfo : c.Type;
			if (!string.IsNullOrEmpty(subtitle)) vs.Children.Add(new Label { Text = subtitle, FontSize =12, TextColor = Colors.Black });
			vs.Children.Add(new Label { Text = c.Value, FontSize =16, TextColor = Colors.Black });
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
			var nameEntry = FindByName("NameEntry") as Entry;
			var emailEntry = FindByName("EmailEntry") as Entry;
			var messageEditor = FindByName("MessageEditor") as Editor;
			var name = nameEntry?.Text;
			var email = emailEntry?.Text;
			var message = messageEditor?.Text;
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
			{
				Application.Current?.MainPage?.DisplayAlert("Error", "Completa todos los campos obligatorios", "Aceptar");
				return;
			}
			Application.Current?.MainPage?.DisplayAlert("Enviado", "Tu mensaje ha sido enviado", "Aceptar");
		}

		private void OnSizeChanged(object? sender, EventArgs e)
		{
			try
			{
				var w = Width;
				if (w <=0) return;
				var title = FindByName("TitleLabel") as Label;
				var subtitle = FindByName("SubtitleLabel") as Label;
				var frame = FindByName("ModalFrame") as Frame;
				var scroll = FindByName("ContentScroll") as ScrollView;

				if (w <=360)
				{
					if (title != null) title.FontSize =20;
					if (subtitle != null) subtitle.FontSize =12;
					if (frame != null) frame.WidthRequest =320;
					if (scroll != null) scroll.HeightRequest =420;
				}
				else if (w <=420)
				{
					if (title != null) title.FontSize =22;
					if (subtitle != null) subtitle.FontSize =12;
					if (frame != null) frame.WidthRequest =360;
					if (scroll != null) scroll.HeightRequest =480;
				}
				else if (w <=760)
				{
					if (title != null) title.FontSize =24;
					if (subtitle != null) subtitle.FontSize =13;
					if (frame != null) frame.WidthRequest =600;
					if (scroll != null) scroll.HeightRequest =520;
				}
				else
				{
					if (title != null) title.FontSize =24;
					if (subtitle != null) subtitle.FontSize =13;
					if (frame != null) frame.WidthRequest =700;
					if (scroll != null) scroll.HeightRequest =520;
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
