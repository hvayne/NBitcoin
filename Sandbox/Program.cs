using System.Diagnostics;
using System.Numerics;
using System.Text;
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
        static QREncoder ncoder = new();
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

            RandomUtils.UseAdditionalEntropy = true;
            RandomUtils.AddEntropy(Encoding.UTF8.GetBytes(Prompt.Password("Add more entropy")));

            while (threadCount > 0)
            {
                Thread thrd = new(new ThreadStart(() =>
                {
                    Seedgen generator = new(passphrase, network, HandleMessage);
                    generator.Start();
                }));
                thrd.Start();
                threadCount--;
            }
            sw = new Stopwatch();
            sw.Start();

            Thread thrd2 = new(new ThreadStart(() =>
            {
                PrintStats();
            }));
            thrd2.Start();

            Console.ReadKey();
        }
        static void PrintStats()
        {
            while (true)
            {
                double aps = total / sw.Elapsed.TotalSeconds;
                Console.WriteLine($"seedgen {lucky}/{total} {aps:0}addr/s");
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
            ncoder.SaveQr(msg.Wallet);
            Console.WriteLine(msg.Wallet.Address);
        }
    }
}
