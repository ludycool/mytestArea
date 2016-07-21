using System;
using SuperSocket.Common;
using consolTest.Protocol;
using SuperSocket.SocketBase.Protocol;


namespace consolTest.Protocol.Filter
{
    /// <summary>
    /// It is the kind of protocol that
    /// the first two bytes of each command are { 0x68, 0x68 }
    /// and the last two bytes of each command are { 0x0d, 0x0a }
    /// and the 16th byte (data[15]) of each command indicate the command type
    /// if data[15] = 0x10, the command is a keep alive one
    /// if data[15] = 0x1a, the command is position one
    /// </summary>
    class BaseReceiveFilter : ReceiveFilterBase<BinaryRequestInfo>
    {
        public BaseReceiveFilter()
            : base()
        {

        }

        public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            return new BinaryRequestInfo("HELLO", readBuffer.CloneRange(offset, length));
        }
    }
}
