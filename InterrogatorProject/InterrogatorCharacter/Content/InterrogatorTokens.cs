﻿using System;
using InterrogatorMod.Modules;
using InterrogatorMod.Interrogator;
using InterrogatorMod.Interrogator.Achievements;
using UnityEngine.UIElements;

namespace InterrogatorMod.Interrogator.Content
{
    public static class InterrogatorTokens
    {
        public static void Init()
        {
            AddInterrogatorTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Spy.txt");
            //todo guide
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddInterrogatorTokens()
        {
            #region Interrogator
            string prefix = InterrogatorSurvivor.INTERROGATOR_PREFIX;

            string desc = "Interrogator relishes the pain of others. Don't have too much fun hurting your allies, or do...<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Punish the Guilty after they hit you to gain attack speed and move speed. No running from justice." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > If you need a quick and dirty Guilty buff, swing and hit yourself instead. The law applies to everyone!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Falsify is a great way to spot the Guilty before they commit crimes. Unethical? What do you mean?" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Convict a Guilty target to make sure they are punished for their acts. Guilty until proven innocent after all." + Environment.NewLine + Environment.NewLine;

            string lore = "Insert goodguy lore here";
            string outro = "..and so he left, itching to enact more \"justice\".";
            string outroFailure = "..and so he vanished, punished for his crimes.";
            
            Language.Add(prefix + "NAME", "Interrogator");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Unhinged Tormentor");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Torment");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"<color=#FFBF66>Interrogator</color> can hit and be hit by both allies and enemies. " +
                $"Attackers that hit <color=#FFBF66>Interrogator</color> are marked as <color=#FFBF66>Guilty</color>, " +
                $"granting <color=#FFBF66>Interrogator</color> <style=cIsDamage>attack speed</style>, <style=cIsHealing>health regen</style>, and " +
                $"<style=cIsDamage>damage</style> for each <color=#FFBF66>Guilty</color> target.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SWING_NAME", "Brutal Bash");
            Language.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"Swing in front dealing " +
                $"<style=cIsDamage>{InterrogatorConfig.brutalBashDamageCoefficient.Value * 100f}% damage</style>. " +
                $"Missing the attack causes you to take <style=cIsDamage>damage</style> instead.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_AFFRAY_NAME", "Affray");
            Language.Add(prefix + "SECONDARY_AFFRAY_DESCRIPTION", $"<style=cIsDamage>Slayer.</style> Launch a cleaver that deals " +
                $"<style=cIsDamage>{InterrogatorConfig.affrayDamageCoefficient.Value * 100f}% damage</style>. " +
                $"If <color=#FFBF66>Affray</color> kills its target, apply <style=cIsDamage>Hemmorhage</style> and " +
                $"<color=#FFBF66>Pressure</color> to everyone in the area.");
            #endregion

            #region Utility 
            Language.Add(prefix + "UTILITY_FALSIFY_NAME", "Falsify");
            Language.Add(prefix + "UTILITY_FALSIFY_DESCRIPTION", $"Dash forward dealing " +
                $"<style=cIsDamage>{InterrogatorConfig.falsifyDamageCoefficient.Value * 100f}% damage</style> " +
                $"applying <color=#FFBF66>Guilty</color> to targets hit.");

            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_CONVICT_NAME", "Convict");
            Language.Add(prefix + "SPECIAL_CONVICT_DESCRIPTION", $"Target a <color=#FFBF66>Guilty</color> enemy and fight them for 10 seconds. " +
                $"Your primary can no longer hit you but can continuously stack <color=#FFBF66>Guilty's</color> buff. " +
                $"During <color=#FFBF66>Convict</color> all external <style=cIsDamage>damage</style> is negated including your own.");

            Language.Add(prefix + "SPECIAL_SCEPTER_CONVICT_NAME", "Punish");
            Language.Add(prefix + "SPECIAL_SCEPTER_CONVICT_DESCRIPTION", $"Target a <color=#FFBF66>Guilty</color> enemy and force them to fight you for 10 seconds. Your primary can no longer hit you but will continuously add <color=#FFBF66>Guilty's</color> buff to you. " +
                $"During this time all external <style=cIsDamage>damage</style> is negated but all your <style=cIsDamage>damage</style> dealt to others is <style=cIsUtility>negated</style>." + Tokens.ScepterDescription("Convict can target enemies without Guilty and damage you deal is no longer negated but is reduced by 75%."));
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(InterrogatorMasterAchievement.identifier), "Interrogator: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(InterrogatorMasterAchievement.identifier), "As Interrogator, beat the game or obliterate on Monsoon.");
            /*
            Language.Add(Tokens.GetAchievementNameToken(SpyUnlockAchievement.identifier), "Dressed to Kill");
            Language.Add(Tokens.GetAchievementDescriptionToken(SpyUnlockAchievement.identifier), "Get a Backstab.");
            */
            #endregion

            #endregion
        }
    }
}