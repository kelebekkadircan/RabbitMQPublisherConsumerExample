// CONSUMER
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer is running...");


ConnectionFactory factory = new();

// bağlantı oluşturma
factory.Uri = new("amqps://fkudeoip:aU7HKGiz5pqCZ1YiEDj_PKeJK0dvCKAs@rattlesnake.rmq.cloudamqp.com/fkudeoip");

// bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

// queue oluşturma
await channel.QueueDeclareAsync(queue: "example-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
// publisher ile aynı yapılandırmada olmalıdır.

// queue dinleme
AsyncEventingBasicConsumer eventConsumer = new(channel);
 await channel.BasicConsumeAsync(queue: "example-queue", autoAck: false, consumer: eventConsumer);

 eventConsumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs @event) =>
{
    string message = Encoding.UTF8.GetString(@event.Body.ToArray());
    Console.WriteLine($"Gelen Mesaj: {message}");
    // mesaj işlendiğinde rabbitmq'ya mesajın işlendiğine dair bilgi verilir.
    await channel.BasicAckAsync(@event.DeliveryTag, false);
    // mesaj işlenmediğinde rabbitmq'ya mesajın işlenmediğine dair bilgi verilir.
    //await channel.BasicNackAsync(@event.DeliveryTag,multiple: false,requeue: false);

};

Console.Read();