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
    protected float fVisionDistance = 150f;

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

    //Weapon
    public Transform shootPosition;
    public Bullet bullet;
    protected float fAttackTime = 3;

    protected RaycastHit2D[] raycasts = new RaycastHit2D[5]; //raycast results list

    //Health bar
    protected HealthBarHandler HealthBarHandler;

    protected Collider2D[] players; //  possible players in range of minDist

    private float minDistance = 5f; // closest a enemy will be in Safe Movement

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

        /*
        if(fHP < 101 && SafteyRadius())
        {
            aiMvmt = MovementTypeEnum.Safe;
        }
        */

        if (EnemyRayCast() > -1)
        {
            aiMvmt = MovementTypeEnum.Chase;
        }
        else
        {
            aiMvmt = MovementTypeEnum.Patrol;
        }
        
        if(aiMvmt == MovementTypeEnum.Chase && playerTarget != null && Time.time > fAttackTime)
        {
            //face player
            Vector2 dir = playerTarget.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //shoot straight
            var x = Instantiate(bullet, this.shootPosition.position, this.shootPosition.rotation);
            x.SetDamage(30);
            x.SetTarget("Player");
            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * 500);

            fAttackTime = Time.time + 1 / iBaseAttackRate;
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
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, fSpeed * Time.deltaTime);
    }

    protected void MvmtSafe()// Should be used when enemy has low health?
    {
        if(Vector2.Distance(transform.position, playerTarget.transform.position) < minDistance)
        {
            transform.LookAt(2 * transform.position - playerTarget.transform.position);
            transform.position += transform.forward * fSpeed * Time.deltaTime;
            Debug.Log("Player is too close.");

        }

        else
        {
            transform.LookAt(playerTarget.transform.position);
        }
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

    private int EnemyRayCast()
    {
        raycasts[(int)RayCastDir.Forward] = Physics2D.Raycast(transform.position, transform.right, fVisionDistance);
        raycasts[(int)RayCastDir.Left20] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(20, transform.forward) * transform.right, fVisionDistance);
        raycasts[(int)RayCastDir.Left45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(45, transform.forward) * transform.right, fVisionDistance);
        raycasts[(int)RayCastDir.Right20] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-20, transform.forward) * transform.right, fVisionDistance);
        raycasts[(int)RayCastDir.Right45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-45, transform.forward) * transform.right, fVisionDistance);

        for(int ii = 0; ii<5; ii++)
        {
            if(raycasts[ii].collider!=null  && raycasts[ii].collider.gameObject.tag == "Player")
            {
                playerTarget = raycasts[ii].collider.gameObject;
                return ii;
            }
        }
        
        playerTarget = null;
        return -1;
    }

    private bool SafteyRadius()
    {
        players = Physics2D.OverlapCircleAll(transform.position, minDistance);
        if(players.Length > 0)
        {
            playerTarget = gameObject;
            playerTarget.transform.position = Vector2.zero;
            foreach(Collider2D player in players)
            {
                if(player.gameObject.tag == "Player")
                {
                    playerTarget.transform.position += player.transform.position;
                }
            }

            return true;
        }
        playerTarget = null;

        return false;
    }

}
