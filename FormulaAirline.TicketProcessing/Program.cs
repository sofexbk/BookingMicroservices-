using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace FormulaAirline.TicketProcessing
{
    class Program
    {
      static void Main(string[] args)
{
    Console.WriteLine("Welcome to the ticketing service");

    try
    {
        var factory = new ConnectionFactory()
        {
            HostName = "192.168.207.3",
            UserName = "user",
            Password = "mypass",
            VirtualHost = "/"
        };

        using (var conn = factory.CreateConnection())
        using (var channel = conn.CreateModel())
        {
            channel.QueueDeclare("bookings", durable: true, exclusive: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"New Ticket processing is initiated for {message}");
            };

            channel.BasicConsume("bookings", true, consumer);

            Console.WriteLine("Consumer connected and listening for messages...");

            // Keep the application running until explicitly terminated
            while (true)
            {
                // Add a small delay to prevent high CPU usage
                Thread.Sleep(1000);
            }
        }
    }
    catch (OperationInterruptedException ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}


    }
}
