using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace Sandbox
{
	internal class FirstTestingGround
	{
		public void Start()
		{
			Mnemonic mnemonic = new("elbow silk cost label swamp " +
				"exist throw fiction stamp aerobic hamster melt length " +
				"spoon step rhythm print glass box rigid album burden" +
				" ripple wagon", Wordlist.English);

			ExtKey hdRoot = mnemonic.DeriveExtKey();

			var keyPath = new KeyPath("44'/529'/0'/0/0");

			ExtKey wallet = hdRoot.Derive(keyPath);

			Key pk = wallet.PrivateKey;

			BigInteger pk_dec = new(pk.ToBytes(), true, true);
			Console.WriteLine($"base64 key = {Convert.ToBase64String(pk.ToBytes())}");
			Console.WriteLine($"pk hex = {pk.ToHex()}");
			Console.WriteLine($"pk decimal = {pk_dec}");

			Console.ReadKey();
		}
		string GetHex(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
		}
	}
}
