using UnityEngine;
using RoR2;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;
using R2API.Networking.Interfaces;
using R2API.Networking;

namespace InterrogatorMod.Interrogator.Components
{
    public class ConvictedController : MonoBehaviour
    {
        public CharacterBody attackerBody;
        public CharacterBody characterBody;
        public int additionalGuiltyTracker = 0;
        private bool hasHadBuff = false;
        private void Awake()
        {
            characterBody = this.GetComponent<CharacterBody>();
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            if(characterBody && attackerBody)
            {
                if(characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff)) hasHadBuff = true;

                if((!characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff) || !characterBody.healthComponent.alive) && hasHadBuff)
                {
                    Component.Destroy(this);
                }
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active)
            {
                attackerBody.SetBuffCount(InterrogatorBuffs.interrogatorGuiltyBuff.buffIndex, attackerBody.GetBuffCount(InterrogatorBuffs.interrogatorGuiltyBuff) - additionalGuiltyTracker);
                attackerBody.RemoveOldestTimedBuff(InterrogatorBuffs.interrogatorConvictBuff);

                if (!characterBody.healthComponent.alive)
                {
                    attackerBody.healthComponent.AddBarrier(attackerBody.healthComponent.fullCombinedHealth * 0.5f);
                    attackerBody.AddTimedBuff(RoR2.RoR2Content.Buffs.ArmorBoost, 3f);
                }


                NetworkIdentity identity = attackerBody.gameObject.GetComponent<NetworkIdentity>();
                if (!identity) return;

                new SyncSword(identity.netId, false).Send(NetworkDestination.Clients);
            }
        }
    }
}