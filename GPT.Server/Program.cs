using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Server
{
    public class Programm {

        static async Task<int> Main()
        {
            Console.WriteLine("GoProTunnel Server");
            Server server = new Server();
            
            while(true)
            {
                Console.WriteLine("start, stop, exit");

                string? input = Console.ReadLine();
                if (input == "start")
                {
                    Console.WriteLine("Please enter your server port:");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out int port))
                    {
                        await server.Start(new IPEndPoint(IPAddress.Any, port));
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid port");
                    }
                }
                else if (input == "stop")
                {
                    Console.WriteLine("Stopping...");
                    await server.Stop();
                    Console.WriteLine("Stopped");
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Goodnight");
                    Environment.Exit(0);
                    break;
                }
            }

            return 0;
        }
    }
}
