using Engine.Characters.Entity.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.FiniteStateMachine.States
{
    public class WaveState : FSMState
    {
        private bool Toggle;

        public WaveState(FSMEngine fsmEngine) : base(fsmEngine) { }

        internal override StatesEnum Type
        {
            get { return StatesEnum.Wave; }
        }

        internal override void StateEnter()
        {
            Toggle = false;
        }

        internal override void StateExit()
        {

        }

        internal override void Update()
        {
            NativeController character = FSM.nativeController;
            BackwardPersonCamera backcamera = character.BackwardPersoncamera;
            
            if (!Toggle)
            {
                character.Wave();
                Toggle = true;
                return;
            }

            if (character.IsWave() == false )   //release camera after anim wave
            {
                backcamera.ReleaseCamera();
                FSM.SetState(this, StatesEnum.Idle);
                return;
            }

        }

        internal override void FixedUpdate()
        {
            
        }
       
    }
}
