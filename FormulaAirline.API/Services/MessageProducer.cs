using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace FormulaAirline.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory _factory;

        public MessageProducer()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "192.168.207.3",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };
        }

        public void SendingMessage<T>(T message)
        {
            try
            {
                using var conn = _factory.CreateConnection();
                using var channel = conn.CreateModel();
                channel.QueueDeclare("bookings", durable: true, exclusive: false);
                
                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);

                channel.BasicPublish("", "bookings", body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}
