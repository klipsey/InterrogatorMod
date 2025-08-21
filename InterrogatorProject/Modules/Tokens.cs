using InterrogatorMod.Interrogator.Content;

namespace InterrogatorMod.Modules
{
    internal static class Tokens
    {
        public const string agilePrefix = "<style=cIsUtility>Agile</style>";

        public const string interrogatorPressuredPrefix = "Pressured";

        public static string agileKeyword = KeywordText("Agile", "The skill can be used while sprinting.");

        public static string slayerKeyword = KeywordText("Slayer", "Deals up to <style=cIsDamage>3x</style> damage against low health enemies.");

        public static string hemKeyword = KeywordText("Hemorrhage", "Deal <style=cIsDamage>2000%</style> base damage over 15s. <i>Hemorrhage</i> can stack.");

        public static string interrogatorPressuredKeyword = KeywordText("Pressured", "Boost <style=cIsDamage>attack speed</style> and <style=cIsUtility>move speed</style> but lowers <style=cIsHealing>armor</style> and <style=cIsDamage>damage</style>.");
        
        public static string interrogatorGuiltyKeyword = KeywordText("Guilty", "Grants <color=#FFBF66>Interrogator</color> <style=cIsDamage>attack speed</style>, <style=cIsHealing>health regen</style>, and " +
                $"<style=cIsDamage>damage</style> for each <color=#FFBF66>Guilty</color> target.");

        public static string interrogatorAllyKeyword = KeywordText("Ally Damage", "Allies take and deal less damage to each other and <color=#FFBF66>Guilty</color> expires from them after 10 seconds.");

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