using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServer
{
    private TcpListener _listener;
    private List<TcpClient> _clients = new List<TcpClient>();

    public ChatServer(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine($"Servidor iniciado en el puerto {((IPEndPoint)_listener.LocalEndpoint).Port}");

        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _clients.Add(client);
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        var buffer = new byte[1024];
        var stream = client.GetStream();

        while (true)
        {
            var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (byteCount == 0)
                break;

            var message = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine($"Mensaje recibido: {message}");
            await BroadcastMessageAsync(message, client);
        }

        _clients.Remove(client);
        client.Close();
    }

    private async Task BroadcastMessageAsync(string message, TcpClient excludeClient)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var client in _clients)
        {
            if (client != excludeClient)
            {
                var stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
