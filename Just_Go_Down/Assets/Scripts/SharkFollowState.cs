using UnityEngine;

public class SharkFollowState : SharkState
{
    public float speed = 8f;
    public float turnSpeed = 2.5f;
    public float stopDistance = 1.5f;

    Transform player;
    Vector3 lastKnownPos;

    Animator animator;
    bool canAttack;

    public override void Enter()
    {
        Debug.Log("ENTER FOLLOW");

        canAttack = false;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("canFollow", true);
        animator.SetBool("canSwim", false);
        animator.SetBool("canBite", false);

        // Enable attack AFTER 1 frame
        Invoke(nameof(EnableAttack), 0f);
    }

    void EnableAttack()
    {
        canAttack = true;
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public override void UpdateState()
    {
        Vector3 target = player != null ? player.position : lastKnownPos;
        Vector3 dir = target - transform.position;

        if (player == null && dir.magnitude <= stopDistance)
        {
            fsm.ChangeState(GetComponent<SharkRoamState>());
            return;
        }

        if (player != null)
            lastKnownPos = player.position;

        dir.Normalize();

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            turnSpeed * Time.deltaTime
        );

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetPlayer(Transform t)
    {
        player = t;
        if (t != null)
            lastKnownPos = t.position;
    }
}
