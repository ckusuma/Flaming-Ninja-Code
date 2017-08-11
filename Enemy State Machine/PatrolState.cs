using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MonoBehaviour, EnemyState  {

    public EnemyStatePattern enemy;

    bool soundsStarted;

    /*public PatrolState(EnemyStatePattern enemyStatePattern)
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
        return;
    }

    public void ToInvestigateState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.investigateState;
    }

    public void ToChaseState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.attackState;
    }

    public void ToBlockSwordState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.blockSwordState;
    }

    public void ToBlockShurikenState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.blockShurikenState;
    }

    public void ToEngardeState()
    {
        soundsStarted = false;
        enemy.enemyAudio.Stop();
        enemy.currentState = enemy.engardeState;
    }

    void Updating()
    {
        /*if (!soundsStarted)
        {
            soundsStarted = true;
            enemy.PlaySound(enemy.enemyAudio, enemy.patrolSound);
            enemy.enemyAudio.loop = true;
        }*/
        //randomly move around
        if (enemy.GetPriorityPatrol())
        {           
            enemy.PriorityPatrol();
        }
        else
        {
            enemy.Patrol();
        }

        //TRANSITIONS GO HERE
        if ((enemy.PlayerDetected() || enemy.PlayerInFieldOfView()))
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("investigate", true);
            ToInvestigateState();
        }
        else if (enemy.PlayerIsAttacking() && enemy.WillAttemptToCounter() && enemy.PlayerIsWithinStrikingDistance() && enemy.PlayerInFieldOfView())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("block", true);
            ToBlockSwordState();
        }
        else
        {
            ToPatrolState();
        }
    }

    IEnumerator PatrolSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            enemy.PlaySound(enemy.enemyAudio, enemy.patrolSound);
        }
    }
}
