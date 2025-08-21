using System;
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

            string lore = "Snow softly fell from the skies, as was usual for the region that Dylan had found himself in. He, with his squad, stood on duty, looking out for either danger, or someone who needed rescue.\n\n" +
                "\"Sir, I think I see someone.\" The private turned around to face his superior. The commander didn’t turn around at first, only grunting." +
                " His voice was tired. The sheer amount of people lost to this planet had begun to exhaust even people like him, a veteran of the 2019 War. " +
                "This rescue operation had been a disaster so far, and that’s excluding what happened to the crew of the Contact Light.\n\n" +
                "\"It’s a person, I’m certain,\"" +
                " The soldier continued, turning back to face the vast white hollow, with the occasional tree scattered around. " +
                "And he was correct: it was a person, lazily strolling through snow. " +
                "The weather last night was nightmarish: the elemental assault of the planet had covered the valley in a higher-than-knee deep layer of cold carpet, and only the top part of the figure was visible. \n\n" +
                "Dylan raised his head from the binoculars to see that his commander was finally facing in the same direction as him.\n\n" +
                "\"Sir, should we send someone?\" Dylan inquired.\n\n" +
                "\"No,\" replied his commander.\n\n" +
                "The private turned back to the landscape for a moment. \"Why?\"\n\n" +
                "\"It's not a human.\"\n\n" +
                "\"..Sure looks like one,\" the private said as he peered back through the binoculars.\n\n" +
                "\"Not a living one, at the very least.\"\n\n" +
                "\"... You think that planet could raise the dead?\"\n\n" +
                "\"I wouldn't be surprised\"\n\n" +
                "The soldier kept looking through his binoculars. " +
                "The longer he looked at the stranger, the more apparent the oddities that threw off his higher up became. " +
                "The lone wanderer was wearing a big, black jacket, the hood of which had thick dirty fur. " +
                "The man was well-prepared for cold weather, save for the fact that the sleeves of the jacket were torn off, exposing his arms. " +
                "He turned slightly, allowing the private a look at his torso: they were wearing a standard-issued chest plate, just like the one the private had on his chest, but it was an older model, probably outdated by a year or more. " +
                "The orange paint was completely chipped away, leaving the heavily scratched black metallic surface exposed. The helmet looked like it matched the chestplate; standard issue commando gear, a bit out of date. " +
                "The color had been slightly bleached by the sun.\n\n" +
                "Whoever- or whatever it was, it seemed like a ruse. " +
                "A mimic, luring people in. " +
                "As if something was trying to play human, but didn’t understand what made something human in the first place.\n\n" +
                "\"…Yeah, sir. You’re probably right.\"\n\n" +
                "What they found the next morning made that thing seem even less human. " +
                "A squad of soldiers that Dylan was part of was sent on a scouting mission, and remembering what transpired yesterday, the private convinced his comrades and commander to investigate as to where, or from where, the figure was walking. " +
                "The trail of footsteps led them to a brutal scene: a torn down Contact Light camp. It was a small one that had no more than 5 people. 5 poor souls murdered in ways that eyes could never truly comprehend, in a way that no human being should be able to. " +
                "And yet, there were no bite marks, no claw marks, no scales, pieces of tar or fur, no blood samples of native species: nothing was alien. Everything was human.";
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
                $"Attackers that hit <color=#FFBF66>Interrogator</color> are marked as <color=#FFBF66>Guilty</color>.");
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
                $"If <color=#FFBF66>Affray</color> kills its target, apply <style=cIsHealth>Hemmorhage</style> and " +
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
            Language.Add(prefix + "SPECIAL_CONVICT_DESCRIPTION", $"Duel a <color=#FFBF66>Guilty</color> enemy for 10 seconds. Your primary no longer hits you and grants a <color=#FFBF66>Guilty</color> stack on hit. " +
                $"During the duel, external <style=cIsDamage>damage</style> to you is negated but <style=cIsDamage>damage</style> dealt to enemies other than the target is <style=cIsUtility>negated</style>.");

            Language.Add(prefix + "SPECIAL_SCEPTER_CONVICT_NAME", "Punish");
            Language.Add(prefix + "SPECIAL_SCEPTER_CONVICT_DESCRIPTION", $"Duel a <color=#FFBF66>Guilty</color> enemy for 10 seconds. Your primary no longer hits you and grants a <color=#FFBF66>Guilty</color> stack on hit. " +
                $"During the duel, external <style=cIsDamage>damage</style> to you is negated but <style=cIsDamage>damage</style> dealt to enemies other than the target is <style=cIsUtility>negated</style>." + Tokens.ScepterDescription("Convict can target enemies without Guilty and damage you deal is no longer negated but is reduced by 75%."));
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