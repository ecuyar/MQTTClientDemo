using MQTTnet.Client;
using MQTTnet;
using MQTTListener.Dtos;
using MQTTnet.Server;

namespace MQTTListener.Holders
{
	public class ConnectionHolder
	{
		public IMqttClient MqttClient { get; set; }
		private readonly MqttFactory MqttFactory;
		private ConnectionProps ConnectionProps { get; set; }

		public ConnectionHolder()
		{
			MqttFactory = new MqttFactory();
			MqttClient = MqttFactory.CreateMqttClient();
			ConnectionProps = new ConnectionProps()
			{
				ServerIp = "wss://a989a75157744b01a88b4876006e827d.s2.eu.hivemq.cloud:8884/mqtt",
				Port = 8884,
				Username = "mqttdeneme",
				Password = "mqttdeneme",
				ClientId = Guid.NewGuid()
			};
		}

		public async Task ConnectToServer()
		{
			var mqttOptions = new MqttClientOptionsBuilder()
				.WithWebSocketServer(ConnectionProps.ServerIp)
				.WithCredentials(ConnectionProps.Username, ConnectionProps.Password)
				.WithClientId(ConnectionProps.ClientId.ToString())
				.WithTls()
				.Build();

			MqttClientConnectResult response = new();
			try
			{
				response = await MqttClient.ConnectAsync(mqttOptions);
			}
			catch (Exception ex)
			{
				//await IsConnectedChanged.InvokeAsync(false);
				//ErrorMessage = ex.Message;
			}
		}
	}
}
