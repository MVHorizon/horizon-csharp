using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace HorizonChat;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentBag<WebSocket> _clients = new();

    public WebSocketMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                _clients.Add(socket);
                Console.WriteLine($"Client connected. Total clients: {_clients.Count}");

                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {text}");

                    // Broadcast to all connected clients
                    await BroadcastMessageAsync(text, socket);

                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                }

                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                Console.WriteLine($"Client disconnected. Total clients: {_clients.Count}");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        else
        {
            await _next(context);
        }
    }

    private static async Task BroadcastMessageAsync(string message, WebSocket sender)
    {
        var outgoing = Encoding.UTF8.GetBytes(message);
        var tasks = new List<Task>();

        foreach (var client in _clients)
        {
            // Send to all clients including sender for consistency
            if (client.State == WebSocketState.Open)
            {
                tasks.Add(client.SendAsync(
                    new ArraySegment<byte>(outgoing),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }
        }

        await Task.WhenAll(tasks);
    }
}
