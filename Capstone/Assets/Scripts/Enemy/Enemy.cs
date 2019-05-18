using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.DateTime;

public class Enemy : MonoBehaviour
{
    private const int MAX_FRAMES_BETWEEN_RAYCASTS = 1;
    private const float LOW_HP_THRESHOLD = 20f;

    /*
     * Patrol - move inbetween a few set spots
     * Chase  - follow the palyer as aggressively as possible without worrying about taking damage
     * Safe   - Attack, but try to keep your distance.
     * Manual - Set by enemy spawning houses. This forces the enemy to walk to a given spot before entering the patrol state.
     */

    public enum MovementTypeEnum { Patrol, Chase, Safe, Manual, None };
    public enum RayCastDir { Left90 = 0, Left45 = 1, Forward = 2, Right45 = 3, Right90 = 4 } //raycast angles from player's transform position

    public enum Direction { Towards, Away };

    // Start is called before the first frame update
    public int iBaseAttackRate = 1;
    protected LayerMask layerMask;
    public float fHP = 100f;
    protected bool canShoot = true;
    protected MovementTypeEnum aiMvmt = MovementTypeEnum.None;
    protected Animator feetAnimation;

    public float fMoveSpeed = 3f;
    protected float fRotationSpeed = 150f;
    protected float fWaitTime;
    protected float fStartWaitTime = 3f;
    protected float fVisionDistance = 100f;
    protected Vector3 lastPosition = new Vector3(0, 0, 0); //used to see if we are blocked and the front raycast can't detect it.
    protected Vector3 goalLocation; //used by house enemy spawner


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
    public string FireSound;
    public float fAttackTime = 3;
    public float attackDistance = 50;
    public float fDamage = 10f;         //default damage if no weapon is equipped

    private int frameCount = 0;
    private int frameToCastOn = 0;
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
        layerMask = (1 << 4) | (1 << 9) | (1 << 12);
        //layersToAvoid.layerMask = 4608;//
        layersToAvoid.layerMask = LayerMask.GetMask("Obstacles", "Player"); // It may be helpful to put Enemies in seperate layer or get rid of Player layer.
        playersToAvoid = LayerMask.GetMask("Player");
        fWaitTime = fStartWaitTime;

        frameToCastOn = Random.Range(0, MAX_FRAMES_BETWEEN_RAYCASTS);

        iRandomSpot = 0;
        if (aiMvmt == MovementTypeEnum.None)
        {
            FillMoveSpots();
            aiMvmt = MovementTypeEnum.Patrol;
        }
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
        frameCount += 1;
        frameCount %= MAX_FRAMES_BETWEEN_RAYCASTS;
        if (aiMvmt != MovementTypeEnum.Manual && frameCount == frameToCastOn)
        {
            EnemyRayCast();
        }

        if (fHP <= LOW_HP_THRESHOLD)
        {
            aiMvmt = MovementTypeEnum.Safe;
        }
        else if (playerTarget != null)
        {
            aiMvmt = MovementTypeEnum.Chase;
        }
        else if (aiMvmt != MovementTypeEnum.Manual)
        {
            aiMvmt = MovementTypeEnum.Patrol;
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
            case MovementTypeEnum.Manual:
                MvmtManual();
                break;
        }

        if (playerTarget != null && playerTarget.GetComponent<Player>().PlayerState != PlayerState.ALIVE)
        {
            playerTarget = null;
        }

        if ((canShoot && ((aiMvmt == MovementTypeEnum.Chase && playerTarget != null) ||
           (aiMvmt == MovementTypeEnum.Safe && !withInMinDistance)) && Time.time > fAttackTime) &&
           raycasts[(int)RayCastDir.Forward].collider != null && raycasts[(int)RayCastDir.Forward].collider.gameObject == playerTarget &&
           Vector2.Distance(transform.position, playerTarget.transform.position) < attackDistance)
        {
            Debug.Log("RayCastDir.forward collider tag = " + raycasts[(int)RayCastDir.Forward].collider.tag + "    collider name = " + raycasts[(int)RayCastDir.Forward].collider.name);
            //face player
            Vector2 dir = playerTarget.transform.position - shootPosition.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);

            //shoot straight
            var x = Instantiate(bullet, this.shootPosition.position, this.shootPosition.rotation);
            x.SetDamage(fDamage);
            x.SetTarget("Player");

