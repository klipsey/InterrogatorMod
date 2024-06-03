using InterrogatorMod.Modules.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using InterrogatorMod.Interrogator.Content;
using R2API;

namespace InterrogatorMod.Interrogator.SkillStates
{
    public class Falsify : BaseInterrogatorSkillState
    {
        private Transform modelTransform;

        public static GameObject dashPrefab = InterrogatorAssets.dashEffect;

        public static float smallHopVelocity = 12f;

        public static float dashDuration = 0.3f;

        public static float speedCoefficient = 10f;

        public static string beginSoundString = "sfx_interrogator_dash";

        public static string endSoundString = "";

        public static float damageCoefficient = InterrogatorStaticValues.falsifyDamageCoefficient;

        public static float procCoefficient = 1f;

        public static GameObject hitEffectPrefab = InterrogatorAssets.batHitEffectRed;

        public static float hitPauseDuration = 0.012f;

        private float stopwatch;

        private Vector3 dashVector = Vector3.zero;

        private Animator animator;

        private CharacterModel characterModel;

        private HurtBoxGroup hurtboxGroup;

        private OverlapAttack overlapAttack;

        private ChildLocator childLocator;

        private bool isDashing;

        private bool inHitPause;

        private float hitPauseTimer;

        private CameraTargetParams.AimRequest aimRequest;

        public bool hasHit { get; private set; }

        public int dashIndex { private get; set; }

        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            Util.PlaySound(beginSoundString, base.gameObject);
            modelTransform = GetModelTransform();
            if (base.cameraTargetParams)
            {
                aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }
            if (modelTransform)
            {
                animator = modelTransform.GetComponent<Animator>();
                characterModel = modelTransform.GetComponent<CharacterModel>();
                childLocator = modelTransform.GetComponent<ChildLocator>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            SmallHop(base.characterMotor, smallHopVelocity);
            PlayAnimation("FullBody, Override", "Dash", "Dash.playbackRate", dashDuration);
            dashVector = base.inputBank.aimDirection;
            overlapAttack = InitMeleeOverlap(damageCoefficient, hitEffectPrefab, modelTransform, "MeleeHitbox");
            overlapAttack.damageType = DamageType.Stun1s;
            overlapAttack.AddModdedDamageType(DamageTypes.InterrogatorGuilty);
            if (this.isConvicting) overlapAttack.AddModdedDamageType(DamageTypes.InterrogatorConvict);
            if (NetworkServer.active)
            {
                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
            }
        }

        private void CreateDashEffect()
        {
            Transform transform = childLocator.FindChild("Chest");
            if (transform && dashPrefab)
            {
                UnityEngine.Object.Instantiate(dashPrefab, transform.position, Util.QuaternionSafeLookRotation(dashVector), transform);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterDirection.forward = dashVector;
            if (!isDashing)
            {
                isDashing = true;
                dashVector = base.inputBank.aimDirection;
                CreateDashEffect();
                base.gameObject.layer = LayerIndex.fakeActor.intVal;
                base.characterMotor.Motor.RebuildCollidableLayers();
            }
            if (!isDashing)
            {
                stopwatch += Time.fixedDeltaTime;
            }
            else if (base.isAuthority)
            {
                base.characterMotor.velocity = Vector3.zero;
                if (!inHitPause)
                {
                    bool num = overlapAttack.Fire();
                    stopwatch += Time.fixedDeltaTime;
                    if (num)
                    {
                        if (!hasHit)
                        {
                            hasHit = true;
                        }
                        inHitPause = true;
                        hitPauseTimer = hitPauseDuration / attackSpeedStat;
                    }
                    base.characterMotor.rootMotion += dashVector * moveSpeedStat * speedCoefficient * Time.fixedDeltaTime;
                }
                else
                {
                    hitPauseTimer -= Time.fixedDeltaTime;
                    if (hitPauseTimer < 0f)
                    {
                        inHitPause = false;
                    }
                }
            }
            if (stopwatch >= dashDuration / attackSpeedStat && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
            Util.PlaySound(endSoundString, base.gameObject);
            if (base.isAuthority)
            {
                base.characterMotor.velocity *= 0.1f;
                SmallHop(base.characterMotor, smallHopVelocity);
            }
            aimRequest?.Dispose();
            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
            }
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)dashIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            dashIndex = reader.ReadByte();
        }
    }
}
