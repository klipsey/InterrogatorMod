using UnityEngine;
using RoR2;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;

namespace InterrogatorMod.Interrogator.Components
{
    public class ConvictedController : MonoBehaviour
    {
        public CharacterBody attackerBody;
        public CharacterBody characterBody;
        public int additionalGuiltyTracker;
        private bool hasHadBuff;
        private float delay = 0.5f;
        private float timer;
        private void Awake()
        {
            characterBody = this.GetComponent<CharacterBody>();
            additionalGuiltyTracker = 0;
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if(characterBody && timer >= delay)
            {
                if(characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff)) hasHadBuff = true;
                if((!characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff) || characterBody.healthComponent.alive == false) && hasHadBuff)
                {
                    if (NetworkServer.active)
                    {
                        attackerBody.SetBuffCount(InterrogatorBuffs.interrogatorGuiltyBuff.buffIndex, attackerBody.GetBuffCount(InterrogatorBuffs.interrogatorGuiltyBuff) - additionalGuiltyTracker);
                        attackerBody.RemoveOldestTimedBuff(InterrogatorBuffs.interrogatorConvictBuff);
                        if(characterBody.healthComponent.alive == false)
                        {
                            attackerBody.healthComponent.AddBarrier(attackerBody.healthComponent.fullCombinedHealth * 0.5f);
                            attackerBody.AddTimedBuff(RoR2.RoR2Content.Buffs.ArmorBoost, 3f);
                        }
                    }
                    Component.Destroy(this);
                }
            }
        }
    }
}