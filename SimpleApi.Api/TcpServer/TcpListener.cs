using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleApi.API.TcpServer;


public class TcpListenerService
{
    private readonly TcpListener _listener;
    private readonly CancellationTokenSource _cancellation;

    public TcpListenerService(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _cancellation = new CancellationTokenSource();
    }

    public void Start()
    {
        _listener.Start();
        Task.Run(() => ListenForClientsAsync(_cancellation.Token));
    }

    public void Stop()
    {
        _cancellation.Cancel();
        _listener.Stop();
    }

    private async Task ListenForClientsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                _ = Task.Run(() => HandleClientAsync(client, cancellationToken));
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested)
            {
                // Listener was stopped
                break;
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using (client)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
            {
                var received = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                var numberValues = Parse(received);
                var sum = numberValues.Sum();

                var response = Encoding.UTF8.GetBytes($"Message received: {received}\r\nNumeric-Sum: {sum}");
                await stream.WriteAsync(response, 0, response.Length, cancellationToken).ConfigureAwait(false);

            }
        }
    }

    private IEnumerable<int> Parse(string received)
    {
        var values = received.Split(',');
        var numbers = new List<int>();

        foreach (var value in values)
        {
            int numericValue;
            var isValid = int.TryParse(value, out numericValue);
            if (isValid) numbers.Add(numericValue);
        }
        return numbers;
    }
}
