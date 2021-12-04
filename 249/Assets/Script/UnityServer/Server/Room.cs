﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServer.Common.Packet;

namespace UnityServer.Server
{
    public class Room : MonoBehaviour
    {
        public Server.Session session;
        Transform spheres;
        public float deltaTime;
        public const int sphereCount = 10;
        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Server");
            gameObject.tag = "Server";

            spheres = transform.Find("Spheres");
            Vector3[] initPositions = new Vector3 [] {
                new Vector3(-4, 4, -4),
                new Vector3(-3, 3, -4),
                new Vector3(-2, 2, -4),
                new Vector3(-1, 1, -4),
                new Vector3(-0, 0, -4),
                new Vector3( 1, -1, -4),
                new Vector3( 2, -2, -4),
                new Vector3( 3, -3, -4),
                new Vector3( 4, -4, -4),
                new Vector3( 1, -4, -4)
            };
            for (uint i = 0; i < sphereCount; i++)
            {
                GameObject go = Server.Main.Instance.CreateSphere();
                Common.Sphere sphere = go.AddComponent<Common.Sphere>();
                sphere.id = i+1;
                sphere.gameObject.layer = LayerMask.NameToLayer("Server");
                sphere.gameObject.tag = "Server";
                sphere.gameObject.name = $"Sphere{sphere.id}";
                sphere.rigidBody = sphere.GetComponent<Rigidbody>();
                sphere.transform.localPosition = initPositions[i];
                sphere.transform.SetParent(spheres, false);

                MsgSvrCli_CreateSphere_Ntf ntf = new MsgSvrCli_CreateSphere_Ntf();
                ntf.id = sphere.id;
                ntf.positionX = sphere.transform.localPosition.x;
                ntf.positionY = sphere.transform.localPosition.y;
                ntf.positionZ = sphere.transform.localPosition.z;
                ntf.velocityX = sphere.rigidBody.velocity.x;
                ntf.velocityY = sphere.rigidBody.velocity.x;
                ntf.velocityZ = sphere.rigidBody.velocity.x;
                session.Send<MsgSvrCli_CreateSphere_Ntf>(ntf);
            }
            deltaTime = 0.0f;
        }
        private void Update()
        {
            deltaTime += Time.deltaTime;
            if (Server.Main.Instance.syncInterval <= deltaTime && true == Server.Main.Instance.sync)
            {
                Transform spheres = transform.Find("Spheres");
                for (int i = 0; i < sphereCount; i++)
                {
                    var spheresTransform = spheres.GetChild(i);
                    var sphere = spheresTransform.GetComponent<Common.Sphere>();
                    MsgSvrCli_SyncPosition_Ntf ntf = new MsgSvrCli_SyncPosition_Ntf();
                    ntf.id = sphere.id;
                    ntf.positionX = sphere.transform.localPosition.x;
                    ntf.positionY = sphere.transform.localPosition.y;
                    ntf.positionZ = sphere.transform.localPosition.z;
                    ntf.rotationX = sphere.transform.rotation.x;
                    ntf.rotationY = sphere.transform.rotation.y;
                    ntf.rotationZ = sphere.transform.rotation.z;
                    ntf.rotationW = sphere.transform.rotation.w;
                    ntf.velocityX = sphere.rigidBody.velocity.x;
                    ntf.velocityY = sphere.rigidBody.velocity.x;
                    ntf.velocityZ = sphere.rigidBody.velocity.x;
                    session.Send<MsgSvrCli_SyncPosition_Ntf>(ntf);
                }

                deltaTime -= Server.Main.Instance.syncInterval;
            }
        }
    }
}