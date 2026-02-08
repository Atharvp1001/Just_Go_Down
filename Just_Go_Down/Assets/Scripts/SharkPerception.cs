using UnityEngine;

public class SharkPerception : MonoBehaviour
{
    SharkStateMachine fsm;
    SharkFollowState follow;

    void Awake()
    {
        fsm = GetComponent<SharkStateMachine>();
        follow = GetComponent<SharkFollowState>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        follow.SetPlayer(other.transform);
        fsm.ChangeState(follow);
    }
}
