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
    /*
     * Variables for Patrol AI
     */
    private int iRandomSpot;
    public Vector2[] moveSpots = new Vector2[5]; //could be used if we want the Patrol AI to move between set spots

    void Start()
    {
        fWaitTime = fStartWaitTime;
        iRandomSpot = Random.Range(0, moveSpots.Length);
        FillMoveSpots();
        aiMvmt = MovementTypeEnum.Patrol;
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
        
        //movement
        switch(aiMvmt)
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
    }

    protected void MvmtPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[iRandomSpot], fSpeed * Time.deltaTime);
        //check if we've reached our destination
        if(Vector2.Distance(transform.position, moveSpots[iRandomSpot]) < 0.2f)
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
    }

    protected void MvmtChase()
    {

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
    }
}
