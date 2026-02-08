using UnityEngine;

public class SharkAttackState : SharkState
{
    public float biteDuration = 1.2f;

    float timer;
    Animator animator;

    public override void Enter()
    {
        Debug.Log("ENTER ATTACK");

        fsm.Lock(); // lock FSM

        timer = biteDuration;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("canBite", true);
        animator.SetBool("canFollow", false);
        animator.SetBool("canSwim", false);
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            animator.SetBool("canBite", false);
            animator.SetBool("canFollow", true);

            fsm.Unlock(); // unlock FSM
            fsm.ChangeState(GetComponent<SharkFollowState>());
        }
    }
}
