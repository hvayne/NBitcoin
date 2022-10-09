using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Enums;

namespace Sandbox.Messages
{
    public class CountMessage : ISeedgenMessage
    {
        public uint Count { get; set; }
        public EMsg GetEMsg()
        {
            return EMsg.MessageCount; 
        }
    }
}
