﻿using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

namespace UnityServer.Packet
{
    class HelloWorld : Gamnet.Server.PacketHandler<Server.Session>
    {
        [Serializable]
        public class MsgCliSvr_Greeting_Req
        {
            public const uint MSG_ID = Code.MsgCliSvr_Greeting_Req;
            public string text;
        }

        [Serializable]
        public class MsgSvrCli_Greeting_Ans
        {
            public const uint MSG_ID = Code.MsgSvrCli_Greeting_Ans;
            public string text;
        }

        [Serializable]
        public class MsgCliSvr_Greeting_Ntf
        {
            public const uint MSG_ID = Code.MsgCliSvr_Greeting_Ntf;
            public string text;
        }

        public override uint Id()
        {
            return MsgCliSvr_Greeting_Req.MSG_ID;
        }

        public override IEnumerator OnReceive(Server.Session session, Gamnet.Packet packet)
        {
            MsgCliSvr_Greeting_Req req = packet.Deserialize<MsgCliSvr_Greeting_Req>();
            Gamnet.Log.Write(Gamnet.Log.LogLevel.DEV, req.text);

            {   // verrrry long term task
                var asyncTask = new Gamnet.Async.AsyncTask(session, () =>
                {
                    Thread.Sleep(1000);
                });
                yield return asyncTask; // suspend. but resume again when the task finish.
                if (null != asyncTask.Exception) // check result of task. if null. success.
                {
                    throw asyncTask.Exception;
                }
            }

            MsgSvrCli_Greeting_Ans ans = new MsgSvrCli_Greeting_Ans();
            ans.text = "Thanks";

            Gamnet.Packet ansPacket = new Gamnet.Packet();
            ansPacket.Id = MsgSvrCli_Greeting_Ans.MSG_ID;
            ansPacket.Serialize(ans);
            session.AsyncSend(ansPacket);

            // wait other message async
            const int waitTimeoutSec = 2;
            var asyncReceive = new Gamnet.Async.AsyncReceive(session, MsgCliSvr_Greeting_Ntf.MSG_ID, waitTimeoutSec);
            yield return asyncReceive; // suspend. it would resume when MsgCliSvr_Greeting_Ntf arrives or timeout
            if (null != asyncReceive.Exception)
            {
                Gamnet.Log.Write(Gamnet.Log.LogLevel.DEV, asyncReceive.Exception.ToString());
                yield break;
            }

            MsgCliSvr_Greeting_Ntf ntf = asyncReceive.Packet.Deserialize<MsgCliSvr_Greeting_Ntf>();
            Gamnet.Log.Write(Gamnet.Log.LogLevel.DEV, ntf.text);
        }

        [Gamnet.Server.TestMethod]
        public void Test_HelloWorld(UnityServer.Simulator.Client client)
        {
            client.session.RegisterHandler(MsgSvrCli_Greeting_Ans.MSG_ID, (MsgSvrCli_Greeting_Ans ans) =>
            {
                client.number = 2;
                client.session.UnregisterHandler(MsgSvrCli_Greeting_Ans.MSG_ID);

                MsgCliSvr_Greeting_Ntf ntf = new MsgCliSvr_Greeting_Ntf();
                ntf.text = "fin" + client.number.ToString();

                Gamnet.Packet ntfPacket = new Gamnet.Packet();
                ntfPacket.Id = MsgCliSvr_Greeting_Ntf.MSG_ID;
                ntfPacket.Serialize(ntf);
                client.session.AsyncSend(ntfPacket);
                client.MoveNext();
            });

            client.number = 1;
            MsgCliSvr_Greeting_Req req = new MsgCliSvr_Greeting_Req();
            req.text = "Hello" + client.number.ToString();

            Gamnet.Packet packet = new Gamnet.Packet();
            packet.Id = MsgCliSvr_Greeting_Req.MSG_ID;
            packet.Serialize(req);
            client.session.AsyncSend(packet);
        }
    }
}
