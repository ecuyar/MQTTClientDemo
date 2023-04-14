using MQTTListener.Dtos;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace MQTTListener.Holders
{
	public class ConnectionHolder
	{
		public IMqttClient MqttClient { get; set; }
		private readonly MqttFactory MqttFactory;
		private ConnectionProps ConnectionProps { get; set; }

		public bool? IsConnected { get; set; }
		//[Parameter]
		//public EventCallback<bool> IsConnectedChanged { get; set; }
		public string? ErrorMessage { get; set; }
		//public Messages messagePage { get; set; } = new();
		public MessageHolder messageHolder = new(10);

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

		public async Task ConnectToServer(string topic)
		{
			MqttClient.ConnectedAsync += ConnectedShow_Callback;
			MqttClient.ApplicationMessageReceivedAsync += MessageReceived_Callback;
			MqttClient.DisconnectedAsync += Disconnected_Callback;

			if (MqttClient.IsConnected)
			{
				return;
			}

			//await MqttClient.ReconnectAsync();

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
				IsConnected = false;
				ErrorMessage = ex.Message;
				Console.WriteLine(ex.Message);
			}

			var mqttSubscribeOptions = MqttFactory.CreateSubscribeOptionsBuilder()
				.WithTopicFilter(f => f.WithTopic(topic))
				.Build();

			try
			{
				await MqttClient.SubscribeAsync(mqttSubscribeOptions);
				Console.WriteLine($"{nameof(ConnectionHolder)}=> MQTT client subscribed to \"presentation\" topic.");
				//messagePage.mqttClient = MqttClient;
			}
			catch (Exception ex)
			{
				IsConnected = false;
				//await IsConnectedChanged.InvokeAsync(false);
				ErrorMessage = ex.Message;
				Console.WriteLine(ex.Message);
			}
		}

		private async Task Disconnected_Callback(MqttClientDisconnectedEventArgs arg)
		{
			Console.WriteLine($"{nameof(ConnectionHolder)}=> MQTT client disconnected from the server.");
			//MqttClient.Dispose();
			IsConnected = false;
			await Task.CompletedTask;
		}

		private async Task ConnectedShow_Callback(MqttClientConnectedEventArgs e)
		{
			Console.WriteLine($"{nameof(ConnectionHolder)}=> MQTT client connected to the server.");
			IsConnected = true;
			//await IsConnectedChanged.InvokeAsync(true);

			//TODO Isconnected güncellenmiyor
			await Task.CompletedTask;
		}

		public async Task MessageReceived_Callback(MqttApplicationMessageReceivedEventArgs e)
		{
			string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
			Console.WriteLine($"{nameof(ConnectionHolder)}=> Message from \"{e.ApplicationMessage.Topic}\" topic: " + message);

			messageHolder.AddMessage(message, e.ApplicationMessage.Topic);

			//var allMessages = messageHolder.ShowMessages();
			//messagePage.CurrentMessages = allMessages;

			//await messagePage.CurrentMessagesChanged.InvokeAsync(allMessages.Last());
			await Task.CompletedTask;
		}
	}


}
