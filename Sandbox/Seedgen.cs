using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using Sandbox.Converters;
using Sandbox.Enums;
using Sharprompt;

namespace Sandbox
{
	internal class Seedgen
	{
		IAddressConverter addressConverter;
		public Seedgen()
		{
			// questionaire here
			ENetwork network = (ENetwork)Enum.Parse(typeof(ENetwork), Prompt.Select("Select network", Enum.GetNames(typeof(ENetwork))));
			addressConverter = network switch
			{
				ENetwork.Bitcoin => new BitcoinAddressConverter(),
				ENetwork.SecretNetwork => new SecretNetworkAddressConverter(),
				_ => throw new NotImplementedException()
			};



		}
		public void Start()
		{

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
		internal string ExtKeyToAddress(DataClass wallet)
		{
			return addressConverter.GetAddress(wallet);
		}
		internal bool IsLucky(string address)
		{
			int len = address.Length;
			if (address[len - 1] == 'k' && address[len - 2] == 'c' && address[len - 3] == 'u' && address[len - 4] == 'f') // && address[len - 4] == 's'
				return true;
			return false;
		}
	}
}
