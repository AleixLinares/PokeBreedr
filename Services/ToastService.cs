using PokeBreedr.Enums;
using PokeBreedr.Models;
using System.Collections.ObjectModel;

namespace PokeBreedr.Services
{
    public sealed class ToastService
    {
        private const int MaxToasts = 5;
        private const int AnimationDurationMs = 250;

        private readonly ObservableCollection<ToastModel> _toasts = [];

        public IReadOnlyCollection<ToastModel> Toasts => _toasts;

        public event Action? OnChange;

        public Task Success(string message)
            => ShowAsync(message, ToastType.Success);

        public Task Error(string message)
            => ShowAsync(message, ToastType.Error);

        public Task Info(string message)
            => ShowAsync(message, ToastType.Info);

        public async Task ShowAsync(
            string message,
            ToastType type)
        {
            if (_toasts.Count >= MaxToasts)
            {
                var oldest = _toasts.First();

                await RemoveAnimatedAsync(oldest.Id);
            }

            _toasts.Add(new ToastModel
            {
                Message = message,
                Type = type
            });

            NotifyStateChanged();
        }

        public async Task RemoveAnimatedAsync(Guid id)
        {
            var toast = _toasts.FirstOrDefault(x => x.Id == id);

            if (toast is null)
                return;

            if (toast.IsLeaving)
                return;

            toast.IsLeaving = true;

            NotifyStateChanged();

            await Task.Delay(AnimationDurationMs);

            _toasts.Remove(toast);

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
            => OnChange?.Invoke();
    }
}
