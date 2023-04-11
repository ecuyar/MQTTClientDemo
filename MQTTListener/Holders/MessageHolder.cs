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

		public void AddMessage(string message)
		{
			int count = MessageList.Count;

			if (count >= MessageCapacity)
			{
				RemoveLastMessage();
			}

			var m = new Message(count, message);
			MessageList.Add(m);
		}

		public List<Message> ShowMessages()
		{
			return MessageList.OrderByDescending(x => x.IndexValue).ToList();
		}

		private void RemoveLastMessage()
		{
			if (MessageList.Count >= MessageCapacity)
			{
				MessageList.RemoveAt(MessageList.Count - 1);

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

			public Message(int index, string message)
			{
				IndexValue = index;
				MessageString = message;
			}
		}
	}
}
