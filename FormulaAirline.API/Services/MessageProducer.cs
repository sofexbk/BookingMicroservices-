using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

namespace FormulaAirline.API.Services;

public class MessageProducer:IMessageProducer
{
    public void SendingMessage<T>(T message){
            var factory=new ConnectionFactory()
        {
            HostName="localhost",
            UserName="user",
            Password="mypass",
            VirtualHost="/"
        };
        var conn =factory.CreateConnection();
        using var channel=conn.CreateModel();
        channel.QueueDeclare("bookings",durable:true,exclusive:true);
        var jsonString=JsonSerializer.Serialize(message);
        var body=Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish("","bookings",body:body);
    }
}
