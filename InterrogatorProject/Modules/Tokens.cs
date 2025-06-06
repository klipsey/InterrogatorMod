﻿using InterrogatorMod.Interrogator.Content;

namespace InterrogatorMod.Modules
{
    internal static class Tokens
    {
        public const string agilePrefix = "<style=cIsUtility>Agile</style>";

        public const string interrogatorPressuredPrefix = "Pressured";

        public static string agileKeyword = KeywordText("Agile", "The skill can be used while sprinting.");

        public static string slayerKeyword = KeywordText("Slayer", "The skill deals 2% more damage per 1% of health the target has lost, up to <style=cIsDamage>3x</style> damage.");

        public static string interrogatorPressuredKeyword = KeywordText("Pressured", "Boost attack speed and move speed but lowers armor and damage (decreased ally negative stats).");

        public static string interrogatorGuiltyKeyword = KeywordText("Ally Damage", "Allies take and deal less damage to each other and Guilty expires from them after 10 seconds.");
        public static string DamageText(string text)
        {
            return $"<style=cIsDamage>{text}</style>";
        }
        public static string DamageValueText(float value)
        {
            return $"<style=cIsDamage>{value * 100}% damage</style>";
        }
        public static string UtilityText(string text)
        {
            return $"<style=cIsUtility>{text}</style>";
        }
        public static string RedText(string text) => HealthText(text);
        public static string HealthText(string text)
        {
            return $"<style=cIsHealth>{text}</style>";
        }
        public static string KeywordText(string keyword, string sub)
        {
            return $"<style=cKeywordName>{keyword}</style><style=cSub>{sub}</style>";
        }
        public static string ScepterDescription(string desc)
        {
            return $"\n<color=#d299ff>SCEPTER: {desc}</color>";
        }

        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}