            x.setSound(FireSound);

            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * 800);

            fAttackTime = Time.time + iBaseAttackRate;
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
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //smoother
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);
            feetAnimation.SetBool("Moving", true);
        }
    }

    protected void MvmtChase()
    {
        //handle blocked path
        if (blockedPaths[(int)RayCastDir.Forward] && raycasts[(int)RayCastDir.Forward].distance < 5)
        {
            AvoidObstalces();
        }
        else
        {
            //face player
            Vector2 dir = playerTarget.transform.position - shootPosition.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, playerTarget.transform.position) > attackDistance / 4)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, fMoveSpeed * Time.deltaTime);
                feetAnimation.SetBool("Moving", true);
            }
            else
            {
                feetAnimation.SetBool("Moving", false);
            }
        }
    }

    protected void MvmtSafe()// Should be used when enemy has low health AND enemies are nearby
    {
        SafetyRadius2();
        if (playerTarget == null)
        {
            return;
        }
        else if (Vector2.Distance(transform.position, playerTarget.transform.position) < minDistance)
        {
            withInMinDistance = true;
            LookAt(Direction.Away);
            MoveAtTarget(Direction.Away);
        }

        else
        {
            //Debug.Log("Players outside minDistance.");
            LookAt(Direction.Towards);
            withInMinDistance = false;
            feetAnimation.SetBool("Moving", false);
        }

        // Debug.Log(playerTarget.transform.position);
    }

    protected void MvmtManual()
    {
        if (Vector2.Distance(transform.position, goalLocation) < 0.2f)
        {
            FillMoveSpots();
            aiMvmt = MovementTypeEnum.Patrol;
            Debug.Log("Reached manually set location");
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, goalLocation, fMoveSpeed * Time.deltaTime);
            Vector2 dir = goalLocation - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            feetAnimation.SetBool("Moving", true);
        }
    }

    public void SetManualDestination(Vector3 pDestination)
    {
        Debug.Log("Manual destination set");
        goalLocation = pDestination;
        aiMvmt = MovementTypeEnum.Manual;
    }

    private void MoveAtTarget(Direction direction)
    {
        switch (direction)
        {
            case Direction.Away:
                transform.position = Vector2.MoveTowards(transform.position, 2 * transform.position - playerTarget.transform.position, fMoveSpeed * Time.deltaTime);
                break;
            case Direction.Towards:
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, fMoveSpeed * Time.deltaTime);
                break;
        }
        feetAnimation.SetBool("Moving", true);
    }

    private void LookAt(Direction lookDir)
    {
        Vector3 dir;
        float angle;


        /*Vector2 dir = moveSpots[iRandomSpot] - (Vector2)transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //smoother
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);
        */

        switch (lookDir)
        {
            case Direction.Away:
                //dir = playerTarget.transform.position - shootPosition.position;
                dir = shootPosition.position - playerTarget.transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);
                break;
            case Direction.Towards:
                dir = playerTarget.transform.position - shootPosition.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), fRotationSpeed * Time.deltaTime);
                break;
        }
    }

    // Sorry, this took a while and got pretty messy. Many of the extra statements were used for debugging.
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
                /*    int moveSpotDistanceX, moveSpotDistanceY;
                    do
                    {
                        moveSpotDistanceX = UnityEngine.Random.Range(-10, 10);
                    } while (Mathf.Abs(moveSpotDistanceX) < 2);
                    do
                    {
                        moveSpotDistanceY = UnityEngine.Random.Range(-10, 10);
                    } while (Mathf.Abs(moveSpotDistanceY) < 2);

                    //Debug.Log("Iterations: " + ++j);
                    moveSpots[i] = new Vector2(moveSpotDistanceX + moveSpots[i - 1].x, moveSpotDistanceY + moveSpots[i - 1].y);
                */
                moveSpots[i] = new Vector2(UnityEngine.Random.Range(-10, 10) + moveSpots[i - 1].x, UnityEngine.Random.Range(-10, 10) + moveSpots[i - 1].y);
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

    protected void Damaged(object[] storage)
    {
        try
        {

            Debug.Log("Rececived " + (int)storage[0] + " damage");
            fHP -= (int)storage[0];
            HealthBarHandler.OnDamaged(fHP);

            Debug.Log("Enemy HP =" + fHP);
            Debug.Log("storage = " + storage);
            Debug.Log("storage[0] = " + storage[0]);
            Debug.Log("storage[1] = " + storage[1]);

            if (storage[1] != null)
            {
                GameObject temp = (GameObject)storage[1];
                Debug.Log("player =" + temp);
                if (temp.GetComponent<Player>() != null && fHP <= 0)
                {
                    Debug.Log("Enemy died, exp should have went to " + temp);
                    EventAggregator.GetInstance().Publish<OnEnemyKilledEvent>(new OnEnemyKilledEvent(temp.GetComponent<Player>().playerNumber, 100));
                }
                if (playerTarget == null && Vector3.Distance(temp.transform.position, gameObject.transform.position) < fVisionDistance)
                {
                    Debug.Log("Detected that " + temp.name + " shot at enemy");
                    playerTarget = temp;
                    aiMvmt = MovementTypeEnum.Chase;
                }
                else
                {
                    Debug.Log("Was shot at but distance [" + Vector3.Distance(temp.transform.position, gameObject.transform.position) + "] was too far");
                }
            }
        }
        catch (System.InvalidCastException e)
        {
            Debug.Log("Cast exception in enemy's damaged function");
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            //player.Damaged(); // simple test
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
            if (Vector2.Distance(transform.position, playerTarget.transform.position) > fVisionDistance)
            {
                playerTarget = null;
                Debug.Log("Lost sight of player");
            }
        }

        //use cone of vision to detect new enemy
        raycasts[(int)RayCastDir.Forward] = Physics2D.CircleCast(transform.position, 0.8f, transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Left45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(45, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Left90] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(90, transform.forward) * transform.right, 5, layerMask);
        raycasts[(int)RayCastDir.Right45] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-45, transform.forward) * transform.right, fVisionDistance, layerMask);
        raycasts[(int)RayCastDir.Right90] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-90, transform.forward) * transform.right, 5, layerMask);
        foreach (RayCastDir castDir in (RayCastDir[])System.Enum.GetValues(typeof(RayCastDir)))
        {

            int ii = (int)castDir;
            if (raycasts[ii].collider != null && raycasts[ii].collider.gameObject.tag == "Player" && raycasts[ii].collider.gameObject.GetComponent<Player>().PlayerState == PlayerState.ALIVE)
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
            else if (raycasts[ii].collider != null )//&& Vector2.Distance(raycasts[ii].collider.gameObject.transform.position, transform.position) < 5)
            {
                //Debug.Log("Blocked path at " + ii + "  tag = " + raycasts[ii].collider.tag + " distance = "+ Vector2.Distance(raycasts[ii].collider.gameObject.transform.position, transform.position));
                blockedPaths[ii] = true;
            }
            else
            {
                blockedPaths[ii] = false;
            }
        }

        return playerTarget;
    }

    protected void AvoidObstalces()
    {
        feetAnimation.SetBool("Moving", false);
        Vector2 dir = playerTarget.transform.position - shootPosition.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //decide to go left or right
        if (Quaternion.Angle(Quaternion.AngleAxis(transform.eulerAngles.z + 1, Vector3.forward), Quaternion.AngleAxis(angle, Vector3.forward)) > Quaternion.Angle(Quaternion.AngleAxis(transform.eulerAngles.z - 1, Vector3.forward), Quaternion.AngleAxis(angle, Vector3.forward)))
        {
            //Debug.Log("Go Right");
            //listed in order of move priority
            if (!blockedPaths[(int)RayCastDir.Right45] || !blockedPaths[(int)RayCastDir.Right90])
            {
                //Debug.Log("turning right");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 20), fRotationSpeed/3 * Time.deltaTime);
            }
            else
            {
                //Debug.Log("Right is blocked, going left");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 20), fRotationSpeed/3 * Time.deltaTime);
            }
        }
        else
        {
            //Debug.Log("Go Left");
            if ( !blockedPaths[(int)RayCastDir.Left45] || !blockedPaths[(int)RayCastDir.Left90])
            {
                //Debug.Log("turning left");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 20), fRotationSpeed/3 * Time.deltaTime);
            }
            else
            {
                //Debug.Log("left is blocked, going right");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 20), fRotationSpeed/3 * Time.deltaTime);
            }

        }
        //Debug.Log("path blocked");

        transform.position += transform.right * Time.deltaTime * fMoveSpeed;
        feetAnimation.SetBool("Moving", true);
        canShoot = false;
    }

    private int OutsideSafteyRadius()
    {



        return -1;
    }

    private int SafetyRadius2()
    {
        players = Physics2D.OverlapCircleAll(transform.position, fVisionDistance, playersToAvoid);//may be memory consuming
        //if (players != null && players.Length > 0)
        if (players == null)
        {
            Debug.Log("no players around");
        }
        if (players.Length > 0)
        {
            foreach (Collider2D player in players)
            {
                if (player.gameObject.tag == "Player")  // does this return the closest player?
                {
                    playerTarget = player.gameObject;
                    return 1;
                }
            }
        }
        playerTarget = null;
        return -1;
    }

    private int SafetyRadius()
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
                    Debug.Log(this.name + numPlayers + "");

                }
            }

            Debug.Log(this.name + ": Players in radius = " + numPlayers);
            if (numPlayers > 0)
            {
                playerTarget.transform.position /= numPlayers; // Average out the positions of all players
                Debug.Log(this.name + ": PlayerTarget position: " + playerTarget.transform.position);
                return numPlayers;
            }
            //Destroy(playerTarget);
        }

        playerTarget = null;
        return -1;
    }

}
