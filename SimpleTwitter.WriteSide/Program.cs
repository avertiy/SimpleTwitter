using System;
using System.Reflection;
using Autofac;
using NServiceBus;
using NServiceBus.Logging;
using SimpleTwitter.Common;
using SimpleTwitter.WriteSide.Data;

namespace SimpleTwitter.WriteSide
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SimpleTwitter.WriteSide application starting..");
            NSBHelper.UseSimpleConsoleLogger();
            try
            {
                using (IBus bus = NSBHelper.CreateAndStart(NSBHelper.ENDPOINT_WRITESIDE,Assembly.GetExecutingAssembly(), ConfigureAutofacContainer()))
                {
                    Console.WriteLine("SimpleTwitter.WriteSide is ready");
                    Console.WriteLine("Press <Enter> to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical error has occured: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine();
                }
                Console.ReadLine();
            }
        }

        static IContainer ConfigureAutofacContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            //DI inject WriteSideUnitOfWork into CommandHandlers
            builder.RegisterInstance(NSBHelper.Logger);
            builder.RegisterInstance(new WriteSideUnitOfWork(new WriteSideDbFakeDbContext()));
            return builder.Build();
        }
    }
}
