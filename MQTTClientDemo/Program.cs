using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

Console.WriteLine("Hello, MQTT Fellows!");

MqttFactory mqttFactory = new();

using (var mqttClient = mqttFactory.CreateMqttClient())
{
    var mqttOptions = new MqttClientOptionsBuilder()
        .WithTcpServer("a989a75157744b01a88b4876006e827d.s2.eu.hivemq.cloud", 8883)
        .WithCredentials("mqttdeneme", "mqttdeneme")
        .WithClientId(Guid.NewGuid().ToString())
        .WithTls()
        .Build();

    MqttClientConnectResult response;
    try
    {
        response = await mqttClient.ConnectAsync(mqttOptions, CancellationToken.None);
    }
    catch (Exception)
    {
        throw;
    }

    if (response is not null && response.ResultCode == MqttClientConnectResultCode.Success)
    {
        var output = JsonSerializer.Serialize(response);

        Console.WriteLine($"[{output?.GetType().Name}]:\r\n{output}");
    }


}