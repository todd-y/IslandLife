using UnityEngine;
using System.Collections;

public class AnimCtrl {
    private int walkID = Animator.StringToHash("walk");
    private int hitID = Animator.StringToHash("hit");
    private int attackID = Animator.StringToHash("attack");
    private int deathID = Animator.StringToHash("death");
    private int castID = Animator.StringToHash("cast");
    private int attackTypeID = Animator.StringToHash("attackType");
    private int deathTypeID = Animator.StringToHash("deathType");

    private Animator anim;

    public AnimCtrl(Animator _anim) {
        anim = _anim;
    }

    public void PlayWalk() {
        anim.SetBool(walkID, true);
    }
    public void StopWalk() {
        anim.SetBool(walkID, false);
    }

    public void PlayAttack() {
        anim.SetFloat(attackTypeID, UnityEngine.Random.Range(0, 2));
        anim.SetTrigger(attackID);
    }

    public void PlayDeath() {
        anim.SetFloat(deathTypeID, UnityEngine.Random.Range(0, 2));
        anim.SetTrigger(deathID);
    }

    public void PlayHit() {
        anim.SetTrigger(hitID);
    }
}
