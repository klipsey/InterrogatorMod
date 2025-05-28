using UnityEngine;
using RoR2;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;

namespace InterrogatorMod.Interrogator.Components
{
    public class MarkedGuiltyController : MonoBehaviour
    {
        public CharacterBody attackerBody;
        public CharacterBody characterBody;
        private void Awake()
        {
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
                // Buff Linger
                attackerBody.AddTimedBuff(InterrogatorBuffs.interrogatorGuiltyBuff, 3.5f + 1.5f *
                    attackerBody.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));
            }
        }
    }
}