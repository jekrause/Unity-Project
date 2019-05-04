using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//similar code from https://answers.unity.com/questions/532460/wait-for-seconds-c.html by Deniz2014

public class EnemyHouseSpawner : MonoBehaviour
{

    public float fHP = 100;
    public float delaySeconds = 5;
    ContactFilter2D filter;
    protected LayerMask layerMask;
    protected Collider2D[] resultsList = new Collider2D[20];

    public GameObject enemyRifle;
    public GameObject enemyHeavy;
    public GameObject enemyHandgun;
    public GameObject destroyedHouse;
    public Transform spawnPosition;
    public Transform goalPosition;

    //Health bar
    protected HealthBarHandler HealthBarHandler;

    [HideInInspector]
    public bool startSpawning = false;  //script EnemyHouseRadiusChecker of child "TriggerRadius" controls this variable

    private float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 9;
        filter.SetLayerMask(layerMask);
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
            //check to see if there are already 3 enemies in the radius
            
            int totalCollisions = Physics2D.OverlapCircle(transform.position, 20, filter, resultsList);
            int enemyCount = 0;
            for (int ii = 0; ii < totalCollisions; ii++)
            {
                if (resultsList[ii].tag == "Enemy")
                {
                    enemyCount++;
                }
            }
            Debug.Log("Enemy count is " + enemyCount + "total count = " + totalCollisions);
            if (enemyCount < 4)
            {
                int ii = Random.Range(0, 3);
                GameObject newEnemy = null;
                switch (ii)
                {
                    case 0:
                        newEnemy = Instantiate(enemyRifle, spawnPosition.transform.position, spawnPosition.transform.rotation);
                        break;
                    case 1:
                        newEnemy = Instantiate(enemyHeavy, spawnPosition.transform.position, spawnPosition.transform.rotation);
                        break;
                    case 2:
                        newEnemy = Instantiate(enemyHandgun, spawnPosition.transform.position, spawnPosition.transform.rotation);
                        break;
                }
                newEnemy.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90);
                // im not sure if this is correct???


                //Debug.Log("transformz = "+transform.rotation.z);
                //Debug.Log("transformz+90 = " + transform.rotation.z + -90);

                newEnemy.SendMessage("SetManualDestination", goalPosition.transform.position);
            }
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
