using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatClient
{
    private TcpClient _client;
    private NetworkStream _stream;

    public ChatClient(string host, int port)
    {
        _client = new TcpClient();
        _client.Connect(host, port);
        _stream = _client.GetStream();

        Task.Run(() => ReceiveMessagesAsync());
    }

    public async Task SendMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        await _stream.WriteAsync(buffer, 0, buffer.Length);
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024];

        while (true)
        {
            var byteCount = await _stream.ReadAsync(buffer, 0, buffer.Length);
            if (byteCount == 0)
                break;

            var message = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine($"Mensaje recibido: {message}");
        }
    }

    public void Close()
    {
        _stream.Close();
        _client.Close();
    }
}
