using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Polly;
using System.Threading.Tasks;

namespace InfrastructureLibrary
{
    public class RabbitMQMessageHandler : IMessageHandler
    {
        private readonly List<string> _hosts;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly string _queueName;
        private readonly string _routingKey;
        private IConnection _connection;
        private IModel _model;
        private AsyncEventingBasicConsumer _consumer;
        private string _consumerTag;
        private IMessageHandlerCallback _callback;

        public RabbitMQMessageHandler(string host, string userName, string password, string exchangeName, string exchangeType, string queueName, string routingKey)
            : this(new List<string>() { host }, userName, password, exchangeName, exchangeType, queueName, routingKey)

        {
            _hosts = new List<string> { host };
            _userName = userName;
            _password = password;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _queueName = queueName;
            _routingKey = routingKey;
        }

        public RabbitMQMessageHandler(List<string> hosts, string userName, string password, string exchangeName, string exchangeType, string queueName, string routingKey)
        {
            _hosts = hosts;
            _userName = userName;
            _password = password;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _queueName = queueName;
            _routingKey = routingKey;
        }
        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 5 seconds"); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { UserName = _userName, Password = _password, DispatchConsumersAsync = true };
                    _connection = factory.CreateConnection(_hosts);
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_exchangeName, _exchangeType, durable: true, autoDelete: false);
                    _model.QueueDeclare(_queueName, durable: true, autoDelete: false, exclusive: false);
                    _model.QueueBind(_queueName, _exchangeName, _routingKey);
                    _consumer = new AsyncEventingBasicConsumer(_model);
                    _consumer.Received += Consumer_Received;
                    _consumerTag = _model.BasicConsume(_queueName, false, _consumer);
                });
        }

        public void Stop()
        {
            _model.BasicCancel(_consumerTag);
            _model.Close(200, "Stopped");
            _connection.Close();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            if(await HandleEvent(@event))
            {
                _model.BasicAck(@event.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs @event)
        {
            var messageType = Encoding.UTF8.GetString((byte[])@event.BasicProperties.Headers["MessageType"]);
            var body = Encoding.UTF8.GetString(@event.Body);
            return _callback.HandleMessageAsync(messageType, body);
        }

       
    }
}
