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
        public static DamageAPI.ModdedDamageType InterrogatorPressureBleed;

        internal static void Init()
        {
            Default = DamageAPI.ReserveDamageType();
            InterrogatorPressure = DamageAPI.ReserveDamageType();
            InterrogatorGuilty = DamageAPI.ReserveDamageType();
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
            InterrogatorController iComponent = attackerBody.GetComponent<InterrogatorController>();
            if (NetworkServer.active)
            {
                if (iComponent)
                {
                    if (damageInfo.HasModdedDamageType(InterrogatorGuilty))
                    {
                        if (victimBody && !victimBody.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff))
                        {
                            if (attackerBody.teamComponent.teamIndex == victimBody.teamComponent.teamIndex)
                            {
                                attackerBody.AddTimedBuff(InterrogatorBuffs.interrogatorGuiltyDebuff, 10f);
                            }
                            else
                            {
                                attackerBody.AddBuff(InterrogatorBuffs.interrogatorGuiltyDebuff);
                            }
                        }
                    }

                    if (damageInfo.HasModdedDamageType(InterrogatorPressureBleed))
                    {
                        if (victimBody && victimBody.healthComponent && victimBody.healthComponent.alive)
                        {
                            DotController.InflictDot(
                                victim.gameObject,
                                damageInfo.attacker,
                                RoR2.DotController.DotIndex.SuperBleed,
                                15f * damageInfo.procCoefficient,
                                1f);
                            victimBody.AddBuff(InterrogatorBuffs.interrogatorPressuredBuff);
                        }
                    }
                }
            }
        }
    }
}
