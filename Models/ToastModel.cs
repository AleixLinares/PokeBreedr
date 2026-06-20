using PokeBreedr.Enums;

namespace PokeBreedr.Models
{
    public sealed class ToastModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Message { get; init; } = string.Empty;

        public ToastType Type { get; init; }

        public bool IsLeaving { get; set; }

    }
}
