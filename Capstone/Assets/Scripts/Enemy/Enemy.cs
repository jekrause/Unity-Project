using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /*
     * Patrol - move inbetween a few set spots
     * Chase  - follow the palyer as aggressively as possible without worrying about taking damage
     * Safe   - Attack, but try to keep your distance.
     */

    public enum MovementTypeEnum {Patrol, Chase, Safe};
    public enum RayCastDir { Left45 = 0, Left20 = 1, Forward = 2, Right20 = 3, Right45 = 4 } //raycast angles from player's transform position

    // Start is called before the first frame update
    public const int iBaseAttackRate = 1;
    protected float fHP = 100f;
    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected MovementTypeEnum aiMvmt;

    protected float fSpeed = 3f;
    protected float fWaitTime;
    protected float fStartWaitTime = 3f;
    protected float fVisionDistance = 5f;

    /*
     * Variables for Patrol AI
     */
    private int iRandomSpot;
    public Vector2[] moveSpots = new Vector2[5]; //could be used if we want the Patrol AI to move between set spots. Alternatively, fill this with key points on the map

    //Rigidbody2D rb; // for enemyposition

    //private Vector2 e_location; // this enemies location;

    private Rigidbody2D rb;
    protected GameObject playerTarget; //the player being attacked (JEK: we might want to use this rather than just the vector "p_location")
    private Vector2 p_location; // the player being attacked 

    protected RaycastHit2D[] raycasts = new RaycastHit2D[5]; //raycast results list

    //Health bar
    protected HealthBarHandler HealthBarHandler;

    void Start()
    {
        fWaitTime = fStartWaitTime;
        iRandomSpot = Random.Range(0, moveSpots.Length);
        FillMoveSpots();
        aiMvmt = MovementTypeEnum.Patrol;
        rb = GetComponent<Rigidbody2D>();
        HealthBarHandler = GetComponent<HealthBarHandler>();
        HealthBarHandler.SetMaxHP(fHP);
    }

    // Update is called once per frame
    void Update()
    {
        //death 
        if(fHP <= 0)
        {
            Object.Destroy(gameObject);
            Destroy(this);
        }

        switch (aiMvmt)
        {
            case MovementTypeEnum.Patrol:
                MvmtPatrol();
                break;
            case MovementTypeEnum.Chase:
                MvmtChase();
                break;
            case MovementTypeEnum.Safe:
                MvmtSafe();
                break;
        }

        if (EnemyRayCast())
        {
   
        }

    }

    protected void MvmtPatrol()
    {
        //check if we've reached our destination
        if (Vector2.Distance(transform.position, moveSpots[iRandomSpot]) < 0.2f)
        {
            if(fWaitTime <= 0)
            {
                iRandomSpot = Random.Range(0, moveSpots.Length);
                fWaitTime = fStartWaitTime;
            }
            else
            {
                fWaitTime -= Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[iRandomSpot], fSpeed * Time.deltaTime);

            Vector2 dir = moveSpots[iRandomSpot] - (Vector2)transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    protected void MvmtChase()
    {
        rb.MovePosition(rb.position + p_location * Time.deltaTime);
    }

    protected void MvmtSafe()
    {

    }

    private void FillMoveSpots()
    {
        moveSpots[0] = transform.position;
        for(int i = 1; i<moveSpots.Length; i++)
        {
            //TODO: check for collision at move spot before adding to list
            moveSpots[i] = new Vector2(Random.Range(5, 25), Random.Range(5, 25));
        }
    }

    protected void Damaged(float f)
    {
        fHP -= f;
        HealthBarHandler.OnDamaged(fHP);
    }

    private void setPlayerLocation()
    {
        p_location = gameObject.GetComponent("Player").transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damaged(10); // simple test
        }
    }

    private bool EnemyRayCast()
    {
        raycasts[(int)RayCastDir.Forward] = Physics2D.Raycast(transform.position, transform.right, fVisionDistance);
        if(raycasts[(int)RayCastDir.Forward].collider != null)
        {
            if(raycasts[(int)RayCastDir.Forward].collider.gameObject.tag != "Untagged")
                Debug.Log("raycast forward hit " + raycasts[(int)RayCastDir.Forward].collider.tag);
        }
        
        return false;
    }

}
