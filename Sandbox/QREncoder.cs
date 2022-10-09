using System.Text;
using Net.Codecrete.QrCodeGenerator;

namespace Sandbox
{
    internal class QREncoder
    {
        public void SaveQr(DataClass wallet)
        {
            string html = string.Empty;
            // address QR
            html += GetQrHtml(wallet.Address);
            // address string, hd path, network
            html += $"<h1 align=\"center\">" +
                $"HdPath: {wallet.HdPath}   " +
                $"{wallet.Network}\n</h1>";
            // mnemonic QR
            html += GetQrHtml(wallet.Mnemonic.ToString());
            Directory.CreateDirectory("QRs");
            File.WriteAllText($"QRs/{new string(wallet.Address.TakeLast(8).ToArray())}.html", html, Encoding.UTF8);
        }
        internal string GetQrHtml(string data)
        {
            string html = QrCode.EncodeText(data, QrCode.Ecc.Medium).ToSvgString(5);
            html += $"<h1 align=\"center\">{data}</h1>";
            return html;
        }
    }
}
