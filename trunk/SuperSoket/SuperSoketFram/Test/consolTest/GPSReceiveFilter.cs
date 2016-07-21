﻿using System;
using SuperSocket.Common;
using consolTest.Protocol;
using SuperSocket.SocketBase.Protocol;


namespace consolTest
{
    /// <summary>
    /// It is the kind of protocol that
    /// the first two bytes of each command are { 0x68, 0x68 }
    /// and the last two bytes of each command are { 0x0d, 0x0a }
    /// and the 16th byte (data[15]) of each command indicate the command type
    /// if data[15] = 0x10, the command is a keep alive one
    /// if data[15] = 0x1a, the command is position one
    /// </summary>
    class GPSReceiveFilter : BeginEndMarkReceiveFilter<BinaryRequestInfo>
    {
        private readonly static byte[] BeginMark = new byte[] { 0xFA, 0xFA };
        private readonly static byte[] EndMark =new byte[] { 0xFA, 0xFA };

        public GPSReceiveFilter()
            : base(BeginMark, EndMark)
        {

        }

        protected override BinaryRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            return new BinaryRequestInfo("ss", readBuffer.CloneRange(offset, length));
        }
    }
}
