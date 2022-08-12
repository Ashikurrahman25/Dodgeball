using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.gameObject.layer == 8)
        {
            if (other.GetComponent<EnemyView>())
            {
                other.GetComponent<EnemyView>().TakeDamage(50f);
            }
            Debug.Log(other.GetComponent<EnemyView>());

            Debug.Log("Enemy hited");
        }
    }
}
