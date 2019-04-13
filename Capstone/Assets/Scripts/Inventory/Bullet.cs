using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string TAG_OBSTACLE = "Obstacle";
    protected float damage = 10;
    protected float distance = 100;
    protected string targetTag = "Enemy";
    protected Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Play("AssaultFire");
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
            col.gameObject.SendMessage("Damaged", damage);
            Destroy(gameObject);
            AudioManager.Play("BulletHit");
        }
        else if(col.gameObject.tag == TAG_OBSTACLE)
        {
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
}
