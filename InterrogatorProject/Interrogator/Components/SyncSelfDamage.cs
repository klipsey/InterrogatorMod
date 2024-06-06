using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace InterrogatorMod.Interrogator.Components
{
    public class SyncSelfDamage : INetMessage
    {
        private NetworkInstanceId netId;
        private bool selfDamage;

        public SyncSelfDamage()
        {
        }

        public SyncSelfDamage(NetworkInstanceId netId, bool selfDamage)
        {
            this.netId = netId;
            this.selfDamage = selfDamage;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.selfDamage = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            bodyObject.GetComponent<InterrogatorController>().DidHit(selfDamage);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.selfDamage);
        }
    }
}