using System.Diagnostics;
using System.Numerics;
using NBitcoin;
using Sandbox.Converters;
using Sandbox.Enums;
using Sandbox.Messages;
using Sharprompt;

namespace Sandbox
{
    internal class Program
    {
        static uint lucky = 0;
        static uint total = 0;
        static Stopwatch sw;
        static void Main()
        {
            IAddressConverter addressConverter;
            ENetwork network;
            int threadCount;
            string passphrase;

            network = (ENetwork)Enum.Parse(typeof(ENetwork), Prompt.Select("Select network", Enum.GetNames(typeof(ENetwork))));
            addressConverter = network switch
            {
                ENetwork.Bitcoin => new BitcoinAddressConverter(),
                ENetwork.SecretNetwork => new SecretNetworkAddressConverter(),
                _ => throw new NotImplementedException()
            };
            passphrase = Prompt.Password("Input passphrase");
            threadCount = Prompt.Input<int>("Input number of threads", 1);


            while (threadCount > 0)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Seedgen generator = new(passphrase, network, HandleMessage);
                    generator.Start();
                });
                threadCount--;
            }
            sw = new Stopwatch();
            sw.Start();
            // Timer timer = new(PrintStats, null, 100, 1000);
            ThreadPool.QueueUserWorkItem((o) => PrintStats());

            Console.ReadKey();
        }
        static void PrintStats()
        {
            while (true)
            {
                double aps = total / sw.Elapsed.TotalSeconds;
                Console.WriteLine( $"seedgen {lucky}/{total} {aps:0}addr/s");
                Thread.Sleep(700);
            }
        }
        static void HandleMessage(ISeedgenMessage msg)
        {
            switch (msg.GetEMsg())
            {
                case EMsg.MessageCount:
                    OnCountMessage(msg as CountMessage);
                    break;
                case EMsg.MessageLucky:
                    OnLuckyMessage(msg as LuckyMessage);
                    break;
                default:
                    break;
            }
        }
        static object locky = new();
        static void OnCountMessage(CountMessage msg)
        {
            lock (locky)
            {
                total += msg.Count;
            }
        }
        static void OnLuckyMessage(LuckyMessage msg)
        {
            lock (locky)
            {
                lucky++;
            }
            Console.WriteLine(msg.Wallet.Address);
        }
    }
}
