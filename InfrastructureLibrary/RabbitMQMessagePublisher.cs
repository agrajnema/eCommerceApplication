using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Polly;

namespace InfrastructureLibrary
{
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly List<string> _hosts;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _exchangeName;
        private readonly string _exchangeType;

        public RabbitMQMessagePublisher(string host, string userName, string password, string exchangeName, string exchangeType)
            : this(new List<string>() { host }, userName, password, exchangeName, exchangeType)

        {
            _hosts = new List<string> { host };
            _userName = userName;
            _password = password;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
        }

        public RabbitMQMessagePublisher(List<string> hosts, string userName, string password, string exchangeName, string exchangeType)
        {
            _hosts = hosts;
            _userName = userName;
            _password = password;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
        }

        public Task PublishMessageAsync(string messageType, object message, string routingKey)
        {
            return Task.Run(() =>
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 5 seconds"); })
                    .Execute(() =>
                        {
                            var factory = new ConnectionFactory() { UserName = _userName, Password = _password };
                            //factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
                            using (var connection = factory.CreateConnection(_hosts))
                            {
                                using (var model = connection.CreateModel())
                                {
                                    model.ExchangeDeclare(_exchangeName, _exchangeType, durable: true, autoDelete: false);
                                    var serializedData = JsonConvert.SerializeObject(message);
                                    var binaryBody = Encoding.UTF8.GetBytes(serializedData);
                                    var properties = model.CreateBasicProperties();
                                    properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
                                    model.BasicPublish(_exchangeName, routingKey, properties, binaryBody);
                                }
                            }
                        }));
        }
    }
}
