using Assets.Engine.AI.FiniteStateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Engine.AI.FiniteStateMachine
{
    public abstract class FSMState
    {
        internal Dictionary<StatesEnum, FSMState> NextStates = new Dictionary<StatesEnum, FSMState>();
        internal Dictionary<StatesEnum, FSMState> PrevStates = new Dictionary<StatesEnum, FSMState>();

        internal FSMEngine FSM;

        public FSMState(FSMEngine FSM)
        {
            this.FSM = FSM;
        }

        internal abstract void Update();
        internal abstract void FixedUpdate();
        internal abstract void StateEnter();
        internal abstract void StateExit();
        internal abstract StatesEnum Type{ get; }
        public void AddTransition(FSMState state)
        {
            NextStates.Add( state.Type , state );
            state.PrevStates.Add( Type , this );
        }
    }
}
