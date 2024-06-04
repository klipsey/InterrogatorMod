using UnityEngine;
using RoR2;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;

namespace InterrogatorMod.Interrogator.Components
{
    public class StinkyLoserController : MonoBehaviour
    {
        public CharacterBody attackerBody;
        public CharacterBody characterBody;
        private void Awake()
        {
            Log.Debug("YOU ARE A STINKY LOSER NOW");
            characterBody = this.GetComponent<CharacterBody>();
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            if(characterBody)
            {
                if(!characterBody.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff))
                {
                    Component.Destroy(this);
                }
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active) 
            {
                attackerBody.RemoveBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
            }
        }
    }
}