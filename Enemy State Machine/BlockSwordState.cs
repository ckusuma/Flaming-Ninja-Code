using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSwordState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    /*public BlockSwordState(EnemyStatePattern enemyStatePattern)
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
        return;
    }

    public void ToBlockShurikenState()
    {
        enemy.currentState = enemy.blockShurikenState;
    }

    public void ToEngardeState()
    {
        enemy.currentState = enemy.engardeState;
    }

    void Updating()
    {
        //dodge sword
        enemy.PlaySound(enemy.enemyAudio, enemy.attackSound);
        enemy.AvoidSword();
        //TRANSITIONS GO HERE
        if (!enemy.PlayerInFieldOfView() && !enemy.PlayerIsWithinStrikingDistance() && !enemy.PlayerDetected())
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("patrol", true);
            ToPatrolState();
        }
        else 
        {
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("chase", true);
            ToChaseState();
        }
    }

}
