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
        
        // Starting the Job Worker
        using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
        {
	        string jobType = "dotnet-work";
	        zeebeClient.NewWorker()
	                   .JobType(jobType)
	                   .Handler(TriggerApproximation)
	                   .MaxJobsActive(5)
	                   .Name(Environment.MachineName)
	                   .AutoCompletion()
	                   .PollInterval(TimeSpan.FromSeconds(1))
	                   .Timeout(TimeSpan.FromSeconds(10))
	                   .Open();

	        signal.WaitOne();
        }
    }
    
    private static void TriggerApproximation(IJobClient jobClient, IJob job)
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