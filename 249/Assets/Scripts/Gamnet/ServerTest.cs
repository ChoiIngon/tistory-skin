﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gamnet
{
    public class ServerTest : MonoBehaviour
    {
        public string Host;
        public int Port;
        public int SessionCount;
        public int LoopCount;

        private void Start()
        {
        }

        public void Run()
        {
            for (int i = 0; i < SessionCount; i++)
            {
                GameObject go = new GameObject();
                go.transform.SetParent(transform);

                Assets.Client client = go.AddComponent<Assets.Client>();
                client.session = new Gamnet.ClientSession();
                client.session.RegisterHandler<Assets.Scripts.Message>(1, (Assets.Scripts.Message msg) =>
                {
                    Packet p = new Packet();
                    p.Id = 2;
                    p.Serialize(msg);
                    client.session.AsyncSend(p);
                });

                client.session.OnConnectEvent += () =>
                {
                    Assets.Scripts.Message message = new Assets.Scripts.Message();

                    message.greeting = "Hello World";
                    Packet req = new Packet();
                    req.Id = 1;
                    req.Serialize(message);
                    client.session.AsyncSend(req);
                };
                client.session.AsyncConnect(Host, Port);
            }
        }

        private void Update()
        {

        }
    }
}
