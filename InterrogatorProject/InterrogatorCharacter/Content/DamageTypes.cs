using HG;
using Newtonsoft.Json.Linq;
using R2API;
using RoR2;
using RoR2.Projectile;
using InterrogatorMod.Modules;
using InterrogatorMod.Interrogator.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static RoR2.DotController;

namespace InterrogatorMod.Interrogator.Content
{
    public static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType Default;
        public static DamageAPI.ModdedDamageType InterrogatorPressure;
        public static DamageAPI.ModdedDamageType InterrogatorGuilty;
        public static DamageAPI.ModdedDamageType InterrogatorConvict;
        public static DamageAPI.ModdedDamageType InterrogatorPressureBleed;

        internal static void Init()
        {
            Default = DamageAPI.ReserveDamageType();
            InterrogatorPressure = DamageAPI.ReserveDamageType();
            InterrogatorGuilty = DamageAPI.ReserveDamageType();
            InterrogatorConvict = DamageAPI.ReserveDamageType();
            InterrogatorPressureBleed = DamageAPI.ReserveDamageType();
            Hook();
        }
        private static void Hook()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }
        private static void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            DamageInfo damageInfo = damageReport.damageInfo;
            if (!damageReport.attackerBody || !damageReport.victimBody)
            {
                return;
            }
            HealthComponent victim = damageReport.victim;
            GameObject inflictorObject = damageInfo.inflictor;
            CharacterBody victimBody = damageReport.victimBody;
            EntityStateMachine victimMachine = victimBody.GetComponent<EntityStateMachine>();
            CharacterBody attackerBody = damageReport.attackerBody;
            GameObject attackerObject = damageReport.attacker.gameObject;
            InterrogatorController iController = attackerBody.GetComponent<InterrogatorController>();
            if (NetworkServer.active)
            {
                if (iController && attackerBody.bodyIndex == BodyCatalog.FindBodyIndex("InterrogatorBody"))
                {
                    if (damageInfo.HasModdedDamageType(InterrogatorGuilty))
                    {
                        MarkedGuiltyController stink = victimBody.gameObject.GetComponent<MarkedGuiltyController>();
                        if (victimBody && !victimBody.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff) && !stink)
                        {
                            stink = victimBody.gameObject.AddComponent<MarkedGuiltyController>();
                            stink.attackerBody = attackerBody;
                            attackerBody.AddBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
                            if (attackerBody.teamComponent.teamIndex == victimBody.teamComponent.teamIndex)
                            {
                                victimBody.AddTimedBuff(InterrogatorBuffs.interrogatorGuiltyDebuff, 10f);
                            }
                            else
                            {
                                victimBody.AddBuff(InterrogatorBuffs.interrogatorGuiltyDebuff);
                            }
                        }
                    }

                    if (damageInfo.HasModdedDamageType(InterrogatorConvict) && victimBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff))
                    {
                        if (victimBody && victimBody.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff))
                        {
                            attackerBody.AddBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
                            if(victimBody.gameObject.TryGetComponent<ConvictedController>(out ConvictedController i))
                            {
                                i.additionalGuiltyTracker++;
                            }
                        }
                    }

                    if (damageInfo.HasModdedDamageType(InterrogatorPressureBleed))
                    {
                        if (victimBody && victimBody.healthComponent && victimBody.healthComponent.alive)
                        {
                            if(victimBody.teamComponent.teamIndex == TeamIndex.Player)
                            {
                                DotController.InflictDot(
                                    victim.gameObject,
                                    damageInfo.attacker,
                                    DotController.DotIndex.SuperBleed,
                                   5f * damageInfo.procCoefficient,
                                    0.25f);
                            }
                            else
                            {
                                DotController.InflictDot(
                                    victim.gameObject,
                                    damageInfo.attacker,
                                    DotController.DotIndex.SuperBleed,
                                    15f * damageInfo.procCoefficient,
                                    1f);
                            }
                            victimBody.AddTimedBuff(InterrogatorBuffs.interrogatorPressuredBuff, 7f);
                        }
                    }
                }
            }
        }
    }
}
