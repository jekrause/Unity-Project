using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.DateTime;

public class Enemy : MonoBehaviour
{

    /*
     * Patrol - move inbetween a few set spots
     * Chase  - follow the palyer as aggressively as possible without worrying about taking damage
     * Safe   - Attack, but try to keep your distance.
     */


    public enum MovementTypeEnum { Patrol, Chase, Safe };
    public enum RayCastDir { Left90 = 0, Left45 = 1, Left20 = 2, Forward = 3, Right20 = 4, Right45 = 5, Right90 = 6 } //raycast angles from player's transform position

    public enum LookDir { Towards, Away };

    // Start is called before the first frame update
    public const int iBaseAttackRate = 1;
    protected LayerMask layerMask;
    protected float fHP = 100f;
    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected bool canShoot = true;
    protected MovementTypeEnum aiMvmt;
    protected Animator feetAnimation;

    protected float fMoveSpeed = 3f;
    protected float fRotationSpeed = 20f;
    protected float fWaitTime;
    protected float fStartWaitTime = 3f;
    protected float fVisionDistance = 150f;
    protected Vector3 lastPosition = new Vector3(0,0,0); //used to see if we are blocked and the front raycast can't detect it.


    /*
     * Variables for Patrol AI
     */
    private int iRandomSpot;
    public Vector2[] moveSpots = new Vector2[5]; //could be used if we want the Patrol AI to move between set spots. Alternatively, fill this with key points on the map

    //Rigidbody2D rb; // for enemyposition

    //private Vector2 e_location; // this enemies location;

    private Rigidbody2D rb;
    protected GameObject playerTarget; //the player being attacked (JEK: we might want to use this rather than just the vector "p_location")
    //private Vector2 p_location; // the player being attacked 

    //Weapon
    public Transform shootPosition;
    public Bullet bullet;
    protected float fAttackTime = 3;

    protected RaycastHit2D[] raycasts = new RaycastHit2D[7]; //raycast results list
    protected bool[] blockedPaths = new bool[7]; //array for the directions in the RayCastDir enum. Index will be true if blocked, false if clear.

    //Health bar
    protected HealthBarHandler HealthBarHandler;

    protected Collider2D[] players; //  possible players in range of minDist

    protected float minDistance = 10f; // closest a enemy will be in Safe Movement
    bool withInMinDistance = true; // poor bug evasion, but works

    ContactFilter2D layersToAvoid;

    int playersToAvoid;// = LayerMask.GetMask("Player"); // Used by Safe aiMvmt.

