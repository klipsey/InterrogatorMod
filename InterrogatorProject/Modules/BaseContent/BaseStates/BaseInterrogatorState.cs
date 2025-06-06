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
    public abstract class BaseInterrogatorState : BaseState
    {
        protected InterrogatorController interrogatorController;

        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RefreshState();
        }
        protected void RefreshState()
        {
            if (!interrogatorController)
            {
                interrogatorController = base.GetComponent<InterrogatorController>();
            }
        }
    }
}
