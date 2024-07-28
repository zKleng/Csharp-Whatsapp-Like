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
    }

    public async Task SendMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        await _stream.WriteAsync(buffer, 0, buffer.Length);
    }

    public void Close()
    {
        _stream.Close();
        _client.Close();
    }
}
