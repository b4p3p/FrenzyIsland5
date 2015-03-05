using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.FiniteStateMachine.States
{
    public class IdleState : FSMState 
    {
        public IdleState(FSMEngine fsmEngine) : base(fsmEngine) { }

        internal override StatesEnum Type
        {
            get { return StatesEnum.Idle; }
        }

        internal override void StateEnter()
        {

        }

        internal override void StateExit()
        {

        }

        internal override void Update()
        {
            if ( !FSM.nativeController.IsIdle )
            {
                FSM.SetState(this , StatesEnum.FollowPath ); 
            }
        }

        internal override void FixedUpdate()
        {
            
        }

    }
}
