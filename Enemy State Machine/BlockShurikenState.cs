using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShurikenState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    /*public BlockShurikenState(EnemyStatePattern enemyStatePattern)
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
        enemy.currentState = enemy.patrolState;
    }

    public void ToInvestigateState()
    {
        enemy.currentState = enemy.investigateState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToBlockSwordState()
    {
        enemy.currentState = enemy.blockSwordState;
    }

    public void ToBlockShurikenState()
    {
        return;
    }

    public void ToEngardeState()
    {
        enemy.currentState = enemy.engardeState;
    }

    void Updating()
    {
        //dodge shuriken

        //TRANSITIONS GO HERE
        if (enemy.PlayerInFieldOfView() && enemy.PlayerIsWithinStrikingDistance())
        {
            ToAttackState();
        }
        else 
        {
            ToChaseState();
        }
    }
}
