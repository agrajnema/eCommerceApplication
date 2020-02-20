using EmailNotificationService.Channels;
using EmailNotificationService.Events;
using InfrastructureLibrary;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailNotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        IEmailNotifier _emailNotifier;
        public NotificationManager(IMessageHandler messageHandler, IEmailNotifier emailNotifier)
        {
            _messageHandler = messageHandler;
            _emailNotifier = emailNotifier;

        }
        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            try
            {
                var messageObject = JsonConvert.DeserializeObject<JObject>(message);
                switch (messageType)
                {
                    case "CustomerRegistered":
                        await HandleAsync(messageObject.ToObject<CustomerRegisteredEvent>());
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error while handling: {messageType} event");
            }
            return true;
        }

        private async Task HandleAsync(CustomerRegisteredEvent customerRegisteredEvent)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendLine($"Dear {customerRegisteredEvent.FirstName},");
            mailBody.AppendLine($"Welcome! Your email address: {customerRegisteredEvent.EmailAddress} is registered with us.");

            await _emailNotifier.SendEmailAsync(customerRegisteredEvent.EmailAddress, "noreply@ecommerceapp.com", "Welcome!", mailBody.ToString());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }
    }
}
