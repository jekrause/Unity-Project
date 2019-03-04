using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    protected float speed = 5;

    private Vector2 target; //target for bullet

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    void SetTargetPosition(Vector3 pos)
    {
        target = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        print("collision");

        if (col.gameObject.tag != "Weapon" && col.gameObject.tag != "Player")
        {
            return;
        }

        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.SendMessage("Damaged", 20);
        }
        
        Destroy(gameObject);

    }
}
