using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    //protected float damage = 5;//20; 
    protected float damage = 1f;
    private const string TAG_OBSTACLE = "Obstacle";
    protected string targetTag = "Enemy";
    protected GameObject shooter = null;


    private void Start()
    {
        AudioManager.Play("RocketExplode");
    }

    //void OnTriggerEnter2D(Collider2D col)
    void OnTriggerStay2D(Collider2D col)
    {
        print("explosion collided with " + col.tag);

        if (col.gameObject.tag == targetTag)
        {
            Debug.Log("sending ExplosiveDamage message to " + col.tag);
            object[] tempStorage = new object[2];
            tempStorage[0] = (int)damage;
            tempStorage[1] = shooter;
            col.gameObject.SendMessage("Damaged", tempStorage);
            Debug.Log("shooterPlayer = " + shooter);
            Debug.Log("tempstorage = " + tempStorage);
            Debug.Log("tempstorage[0] = " + tempStorage[0]);
            Debug.Log("tempstorage[1] = " + tempStorage[1]);
            if (shooter != null && shooter.GetComponents<Player>() != null)
            {
                Debug.Log("should be sending exp to player??" + shooter);
                EventAggregator.GetInstance().Publish(new OnBulletCollisionEvent(shooter.GetComponent<Player>().playerNumber, col.gameObject.tag));
            }
            // AudioManager.Play("BulletHit");
        }
        else if (col.gameObject.tag == TAG_OBSTACLE)
        {
            //Instantiate(bulletPuff, gameObject.transform.position, gameObject.transform.rotation);
            //Destroy(gameObject);
        }


    }

    public void SetDamage(float pDmg)
    {
        damage = (pDmg/10f);  //set it to 1/10th damage for balance, it damages every frame
        if(damage < 1)
        {
            damage = 1f;    //should always do at least one damage
        }
    }

    public void setShooter(GameObject pShooter)
    {
        shooter = pShooter;
        Debug.Log("shooter should have been set to" + pShooter);
    }


}
