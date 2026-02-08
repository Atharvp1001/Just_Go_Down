using UnityEngine;

public class SharkRoamState : SharkState
{
    public float speed = 5f;
    public float turnSpeed = 2f;
    public float reachDistance = 1.5f;
    public Transform[] roamPoints;

    int index = -1;
    Transform target;
    Animator animator;

    public override void Enter()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("canSwim", true);
        animator.SetBool("canFollow", false);
        animator.SetBool("canBite", false);

        PickPoint();
    }

    public override void UpdateState()
    {
        if (fsm.currentState != this || target == null)
            return;

        Vector3 dir = target.position - transform.position;

        if (dir.magnitude <= reachDistance)
        {
            fsm.ChangeState(GetComponent<SharkIdleState>());
            return;
        }

        dir.Normalize();
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            turnSpeed * Time.deltaTime
        );

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void PickPoint()
    {
        if (roamPoints.Length == 0) return;

        int next = index;
        while (next == index && roamPoints.Length > 1)
            next = Random.Range(0, roamPoints.Length);

        index = next;
        target = roamPoints[index];
    }
}
