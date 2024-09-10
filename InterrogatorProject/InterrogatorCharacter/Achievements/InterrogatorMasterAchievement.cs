using RoR2;
using InterrogatorMod.Modules.Achievements;
using InterrogatorMod.Interrogator;

namespace InterrogatorMod.Interrogator.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 0)]
    public class InterrogatorMasterAchievement : BaseMasteryAchievement
    {
        public const string identifier = InterrogatorSurvivor.INTERROGATOR_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = InterrogatorSurvivor.INTERROGATOR_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => InterrogatorSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}