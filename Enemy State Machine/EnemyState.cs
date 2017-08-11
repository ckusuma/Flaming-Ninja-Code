using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState {

    void StartState();

    void UpdateState();

    void ToPatrolState();

    void ToChaseState();

    void ToAttackState();

    void ToBlockSwordState();

    void ToBlockShurikenState();

    void ToInvestigateState();

    void ToEngardeState();

}
