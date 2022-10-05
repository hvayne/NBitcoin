using NBitcoin;

namespace Sandbox
{
	internal class Program
	{
		static void Main()
		{
			Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.TwentyFour);
			Console.WriteLine(mnemo);
			Console.ReadKey();
		}
	}
}
