using System;
using System.Linq;
using System.Reflection;
using Autofac;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.ReadSide.Data;
using SimpleTwitter.ReadSide.Data.DTOs;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide
{
    class Program
    {
        public const int REDIS_DATABASE_NUMBER = 0;
        public const string REDIS_CONFIGURTION = "localhost:6379,connectTimeout=5000";
        static void Main(string[] args)
        {
            Console.WriteLine("SimpleTwitter.ReadSide starting..");
            NSBHelper.UseSimpleConsoleLogger();
            try
            {
                using (IBus bus = NSBHelper.CreateAndStart(NSBHelper.ENDPOINT_READSIDE,Assembly.GetExecutingAssembly(), BuidAutofacContainer()))
                {
                    Console.WriteLine("SimpleTwitter.ReadSide is ready");
                    Console.WriteLine("Press <Enter> to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CRITICAL ERROR has occurred:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine();
                }
                Console.WriteLine("The application will be closed.");
                Console.ReadLine();
            }
            finally
            {
                if (ReadSideService.Connection != null)
                {
                    ReadSideService.Connection.Dispose();
                }
            } 
        }
        

        private static IContainer BuidAutofacContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(NSBHelper.Logger);
            //todo initialize redis configuration from app.config 
            builder.RegisterInstance(ReadSideService.GetInstance(REDIS_CONFIGURTION, REDIS_DATABASE_NUMBER));
            return builder.Build();
        }
      
    }
}
