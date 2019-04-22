using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    protected float damage = 20; 
    private const string TAG_OBSTACLE = "Obstacle";
    protected string targetTag = "Enemy";


    private void Start()
    {
        AudioManager.Play("RocketExplode");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("explosion collided with " + col.tag);

        if (col.gameObject.tag == targetTag)
        {
            Debug.Log("sending ExplosiveDamage message to " + col.tag);
            object[] tempStorage = new object[2];
            tempStorage[0] = (int)damage;
            col.gameObject.SendMessage("Damaged", tempStorage);
           // AudioManager.Play("BulletHit");
        }
        else if (col.gameObject.tag == TAG_OBSTACLE)
        {
            //Instantiate(bulletPuff, gameObject.transform.position, gameObject.transform.rotation);
            //Destroy(gameObject);
        }
    }


}
