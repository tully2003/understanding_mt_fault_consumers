using System;
using BoundedContextA.Messages;
using MassTransit;

namespace BoundedContextA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[BoundedContextA] Initialising");
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/fh"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
            bus.Start();
            Console.WriteLine("[BoundedContextA] Initialised");

            bus.Publish<EventThatOccuredInA>(new { Id = Guid.NewGuid(), OccurenceDate = DateTimeOffset.Now });
            //bus.Publish<EventThatOccuredInA>(new { Id = Guid.NewGuid(), OccurenceDate = DateTimeOffset.Now });

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            bus.Stop();
            Console.WriteLine("[BoundedContextA] Shutting down....");
        }
    }
}
