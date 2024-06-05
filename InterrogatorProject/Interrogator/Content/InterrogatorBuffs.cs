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
            interrogatorGuiltyBuff = Modules.Content.CreateAndAddBuff("InterrogatorGuiltyBuff", assetBundle.LoadAsset<Sprite>("texGuiltyBuff"),
                InterrogatorAssets.interrogatorColor, true, false, false);

            interrogatorGuiltyDebuff = Modules.Content.CreateAndAddBuff("InterrogatorGuiltyDebuff", assetBundle.LoadAsset<Sprite>("texGuiltyDebuff"),
                InterrogatorAssets.interrogatorColor, false, true, false);

            interrogatorPressuredBuff = Modules.Content.CreateAndAddBuff("InterrogatorPressuredDebuff", Addressables.LoadAssetAsync<Sprite>("RoR2/Base/CritOnUse/texBuffFullCritIcon.tif").WaitForCompletion(),
                InterrogatorAssets.interrogatorColor, false, false, false);
            
            interrogatorConvictBuff = Modules.Content.CreateAndAddBuff("InterrogatorConvictBuff", assetBundle.LoadAsset<Sprite>("texConvictBuff"), 
                InterrogatorAssets.interrogatorColor, true, false, false);
        }
    }
}
