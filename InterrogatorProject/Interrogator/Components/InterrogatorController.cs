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
        public string currentSkinNameToken => this.skinController.skins[this.skinController.currentSkinIndex].nameToken;

        public string altSkinNameToken => InterrogatorSurvivor.INTERROGATOR_PREFIX + "MASTERY_SKIN_NAME";

        private bool hasPlayed = false;
        public bool isConvicted => this.characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff);
        private bool stopwatchOut = false;
        public bool pauseTimer = false;

        public float convictDurationMax = InterrogatorStaticValues.baseConvictTimerMax;

        private int guiltyCounter = 0;

        private uint playID1;

        private ParticleSystem swordEffect;

        public Action onConvictDurationChange;

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
                this.swordEffect = this.childLocator.FindChild("SpecialEffectHand").gameObject.GetComponent<ParticleSystem>();
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
        private void Inventory_onItemAddedClient(ItemIndex itemIndex)
        {
            if (itemIndex == DLC1Content.Items.EquipmentMagazineVoid.itemIndex)
            {
                this.convictDurationMax = InterrogatorStaticValues.baseConvictTimerMax + this.characterBody.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
            }
        }

        #endregion
        private void FixedUpdate()
        {
            if(this.characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff))
            {
                EnableSword();
                onConvictDurationChange.Invoke();
            }
            else if (!characterBody.HasBuff(InterrogatorBuffs.interrogatorConvictBuff) && guiltyCounter > 0 && !hasPlayed)
            {
                hasPlayed = true;
                DisableSword();
                if (NetworkServer.active)
                {
                    for (int i = 0; i < this.guiltyCounter; i++)
                    {
                        this.characterBody.RemoveBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
                    }
                }
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
        public void ChangeCounter(int i)
        {
            guiltyCounter += i;
        }

        public void AddToCounter()
        {
            NetworkIdentity identity = this.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncCounter(identity.netId, this.gameObject, true).Send(NetworkDestination.Clients);
        }

        public void RemoveFromCounter()
        {
            NetworkIdentity identity = this.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncCounter(identity.netId, this.gameObject, false).Send(NetworkDestination.Clients);
        }

        public void RemoveBuff()
        {
            if (NetworkServer.active) this.characterBody.RemoveBuff(InterrogatorBuffs.interrogatorGuiltyBuff);
        }
        public void EnableSword()
        {
            if (!this.swordEffect.isPlaying) swordEffect.Play();
        }
        public void DisableSword() 
        {
            if(this.swordEffect.isPlaying) swordEffect.Stop();
        }
        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(this.playID1);

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient -= this.Inventory_onItemAddedClient;
            }

            On.RoR2.FriendlyFireManager.ShouldSplashHitProceed -= FriendlyFireManager_ShouldSplashHitProceed;
            On.RoR2.FriendlyFireManager.ShouldDirectHitProceed -= FriendlyFireManager_ShouldDirectHitProceed;
            On.RoR2.FriendlyFireManager.ShouldSeekingProceed -= FriendlyFireManager_ShouldSeekingProceed;
        }
    }
}
