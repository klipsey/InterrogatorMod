﻿using EntityStates;
using RoR2;
using InterrogatorMod.Interrogator.Components;
using InterrogatorMod.Interrogator.Content;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace InterrogatorMod.Modules.BaseStates
{
    public abstract class BaseInterrogatorSkillState : BaseSkillState
    {
        protected InterrogatorController interrogatorController;

        protected bool isConvicting;
        public virtual void AddRecoil2(float x1, float x2, float y1, float y2)
        {
            this.AddRecoil(x1, x2, y1, y2);
        }
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        protected void RefreshState()
        {
            if (!interrogatorController)
            {
                interrogatorController = base.GetComponent<InterrogatorController>();
                isConvicting = interrogatorController.isConvicted;
            }
        }
    }
}
