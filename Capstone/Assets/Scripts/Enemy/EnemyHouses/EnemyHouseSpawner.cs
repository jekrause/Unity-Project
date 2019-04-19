using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//similar code from https://answers.unity.com/questions/532460/wait-for-seconds-c.html by Deniz2014

public class EnemyHouseSpawner : MonoBehaviour
{

    public float fHP = 100;
    public float delaySeconds = 5;
    public GameObject enemy;
    public GameObject destroyedHouse;
    public Transform spawnPosition;

    //Health bar
    protected HealthBarHandler HealthBarHandler;

    [HideInInspector]
    public bool startSpawning = false;  //script EnemyHouseRadiusChecker of child "TriggerRadius" controls this variable

    private float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        //to catch bugs
        if (fHP <= 0f)
        {
            fHP = 1f;
        }
        if (delaySeconds <= 0f)
        {
            delaySeconds = 1f;
        }


        HealthBarHandler = GetComponent<HealthBarHandler>();
        HealthBarHandler.SetMaxHP(fHP);
    }

    // Update is called once per frame
    void Update()
    {

        if (fHP <= 0)
        {
            Instantiate(destroyedHouse, transform.position, transform.rotation);
            Object.Destroy(gameObject);
            return;
        }

        if (Waited() && (startSpawning == true))
        {
            GameObject newEnemy = Instantiate(enemy, spawnPosition.transform.position, spawnPosition.transform.rotation);
            newEnemy.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z-90);
            // im not sure if this is correct???


            //Debug.Log("transformz = "+transform.rotation.z);
            //Debug.Log("transformz+90 = " + transform.rotation.z + -90);
        }

    }


    private bool Waited()
    {
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > delaySeconds)
        {
            timePassed = 0;
            return true;
        }
        else
        {
            return false;
        }
    }


    //message[0] = damage
    //message[1] = player who shot at it (might be null)
    protected void Damaged(object[] message)
    {
        fHP -= (int)message[0];
        HealthBarHandler.OnDamaged(fHP);
    }


}
