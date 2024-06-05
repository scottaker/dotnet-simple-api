using System.Net.Sockets;
using System.Text;

namespace TcpServer.Tests;



class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (var client = new TcpClient("localhost", 5151))
            using (var stream = client.GetStream())
            {
                var message = "1,2,10,12";
                var data = Encoding.UTF8.GetBytes(message);


                // SEND
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);


                // RECEIVE
                data = new byte[1024];
                var bytesRead = stream.Read(data, 0, data.Length);
                var response = Encoding.UTF8.GetString(data, 0, bytesRead);
                Console.WriteLine("Received: {0}", response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }
}
