using GPT.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Server
{
    public class Programm
    {

        static async Task<int> Main()
        {
            Console.WriteLine("GoProTunnel Client");

            while (true)
            {
                Console.WriteLine("Commands: connect, exit");

                string? input = Console.ReadLine();
                if (input == "connect")
                {
                    Console.WriteLine("Please enter a address");
                    input = Console.ReadLine();
                    if (input != null && IPEndPoint.TryParse(input, out IPEndPoint? address))
                    {
                        await Connect(address);
                    } else
                    {
                        Console.WriteLine("Please enter a valid address");
                    }
                }
                else if (input == "exit")
                {
                    Environment.Exit(0);
                }
            }
        }

        public static async Task Connect(IPEndPoint address)
        {
            StreamClient client = new();

            try
            {
                while (true)
                {
                    Console.WriteLine("Commands: start, stop, follow, unfollow, back");

                    string? input = Console.ReadLine();
                    if (input == "start")
                    {
                        Console.WriteLine("Starting...");
                        await client.Start(address);
                        Console.WriteLine("Started");
                    }
                    else if (input == "stop")
                    {
                        Console.WriteLine("Stopping...");
                        await client.Start(address);
                        Console.WriteLine("Stopped");
                    }
                    else if (input == "follow")
                    {
                        Console.WriteLine("Please enter a camera id");
                        input = Console.ReadLine();
                        if (input != null && int.TryParse(input, out int cameraId))
                        {
                            Console.WriteLine("Please your local streaming port");
                            input = Console.ReadLine();
                            if (input != null && int.TryParse(input, out int port))
                            {
                                client.follow(cameraId, port);
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid streaming port");
                            }
                        } else
                        {
                            Console.WriteLine("Please enter a valid camera id");
                        }
                    }
                    else if (input == "unfollow")
                    {
                        Console.WriteLine("Please enter a camera id");
                        input = Console.ReadLine();
                        if (input != null && int.TryParse(input, out int cameraId))
                        {
                            client.unfollow(cameraId);
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid camera id");
                        }
                    }
                    else if (input == "back")
                    {
                        await client.Stop();
                    }
                }
            }
            finally
            {
                await client.Stop();
            }
        }
    }
}
