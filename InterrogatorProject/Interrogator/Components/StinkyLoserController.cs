﻿using UnityEngine;
using RoR2;
using InterrogatorMod.Interrogator.Content;

namespace InterrogatorMod.Interrogator.Components
{
    public class StinkyLoserController : MonoBehaviour
    {
        public InterrogatorController interrogatorController;
        public CharacterBody characterBody;
        private void Awake()
        {
            Log.Debug("YOU ARE A STINKY LOSER NOW");
            characterBody = this.GetComponent<CharacterBody>();
        }

        private void Start()
        {
            if(interrogatorController)
            {
                interrogatorController.StinkyLoserHasAlived();
            }
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
            if (interrogatorController) 
            {
                interrogatorController.StinkyLoserHasDied();
            }
        }
    }
}