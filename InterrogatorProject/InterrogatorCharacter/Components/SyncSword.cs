using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace InterrogatorMod.Interrogator.Components
{
    public class SyncSword : INetMessage
    {
        private NetworkInstanceId netId;
        private bool enableSword;

        public SyncSword()
        {
        }

        public SyncSword(NetworkInstanceId netId, bool enableSword)
        {
            this.netId = netId;
            this.enableSword = enableSword;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.enableSword = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            if(enableSword) bodyObject.GetComponent<InterrogatorController>().EnableSword();
            else bodyObject.GetComponent<InterrogatorController>().DisableSword();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.enableSword);
        }
    }
}