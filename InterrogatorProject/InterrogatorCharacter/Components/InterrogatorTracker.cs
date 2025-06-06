﻿using System.Linq;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using InterrogatorMod.Interrogator;
using InterrogatorMod.Interrogator.Content;

namespace InterrogatorMod.Interrogator.Components

{
    [RequireComponent(typeof(TeamComponent))]
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    public class InterrogatorTracker : MonoBehaviour
    {
        public float maxTrackingDistance = 30f;

        public float maxTrackingAngle = 30f;

        public float trackerUpdateFrequency = 10f;

        private HurtBox trackingTarget;

        private CharacterBody characterBody;

        private TeamComponent teamComponent;

        private InputBankTest inputBank;

        private float trackerUpdateStopwatch;

        private Indicator indicator;

        private readonly BullseyeSearch search = new BullseyeSearch();

        private void Awake()
        {
            indicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
        }

        private void Start()
        {
            characterBody = GetComponent<CharacterBody>();
            inputBank = GetComponent<InputBankTest>();
            teamComponent = GetComponent<TeamComponent>();
        }

        public HurtBox GetTrackingTarget()
        {
            return trackingTarget;
        }
        private void OnEnable()
        {
            indicator.active = true;
        }

        private void OnDisable()
        {
            indicator.active = false;
        }

        private void FixedUpdate()
        {
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
            {
                trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
                _ = trackingTarget;
                Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                SearchForTarget(aimRay);
                indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
            }
        }

        private void SearchForTarget(Ray aimRay)
        {
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = maxTrackingDistance;
            search.maxAngleFilter = maxTrackingAngle;
            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);
            foreach (HurtBox hurt in this.search.GetResults())
            {
                if (hurt && hurt.healthComponent && hurt.healthComponent.body)
                {
                    if (!hurt.healthComponent.body.HasBuff(InterrogatorBuffs.interrogatorGuiltyDebuff) && characterBody.skillLocator.special.skillNameToken != InterrogatorSurvivor.INTERROGATOR_PREFIX + "SPECIAL_SCEPTER_CONVICT_NAME")
                    {
                        this.search.FilterOutGameObject(hurt.healthComponent.gameObject);
                    }
                }
            }
            trackingTarget = search.GetResults().FirstOrDefault();
        }
    }
}

