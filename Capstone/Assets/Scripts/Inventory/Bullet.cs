using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string TAG_OBSTACLE = "Obstacle";
    protected float damage = 10f;
    protected float distance = 100;
    protected GameObject shooter = null;
    protected string targetTag = "Enemy";
    protected Vector2 spawnPosition;
    public GameObject bulletPuff;
    public string spawnSound;

    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.Play(spawnSound);
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(spawnPosition, transform.position) > distance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("collided with " + col.tag);

        if (col.gameObject.tag == targetTag)
        {
            Debug.Log("sending Damage message to " + col.tag);
            object[] tempStorage = new object[2];
            tempStorage[0] = (int)damage;
            tempStorage[1] = shooter;
            col.gameObject.SendMessage("Damaged", tempStorage);
            if (shooter != null && shooter.GetComponents<Player>() != null)
                EventAggregator.GetInstance().Publish(new OnBulletCollisionEvent(shooter.GetComponent<Player>().playerNumber, col.gameObject.tag));
            Instantiate(bulletPuff, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            AudioManager.Play("BulletHit");
        }
        else if(col.gameObject.tag == TAG_OBSTACLE || 
            (Vector2.Distance(spawnPosition, transform.position) > 2 && (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")))
        {
            if(shooter != null && shooter.GetComponents<Player>() != null)
                EventAggregator.GetInstance().Publish(new OnBulletCollisionEvent(shooter.GetComponent<Player>().playerNumber, col.gameObject.tag));

            AudioManager.Play("BulletHit");
            Instantiate(bulletPuff, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float pDmg)
    {
        damage = pDmg;
    }

    public void SetDistance(float pDist)
    {
        distance = pDist;
    }

    public void SetTarget(string tag)
    {
        targetTag = tag;
    }

    public void setShooter(GameObject pShooter)
    {
        shooter = pShooter;
    }
}
