using Assets.Engine.AI.FiniteStateMachine.States;
using Engine.Characters.Entity.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.FiniteStateMachine
{
    public class FSMEngine : MonoBehaviour
    {
        public FSMState CurrentState { get; set; }
        
        internal NativeController nativeController { get; set; }
        
        private FSMState InitialState { get; set; }
       
        public FSMEngine(NativeController nativeController) 
        {
            this.nativeController = nativeController;
        }

        public void SetInitialState(FSMState initialState)
        {
            InitialState = initialState;
            CurrentState = initialState;
        }

        public void Update()
        {
            if ( CurrentState != null )
            CurrentState.Update();
        }

        void FixedUpdate()
        {
            if (CurrentState != null)            
            CurrentState.FixedUpdate();
        }

        internal void SetState(FSMState state, StatesEnum type)
        {
            FSMState newState = null;

            bool found = state.NextStates.TryGetValue( type, out newState);

            if ( !found )
            {
                Debug.LogError(state.Type + " -> " + type);
                throw new StateNotFoundException();
            }

            newState.StateEnter();
            CurrentState.StateExit();

            CurrentState = newState;

        }
    }
}
