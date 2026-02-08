using UnityEngine;

public class SharkStateMachine : MonoBehaviour
{
    public SharkState currentState;
    bool locked;

    void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(SharkState newState)
    {
        if (locked)
            return;

        if (newState == null || newState == currentState)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
}
