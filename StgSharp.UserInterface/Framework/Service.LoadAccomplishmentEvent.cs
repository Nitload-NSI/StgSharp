namespace StgSharp.UserInterface
{
    public sealed record ServiceLoadAccomplishmentEvent
    {

        public string? Message { get; init; }

        public ServiceHost? Host { get; init; }

        public bool IsLoaded { get; init; }

        public string ServiceName { get; init; } = string.Empty;

    }
}
