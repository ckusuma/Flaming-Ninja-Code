using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    bool soundsStarted;
    /*public ChaseState(EnemyStatePattern enemyStatePattern)
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
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.patrolState;
    }

    public void ToInvestigateState()
    {
        soundsStarted = false;
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.investigateState;
    }

    public void ToChaseState()
    {
        return;
    }

    public void ToAttackState()
    {
        soundsStarted = false;
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.attackState;
    }

    public void ToBlockSwordState()
    {
        soundsStarted = false;
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.blockSwordState;
    }

    public void ToBlockShurikenState()
    {
        soundsStarted = false;
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.blockShurikenState;
    }

    public void ToEngardeState()
    {
        soundsStarted = false;
        StopCoroutine("ChaseSound");
        enemy.currentState = enemy.engardeState;
    }

    void Updating()
    {
        if (!soundsStarted)
        {
            soundsStarted = true;
            StartCoroutine("ChaseSound");
        }
        enemy.ChasePlayer();

        if (enemy.gameObject.name == "Enemy (1)")
        {
            Debug.Log("this should be called");
        }

        //TRANSITIONS GO HERE
        if (!enemy.PlayerDetected() && !enemy.PlayerInFieldOfView())
        {
            enemy.SetPriorityPatrol(true);
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("patrol", true);
            ToPatrolState();
        }
        else if (enemy.EnemyIsTooClose())
        {
            GetComponent<Rigidbody>().AddForce(-transform.forward * 5, ForceMode.Impulse);
        }
        else if (enemy.EnemyIsWithinAllowableBounds() && enemy.PlayerInFieldOfView())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("engarde", true);
            ToEngardeState();
        }
        else
        {
            ToChaseState();
        }
    }

    IEnumerator ChaseSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            enemy.PlaySound(enemy.enemyAudio, enemy.chaseAndEngardeSound);
        }
    }
}
