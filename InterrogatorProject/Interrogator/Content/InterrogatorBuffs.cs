using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace InterrogatorMod.Interrogator.Content
{
    public static class InterrogatorBuffs
    {
        public static BuffDef interrogatorGuiltyDebuff;
        public static BuffDef interrogatorGuiltyBuff;
        public static BuffDef interrogatorPressuredBuff;
        public static BuffDef interrogatorConvictBuff;
        public static void Init(AssetBundle assetBundle)
        {
            interrogatorGuiltyBuff = Modules.Content.CreateAndAddBuff("InterrogatorGuiltyDebuff", Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/texBuffSlow50Icon.tif").WaitForCompletion(),
                InterrogatorAssets.interrogatorColor, true, false, false);
            interrogatorGuiltyDebuff = Modules.Content.CreateAndAddBuff("InterrogatorGuiltyBuff", Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/texBuffSlow50Icon.tif").WaitForCompletion(),
                InterrogatorAssets.interrogatorColor, false, true, false);
            
            interrogatorPressuredBuff = Modules.Content.CreateAndAddBuff("InterrogatorPressuredDebuff", Addressables.LoadAssetAsync<Sprite>("RoR2/Base/CritOnUse/texBuffFullCritIcon.tif").WaitForCompletion(),
                InterrogatorAssets.interrogatorColor, false, false, false);
            
            interrogatorConvictBuff = Modules.Content.CreateAndAddBuff("InterrogatorConvictBuff", LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                InterrogatorAssets.interrogatorColor, false, false, false);
        }
    }
}
