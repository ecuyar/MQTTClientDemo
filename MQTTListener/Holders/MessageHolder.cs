namespace MQTTListener.Holders
{
	public class MessageHolder
	{
		public int MessageCapacity { get; init; }
		public List<Message> MessageList { get; set; } = new();

		public MessageHolder(int MessageCount)
		{
			MessageCapacity = MessageCount;
			MessageList = new();
		}

		public void AddMessage(string message, string messageTopic = "")
		{
			int count = MessageList.Count;

			if (count >= MessageCapacity)
			{
				RemoveFirstMessage();
			}

			var m = new Message(count, message, messageTopic);
			MessageList.Add(m);
		}

		public List<Message> ShowMessages()
		{
			return MessageList.OrderBy(x => x.IndexValue).ToList();
		}

		private void RemoveFirstMessage()
		{
			if (MessageList.Count >= MessageCapacity)
			{
				MessageList.RemoveAt(0);

				foreach (var listItem in MessageList)
				{
					listItem.IndexValue -= 1;
				}
			}
		}

		public class Message
		{
			public int IndexValue { get; set; }
			public string MessageString { get; set; }
			public string MessageTopic { get; set; }

			public Message(int index, string message, string messageTopic = "")
			{
				IndexValue = index;
				MessageString = message;
				MessageTopic = messageTopic;
			}
		}
	}
}
