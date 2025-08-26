using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

public class Program
{
    private static readonly String _ClientID = "~eT-aDSlX8f0H~xOgsEsb_ywDYRnxwcr";
    private static readonly String _ClientSecret = "wE.UCkq~byEZDx4l-eiH58pK.pik2qUCl0tj6wWdsslaGls5ZC-WMupkG5p.uvco";
    
    private static readonly String _ClusterId = "05b10b58-5a37-484d-9802-1caf33ee4daf";
    private static readonly string _ClientCloudRegion = "lhr-1";

    private static string ContactPoint => $"{_ClusterId}.{_ClientCloudRegion}.zeebe.camunda.io:443";
    
    
    public static IZeebeClient zeebeClient;
    
    public static async Task Main(string[] args)
    {
        zeebeClient = CamundaCloudClientBuilder
                      .Builder()
                      .UseClientId(_ClientID)
                      .UseClientSecret(_ClientSecret)
                      .UseContactPoint(ContactPoint)
                      .Build();
        
        _ = ReceiveInput();
        
        // Starting the Job Worker
        using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
        {
	        string jobType1 = "dotnet-work" + 1;
	        zeebeClient.NewWorker()
	                   .JobType(jobType1)
	                   .Handler(WorkerHandler_dotnet_work1)
	                   .MaxJobsActive(5)
	                   .Name(Environment.MachineName)
	                   .AutoCompletion()
	                   .PollInterval(TimeSpan.FromSeconds(1))
	                   .Timeout(TimeSpan.FromSeconds(10))
	                   .Open();
	        
	        string jobType2 = "dotnet-work" + 2;
	        zeebeClient.NewWorker()
	                   .JobType(jobType2)
	                   .Handler(WorkerHandler_dotnet_work2)
	                   .MaxJobsActive(5)
	                   .Name(Environment.MachineName)
	                   .AutoCompletion()
	                   .PollInterval(TimeSpan.FromSeconds(1))
	                   .Timeout(TimeSpan.FromSeconds(10))
	                   .Open();

	        signal.WaitOne();
        }
        
       
    }

    private static async Task ReceiveInput()
    {
	    await Task.Run(async () =>
	             {
		             while (true)
		             {
			             Console.WriteLine("Write a message to start the process:");
			             string? processStartMessage = Console.ReadLine();
			             if (processStartMessage != null)
			             {
				             await StartNewWorkflow();
			             }
		             }
	             });

    }
    
    private static async Task StartNewWorkflow()
    {
	    var payload = new {
		    orderId = 123,
		    userId = "abc"
	    };
	    var jPayload = JsonConvert.SerializeObject(payload);
	        
	    await zeebeClient.NewPublishMessageCommand()
	                     .MessageName("start-workflow")          // Must match BPMN message name
	                     .CorrelationKey("business-key-or-similar") // Matches BPMN `Message Catch Event`
	                     .Variables(jPayload)
	                     .Send();
    }
    
    private static void WorkerHandler_dotnet_work1(IJobClient jobClient, IJob job)
    {
	    JObject jsonObject = JObject.Parse(job.Variables);
	    
	    Console.WriteLine("Working on Task"); 
	  
	    jobClient.NewCompleteJobCommand(job.Key)
	             .Variables("""{"status": "Completed", "person":{"name": "george", "age": 25}}""")
	             .Send()
	             .GetAwaiter()
	             .GetResult();
	    Console.WriteLine("Completed the fetched Task");
    }  
    
    private static void WorkerHandler_dotnet_work2(IJobClient jobClient, IJob job)
    {
	    JObject jsonObject = JObject.Parse(job.Variables);
	    
	    Console.WriteLine("Working on Task"); 
	  
	    jobClient.NewCompleteJobCommand(job.Key)
	             .Variables("""{"status": "Completed", "person":{"name": "george", "age": 25}}""")
	             .Send()
	             .GetAwaiter()
	             .GetResult();
	    Console.WriteLine("Completed the fetched Task");
    }  
}