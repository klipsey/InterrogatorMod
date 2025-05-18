using UnityEngine;
using EntityStates;
using InterrogatorMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using InterrogatorMod.Interrogator.Content;
using static R2API.DamageAPI;
using UnityEngine.Networking;
using R2API.Networking;
using InterrogatorMod.Interrogator.Components;
using R2API.Networking.Interfaces;

namespace InterrogatorMod.Interrogator.SkillStates
{
    public class Swing : BaseMeleeAttack
    {
        public bool hitSelf = true;
        private bool hasHitSelf = false;
        public override void OnEnter()
        {
            hasHitSelf = false;
            RefreshState();
            hitboxGroupName = "MeleeHitbox";

            damageType = DamageType.Generic;
            damageCoefficient = InterrogatorConfig.brutalBashDamageCoefficient.Value;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.625f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.4f;
            attackEndPercentTime = 0.6f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 1f;

            hitStopDuration = 0.05f;
            attackRecoil = 0.5f;
            hitHopVelocity = this.isConvicting ? 4f : 8f;

            swingSoundString = this.isConvicting ? "Play_merc_sword_swing" : "sfx_driver_swing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingMuzzle1" : "SwingMuzzle2";
            playbackRateParam = "Swing.playbackRate";
            swingEffectPrefab = this.isConvicting ? InterrogatorAssets.swordSwingEffect : InterrogatorAssets.batSwingEffect;
            if (this.isConvicting)
            {
                moddedDamageTypeHolder.Add(DamageTypes.InterrogatorConvict);
            }
            hitEffectPrefab = InterrogatorAssets.batHitEffect;

            impactSound = this.isConvicting ? InterrogatorAssets.swordImpactSoundEvent.index : InterrogatorAssets.batImpactSoundEvent.index;

            base.OnEnter();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();

            hitSelf = false;
        }

        protected override void FireAttack()
        {
            if (base.isAuthority)
            {
                Vector3 direction = this.GetAimRay().direction;
                direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
                this.FindModelChild("MeleePivot").rotation = Util.QuaternionSafeLookRotation(direction);
                base.FireAttack();

                NetworkIdentity identity = base.gameObject.GetComponent<NetworkIdentity>();
                if (!identity) return;

                new SyncSelfDamage(identity.netId, hitSelf).Send(NetworkDestination.Server);
            }
        }

        protected override void PlaySwingEffect()
        {
            Util.PlaySound(this.swingSoundString, this.gameObject);
            if (this.swingEffectPrefab)
            {
                Transform muzzleTransform = this.FindModelChild(this.muzzleString);
                if (muzzleTransform)
                {
                    this.swingEffectPrefab = Object.Instantiate<GameObject>(this.swingEffectPrefab, muzzleTransform);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool fireEnded = stopwatch >= duration * attackEndPercentTime;

            if (fireEnded && this.interrogatorController.hitSelf && !isConvicting && !hasHitSelf)
            {
                DamageInfo selfDamage = new DamageInfo();
                selfDamage.attacker = base.gameObject;
                selfDamage.inflictor = base.gameObject;
                selfDamage.damage = this.damageCoefficient * this.damageStat * 0.75f;
                selfDamage.procCoefficient = 0.5f;
                selfDamage.crit = RollCrit();
                selfDamage.damageType = DamageType.NonLethal;
                selfDamage.AddModdedDamageType(DamageTypes.InterrogatorGuilty);
                selfDamage.canRejectForce = true;
                selfDamage.damageColorIndex = DamageColorIndex.SuperBleed;
                selfDamage.force = Vector3.zero;
                selfDamage.dotIndex = DotController.DotIndex.None;
                selfDamage.position = base.transform.position;
                selfDamage.damageType.damageSource = DamageSource.Primary;

                this.healthComponent.TakeDamage(selfDamage);

                hasHitSelf = true;

                Util.PlaySound("sfx_scout_baseball_impact", base.gameObject);
            }
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "Swing" + (1 + swingIndex), playbackRateParam, duration * 2.2f, 0.05f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(hitSelf);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            hitSelf = reader.ReadBoolean();
        }
    }
}