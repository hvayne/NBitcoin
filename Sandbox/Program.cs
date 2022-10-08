using System.Numerics;
using NBitcoin;

namespace Sandbox
{
	internal class Program
	{
		static void Main()
		{
			Seedgen generator = new();
			generator.Start();
		}
		static string GetHex(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
		}
	}
}
