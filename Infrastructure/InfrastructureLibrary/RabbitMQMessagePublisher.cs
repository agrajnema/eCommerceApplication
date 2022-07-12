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
        private const int DEFAULT_PORT = 5672;
        private readonly List<string> _hosts;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private IConnection _connection;
        private IModel _model;

        public RabbitMQMessagePublisher(string host, string userName, string password, string exchangeName, string exchangeType)
            : this(new List<string>() { host }, userName, password, exchangeName, exchangeType, DEFAULT_PORT)

        {
        }

        public RabbitMQMessagePublisher(string host, string userName, string password, string exchangeName, string exchangeType, int port)
            : this(new List<string>() { host }, userName, password, exchangeName, exchangeType, port)

        {
        }

        public RabbitMQMessagePublisher(List<string> hosts, string userName, string password, string exchangeName, string exchangeType)
            : this(hosts, userName, password, exchangeName, exchangeType, DEFAULT_PORT)

        {
        }

        public RabbitMQMessagePublisher(List<string> hosts, string userName, string password, string exchangeName, string exchangeType, int port)
        {
            _hosts = hosts;
            _port = port;
            _userName = userName;
            _password = password;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;

            var logMessage = new StringBuilder();
            logMessage.AppendLine("Create RabbitMQ message-publisher instance using config:");
            logMessage.AppendLine($" - Hosts: {string.Join(',', _hosts.ToArray())}");
            logMessage.AppendLine($" - Port: {_port}");
            logMessage.AppendLine($" - UserName: {_userName}");
            logMessage.AppendLine($" - Password: {new string('*', _password.Length)}");
            logMessage.Append($" - Exchange Name: {_exchangeName}");
            logMessage.Append($" - Exchange Type: {_exchangeType}");
            Console.WriteLine(logMessage.ToString());

            Connect();
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="message">The message to publish.</param>
        /// <param name="routingKey">The routingkey to use (RabbitMQ specific).</param>
        public Task PublishMessageAsync(string messageType, object message, string routingKey)
        {
            return Task.Run(() =>
            {
                string data = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(data);
                IBasicProperties properties = _model.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
                _model.BasicPublish(_exchangeName, routingKey, properties, body);
            });
        }

        private void Connect()
        {
            //Policy
            //    .Handle<Exception>()
            //    .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 5 sec."); })
            //    .Execute(() =>
            //    {
            try
            {
                var factory = new ConnectionFactory() { UserName = _userName, Password = _password, Port = _port };
                factory.AutomaticRecoveryEnabled = true;
                _connection = factory.CreateConnection(_hosts);
                _model = _connection.CreateModel();
                _model.ExchangeDeclare(_exchangeName, _exchangeType, durable: true, autoDelete: false);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           //     });
        }

        public void Dispose()
        {
            _model?.Dispose();
            _model = null;
            _connection?.Dispose();
            _connection = null;
        }

        ~RabbitMQMessagePublisher()
        {
            Dispose();
        }
    }
}
