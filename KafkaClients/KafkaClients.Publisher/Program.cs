using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Kafka.Dto;
using Newtonsoft.Json;

namespace KafkaClients.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            var pb = new ProducerBuilder<Null, string>(config);
            using (var p = pb.Build())
            {
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                     
                    var user = new User {Name = "Foobar", Age = 66, Foo = new Foo {Bar = DateTime.Now.ToShortDateString()}};
                    var json = JsonConvert.SerializeObject(user);
                    var dr = await p.ProduceAsync("json-topic", new Message<Null, string> { Value = json });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
