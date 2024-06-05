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
            if(characterBody)
            {
                if(!characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff))
                {
                    Component.Destroy(this);
                }
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active) 
            {
                for(int i = 0; i < additionalGuiltyTracker; i++)
                {
                    attackerBody.RemoveBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
                }
                attackerBody.RemoveOldestTimedBuff(InterrogatorBuffs.interrogatorConvictBuff);
            }
        }
    }
}