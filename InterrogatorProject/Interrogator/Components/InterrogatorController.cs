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

        public float convictTimerMax = 0f;
        private bool hasPlayed = false;
        public bool isConvicted = false;
        private bool stopwatchOut = false;
        private bool hasSpun = true;
        private bool hasPunishedChainStab = true;
        private bool hasPlayedRecharge = true;
        public bool pauseTimer = false;

        private float spinTimer = 0f;
        private float gracePeriod = 0f;
        public float convictTimer = 0f;

        private int chainStabComboCounter = 0;

        private uint playID1;

        private ParticleSystem handElectricityEffect;
        private ParticleSystem chainStabCombo;
        private ParticleSystem chainStabComboHand;
        private GameObject spinInstance;
        private GameObject muzzleTrail;

        public Action onConvictChange;

        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.childLocator = modelLocator.modelBaseTransform.GetComponentInChildren<ChildLocator>();
            this.animator = modelLocator.modelBaseTransform.GetComponentInChildren<Animator>();
            this.characterModel = modelLocator.modelBaseTransform.GetComponentInChildren<CharacterModel>();
            this.skillLocator = this.GetComponent<SkillLocator>();
            this.skinController = modelLocator.modelTransform.gameObject.GetComponent<ModelSkinController>();
        }

        private void Start()
        {
        }
        private void Update()
        {

        }
        private void FixedUpdate()
        {
            if(convictTimer > 0f)
            {
                convictTimer -= Time.fixedDeltaTime;
                onConvictChange.Invoke();
            }
        }
        public void ActivateCritLightning()
        {
            if (this.handElectricityEffect)
            {
                if (!this.handElectricityEffect.isPlaying) this.handElectricityEffect.Play();
                if (!hasPlayed)
                {
                    this.playID1 = Util.PlaySound("sfx_scout_atomic_duration", this.gameObject);
                    hasPlayed = true;
                }
            }
        }
        
        public void DeactivateCritLightning(bool willReturn = false)
        {
            if(this.handElectricityEffect)
            {
                if (willReturn) hasPlayed = false;
                if (this.handElectricityEffect.isPlaying) this.handElectricityEffect.Stop();
                AkSoundEngine.StopPlayingID(this.playID1);
            }
        }

        private void Inventory_onItemAddedClient(ItemIndex itemIndex)
        {
            if (itemIndex == DLC1Content.Items.EquipmentMagazineVoid.itemIndex)
            {
                this.convictTimerMax = InterrogatorStaticValues.baseConvictTimerMax + this.characterBody.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
            }
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(this.playID1);

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient -= this.Inventory_onItemAddedClient;
            }
        }
    }
}
