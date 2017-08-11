using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, EnemyState {

    public EnemyStatePattern enemy;

    bool startedAttack;
    bool endedAttack;

    /*public AttackState(EnemyStatePattern enemyStatePattern)
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
        return;
    }

    public void ToBlockSwordState()
    {
        enemy.currentState = enemy.blockSwordState;
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
        //aim enemy sword/projectile at player and attack      

        if (!startedAttack)
        {
            enemy.PlaySound(enemy.enemyAudio, enemy.attackSound);
            startedAttack = true;
            enemy.Attack();
        }

        if (enemy.GetRigidBody().velocity.magnitude >= 0 && enemy.GetRigidBody().velocity.magnitude <= 0.1f)
        {
            endedAttack = true;
        }


        //TRANSITIONS GO HERE
        if ((enemy.PlayerInFieldOfView() || enemy.PlayerDetected()) && !enemy.PlayerIsWithinStrikingDistance() && (endedAttack || (enemy.GetRigidBody().velocity.magnitude >= 0 && enemy.GetRigidBody().velocity.magnitude <= 0.1f)))
        {
            startedAttack = false;
            endedAttack = false;
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("chase", true);
            ToChaseState();
        }
        else if (enemy.PlayerInFieldOfView() && enemy.PlayerIsWithinStrikingDistance() && (endedAttack || (enemy.GetRigidBody().velocity.magnitude >= 0 && enemy.GetRigidBody().velocity.magnitude <= 0.1f)))
        {
            startedAttack = false;
            endedAttack = false;
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("engarde", true);
            ToEngardeState();
        }
        else 
        {
            startedAttack = false;
            endedAttack = false;
            enemy.ResetAnimatorBools(enemy.anim);
            enemy.anim.SetBool("patrol", true);
            ToPatrolState();
        }
    }
}
