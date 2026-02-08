using UnityEngine;

public class SharkFollowTrigger : MonoBehaviour
{
    [Header("Assigned Collider")]
    public Collider followCollider;

    [Header("References")]
    public SharkStateMachine fsm;
    public SharkFollowState followState;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // This script only cares about FOLLOW collider
        if (followCollider == null)
            return;

        Debug.Log("FOLLOW COLLIDER TRIGGERED");

        followState.SetPlayer(other.transform);
        fsm.ChangeState(followState);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        followState.SetPlayer(null);
    }
}
