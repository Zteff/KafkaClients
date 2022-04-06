using System;
using System.Threading;
using Confluent.Kafka;
using Kafka.Dto;
using Newtonsoft.Json;

namespace KafkaClients.Consumer.Regular
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "user-json-group2",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false
            };

            using (var c = new ConsumerBuilder<Null, string>(conf).Build())
            {
                //c.Subscribe("^meeting-booked|meeting-cancelled");
                c.Subscribe("json-topic");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                            var user = JsonConvert.DeserializeObject<User>(cr.Message.Value);
                            Console.WriteLine($"Name: {user.Name} Foo.Bar: {user.Foo.Bar}");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }

    public class Foo
    {
        public string Bar { get; set; }
    }
}
