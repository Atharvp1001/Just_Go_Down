using UnityEngine;

public class SharkBootstrap : MonoBehaviour
{
    void Awake()
    {
        var fsm = GetComponent<SharkStateMachine>();

        GetComponent<SharkRoamState>().Init(fsm);
        GetComponent<SharkIdleState>().Init(fsm);
        GetComponent<SharkFollowState>().Init(fsm);
        GetComponent<SharkAttackState>().Init(fsm);

        fsm.ChangeState(GetComponent<SharkRoamState>());
    }
}
