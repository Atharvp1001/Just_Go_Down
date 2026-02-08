using UnityEngine;

public abstract class SharkState : MonoBehaviour
{
    protected SharkStateMachine fsm;

    public void Init(SharkStateMachine machine)
    {
        fsm = machine;
    }

    public virtual void Enter() { }
    public virtual void UpdateState() { }
    public virtual void Exit() { }
}
