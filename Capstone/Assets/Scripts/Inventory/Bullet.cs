using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    protected float damage = 10;
    protected string targetTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("collided with " + col.tag);

        if (col.gameObject.tag == targetTag)
        {
            Debug.Log("sending Damage message to " + col.tag);
            col.gameObject.SendMessage("Damaged", damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTarget(string tag)
    {
        targetTag = tag;
    }
}
