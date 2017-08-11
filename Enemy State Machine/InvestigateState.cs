using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    public bool soundStarted;
    /*public InvestigateState(EnemyStatePattern enemyStatePattern)
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
        enemy.SetPriorityPatrol(true);
        enemy.PriorityPatrolSetup();
        soundStarted = false;
        enemy.currentState = enemy.patrolState;
    }

    public void ToInvestigateState()
    {
        return;
    }

    public void ToChaseState()
    {
        soundStarted = false;
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        soundStarted = false;
        enemy.currentState = enemy.attackState;
    }

    public void ToBlockSwordState()
    {
        soundStarted = false;
        enemy.currentState = enemy.blockSwordState;
    }

    public void ToBlockShurikenState()
    {
        soundStarted = false;
        enemy.currentState = enemy.blockShurikenState;
    }

    public void ToEngardeState()
    {
        soundStarted = false;
        enemy.currentState = enemy.engardeState;
    }

    void Updating()
    {
        //shoot out raycasts and wait for amount of time before returning back to patrol
        if (!soundStarted)
        {
            soundStarted = true;
            enemy.PlaySound(enemy.enemyAudio, enemy.investigateSound);
        }
        enemy.Investigate();
        
        //TRANSITIONS GO HERE

    }
}
