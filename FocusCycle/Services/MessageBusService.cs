using FocusCycle.Services.Interfaces;
using System.Diagnostics;

namespace FocusCycle.Services
{
    internal class MessageBusService : IMessageBus
    {
        private class Subscription<T>(MessageBusService bus, Action<T> handler) : IDisposable
        {
            private readonly WeakReference<MessageBusService> _bus = new(bus);

            public Action<T> Handler { get; } = handler;

            public void Dispose()
            {
                if (!_bus.TryGetTarget(out var bus)) return;
                var @lock = bus._lock;
                @lock.EnterWriteLock();
                Type messageType = typeof(T);
                try
                {
                    if (!bus._subscriptions.TryGetValue(messageType, out var refs)) return;
                    List<WeakReference> updatedRefs = refs.Where(i => i.IsAlive).ToList();
                    WeakReference? currentRef = null;
                    foreach (var @ref in updatedRefs)
                    {
                        if (ReferenceEquals(@ref.Target, this))
                        {
                            currentRef = @ref;
                            break;
                        }
                    }
                    if (currentRef != null)
                    {
                        updatedRefs.Remove(currentRef);
                        bus._subscriptions[messageType] = updatedRefs;
                    }
                }
                finally { @lock.ExitWriteLock(); }
            }
        }

        private readonly Dictionary<Type, IEnumerable<WeakReference>> _subscriptions = [];
        private readonly ReaderWriterLockSlim _lock = new();

        public IDisposable RegisterHandler<T>(Action<T> handler)
        {
            var subscription = new Subscription<T>(this, handler);
            _lock.EnterWriteLock();
            try
            {
                WeakReference subscriptionRef = new(subscription);
                var messageType = typeof(T);
                _subscriptions[messageType] = _subscriptions.TryGetValue(messageType, out var subscriptions)
                    ? subscriptions.Append(subscriptionRef)
                    : [subscriptionRef];
            }
            finally { _lock.ExitWriteLock(); }
            return subscription;
        }

        private List<Action<T>>? GetActions<T>()
        {
            List<Action<T>> actions = [];
            Type messageType = typeof(T);
            bool existDieRefs = false;
            _lock.EnterReadLock();
            try
            {
                if (!_subscriptions.TryGetValue(messageType, out var subscriptions))
                    return null;
                foreach (var subscription in subscriptions)
                    if (subscription.Target is Subscription<T> { Handler: var handler })
                        actions.Add(handler);
                    else
                        existDieRefs = true;
            }
            finally { _lock.ExitReadLock(); }
            if (!existDieRefs) return actions;
            _lock.EnterWriteLock();
            try
            {
                if (_subscriptions.TryGetValue(messageType, out var subscriptions))
                    if (subscriptions.Where(i => i.IsAlive).ToArray() is { Length: > 0 } newSubscriptions)
                        _subscriptions[messageType] = newSubscriptions;
                    else
                        _subscriptions.Remove(messageType);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return actions;
        }

        public void Send<T>(object? sender, T message)
        {
            IEnumerable<Action<T>>? handlers = GetActions<T>();
            if (handlers is not null)
            {
                foreach (var handler in handlers)
                {
                    if (!ReferenceEquals(handler.Target, sender))
                    {
                        try { handler(message); }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Handler error: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
