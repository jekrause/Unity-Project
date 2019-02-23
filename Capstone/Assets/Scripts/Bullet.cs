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
        target = GameObject.Find("BulletTarget").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            Object.Destroy(gameObject);
        }
    }
}
