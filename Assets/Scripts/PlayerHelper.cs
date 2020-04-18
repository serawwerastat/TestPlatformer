using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    private Animator _anim;
    private static readonly int State = Animator.StringToHash("State");

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayStandingAnimation()
    {
        _anim.SetInteger(State, 1);
    }
    public void PlayWalkAnimation()
    {
        _anim.SetInteger(State, 2);
    }
    
    public void PlayJumpAnimation()
    {
        _anim.SetInteger(State, 3);
    }
    public void PlaySwimmingAnimation()
    {
        _anim.SetInteger(State, 4);
    }
    
    public void PlayLadderIdleAnimation()
    {
        _anim.SetInteger(State, 5);
    }
    
    public void PlayLadderClimingAnimation()
    {
        _anim.SetInteger(State, 6);
    }
    
    
    
}
