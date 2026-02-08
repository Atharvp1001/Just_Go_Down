using UnityEngine;

public class SharkIdleState : SharkState
{
    public float idleTime = 2f;
    float timer;
    Animator animator;

    public override void Enter()
    {
        timer = idleTime;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("canSwim", false);
        animator.SetBool("canFollow", false);
        animator.SetBool("canBite", false);
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            fsm.ChangeState(GetComponent<SharkRoamState>());
    }
}
