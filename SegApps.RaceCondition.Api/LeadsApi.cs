using System.Text.Json;
using SegApps.RaceCondition.Models;

namespace SegApps.RaceCondition.WebApi;

public static class LeadApi
{
    private const string DataFilePath = @"E:\\Projetos\unisinos\segapps\segapps-race-condition\db.json";
    
    private static readonly object LockObject = new();
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
    
    public static void MapLeadsApiEndpoints(this IEndpointRouteBuilder app)
    {
        MapPost(app);
        MapPostV2(app);
    }

    private static void MapPost(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/leads", async (Lead newLead) =>
        {
            var db = await File.ReadAllTextAsync(DataFilePath);

            var leads = JsonSerializer.Deserialize<List<Lead>>(db) ?? new List<Lead>();

            leads.Add(newLead);
            
            var novoJson = JsonSerializer.Serialize(leads, SerializerOptions );
    
            // Simulação de uma operação externa
            Thread.Sleep(500);
            await File.WriteAllTextAsync(DataFilePath, novoJson);

            return Results.Ok();
        });
    }
    
    private static void MapPostV2(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v2/leads", (Lead newLead) =>
        {
            lock (LockObject)
            {
                var data = File.ReadAllText(DataFilePath);
                var leads = JsonSerializer.Deserialize<List<Lead>>(data) ?? new List<Lead>();
                leads.Add(newLead);
                
                // Simulação de uma operação externa
                Thread.Sleep(500);
                File.WriteAllText(DataFilePath, JsonSerializer.Serialize(leads, SerializerOptions ));
            }

            return Results.Ok();
        });
    }
}