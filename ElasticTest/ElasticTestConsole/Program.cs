using Elastic.Channels;
using Elastic.Clients.Elasticsearch;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using ElasticTestConsole.Models;
using Serilog;
using System.Threading;

Console.WriteLine("Starting up...");
var client = new HttpClient();
var response = await client.GetAsync("http://elasticsearch:9200/");

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new[] { new Uri("http://elastic:elasticPassword@elasticsearch:9200/") }, options =>
//    {
//        options.DataStream = new DataStreamName("logs", "elastic-test", "test");
//        options.BootstrapMethod = BootstrapMethod.Failure;
//        options.ConfigureChannel = config =>
//        {
//            config.BufferOptions = new BufferOptions
//            {
//                ExportMaxConcurrency = 10
//            };
//        };
//    })
//    .MinimumLevel.Is(Serilog.Events.LogEventLevel.Verbose)
//    .CreateLogger();

Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

var elasticClientSettings = new ElasticsearchClientSettings(new Uri("http://elasticsearch:9200/"))
    .Authentication(new BasicAuthentication("elastic", "elasticPassword"));

var elasticClient = new ElasticsearchClient(elasticClientSettings);
var resp = await elasticClient.Indices.CreateAsync("test_index");

await Parallel.ForAsync(0, 100, async (i, cancellationToken) =>
{
    Console.WriteLine($"Processing request {i}...");

    var response = await client.GetAsync("https://en.wikipedia.org/wiki/Special:Random", cancellationToken);

    Console.WriteLine($"Finished processing request {i} to {response.RequestMessage?.RequestUri}");

    var content = await response.Content.ReadAsStringAsync();
    var document = new WikipediaDocument()
    {
        Uri = response.RequestMessage.RequestUri.ToString(),
        Content = content
    };

    var elasticResponse = await elasticClient.IndexAsync(document, (IndexName)"test_index");
});

await Task.Delay(5000);