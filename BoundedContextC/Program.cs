using System;
using System.Threading.Tasks;
using BoundedContextA.Messages;
using MassTransit;

namespace BoundedContextC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[BoundedContextC] Initialising");
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/fh"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "queue_c", ec =>
                {
                    ec.Consumer<EventsConsumer>();
                });
            });
            bus.Start();
            Console.WriteLine("[BoundedContextC] Initialised");


            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            bus.Stop();
            Console.WriteLine("[BoundedContextC] Shutting down....");
        }
    }

    public class EventsConsumer : IConsumer<EventThatOccuredInA>, IConsumer<Fault<EventThatOccuredInA>>
    {
        public Task Consume(ConsumeContext<EventThatOccuredInA> context)
        {
            Console.WriteLine($"[BoundedContextC]:[EventThatOccuredInContextA] => Id = {context.Message.Id}");
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<Fault<EventThatOccuredInA>> context)
        {
            Console.WriteLine($"[BoundedContextC]:[Fault<EventThatOccuredInContextA>] => Id = {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
