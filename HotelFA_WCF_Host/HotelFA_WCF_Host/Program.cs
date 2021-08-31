using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelFA_WCF;
using System.ServiceModel;
using System.ServiceModel.Description;
using HotelFA_WCF;

namespace HotelFA_WCF_Host
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServiceHost host = new ServiceHost(typeof(HotelFA_WCF.WorkClasses));
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            host.Description.Behaviors.Add(behavior);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), new Uri("http://localhost:8080/HotelFA_WCF/mex"));
            host.Open();

            Console.WriteLine("Служба запущенна");

            Console.ReadLine();

            host.Close();


            //ServiceHost host = new ServiceHost(typeof(HotelFA_WCF.WorkClasses));
            ////ServiceHost host = new ServiceHost(typeof(HotelFA_WCF.WorkClasses), new Uri("http://localhost:8733/Design_Time_Addresses/HotelFA_WCF/Service1/"));
            //host.AddServiceEndpoint(typeof(HotelFA_WCF.IWorkClasses), new BasicHttpBinding(), "");
            //host.Open();
            //Console.WriteLine("Служба запущена!");
            //Console.ReadLine();
            //host.Close();
        }
    }
}
