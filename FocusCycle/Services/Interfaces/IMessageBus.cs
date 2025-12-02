namespace FocusCycle.Services.Interfaces
{
    interface IMessageBus
    {
        IDisposable RegisterHandler<T>(Action<T> handler);
        void Send<T>(object? sender, T message);
    }
}
