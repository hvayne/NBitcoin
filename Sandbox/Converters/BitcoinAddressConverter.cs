using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bech32;
using NBitcoin;
using Org.BouncyCastle.Crypto.Digests;
using Sandbox.Converters;

namespace Sandbox.Converters
{
	public class BitcoinAddressConverter : IAddressConverter
	{
        KeyPath hdPath = new("84'/0'/0'/0/0");
        public string GetAddress(ExtKey hdRoot)
        {
            return hdRoot.Derive(hdPath).PrivateKey.GetAddress(ScriptPubKeyType.Segwit, Network.Main).ToString();
        }       
        public KeyPath GetHdPath()
        {
            return hdPath;
        }
        public void SetHdPath(KeyPath path)
        {
            hdPath = new(path.ToString());
        }
    }
}
