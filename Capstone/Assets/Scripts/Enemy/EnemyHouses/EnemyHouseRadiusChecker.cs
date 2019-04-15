using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHouseRadiusChecker : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            transform.GetComponentInParent<EnemyHouseSpawner>().startSpawning = true;
            this.enabled = false;   //disable this script
        }
    }
}
