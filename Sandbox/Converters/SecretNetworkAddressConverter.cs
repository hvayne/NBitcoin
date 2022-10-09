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
    public class SecretNetworkAddressConverter : IAddressConverter
    {
        SHA256 shaHasher = SHA256.Create();
        RipeMD160Digest ripedHasher = new();
        KeyPath hdPath = new("44'/529'/0'/0/0");
        public string GetAddress(ExtKey hdRoot)
        {
            Key privateKey = hdRoot.Derive(hdPath).PrivateKey;

            byte[] pubkeyHash = shaHasher.ComputeHash(privateKey.PubKey.ToBytes());

            ripedHasher.BlockUpdate(pubkeyHash, 0, pubkeyHash.Length);
            byte[] ripedHash = new byte[ripedHasher.GetDigestSize()];
            ripedHasher.DoFinal(ripedHash, 0);

            string address = Bech32Engine.Encode("secret", ripedHash);
            return address;
        }
        public void SetHdPath(KeyPath path)
        {
            hdPath = new(path.ToString());
        }
        public KeyPath GetHdPath()
        {
            return hdPath;
        }
    }
}
