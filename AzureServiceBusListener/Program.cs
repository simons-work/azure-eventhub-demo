namespace AzureServiceBusListener
{
    using AzureServiceBusListener.Models;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using WTW.ESB.Model;

    /* 
     NOTE: 
     There's probably a few ways to do this, you could create an Azure Function/Logic App to run periodically in the cloud and calls On Prem Fenetre Servicing Shell (SS) Web API / TestHarness in my case
     However if Fenetre Web API will be On-Prem, then think this could would be better On-Prem and it probably should be separate to the Fenetre SS Web API
     And it has to be long running, so have done it as  Console App to make the code simpler, but probably should be a Windows Service or other options for running background tasks within Web API do exist
     if you don't want the listener to be separate

     This code came from docs.microsoft.com - step 8 of https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions#receive-messages-from-the-subscription
     Only modification really is to use a concrete type TaskStatusEvent for the payload and to pass this object onto another Web API

    */

    internal static class Program
    {
        #region Local testing with my own subscription
        //const string ServiceBusConnectionString = "Endpoint=sb://wtw-sb-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EhlmSxOK51f12FwYeBX0N0SGinQN3kHyARwgUmr9+Ck=";
        //const string TopicName = "TestTopic";
        //const string SubscriptionName = "TestTopicSubscription";
        #endregion

        const string ServiceBusConnectionString = "Endpoint=sb://crbbro-sh-em20-d-messaging.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ruzVSop0RKsU08lrrHayQyrQSFUNIRPaIFPi9PQJrMQ=";
        const string TopicName = "ServiceHubResponse";
        const string SubscriptionName = "ServiceHubResponseSubscription";
        static ISubscriptionClient subscriptionClient;
        static HttpClient client = new HttpClient();
        const string FenetreServicingShellWebApiBaseAddress = "http://localhost:65305/";

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("logfile.log", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            MainAsync().GetAwaiter().GetResult();

        }

        static async Task MainAsync()
        {
            client.BaseAddress = new Uri(FenetreServicingShellWebApiBaseAddress);
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Register subscription message handler and receive messages in a loop.
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {

            var taskStatusEvent = GetTaskStatusEvent(message);

            #region Dump object to console
            var messageBody = Encoding.UTF8.GetString(message.Body);
            Log.Debug($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber}");
            Log.Debug("");
            Log.Debug($"Message Body:");
            Log.Debug(Dump(messageBody));
            Log.Debug("");
            Log.Debug($"Inner Message body:");
            Log.Debug(Dump(taskStatusEvent));
            Log.Debug($"Posting the message body TaskStatusEvent body to Fenetre Test Harness");
            Log.Debug("");
            Log.Debug("----------------------------------------------------------------------");
            #endregion

            await PostTaskStatusEvent(taskStatusEvent);

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        static async Task<bool> PostTaskStatusEvent(TaskStatusEvent message)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // This calls a Fenetre Servicing Shell Test Harness but it can call any Web API that you decide will contain the logic to handle this event
            HttpResponseMessage response = await client.PostAsJsonAsync("api/serviceHubEvent", message);
            var result = response.IsSuccessStatusCode;
            return result;
        }


        static TaskStatusEvent GetTaskStatusEvent(Message message)
        {
            var taskStatusEvent = new TaskStatusEvent();

            // You would normally do something like DeserializeObject into TaskStatusEvent 
            // but at this point in time Service Hub is sending out a message within a message by mistake and I don't really want to bother 
            // adding these four MessageWrapper classes that wrap the TaskStatusEvent into this project so use dynamic temporarily until I get them to change their code

            var messageBody = Encoding.UTF8.GetString(message.Body);
            var innerMessage = JsonConvert.DeserializeObject<Message<TaskStatusEvent>>(messageBody);
            var entityBody = innerMessage.EntityBody;

            taskStatusEvent.TaskId = entityBody.TaskId;
            taskStatusEvent.TaskTemplate = entityBody.TaskTemplate;
            taskStatusEvent.StatusId = entityBody.StatusId;
            taskStatusEvent.Status = entityBody.Status;
            taskStatusEvent.StatusChangedBy = entityBody.StatusChangedBy;
            taskStatusEvent.StatusChangedOn = entityBody.StatusChangedOn;
            taskStatusEvent.StatusChangeReason = entityBody.StatusChangeReason;
            taskStatusEvent.Comments = entityBody.Comments;
            taskStatusEvent.AdditionalDetails = entityBody.AdditionalDetails;
            return taskStatusEvent;
        }

        public static string Dump(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}