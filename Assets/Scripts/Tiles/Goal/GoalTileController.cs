using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventNames.END_MOVE, CheckForPlayerOnTile);
    }


    void CheckForPlayerOnTile(object arg0)
    {
        Collider2D hit = Physics2D.OverlapCircle(this.transform.position, .25f);
        if (hit != null && hit.transform.gameObject.tag == "Player")
        {
            var healthController = hit.gameObject.GetComponent<PlayerHealthController>();
            healthController.ReachGoal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
