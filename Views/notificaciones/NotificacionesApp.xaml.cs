using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSena.MAUI.Views.notificaciones
{
    public partial class NotificacionesApp : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<NotificationItem> Notifications { get; } = new();
        public ObservableCollection<NotificationItem> DisplayedNotifications { get; } = new();

        private readonly NotificationService _notificationService = new();
        private NotificationWebSocketService? _wsService;

        private int _selectedTab = 0; // 0 = Todas, 1 = Sin leer

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                    OnPropertyChanged(nameof(IsNotLoading));
                }
            }
        }

        public bool IsNotLoading => !IsLoading;

        public NotificacionesApp()
        {
            InitializeComponent();
            BindingContext = this;

            // Inicializar WebSocket service (pero no conectar aún)
            _wsService = new NotificationWebSocketService();
            _wsService.NotificationReceived += OnWsNotificationReceived;
            _wsService.ErrorOccurred += OnWsError;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadNotificationsAsync();

            // Iniciar conexión websocket para recibir notificaciones en tiempo real
            try
            {
                if (_wsService != null && !_wsService.IsConnected)
                {
                    await _wsService.StartAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] WS start error: {ex}");
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                if (_wsService != null)
                {
                    // Desconectar pero mantener instancia para reusar si se vuelve a abrir
                    await _wsService.StopAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] WS stop error: {ex}");
            }
        }

        private async Task LoadNotificationsAsync()
        {
            IsLoading = true;
            try
            {
                Notifications.Clear();

                // Obtener rol e id del usuario desde Preferences
                int role = Preferences.Get("UserRole", 0);
                int userId = Preferences.Get("UserId", 0);

                if (role == 0 || userId == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[Notifications] No se encontró rol o userId en Preferences");
                    // Cargar ejemplo si no hay datos
                    AddSampleData();
                    UpdateDisplayed();
                    UpdateTabVisuals();
                    return;
                }

                // Mapear rol a query param esperado por el backend
                var roleQuery = RoleToQueryParam(role);
                if (string.IsNullOrEmpty(roleQuery))
                {
                    System.Diagnostics.Debug.WriteLine($"[Notifications] Rol desconocido: {role}");
                    AddSampleData();
                    UpdateDisplayed();
                    UpdateTabVisuals();
                    return;
                }

                var dtos = await _notificationService.GetNotificationsAsync(roleQuery, userId);
                if (dtos == null) dtos = new System.Collections.Generic.List<NotificationDto>();

                foreach (var d in dtos)
                {
                    // Solo mostrar notificaciones activas
                    if (!d.Active) continue;

                    Notifications.Add(new NotificationItem
                    {
                        Id = d.Id.ToString(),
                        Title = d.Title,
                        Message = d.Message,
                        Timestamp = d.CreatedAt,
                        IsRead = d.IsRead,
                        Active = d.Active
                    });
                }

                UpdateDisplayed();
                UpdateTabVisuals();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] Error cargando notificaciones: {ex}");
                AddSampleData();
                UpdateDisplayed();
                UpdateTabVisuals();
            }
            finally
            {
                IsLoading = false;
                // Ensure RefreshView stops
                try
                {
                    var refresh = this.FindByName<RefreshView>("NotificationsRefresh");
                    if (refresh != null) refresh.IsRefreshing = false;
                }
                catch { }
            }
        }

        private string RoleToQueryParam(int role)
        {
            // Mapeo según la documentación proporcionada
            return role switch
            {
                1 => "admin_id",
                2 => "apprentice_id",
                3 => "instructor_id",
                4 => "coordinator_id",
                5 => "sofia_operator_id",
                _ => string.Empty
            };
        }

        private void AddSampleData()
        {
            Notifications.Add(new NotificationItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Nuevo registro de aprendiz",
                Message = "Se ha registrado un nuevo aprendiz: Brayan Cortes",
                Timestamp = DateTime.Parse("2025-11-26T21:47:48"),
                IsRead = false,
                Active = true
            });

            Notifications.Add(new NotificationItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Informe semanal disponible",
                Message = "Tu informe semanal ya está listo para descarga.",
                Timestamp = DateTime.Now.AddHours(-5),
                IsRead = true,
                Active = true
            });
        }

        // Texto resumen
        public string SummaryText => $"{Notifications.Count(n => !n.IsRead)} notificaciones sin leer";

        public string TodasTabText => $"Todas ({Notifications.Count})";
        public string SinLeerTabText => $"Sin leer ({Notifications.Count(n => !n.IsRead)})";

        // Propiedades para cambiar apariencia de pestañas (simple)
        public string TabTodasBackground => _selectedTab == 0 ? "#E3F2FD" : "#FFFFFF";
        public string TabSinLeerBackground => _selectedTab == 1 ? "#E3F2FD" : "#FFFFFF";
        public string TabTodasTextColor => _selectedTab == 0 ? "#1976D2" : "#333";
        public string TabSinLeerTextColor => _selectedTab == 1 ? "#1976D2" : "#333";

        // Nuevas propiedades para controlar sombra en pestañas
        public bool TabTodasHasShadow => _selectedTab == 0;
        public bool TabSinLeerHasShadow => _selectedTab == 1;

        // Se usa para determiner color de borde
        public Color BorderColor => Colors.LightGray;

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void UpdateDisplayed()
        {
            DisplayedNotifications.Clear();
            var items = _selectedTab == 0 ? Notifications : Notifications.Where(n => !n.IsRead);
            foreach (var it in items) DisplayedNotifications.Add(it);

            OnPropertyChanged(nameof(SummaryText));
            OnPropertyChanged(nameof(TodasTabText));
            OnPropertyChanged(nameof(SinLeerTabText));
        }

        private void UpdateTabVisuals()
        {
            OnPropertyChanged(nameof(TabTodasBackground));
            OnPropertyChanged(nameof(TabSinLeerBackground));
            OnPropertyChanged(nameof(TabTodasTextColor));
            OnPropertyChanged(nameof(TabSinLeerTextColor));
            // Notificar cambios de sombra
            OnPropertyChanged(nameof(TabTodasHasShadow));
            OnPropertyChanged(nameof(TabSinLeerHasShadow));
        }

        // Eventos de pestañas
        private void OnTabTodasClicked(object sender, EventArgs e)
        {
            _selectedTab = 0;
            UpdateDisplayed();
            UpdateTabVisuals();
            // Notify bindings
            OnPropertyChanged(nameof(TabTodasBackground));
            OnPropertyChanged(nameof(TabSinLeerBackground));
            OnPropertyChanged(nameof(TabTodasTextColor));
            OnPropertyChanged(nameof(TabSinLeerTextColor));
        }

        private void OnTabSinLeerClicked(object sender, EventArgs e)
        {
            _selectedTab = 1;
            UpdateDisplayed();
            UpdateTabVisuals();
            OnPropertyChanged(nameof(TabTodasBackground));
            OnPropertyChanged(nameof(TabSinLeerBackground));
            OnPropertyChanged(nameof(TabTodasTextColor));
            OnPropertyChanged(nameof(TabSinLeerTextColor));
        }

        // Acciones
        private async void OnMarkAllReadClicked(object sender, EventArgs e)
        {
            IsLoading = true;
            try
            {
                // El servidor maneja el marcado como leido al obtener la notificacion individual
                var unread = Notifications.Where(n => !n.IsRead).ToList();
                foreach (var n in unread)
                {
                    if (int.TryParse(n.Id, out var nid))
                    {
                        try
                        {
                            var dto = await _notificationService.MarkAsReadAsync(nid);
                            if (dto != null) n.IsRead = dto.IsRead;
                        }
                        catch { }
                    }
                }

                UpdateDisplayed();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void OnDeleteAllClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Eliminar", "¿Eliminar todas las notificaciones?", "Sí", "No");
            if (!answer) return;

            IsLoading = true;
            try
            {
                int role = Preferences.Get("UserRole", 0);
                int userId = Preferences.Get("UserId", 0);
                var roleQuery = RoleToQueryParam(role);
                if (string.IsNullOrEmpty(roleQuery)) return;

                var ok = await _notificationService.DeleteNotificationsByUserAsync(roleQuery, userId);
                if (ok)
                {
                    Notifications.Clear();
                    UpdateDisplayed();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] Error al eliminar todas: {ex}");
                await DisplayAlert("Error", "No se pudo eliminar las notificaciones.", "Aceptar");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 0)
                Navigation.PopModalAsync();
            else
                Navigation.PopAsync();
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string id)
            {
                if (!int.TryParse(id, out var nid))
                {
                    // si el id no es numérico, eliminar localmente
                    var itemLocal = Notifications.FirstOrDefault(x => x.Id == id);
                    if (itemLocal != null) Notifications.Remove(itemLocal);
                    UpdateDisplayed();
                    return;
                }

                var item = Notifications.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    var ok = await DisplayAlert("Eliminar", "¿Eliminar esta notificación?", "Sí", "No");
                    if (!ok) return;

                    IsLoading = true;
                    try
                    {
                        var success = await _notificationService.DeleteNotificationByIdAsync(nid);
                        if (success)
                        {
                            Notifications.Remove(item);
                            UpdateDisplayed();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Notifications] Error al eliminar notificación: {ex}");
                        await DisplayAlert("Error", "No se pudo eliminar la notificación.", "Aceptar");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }
            }
        }

        // Pull-to-refresh handler
        private async void OnRefreshRequested(object sender, EventArgs e)
        {
            try
            {
                var refresh = this.FindByName<RefreshView>("NotificationsRefresh");
                if (refresh != null) refresh.IsRefreshing = true;
                await LoadNotificationsAsync();
            }
            finally
            {
                try
                {
                    var refresh2 = this.FindByName<RefreshView>("NotificationsRefresh");
                    if (refresh2 != null) refresh2.IsRefreshing = false;
                }
                catch { }
            }
        }

        // Evento WS: cuando llega una notificación en tiempo real
        private void OnWsNotificationReceived(object? sender, NotificationDto dto)
        {
            if (dto == null) return;

            // Ignorar notificaciones no activas
            if (!dto.Active) return;

            // Mapear y añadir al inicio de la lista en el hilo UI
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var item = new NotificationItem
                {
                    Id = dto.Id.ToString(),
                    Title = dto.Title,
                    Message = dto.Message,
                    Timestamp = dto.CreatedAt,
                    IsRead = dto.IsRead,
                    Active = dto.Active
                };

                // Insertar al inicio
                Notifications.Insert(0, item);
                UpdateDisplayed();
            });
        }

        private void OnWsError(object? sender, string message)
        {
            System.Diagnostics.Debug.WriteLine($"[WS] Error: {message}");
        }

        // Nuevo handler: tap en notificación -> marcar como leída (GET /general/notifications/{id}/)
        private async void OnNotificationTapped(object? sender, EventArgs e)
        {
            try
            {
                if (sender is VisualElement ve && ve.BindingContext is NotificationItem item)
                {
                    if (string.IsNullOrEmpty(item.Id)) return;

                    if (!int.TryParse(item.Id, out var nid))
                    {
                        // id no numérico: marcar localmente
                        item.IsRead = true;
                        UpdateDisplayed();
                        return;
                    }

                    IsLoading = true;
                    try
                    {
                        var dto = await _notificationService.MarkAsReadAsync(nid);
                        if (dto != null)
                        {
                            // actualizar item en la lista
                            var existing = Notifications.FirstOrDefault(n => n.Id == item.Id);
                            if (existing != null)
                            {
                                existing.IsRead = dto.IsRead;
                            }

                            UpdateDisplayed();

                            // Actualizar contador en MainLayout (TopBar)
                            try { MainLayoutHelper.UpdateNotificationCount(Notifications.Count(n => !n.IsRead)); } catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Notifications] Error marking as read: {ex}");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] OnNotificationTapped error: {ex}");
            }
        }

        // Volver al dashboard
        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            try
            {
                await NavigationHelper.NavigateToDashboardAsync(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifications] Error navigating to dashboard: {ex}");
            }
        }
    }

    // Modelo para UI
    public class NotificationItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool Active { get; set; } = true;

        public string TimestampText => Timestamp.ToString("dd/MM/yyyy, HH:mm:ss");
    }
}