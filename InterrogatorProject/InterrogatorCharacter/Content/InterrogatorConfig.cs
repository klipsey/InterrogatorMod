using BepInEx.Configuration;
using InterrogatorMod.Modules;

namespace InterrogatorMod.Interrogator.Content
{
    public static class InterrogatorConfig
    {
        public static ConfigEntry<bool> forceUnlock;

        public static ConfigEntry<float> maxHealth;
        public static ConfigEntry<float> healthRegen;
        public static ConfigEntry<float> armor;
        public static ConfigEntry<float> shield;

        public static ConfigEntry<int> jumpCount;

        public static ConfigEntry<float> damage;
        public static ConfigEntry<float> attackSpeed;
        public static ConfigEntry<float> crit;

        public static ConfigEntry<float> moveSpeed;
        public static ConfigEntry<float> acceleration;
        public static ConfigEntry<float> jumpPower;

        public static ConfigEntry<bool> autoCalculateLevelStats;

        public static ConfigEntry<float> healthGrowth;
        public static ConfigEntry<float> regenGrowth;
        public static ConfigEntry<float> armorGrowth;
        public static ConfigEntry<float> shieldGrowth;

        public static ConfigEntry<float> damageGrowth;
        public static ConfigEntry<float> attackSpeedGrowth;
        public static ConfigEntry<float> critGrowth;

        public static ConfigEntry<float> moveSpeedGrowth;
        public static ConfigEntry<float> jumpPowerGrowth;

        public static ConfigEntry<float> brutalBashDamageCoefficient;

        public static ConfigEntry<float> affrayDamageCoefficient;

        public static ConfigEntry<float> falsifyDamageCoefficient;

        public static ConfigEntry<float> convictMaxDuration;

        public static ConfigEntry<float> guiltyHealthRegenPerStack;
        public static ConfigEntry<float> guiltyBaseDamagePerStack;
        public static ConfigEntry<float> guiltyAttackSpeedPerStack;

        public static ConfigEntry<float> pressuredAttackSpeed;
        public static ConfigEntry<float> pressuredMoveSpeed;
        public static ConfigEntry<float> pressuredArmorLoss;
        public static ConfigEntry<float> pressuredDamageLoss;

        public static ConfigEntry<float> allyDamage;
        public static void Init()
        {
            string section = "Stats - 01";
            string section2 = "QOL - 02";

            damage = Config.BindAndOptions(section, "Change Base Damage Value", 12f);

            maxHealth = Config.BindAndOptions(section, "Change Max Health Value", 160f);
            healthRegen = Config.BindAndOptions(section, "Change Health Regen Value", 3f);
            armor = Config.BindAndOptions(section, "Change Armor Value", 20f);
            shield = Config.BindAndOptions(section, "Change Shield Value", 0f);

            jumpCount = Config.BindAndOptions(section, "Change Jump Count", 1);

            attackSpeed = Config.BindAndOptions(section, "Change Attack Speed Value", 1f);
            crit = Config.BindAndOptions(section, "Change Crit Value", 1f);

            moveSpeed = Config.BindAndOptions(section, "Change Move Speed Value", 7f);
            acceleration = Config.BindAndOptions(section, "Change Acceleration Value", 80f);
            jumpPower = Config.BindAndOptions(section, "Change Jump Power Value", 15f);

            autoCalculateLevelStats = Config.BindAndOptions(section, "Auto Calculate Level Stats", true);

            healthGrowth = Config.BindAndOptions(section, "Change Health Growth Value", 0.3f);
            regenGrowth = Config.BindAndOptions(section, "Change Regen Growth Value", 0.2f);
            armorGrowth = Config.BindAndOptions(section, "Change Armor Growth Value", 0f);
            shieldGrowth = Config.BindAndOptions(section, "Change Shield Growth Value", 0f);

            damageGrowth = Config.BindAndOptions(section, "Change Damage Growth Value", 0.2f);
            attackSpeedGrowth = Config.BindAndOptions(section, "Change Attack Speed Growth Value", 0f);
            critGrowth = Config.BindAndOptions(section, "Change Crit Growth Value", 0f);

            moveSpeedGrowth = Config.BindAndOptions(section, "Change Move Speed Growth Value", 0f);
            jumpPowerGrowth = Config.BindAndOptions(section, "Change Jump Power Growth Value", 0f);

            brutalBashDamageCoefficient = Config.BindAndOptions(section, "Brutal Bash Damage Coefficient", 2.6f);
            affrayDamageCoefficient = Config.BindAndOptions(section, "Affray Damage Coefficient", 4.5f);
            falsifyDamageCoefficient = Config.BindAndOptions(section, "Falsify Damage Coefficient", 6f);
            convictMaxDuration = Config.BindAndOptions(section, "Convict Base Max Duration", 8f);
                
            guiltyHealthRegenPerStack = Config.BindAndOptions(section, "Guilty Health Regen Multiplier Per Stack", 0.15f);
            guiltyBaseDamagePerStack = Config.BindAndOptions(section, "Guilty Flat Base Damage Per Stack", 0.5f);
            guiltyAttackSpeedPerStack = Config.BindAndOptions(section, "Guilty Attack Speed Multiplier Per Stack", 0.15f);

            pressuredAttackSpeed = Config.BindAndOptions(section, "Pressured Attack Speed Multiplier", 0.3f);
            pressuredMoveSpeed = Config.BindAndOptions(section, "Pressured Move Speed Multiplier", 0.15f);
            pressuredArmorLoss = Config.BindAndOptionsSlider(section, "Pressured Armor Loss Multiplier", 0.1f, "0 - 1 (0% loss - 100% loss)", 0f, 1f);
            pressuredDamageLoss = Config.BindAndOptionsSlider(section, "Pressured Move Speed Multiplier", 0.1f, "0 - 1 (0% loss - 100% loss)", 0f, 1f);

            allyDamage = Config.BindAndOptionsSlider(section, "Percentage of Ally Damage Given And Taken by Interrogator", 0.9f, "0.01 == 1% of total damage", 0.01f, 1f);

            forceUnlock = Config.BindAndOptions(
                section2,
                "Unlock Interrogator",
                false,
                "Unlock Interrogator.", true);
        }
    }
}
