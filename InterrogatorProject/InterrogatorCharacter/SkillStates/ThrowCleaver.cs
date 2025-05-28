using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;
using InterrogatorMod.Interrogator.Content;
using InterrogatorMod.Interrogator.Components;
using R2API;

namespace InterrogatorMod.Interrogator.SkillStates
{
    public class ThrowCleaver : GenericProjectileBaseState
    {
        public static float baseDuration = 0.2f;
        public static float baseDelayDuration = 0.3f * baseDuration;
        public GameObject cleaverPrefab = InterrogatorAssets.cleaverPrefab;
        public InterrogatorController interrogatorController;
        private ChildLocator childLocator;
        public override void OnEnter()
        {
            interrogatorController = base.gameObject.GetComponent<InterrogatorController>();
            base.attackSoundString = "sfx_scout_baseball_hit";

            base.baseDuration = baseDuration;
            base.baseDelayBeforeFiringProjectile = baseDelayDuration;

            base.damageCoefficient = damageCoefficient;
            base.force = 120f;

            base.projectilePitchBonus = -3.5f;

            base.OnEnter();

            if(!characterMotor.isGrounded)
            {
                SmallHop(characterMotor, 5f);
            }
        }

        public override void FireProjectile()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                aimRay = this.ModifyProjectileAimRay(aimRay);
                aimRay.direction = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, 0f, this.projectilePitchBonus);
                ProjectileDamage moddedDamage = cleaverPrefab.GetComponent<ProjectileDamage>();
                interrogatorController = base.GetComponent<InterrogatorController>();
                if(interrogatorController.isConvicted) moddedDamage.damageType.AddModdedDamageType(DamageTypes.InterrogatorConvict);

                ProjectileManager.instance.FireProjectile(cleaverPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), this.gameObject, 
                    this.damageStat * InterrogatorConfig.affrayDamageCoefficient.Value, this.force, this.RollCrit(), DamageColorIndex.Default, null, -1f);

                if (moddedDamage.damageType.HasModdedDamageType(DamageTypes.InterrogatorConvict))
                {
                    moddedDamage.damageType.RemoveModdedDamageType(DamageTypes.InterrogatorConvict);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void PlayAnimation(float duration)
        {
            if (base.GetModelAnimator())
            {
                PlayCrossfade("Gesture, Override", "SwingCleaver", "Swing.playbackRate", this.duration * 5.5f, this.duration * 0.05f);
            }
        }
    }
}