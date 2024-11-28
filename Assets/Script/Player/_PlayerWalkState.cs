using UnityEngine;
public static class Helpers
{
    public static float Map(float value, float originalMin, float originalMax, float newMin, float newMax, bool useClamp)
    {
        float newValue = (value - originalMin) / (originalMax - originalMin) * (newMax - newMin) + newMin;
        if (useClamp) newValue = Mathf.Clamp(newValue, newMin, newMax);
        return newValue;
    }
}
public class _PlayerWalkState : BaseState
{
    Animator animator;
    Rigidbody2D rb;
    float MaxSpeed;
    public _PlayerWalkState(Animator animator, Rigidbody2D rb, float MaxSpeed)
    {
        this.animator = animator;
        this.rb = rb;
        this.MaxSpeed = MaxSpeed;
    }
    public override void OnEnter()
    {
        Debug.Log("Walking");
        animator.SetBool("IsWalking", true);
    }
    public override void OnExit()
    {
        animator.speed = 1;
        animator.SetBool("IsWalking", false);
    }
    public override void OnLogic()
    {
        float speed = Helpers.Map(Mathf.Abs(rb.velocity.x), 0, MaxSpeed, 0, 1, true);
        animator.speed = speed * 3;
    }
}
