using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using Sandbox.Enums;

namespace Sandbox
{
	public class DataClass
	{
		public ENetwork Network { get; set; }
		public Mnemonic Mnemonic { get; set; }
		public ExtKey HdRoot { get; set; }
		public KeyPath HdPath { get; set; }
		public string Address { get; set; }
	}
}
