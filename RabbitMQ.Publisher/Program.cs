// Publisher
using RabbitMQ.Client;
using System.IO.Pipelines;
using System.Text;

ConnectionFactory factory = new();

// bağlantı oluşturma
factory.Uri = new("amqps://fkudeoip:aU7HKGiz5pqCZ1YiEDj_PKeJK0dvCKAs@rattlesnake.rmq.cloudamqp.com/fkudeoip");

// bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = await  factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();


// queue oluşturma
await channel.QueueDeclareAsync(queue: "example-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);



// queue mesaajı gönderme
// rabbitmq mesajları byte türünden kabul eder . mesajları byte dönüştürmemiz gerekir.

var props = new BasicProperties();
props.Persistent = true;


for (int i = 0; i < 20; i++)
{
byte[] mesaj = Encoding.UTF8.GetBytes("Hello World! " + i);
    await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue",mandatory:true,basicProperties: props , body: mesaj);
   await Task.Delay(300);
}

Console.Read();