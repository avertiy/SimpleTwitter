using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using SimpleTwitter.Messages.Commands;

namespace SimpleTwitter.Common
{
    public static class NSBHelper
    {
        public const string ENDPOINT_COMMANDBUS = "SimpleTwitter.COMMANDBUS";
        public const string ENDPOINT_WRITESIDE = "SimpleTwitter.WriteSide";
        public const string ENDPOINT_READSIDE = "SimpleTwitter.ReadSide";
        public static IBus BusInstance { get; set; }
        public static ILogger Logger { get; set; }

        public static void UseSimpleConsoleLogger()
        {
            if (Logger != null && Logger is IDisposable)
            {
                ((IDisposable)Logger).Dispose();
            }
            Logger = new SimpleConsoleLogger();
        }

        /// <remarks>
        /// method with such signature is not used CreateAndStart(string endpoint,IContainer container=null)
        /// to avoid referencing Autofac when it is not needed
        /// </remarks>
        public static IBus CreateAndStart(string endpoint, Assembly assembly)
        {
            return CreateAndStart(endpoint, assembly,null);
        }
        public static IBus CreateAndStart(string endpoint, Assembly assembly, IContainer container)
        {
            LogManager.Use<DefaultFactory>().Level(LogLevel.Warn);
            var busConfig = new BusConfiguration();
            busConfig.EndpointName(endpoint);
            if (container != null)
            {
                busConfig.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
            }
            //busConfig.DisableFeature<SecondLevelRetries>();
            busConfig.AssembliesToScan(assembly
                ,Assembly.GetAssembly(typeof(SimpleTwitter.Messages.Commands.Command))
                //, Assembly.GetAssembly(typeof(BaseCommandHandler))
                );
            busConfig.UseSerialization<XmlSerializer>();
            busConfig.UsePersistence<InMemoryPersistence>();
            busConfig.EnableInstallers();
            IStartableBus bus = Bus.Create(busConfig);
            var res = bus.Start();
            if (Logger == null)
            {
                Logger = new SimpleConsoleLogger();
            }
            Logger.Info("\n\rNServiceBus started. EndpointName '{0}'\n\r", endpoint);
            return res;
        }

        public static async Task<T> SendWithCallback<T>(IBus bus, string endpoint, ICommand command)
        {
            Task<T> task = bus.Send(endpoint, command).Register<T>();
            Logger.CommandSent(command, endpoint);
            var res = await task;
            Logger.CommandExecutionResult(res);
            return res;
        }
        public static void Send(IBus bus, string endpoint, ICommand command)
        {
            try
            {
                bus.Send(endpoint, command);
                Logger.CommandSent(command, endpoint);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Bus can't send the message to endpoint: "+endpoint);
            }
            
        }

        public static void Return<T>(IBus bus, T @enum)
        {
            bus.Return(@enum);
            Logger.Return(@enum);
        }
    }

    
}