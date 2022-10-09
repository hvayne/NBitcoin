using NBitcoin;
using Sandbox.Enums;
using Sandbox.Converters;
using System.Diagnostics;
using Sharprompt;
using Sandbox.Messages;

namespace Sandbox
{
    internal class Seedgen
    {
        IAddressConverter addressConverter;
        ENetwork network;
        string passphrase;
        Action<ISeedgenMessage> smthMagical;
        uint count = 0;
        public Seedgen(string passphrase, ENetwork network, Action<ISeedgenMessage> smthMagical)
        {
            this.passphrase = passphrase;
            this.network = network;
            this.smthMagical = smthMagical;
            addressConverter = network switch
            {
                ENetwork.Bitcoin => new BitcoinAddressConverter(),
                ENetwork.SecretNetwork => new SecretNetworkAddressConverter(),
                _ => throw new NotImplementedException()
            };
        }
        public void Start()
        {
            Thread thrd = new(UpdateStats);
            thrd.Start();

            while (true)
            {
                count++;
                DataClass wallet = new();
                wallet.Mnemonic = GenerateMnemonic();
                wallet.HdRoot = MnemonicToExtKey(wallet.Mnemonic, passphrase);
                wallet.Address = ExtKeyToAddress(wallet.HdRoot);
                if (!IsLucky(wallet.Address))
                    continue;
                wallet.HdPath = GetHdPath();
                wallet.Network = network;
                smthMagical(new LuckyMessage { Wallet = wallet });
            }
        }
        public void UpdateStats()
        {
            while (true)
            {
                smthMagical(new CountMessage { Count = count });
                count = 0;
                Thread.Sleep(4000);
            }
        }
        internal Mnemonic GenerateMnemonic()
        {
            return new Mnemonic(Wordlist.English);
        }
        internal ExtKey MnemonicToExtKey(Mnemonic mnemonic, string passphrase)
        {
            ExtKey key = mnemonic.DeriveExtKey(passphrase);
            return key;
        }
        internal KeyPath GetHdPath()
        {
            return addressConverter.GetHdPath();
        }
        internal string ExtKeyToAddress(ExtKey hdRoot)
        {
            return addressConverter.GetAddress(hdRoot);
        }
        internal bool IsLucky(string address)
        {
            int len = address.Length;
            // && address[len - 4] == 'f'
            if (address[len - 1] == 'k' && address[len - 2] == 'c' && address[len - 3] == 'u') // && address[len - 4] == 's'
                return true;
            return false;
        }
    }
}
