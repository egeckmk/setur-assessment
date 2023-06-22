using System.Net.Sockets;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ;

public class RabbitMQPersistentConnection : IDisposable
{
    private readonly IConnectionFactory connectionFactory;
    private readonly int retryCount;
    private IConnection connection;
    private  object lock_object = new object();
    private bool _disposed;
    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
    {
        this.connectionFactory = connectionFactory;
        retryCount = retryCount;
    }
    

    public bool IsConnected => connection != null && connection.IsOpen;

    public IModel CreateModel()
    {
        return connection.CreateModel();
    }
    
    public void Dispose()
    {
        _disposed = true;
        connection.Dispose();
    }

    public bool TryConnect()
    {
        lock (lock_object)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    
                });
            policy.Execute(() =>
            {
                connection = connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                connection.ConnectionShutdown += Connection_ConnectionShutdown;
                connection.CallbackException += Connection_CallbackException;
                connection.ConnectionBlocked += Connection_ConnectionBlocked;
                // log
                return true;
            }
            return false;
        }
    }

    private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        if (_disposed) return;

        // log Connection_ConnectionShutdown
        TryConnect();
    }
    
    private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;

        // log Connection_CallbackException
        TryConnect();
    }
    
    private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;

        // log Connection_ConnectionBlocked
        TryConnect();
    }
}