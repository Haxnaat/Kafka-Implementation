using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<KafkaProducer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/publish", async (KafkaProducer producer, HttpRequest request) =>
{
    var data = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(request.Body);
    var message = data?["text"] ?? "default";

    var msgObj = new { Id = Guid.NewGuid(), Text = message, CreatedAt = DateTime.UtcNow };
    await producer.PublishAsync("test-topic", msgObj.Id.ToString(), msgObj);

    return Results.Text("Message published to Kafka");
});

app.MapGet("/stream", async (HttpResponse response) =>
{
    response.Headers.Append("Content-Type", "text/event-stream"); 

    var consumer = new KafkaConsumer("test-topic", "demo-group");

    await foreach (var msg in consumer.ConsumeStream())
    {
        await response.WriteAsync($"data: {JsonSerializer.Serialize(msg)}\n\n");
        await response.Body.FlushAsync();
    }
});


app.UseDefaultFiles(); 
app.UseStaticFiles();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
