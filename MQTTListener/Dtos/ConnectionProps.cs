namespace MQTTListener.Dtos
{
	public class ConnectionProps
	{
		public string ServerIp { get; set; } = string.Empty;
		public int Port { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public Guid ClientId { get; set; }
	}
}
