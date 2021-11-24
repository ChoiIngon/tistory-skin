﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamnet.SystemPacket
{
    enum ErrorCode
    {
        Success
    }
    [System.Serializable]
    class MsgCliSvr_EstablishSessionLink_Req
    {
        public const uint MSG_ID = uint.MaxValue - 0; // 4294967295
    }

    [System.Serializable]
    class MsgSvrCli_EstablishSessionLink_Ans
    {
        public const uint MSG_ID = uint.MaxValue - 0; // 4294967295

        public int error_code = 0;
        public uint session_key = 0;
        public string session_token = "";
    }

    [System.Serializable]
    class MsgCliSvr_DestroySessionLink_Req
    {
        public const uint MSG_ID = uint.MaxValue - 1; // 4294967294
    }

    [System.Serializable]
    class MsgSvrCli_DestroySessionLink_Ans
    {
        public const uint MSG_ID = uint.MaxValue - 1; // 4294967294
        public int error_code = 0;
    }

    [System.Serializable]
    class MsgCliSvr_RecoverSessionLink_Req
    {
        public const uint MSG_ID = uint.MaxValue - 2; // 4294967293

        public uint session_key;
        public string session_token;
    }

    [System.Serializable]
    class MsgSvrCli_RecoverSessionLink_Ans
    {
        public const uint MSG_ID = uint.MaxValue - 2; // 4294967293
        public int error_code = 0;
    }

    [System.Serializable]
    class MsgCliSvr_HeartBeat_Req
    {
        public const uint MSG_ID = uint.MaxValue - 3; // 4294967292
    }

    [System.Serializable]
    class MsgSvrCli_HeartBeat_Ans
    {
        public const uint MSG_ID = uint.MaxValue - 3; // 4294967292
        public int error_code = 0;
    }

    [System.Serializable]
    class MsgCliSvr_ReliableAck_Ntf
    {
        public const uint MSG_ID = uint.MaxValue - 4; // 4294967291
        public uint ack_seq;
    }

    [System.Serializable]
    class MsgSvrCli_ReliableAck_Ntf
    {
        public const uint MSG_ID = uint.MaxValue - 4; // 4294967291
        public uint ack_seq;
    }
}
