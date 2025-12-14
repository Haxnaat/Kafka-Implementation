using Confluent.Kafka;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Channels;

public class KafkaConsumer
{
    private readonly string _topic;
    private readonly string _groupId;
    private readonly ConsumerConfig _config;

    public KafkaConsumer(string topic, string groupId)
    {
        _topic = topic;
        _groupId = groupId;

        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
    }

    // Async enumerable for streaming messages
    public async IAsyncEnumerable<Dictionary<string, object>> ConsumeStream()
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe(_topic);

        while (true)
        {
            var result = consumer.Consume();
            var msgObj = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Message.Value);
            yield return msgObj;

            await Task.Yield(); // allow async streaming
        }
    }
}
