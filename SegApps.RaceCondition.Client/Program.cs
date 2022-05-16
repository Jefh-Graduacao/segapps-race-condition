using System.Net.Mime;
using System.Text;
using System.Text.Json;
using SegApps.RaceCondition.Models;
using static System.Console;

var apiEndpoint = "https://localhost:7264/api/";
if (args.Length > 0 && args[0] == "v2")
	apiEndpoint += "v2/";

HttpClient httpClient = new ()
{
	BaseAddress = new Uri(apiEndpoint)
};

WriteLine($"Endpoint usado: {httpClient.BaseAddress}");

var lead1 = new Lead(101, "Joaquim", DateTime.Now);
var lead2 = new Lead(102, "Marcos", DateTime.Now);	

PostLead(lead1);
await Task.Delay(300);
PostLead(lead2);

Thread.Sleep(2000);

WriteLine("Saindo...");

async void PostLead(Lead pessoa)
{
	var json = new StringContent(
		JsonSerializer.Serialize(pessoa),
		Encoding.UTF8,
		MediaTypeNames.Application.Json
	);

	var response = await httpClient.PostAsync("leads", json);
	WriteLine(response.StatusCode);
}