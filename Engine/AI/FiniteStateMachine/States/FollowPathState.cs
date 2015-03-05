using Assets.Engine.SimpleArrow;
using Engine.Characters.Entity.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.FiniteStateMachine.States
{
    public class FollowPathState : FSMState
    {
        public FollowPathState(FSMEngine fsmEngine) : base(fsmEngine) { }

        internal override StatesEnum Type
        {
            get { return StatesEnum.FollowPath; }
        }

        private bool Toggle;

        internal override void StateEnter()
        {
            Toggle = false;
        }

        internal override void StateExit()
        {

        }

        internal override void Update()
        {           
            CheckNewCheckPoint();

        }

        internal override void FixedUpdate()
        {
            
        }

        private void CheckNewCheckPoint()
        {
            NativeController character = FSM.nativeController;
            Transform transform = character.transform;

            //Character character = controller.Character;
            //Animator Anim = controller.Animator;

            if ( character.IsIdle)
            {
                FSM.SetState(this, StatesEnum.Flip);
                return;
            }

            if ( Vector3.Distance( character.Path.FirstCheckPoint, transform.position) < 1 )
            {
                if (Toggle == false)                //grab the camera one time
                {
                    FSM.nativeController.BackwardPersoncamera.GrabCamera(character);
                    Toggle = true;
                }

                GroundQuiver.GoOnNextCheckPoint( character.Path );

            }

            character.SetDirection( character.GetDirectionNextCheckPoint() );
        }        
    }
}
