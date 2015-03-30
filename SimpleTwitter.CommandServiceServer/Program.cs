using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using SimpleTwitter.Common;
//Install-Package NServiceBus -ProjectName SimpleTwitter.Messages

namespace SimpleTwitter.CommandServiceBus
{
    class Program
    {
        const string WcfHostUri = "http://localhost:8001/wcfcommandservice/";
        static void Main(string[] args)
        {
            Console.WriteLine("SimpleTwitter.CommandServiceBus application starting..");
            NSBHelper.UseSimpleConsoleLogger();
            NSBHelper.BusInstance = NSBHelper.CreateAndStart(NSBHelper.ENDPOINT_COMMANDBUS,Assembly.GetExecutingAssembly());

            //debug sending commands
            //var cmd = new CreateUserCommand
            //{
            //    UserName = "user11",
            //    UserId = new Guid("00000000-0000-0000-0000-000000000001")
            //};

            //for (int i = 0; i < 5; i++)
            //{
            //    NSBHelper.Send(NSBHelper.BusInstance, NSBHelper.ENDPOINT_WRITESIDE, cmd);
            //    Console.ReadLine();
            //}
            //return;
            Uri baseAddress = new Uri(WcfHostUri);
            using (ServiceHost host = new ServiceHost(typeof(WcfCommandService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = {PolicyVersion = PolicyVersion.Policy15}
                };
                host.Description.Behaviors.Add(smb);
                try
                {
                    host.Open();
                    Console.WriteLine("CommandServiceBus is ready");
                    Console.WriteLine("WCF services exposed at {0}", baseAddress);
                    Console.WriteLine("Press <Enter> to stop the server.");
                    Console.ReadLine();
                    host.Close();
                }
                catch (CommunicationException ex)
                {
                    Console.WriteLine("Critical error [communication problem] has occured: " + ex.Message);
                    Console.ReadLine();
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
                finally
                {
                    if (NSBHelper.BusInstance != null)
                    {
                        NSBHelper.BusInstance.Dispose();
                    }
                } 
            }
        }
       

    }
}
