using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundedContextA.Messages;
using MassTransit;

namespace BoundedContextB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[BoundedContextB] Initialising");
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/fh"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "queue_b", ec =>
                {
                    ec.Consumer<EventsConsumer>();
                });
            });
            bus.Start();
            Console.WriteLine("[BoundedContextB] Initialised");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            bus.Stop();
            Console.WriteLine("[BoundedContextB] Shutting down....");
        }
    }

    public class EventsConsumer : IConsumer<EventThatOccuredInA>, IConsumer<Fault<EventThatOccuredInA>>
    {
        public Task Consume(ConsumeContext<EventThatOccuredInA> context)
        {
            throw new Exception("exception thrown when consuming event in context b");

            Console.WriteLine($"[BoundedContextB]:[EventThatOccuredInContextA] => Id = {context.Message.Id}");
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<Fault<EventThatOccuredInA>> context)
        {
            Console.WriteLine($"[BoundedContextB]:[Fault<EventThatOccuredInContextA>] => Id = {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
