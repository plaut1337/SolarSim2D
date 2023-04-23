using Raylib_cs;
using System.Numerics;

namespace SolarSim2d
{
    public class StateManager
    {
        List<State> stateList = new List<State>();
        int currentState = 0;

        public StateManager(){}

        public int CurrentState
        {
            get { return currentState; }
        }

        public void AddState(Action method, int stateIndex)
        {
            State state = new State();
            stateList.Add(state);
            state.method = method;
            state.stateIndex = stateIndex;
        }

        public void ChangeState(int index)
        {
            currentState = index;
        }

        public void StartManager()
        {
            stateList[currentState].method();
        }
    }

    public class State
    {
        public Action method;
        public int stateIndex;

        public State(){}
    }
}
