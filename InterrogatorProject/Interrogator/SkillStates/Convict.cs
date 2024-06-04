using UnityEngine;
using EntityStates;
using InterrogatorMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;
using InterrogatorMod.Interrogator.Components;
using static RoR2.OverlapAttack;

namespace InterrogatorMod.Interrogator.SkillStates
{
    public class Convict : BaseInterrogatorSkillState
    {
        public GameObject markedPrefab = InterrogatorAssets.interrogatorConvictedConsume;
        private float baseDuration = 0.5f;

        private float duration;

        private InterrogatorTracker tracker;

        private HurtBox victim;

        private CharacterBody victimBody;

        private CameraTargetParams.AimRequest aimRequest;
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            tracker = this.GetComponent<InterrogatorTracker>();
            if(tracker)
            {
                victim = tracker.GetTrackingTarget();
                if(victim)
                {
                    victimBody = victim.healthComponent.body;
                    if((victimBody.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff) || characterBody.skillLocator.special.skillNameToken == InterrogatorSurvivor.INTERROGATOR_PREFIX + "SPECIAL_SCEPTER_CONVICT_NAME") && !victimBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff))
                    {
                        if (base.cameraTargetParams)
                        {
                            aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
                        }
                        StartAimMode(duration);
                        PlayAnimation("Gesture, Override", "Point", "Swing.playbackRate", duration * 1.5f);
                        EffectManager.SpawnEffect(markedPrefab, new EffectData
                        {
                            origin = victimBody.corePosition,
                            scale = 1.5f
                        }, transmit: true);
                        
                        if(NetworkServer.active)
                        {
                            victimBody.AddTimedBuff(InterrogatorBuffs.interrogatorConvictBuff, this.interrogatorController.convictDurationMax);
                            characterBody.AddTimedBuff(InterrogatorBuffs.interrogatorConvictBuff, this.interrogatorController.convictDurationMax);
                        }
                        this.interrogatorController.convictedVictimBody = victimBody;
                        this.interrogatorController.EnableSword();
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            aimRequest?.Dispose();
        }
    }
}