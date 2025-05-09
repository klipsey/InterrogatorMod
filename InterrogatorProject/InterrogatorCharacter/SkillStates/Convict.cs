using UnityEngine;
using EntityStates;
using InterrogatorMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;
using InterrogatorMod.Interrogator.Components;
using static RoR2.OverlapAttack;
using System;
using R2API.Networking;
using R2API.Networking.Interfaces;

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

            if(base.isAuthority && tracker)
            {
                victim = this.tracker.GetTrackingTarget();
            }

            if (victim && victim.healthComponent && victim.healthComponent.body) victimBody = victim.healthComponent.body;

            if (!victim || !victimBody || !tracker)
            {
                this.skillLocator.special.AddOneStock();
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.cameraTargetParams)
            {
                aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }

            StartAimMode(this.duration);

            PlayCrossfade("Gesture, Override", "Point", "Swing.playbackRate", this.duration * 1.5f, this.duration * 0.05f);

            EffectManager.SpawnEffect(markedPrefab, new EffectData
            {
                origin = victimBody.corePosition,
                scale = 1.5f
            }, transmit: true);

            if(NetworkServer.active)
            {
                characterBody.AddTimedBuff(InterrogatorBuffs.interrogatorConvictBuff, interrogatorController.convictMaxDuration);
                victimBody.AddTimedBuff(InterrogatorBuffs.interrogatorConvictBuff, interrogatorController.convictMaxDuration);

                if (!this.victimBody.gameObject.GetComponent<ConvictedController>())
                {
                    this.victimBody.gameObject.AddComponent<ConvictedController>();
                }
                this.victimBody.gameObject.GetComponent<ConvictedController>().attackerBody = base.characterBody;

                NetworkIdentity identity = base.gameObject.GetComponent<NetworkIdentity>();
                if (!identity) return;

                new SyncSword(identity.netId, true).Send(NetworkDestination.Clients);
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
            base.OnExit();
            aimRequest?.Dispose();
            victim = null;
            victimBody = null;
        }
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(victim));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            victim = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}