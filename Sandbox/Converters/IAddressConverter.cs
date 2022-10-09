using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace Sandbox.Converters
{
	public interface IAddressConverter
	{
		string GetAddress(ExtKey hdRoot);
		KeyPath GetHdPath();
	}
}