    void Start()
    {
        layerMask = (1 << 4) | (1 << 8) | (1<<9);
        //layersToAvoid.layerMask = 4608;//
        layersToAvoid.layerMask = LayerMask.GetMask("Obstacles", "Player"); // It may be helpful to put Enemies in seperate layer or get rid of Player layer.
        playersToAvoid = LayerMask.GetMask("Player");
        fWaitTime = fStartWaitTime;
        //iRandomSpot = Random.Range(0, moveSpots.Length);
        iRandomSpot = 0;
        FillMoveSpots();
        aiMvmt = MovementTypeEnum.Patrol;
        rb = GetComponent<Rigidbody2D>();
        HealthBarHandler = GetComponent<HealthBarHandler>();
        HealthBarHandler.SetMaxHP(fHP);
        feetAnimation = transform.GetChild(2).GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //death 
        if (fHP <= 0)
        {
            Object.Destroy(gameObject);
            Destroy(this);
        }

        canShoot = true;
        EnemyRayCast();

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

        if (fHP < 10 && SafteyRadius() > -1)
        {
            aiMvmt = MovementTypeEnum.Safe;
        }
        else if (playerTarget != null)
        {
            aiMvmt = MovementTypeEnum.Chase;
        }
        else
        {
            aiMvmt = MovementTypeEnum.Patrol;
        }

        if (canShoot && ((aiMvmt == MovementTypeEnum.Chase && playerTarget != null) ||
           (aiMvmt == MovementTypeEnum.Safe && !withInMinDistance)) && Time.time > fAttackTime)
        {
            Debug.Log("Shooting");
            //face player
            Vector2 dir = playerTarget.transform.position - shootPosition.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //shoot straight
            var x = Instantiate(bullet, this.shootPosition.position, this.shootPosition.rotation);
            x.SetDamage(0);
            x.SetTarget("Player");
            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * 800);

            fAttackTime = Time.time + 1 / iBaseAttackRate;
        }
    }


    protected void MvmtPatrol()
    {
        //check if we've reached our destination
        // if (Vector2.Distance(transform.position, moveSpots[iRandomSpot]) < 0.2f)// why not 0f
        if (Vector2.Distance(moveSpots[iRandomSpot], transform.position) <= fMoveSpeed * Time.deltaTime / 2)
        //if (Vector2.Distance(moveSpots[iRandomSpot], transform.position) <= 0)
        {
            if (fWaitTime <= 0)
            {
                //iRandomSpot = Random.Range(0, moveSpots.Length); // this will not work unless different FillMoveSpots is implemented
                iRandomSpot = (++iRandomSpot) % moveSpots.Length;
                fWaitTime = fStartWaitTime;
                Debug.Log("Patrol new spot is " + iRandomSpot);
            }
            else
            {
                feetAnimation.SetBool("Moving", false);
                fWaitTime -= Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[iRandomSpot], fMoveSpeed * Time.deltaTime);
            Vector2 dir = moveSpots[iRandomSpot] - (Vector2)transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            feetAnimation.SetBool("Moving", true);
        }
    }

    protected void MvmtChase()
    {
        //handle blocked path
        if (blockedPaths[(int)RayCastDir.Forward] && raycasts[(int)RayCastDir.Forward].distance < 5)
        {
            feetAnimation.SetBool("Moving", false);
            Vector2 dir = playerTarget.transform.position - shootPosition.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //decide to go left or right
            if(Quaternion.Angle(Quaternion.AngleAxis(transform.eulerAngles.z + 20, Vector3.forward), Quaternion.AngleAxis(angle, Vector3.forward)) > Quaternion.Angle(Quaternion.AngleAxis(transform.eulerAngles.z - 20, Vector3.forward), Quaternion.AngleAxis(angle, Vector3.forward)))
            {
                Debug.Log("Go Right");
                //listed in order of move priority
                if (!blockedPaths[(int)RayCastDir.Right20] || !blockedPaths[(int)RayCastDir.Right45] || !blockedPaths[(int)RayCastDir.Right90])
                {
                    Debug.Log("turning right 1");
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 20), fRotationSpeed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("Right is blocked, going left");
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 20), fRotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                if(!blockedPaths[(int)RayCastDir.Left20] || !blockedPaths[(int)RayCastDir.Left45] || !blockedPaths[(int)RayCastDir.Left90])
                {
                    
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 20), fRotationSpeed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("left is blocked, going right");
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 20), fRotationSpeed * Time.deltaTime);
                }

            }
            Debug.Log("path blocked");
           
            transform.position += transform.right * Time.deltaTime * fMoveSpeed;
            feetAnimation.SetBool("Moving", true);
            canShoot = false;
        }
        else
        {
            Debug.Log("Path not blocked!!!!");
            //face player
            Vector2 dir = playerTarget.transform.position - shootPosition.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, fMoveSpeed * Time.deltaTime);
            feetAnimation.SetBool("Moving", true);
        }
        
    }

    protected void MvmtSafe()// Should be used when enemy has low health AND enemies are nearby
    {

        if (Vector2.Distance(transform.position, playerTarget.transform.position) < minDistance)
        {
            withInMinDistance = true;
            LookAt(LookDir.Away);
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, fMoveSpeed * Time.deltaTime * -1f);
            transform.LookAt(2 * transform.position - playerTarget.transform.position);
            //transform.LookAt(playerTarget.transform.position);
            //transform.position += transform.forward * fSpeed * Time.deltaTime;
            //Debug.Log("Player is within minDistance.");
        }

        else
        {
            //Debug.Log("Players outside minDistance.");
            LookAt(LookDir.Towards);
            withInMinDistance = false;
        }

        Debug.Log(playerTarget.transform.position);
    }

    private void LookAt(LookDir lookDir)
    {
        switch (lookDir)
        {
            case LookDir.Towards:
                transform.forward = playerTarget.transform.position - transform.position;
                break;
            case LookDir.Away:
                transform.forward = transform.position - playerTarget.transform.position;
                break;
        }
    }

    // Sorry, this took a while and got pretty messy. Many of the extra statements were used for debugging.
    private void FillMoveSpots()
    {
        moveSpots[0] = transform.position;
        //fStartWaitTime = 0;
        //fSpeed = 9f;

        //Random.seed = System.DateTime.Now.Millisecond;
        float radius = this.GetComponent<CircleCollider2D>().radius;
        //Debug.Log("radius: " + radius);

        for (int i = 1; i < moveSpots.Length; i++) // last spot must be visible by first and penultimate spots UPDATE: already done in this implementation
        {
            //TODO: check for collision at move spot before adding to list
            /*
            //here the new spot must be obstacle-free from the preceding spot and spot[0]
            do
            {
                //moveSpots[i] = new Vector2(Random.Range(5, 25), Random.Range(5, 25));
                moveSpots[i] = new Vector2(Random.Range(-5, 6) + moveSpots[i-1].x, Random.Range(-5, 6) + moveSpots[i-1].y);

            } while (Physics2D.Linecast(moveSpots[i - 1], moveSpots[i], layersToAvoid)
                        || Physics2D.Linecast(moveSpots[i - 1], moveSpots[0], layersToAvoid));
            */
            //Collider2D shapeCollider = this.GetComponent<Collider2D>(); // maybe put outside for loop
            //int j = 0;
            int p, q; // Not needed if assignment statements are put into the while condition.
            do
            {
                //Debug.Log("Iterations: " + ++j);
                moveSpots[i] = new Vector2(Random.Range(-10, 10) + moveSpots[i - 1].x, Random.Range(-10, 10) + moveSpots[i - 1].y);

                p = Physics2D.CircleCast(moveSpots[i - 1], radius, moveSpots[i] - moveSpots[i - 1], layersToAvoid, raycasts, Vector2.Distance(moveSpots[i], moveSpots[i - 1]));
                q = Physics2D.CircleCast(moveSpots[i], radius, moveSpots[0] - moveSpots[i], layersToAvoid, raycasts, Vector2.Distance(moveSpots[0], moveSpots[i]));
                //Debug.Log("p: " + p + " & q: " + q);
                /*foreach(RaycastHit2D raycast in raycasts)
                {

                    Debug.Log("Tag: " + raycast.collider.gameObject.tag);
                }*/
                // WARNING: this makes use of raycasts before it is used by other methods
            } /*while (Physics2D.CircleCast(moveSpots[i - 1], radius, moveSpots[i] - moveSpots[i - 1], layersToAvoid, raycasts, Vector2.Distance(moveSpots[i], moveSpots[i - 1])) > 0
                    || Physics2D.CircleCast(moveSpots[i], radius, moveSpots[0] - moveSpots[i], layersToAvoid, raycasts, Vector2.Distance(moveSpots[0], moveSpots[i])) > 0);
            */
            while (p > 0 || q > 1); // q always hits the original enemy.
            //while (Physics2D.CircleCast(moveSpots[i - 1], radius, moveSpots[i] - moveSpots[i - 1], LayerMask.GetMask("Obstacles", , raycasts, Vector2.Distance(moveSpots[i], moveSpots[i - 1])) > 0
            //        && Physics2D.CircleCast(moveSpots[i], radius, moveSpots[0] - moveSpots[i], layersToAvoid, raycasts, Vector2.Distance(moveSpots[0], moveSpots[i])) > 0);


        }

    }

    protected void Damaged(float f)
    {
        fHP -= f;
        HealthBarHandler.OnDamaged(fHP);
    }

    /*
    private void setPlayerLocation()
    {
        p_location = gameObject.GetComponent("Player").transform.position;
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.Damaged(10); // simple test
        }
    }

    private GameObject EnemyRayCast()
    {
        int indexPlayerFoundAt = -1;

        //try to cast to an existing enemy
        if (playerTarget != null)
        {
            //var angle = Mathf.Atan2(playerTarget.transform.position.y, playerTarget.transform.position.x) * Mathf.Rad2Deg;
            //if (Physics2D.Raycast(transform.position, Quaternion.AngleAxis(angle, transform.forward) * transform.right, fVisionDistance).collider == null)
            if(Vector2.Distance(transform.position, playerTarget.transform.position) > fVisionDistance)
            {
                playerTarget = null;
                Debug.Log("Lost sight of player");
            }
            else
            {
                Debug.Log("Still chasing Player");
            }
        }

        //use cone of vision to detect new enemy
        raycasts[(int)RayCastDir.Forward] = Physics2D.CircleCast(transform.position, 2f, transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Left20] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(20, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Left45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(45, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Left90] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(90, transform.forward) * transform.right, fVisionDistance / 2, layerMask);
        raycasts[(int)RayCastDir.Right20] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-20, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Right45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-45, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Right90] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-90, transform.forward) * transform.right, fVisionDistance / 2, layerMask);
        foreach (RayCastDir castDir in (RayCastDir[])System.Enum.GetValues(typeof(RayCastDir)))
        {
            
            int ii = (int)castDir;
            if (raycasts[ii].collider != null && raycasts[ii].collider.gameObject.tag == "Player")
            {
                blockedPaths[ii] = false;

                /*
                 * IF a player has not been found OR
                 * the new enemy is closer than the old enemy THEN
                 * set the target to the enemy detected at index ii
                 */
                if (playerTarget == null ||
                    (Vector2.Distance(transform.position, raycasts[ii].collider.gameObject.transform.position) <
                    Vector2.Distance(transform.position, playerTarget.transform.position)))
                {
                    playerTarget = raycasts[ii].collider.gameObject;
                    indexPlayerFoundAt = ii;
                }
            }
            else if (raycasts[ii].collider != null)
            {
                //Debug.Log("Blocked path at " + ii + "  tag = " + raycasts[ii].collider.tag);
                blockedPaths[ii] = true;
            }
            else
            {
                blockedPaths[ii] = false;
            }
        }

        return playerTarget;
    }

    private int SafteyRadius()
    {
        players = Physics2D.OverlapCircleAll(transform.position, fVisionDistance, playersToAvoid);
        int numPlayers = 0;
        if (players.Length > 0)
        {
            playerTarget = new GameObject();
            playerTarget.transform.position = Vector2.zero;
            foreach (Collider2D player in players)
            {
                if (player.gameObject.tag == "Player")
                {
                    playerTarget.transform.position += player.transform.position;
                    ++numPlayers;
                    Debug.Log("Player position: " + player.transform.position);

                }
            }

            Debug.Log("Players in radius = " + numPlayers);
            if (numPlayers > 0)
            {
                playerTarget.transform.position /= numPlayers; // Average out the positions of all players
                Debug.Log("PlayerTarget position: " + playerTarget.transform.position);
                return numPlayers;
            }

        }
        Destroy(playerTarget);

        playerTarget = null;
        return -1;
    }

}
