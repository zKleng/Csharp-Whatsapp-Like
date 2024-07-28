using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 2 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
        {
            Console.WriteLine("Uso: dotnet run <modo> <puerto>");
            Console.WriteLine("Modo: 'server' para iniciar el servidor, 'client' para iniciar el cliente");
            return;
        }

        string mode = args[0];
        if (!int.TryParse(args[1], out int port))
        {
            Console.WriteLine("El puerto debe ser un número entero.");
            return;
        }

        if (mode.ToLower() == "server")
        {
            var server = new ChatServer(port);
            await server.StartAsync();
        }
        else if (mode.ToLower() == "client")
        {
            var client = new ChatClient("localhost", port);

            while (true)
            {
                Console.Write("Ingrese el mensaje: ");
                string? message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("El mensaje no puede estar vacío.");
                    continue;
                }

                await client.SendMessageAsync(message);
                Console.WriteLine($"Mensaje enviado: {message}");
            }
        }
        else
        {
            Console.WriteLine("Modo no reconocido. Usa 'server' o 'client'.");
        }
    }
}
