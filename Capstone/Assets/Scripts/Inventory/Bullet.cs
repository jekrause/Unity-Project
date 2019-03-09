using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    protected float damage = 10;

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
        if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Player" || col.gameObject.tag == "Untagged")
        {
            return;
        }

        if (col.gameObject.tag == "Enemy")
        {
            print("Player did " + damage + " damage");
            col.gameObject.SendMessage("Damaged", damage);
        }
        
        Destroy(gameObject);

    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
        print("damage set to " + damage);
    }
}
