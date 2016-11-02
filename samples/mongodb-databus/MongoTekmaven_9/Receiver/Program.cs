using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.MongoDB;
using NServiceBus.Persistence.MongoDB.DataBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.DataBus.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
        persistence.SetConnectionString("mongodb://localhost:27017/SamplesMongoDBServer");
        var dataBus = endpointConfiguration.UseDataBus<MongoDbDataBus>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}