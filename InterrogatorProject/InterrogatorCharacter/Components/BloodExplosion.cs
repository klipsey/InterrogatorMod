﻿using UnityEngine;
using RoR2;

namespace InterrogatorMod.Interrogator.Components
{
    public class BloodExplosion : MonoBehaviour
    {
        private void Awake()
        {
            CharacterBody characterBody = this.GetComponent<CharacterBody>();

            if (this.transform)
            {
                EffectManager.SpawnEffect(Content.InterrogatorAssets.bloodExplosionEffect, new EffectData
                {
                    origin = this.transform.position,
                    rotation = Quaternion.identity,
                    scale = 0.5f
                }, false);

                GameObject.Instantiate(Content.InterrogatorAssets.bloodSpurtEffect, this.transform);
                Util.PlaySound("sfx_blood_gurgle", base.gameObject);
            }
        }

        private void LateUpdate()
        {
            if (this.transform) this.transform.localScale = Vector3.zero;
        }
    }
}