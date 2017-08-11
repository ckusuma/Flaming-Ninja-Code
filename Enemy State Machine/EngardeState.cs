using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngardeState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    bool soundsStarted;

    /*public EngardeState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
    }*/

    void Start()
    {
        enemy = this.GetComponent<EnemyStatePattern>();
    }

    public void StartState()
    {

    }

    public void UpdateState()
    {
        Updating();
    }

    public void ToPatrolState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.patrolState;
    }

    public void ToInvestigateState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.investigateState;
    }

    public void ToChaseState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.attackState;
    }

    public void ToBlockSwordState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.blockSwordState;
    }

    public void ToBlockShurikenState()
    {
        soundsStarted = false;
        StopCoroutine("EngardeSound");
        enemy.currentState = enemy.blockShurikenState;
    }

    public void ToEngardeState()
    {
        return;
    }

    void Updating()
    {
        if (!soundsStarted)
        {
            soundsStarted = true;
            StartCoroutine("EngardeSound");
        }
        if (!enemy.GetAttackTimer())
        {
            enemy.TimeBeforeAttackStart();
        }
        enemy.Engarde();

        //TRANSITIONS GO HERE
        if (enemy.GetTimerSetOff())
        {
            enemy.SetTimerSetOff(false);
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("attack", true);
            ToAttackState();
        }
        else if (enemy.EnemyIsTooClose())
        {
            GetComponent<Rigidbody>().AddForce(-transform.forward * 5, ForceMode.Impulse);
        }
        else if ((enemy.PlayerInFieldOfView() || enemy.PlayerDetected()) && enemy.EnemyIsOutsideEngardeDistance())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("chase", true);
            ToChaseState();
        }
        else if (!enemy.PlayerInFieldOfView() && !enemy.PlayerDetected() && !enemy.PlayerIsWithinStrikingDistance())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("patrol", true);
            ToPatrolState();
        }
        else if (enemy.PlayerIsAttacking() && enemy.WillAttemptToCounter() && enemy.PlayerIsWithinStrikingDistance())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("block", true);
            ToBlockSwordState();
        }
        else
        {
            ToEngardeState();
        }
    }

    IEnumerator EngardeSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            enemy.PlaySound(enemy.enemyAudio, enemy.chaseAndEngardeSound);
        }
    }

}
