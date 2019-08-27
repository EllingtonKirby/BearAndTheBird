using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExplodeController : MonoBehaviour, EnemyActionProvider
{

    public Explosions explosion;
    private State state;

    public EnemyAction GetAction()
    {
        return new OnTurnExplodeAction(explosion, gameObject.transform, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.DEFAULT;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPrimedForExplosion()
    {
        return state == State.PRIMED;
    }

    public void PrimeForExplosion()
    {
        state = State.PRIMED;
    }

    enum State
    {
        DEFAULT,
        PRIMED,
        EXPLODED
    }
}
