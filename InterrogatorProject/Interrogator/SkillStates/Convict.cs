using UnityEngine;
using EntityStates;
using InterrogatorMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using InterrogatorMod.Interrogator.Content;
using UnityEngine.Networking;

namespace InterrogatorMod.Interrogator.SkillStates
{
    public class Convict : BaseInterrogatorSkillState
    {
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            Util.PlaySound("sfx_scout_swap_weapon", this.gameObject);
            
            if (!this.interrogatorController.IsStopWatchOut())
            {
                if (NetworkServer.active) characterBody.AddBuff(InterrogatorBuffs.interrogatorGuiltyDebuff);
                this.interrogatorController.EnableWatchLayer();
            }
            else
            {
                if (NetworkServer.active) characterBody.RemoveBuff(InterrogatorBuffs.interrogatorGuiltyDebuff);
                this.interrogatorController.DisableWatchLayer();
            }

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}