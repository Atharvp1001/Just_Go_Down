using UnityEngine;

public class SharkBiteTrigger : MonoBehaviour
{
    public SharkStateMachine fsm;
    public SharkFollowState followState;
    public SharkAttackState attackState;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Must already be in FollowState
        if (fsm.currentState != followState)
            return;

        // Must be follow-ready (1-frame delay fix)
        if (!followState.CanAttack())
            return;

        Debug.Log("VALID ATTACK TRIGGER");

        fsm.ChangeState(attackState);
    }
}
