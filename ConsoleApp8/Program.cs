using System.Net;
using System.Net.Sockets;
using System.Text;

public class GetSocket
{
    private static Socket ConnectSocket(string server, int port)
    {
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        IPAddress address = IPAddress.Parse("127.0.0.1");

        IPEndPoint ipe = new IPEndPoint(address, 1111);
        Socket tempSocket =
            new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        tempSocket.Connect(ipe);
        s = tempSocket;
        return s;
    }

    // This method requests the home page content for the specified server.
    public static string SocketSendReceive(string server, int port)
    {
        string request = "GET / HTTP/1.1\r\nHost: " + server +
            "\r\nConnection: Close\r\n\r\n";
        Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
        Byte[] bytesReceived = new Byte[256];
        string page = "";
        int index;
        // Create a socket connection with the specified server and port.
        using (Socket s = ConnectSocket(server, port))
        {

            if (s == null)
                return ("Connection failed");

            // Send request to the server.
            //s.Send(bytesSent, bytesSent.Length, 0);
            // Receive the server home page content.
            int bytes = 0;
            // The following will block until the page is transmitted.
            //00000000000
            while (true)
            {
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);

                index = Array.IndexOf(bytesReceived, (Byte)0);
                if (index == -1)
                    page = System.Text.Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                else
                    page = System.Text.Encoding.ASCII.GetString(bytesReceived, 0, index);

                if (page == "exit")
                    return page;
                Console.WriteLine(page);
            }

        }

    }
}
class Program
{
    public static void Main()
    {

       Task.Run(()=> GetSocket.SocketSendReceive("127.0.0.1", 1111));
        Connect();
        Console.Read();
    }




    static void Connect()
    {
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            string message = "Hello";
            Int32 port = 1111;
            TcpClient client = new TcpClient("127.0.0.1", port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            //// Send the message to the connected TcpServer.
            //stream.Write(data, 0, data.Length);

            //Console.WriteLine("Sent: {0}", message);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];
            int index;
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            while (true)
            {
                Int32 bytes = stream.Read(data, 0, data.Length);

               index= Array.IndexOf(data, (Byte)0);
                if (index == -1)
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                else
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, index);

                if (responseData == "exit")
                {
                    Console.WriteLine("End of work");
                    break;
                }
                Console.WriteLine("Received: {0}", responseData);
            }

            // Close everything.
            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("Установленно соединение с сервером");
        }
        catch (IOException e)
        {
            Console.WriteLine("Сервер прекратил свою работу!");
        }

        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }
}