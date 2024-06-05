using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.HudOverlay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using InterrogatorMod.Interrogator.Content;
using System;

namespace InterrogatorMod.Interrogator.Components
{
    public class InterrogatorController : MonoBehaviour
    {
        private CharacterBody characterBody;
        private ModelSkinController skinController;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;
        private Material[] swordMat;
        private Material[] batMat;
        public string currentSkinNameToken => this.skinController.skins[this.skinController.currentSkinIndex].nameToken;
        public string altSkinNameToken => InterrogatorSurvivor.INTERROGATOR_PREFIX + "MASTERY_SKIN_NAME";

        private bool hasPlayed = false;
        public bool isConvicted => this.characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff);
        private bool stopwatchOut = false;
        public bool pauseTimer = false;

        public float convictDurationMax;

        private uint playID1;
        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.childLocator = modelLocator.modelBaseTransform.GetComponentInChildren<ChildLocator>();
            this.animator = modelLocator.modelBaseTransform.GetComponentInChildren<Animator>();
            this.characterModel = modelLocator.modelBaseTransform.GetComponentInChildren<CharacterModel>();
            this.skillLocator = this.GetComponent<SkillLocator>();
            this.skinController = modelLocator.modelTransform.gameObject.GetComponent<ModelSkinController>();

            Hook();

            this.Invoke("ApplySkin", 0.3f);
        }
        private void Start()
        {
        }
        #region tooMuchCrap
        private void Hook()
        {
            On.RoR2.FriendlyFireManager.ShouldSplashHitProceed += FriendlyFireManager_ShouldSplashHitProceed;
            On.RoR2.FriendlyFireManager.ShouldDirectHitProceed += FriendlyFireManager_ShouldDirectHitProceed;
            On.RoR2.FriendlyFireManager.ShouldSeekingProceed += FriendlyFireManager_ShouldSeekingProceed;

        }
        public void ApplySkin()
        {
            if (this.skinController)
            {
                this.swordMat = new Material[1];
                this.batMat = new Material[1];
                this.swordMat[0] = InterrogatorAssets.swordMat;
                this.batMat[0] = InterrogatorAssets.batMat;
            }
        }
        private bool FriendlyFireManager_ShouldSeekingProceed(On.RoR2.FriendlyFireManager.orig_ShouldSeekingProceed orig, HealthComponent victim, TeamIndex attackerTeamIndex)
        {
            if (victim.body.baseNameToken == "KENKO_INTERROGATOR_NAME" && attackerTeamIndex == victim.body.teamComponent.teamIndex)
            {
                return true;
            }
            else return orig.Invoke(victim, attackerTeamIndex);
        }

        private bool FriendlyFireManager_ShouldDirectHitProceed(On.RoR2.FriendlyFireManager.orig_ShouldDirectHitProceed orig, HealthComponent victim, TeamIndex attackerTeamIndex)
        {
            if (victim.body.baseNameToken == "KENKO_INTERROGATOR_NAME" && attackerTeamIndex == victim.body.teamComponent.teamIndex)
            {
                return true;
            }
            else return orig.Invoke(victim, attackerTeamIndex);
        }

        private bool FriendlyFireManager_ShouldSplashHitProceed(On.RoR2.FriendlyFireManager.orig_ShouldSplashHitProceed orig, HealthComponent victim, TeamIndex attackerTeamIndex)
        {
            if (victim.body.baseNameToken == "KENKO_INTERROGATOR_NAME" && attackerTeamIndex == victim.body.teamComponent.teamIndex)
            {
                return true;
            }
            else return orig.Invoke(victim, attackerTeamIndex);
        }

        #endregion
        private void FixedUpdate()
        {
            if (!characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff) && !hasPlayed)
            {
                hasPlayed = true;
                DisableSword();
            }

            if(skillLocator.secondary.CanExecute() && !childLocator.FindChild("CleaverModel").gameObject.activeSelf)
            {
                childLocator.FindChild("CleaverModel").gameObject.SetActive(true);
            }
            else if(!skillLocator.secondary.CanExecute() && childLocator.FindChild("CleaverModel").gameObject.activeSelf)
            {
                childLocator.FindChild("CleaverModel").gameObject.SetActive(false);
            }
        }

        public void EnableSword()
        {
            this.childLocator.FindChild("MeleeModel").gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = InterrogatorAssets.swordMesh;
            hasPlayed = false;
        }
        public void DisableSword() 
        {
            this.childLocator.FindChild("MeleeModel").gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = InterrogatorAssets.batMesh;
        }
        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(this.playID1);

            On.RoR2.FriendlyFireManager.ShouldSplashHitProceed -= FriendlyFireManager_ShouldSplashHitProceed;
            On.RoR2.FriendlyFireManager.ShouldDirectHitProceed -= FriendlyFireManager_ShouldDirectHitProceed;
            On.RoR2.FriendlyFireManager.ShouldSeekingProceed -= FriendlyFireManager_ShouldSeekingProceed;
        }
    }
}
