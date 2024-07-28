using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServer
{
    private TcpListener _listener;
    private ConcurrentBag<TcpClient> _clients = new ConcurrentBag<TcpClient>();

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
            if (client != null)
            {
                _clients.Add(client);
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        var buffer = new byte[1024];
        NetworkStream? stream = null;

        try
        {
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo el stream: {ex.Message}");
        }

        if (stream == null)
        {
            return;
        }

        while (true)
        {
            int byteCount;
            try
            {
                byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error leyendo del stream: {ex.Message}");
                break;
            }

            if (byteCount == 0)
                break;

            var message = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine($"Mensaje recibido: {message}");
            await BroadcastMessageAsync(message, client);
        }

        _clients.TryTake(out var removedClient);
        removedClient?.Close();
    }

    private async Task BroadcastMessageAsync(string message, TcpClient excludeClient)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var client in _clients)
        {
            if (client != excludeClient)
            {
                NetworkStream? stream = null;
                try
                {
                    stream = client.GetStream();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error obteniendo el stream para el cliente: {ex.Message}");
                }

                if (stream != null)
                {
                    try
                    {
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error escribiendo al stream: {ex.Message}");
                    }
                }
            }
        }
    }
}
