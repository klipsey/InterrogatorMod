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

        internal static void Init()
        {
            Default = DamageAPI.ReserveDamageType();
            InterrogatorPressure = DamageAPI.ReserveDamageType();
            InterrogatorGuilty = DamageAPI.ReserveDamageType();
            Hook();
        }
        private static void Hook()
        {
        }
    }
}
