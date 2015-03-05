using Engine.Characters.Entity.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.FiniteStateMachine.States
{
    public class FlipState : FSMState
    {
        private bool Toggle;
        private float second;

        public FlipState( FSMEngine fsmEngine ) : base(fsmEngine) { }

        internal override StatesEnum Type
        {
            get { return StatesEnum.Flip; }
        }

        internal override void StateEnter()
        {
            Toggle = false;
            second = 0;
        }

        internal override void StateExit()
        {

        }

        internal override void Update()
        {
            NativeController character = FSM.nativeController;
            BackwardPersonCamera backcamera = character.BackwardPersoncamera;
            
            character.Speed = 0;

            if (!Toggle)
            {
                backcamera.Flip();
                Toggle = true;
                return;
            }

            if ( backcamera.IsInFlip == false )
            {
                second += Time.deltaTime;
                if (second < 1) return;     //fix camera delay
                FSM.SetState(this, StatesEnum.Wave);
                return;
            }

        }

        internal override void FixedUpdate()
        {
            
        }

    }
}
