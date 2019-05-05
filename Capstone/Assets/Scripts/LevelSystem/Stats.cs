using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour, ISubscriber<OnEnemyKilledEvent>
{

    //stats
    public int Level { get; private set; } = 1; 
    public float AttackRateMultiplier { get; private set; } = 0.01f;
    public const float MAX_ATTACK_RATE_MULTIPLIER = 0.85f;
    public float ReloadMultiplier { get; private set; } = 0.01f;
    public float DamageMultiplier { get; private set; } = 1.0f;

    //Health
    public float Health;
    public float MaxHealth { get; private set; } = 100f; // default

    private int playerNum;
    private int CurrentExperience = 0;
    private int NextLevelRequirement;
    private const float EXPONENT = 1.5f;
    private const int BASE_EXP = 100;
    public readonly int MAX_LEVEL = 100;


    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnEnemyKilledEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnEnemyKilledEvent>(this);
    }

    void Start()
    {
        playerNum = GetComponent<Player>().playerNumber;
        CalculateNextLevel();
        InitializeStats(); // used for testing higher level
    }

    public void SetMaxHP(float HP)
    {
        if (HP < MaxHealth)
            Debug.Log("WARNING: You've lowered Max HP. Max HP was: " + MaxHealth + ", Max HP is now: " + HP);
        
        MaxHealth = HP;
        Health = MaxHealth;
    }

    public void OnEventHandler(OnEnemyKilledEvent eventData)
    {
        if(eventData.PlayerNumber == this.playerNum)
        {
            ApplyExperience(eventData.EXP);
        }
    }


    private void ApplyExperience(int exp)
    {
        if (Level >= MAX_LEVEL) return;

        int overExp = exp;
        do
        {
            if (CurrentExperience + overExp >= NextLevelRequirement)
            {
                overExp = (CurrentExperience + overExp) - NextLevelRequirement;
                CurrentExperience = 0;
                OnLevelUp();
                CalculateNextLevel();
            }
            else
            {
                CurrentExperience += overExp;
                Debug.Log("Gained EXP, Current is EXP: " + CurrentExperience + ", \nNext Level EXP Requirement: " + NextLevelRequirement);
                overExp = 0;
            }
        } while (overExp > 0);
        
    }

    private int CalculateNextLevel()
    {
        return NextLevelRequirement = 100; // testint used to level up quickly, in general we would use either the two functions below
        //or return NextLevelRequirement = (int)Mathf.Ceil(BASE_EXP * Level);
        //or return NextLevelRequirement = (int)Mathf.Celi(BASE_EXP * (LevelNumber ^ EXPONENT));
    }

    private void OnLevelUp()
    {
        Level = ++Level >= MAX_LEVEL ? MAX_LEVEL : Level;
        Debug.Log("You've Leveled Up To Level: " + Level + "!!!");
        if(AttackRateMultiplier < MAX_ATTACK_RATE_MULTIPLIER) AttackRateMultiplier += 0.01f;
        ReloadMultiplier += 0.01f;
        DamageMultiplier += 0.2f;
        MaxHealth = MaxHealth + 10;
        EventAggregator.GetInstance().Publish<OnLevelUpEvent>(new OnLevelUpEvent(playerNum, this));
    }

    private void InitializeStats()
    {
        AttackRateMultiplier = Level * AttackRateMultiplier;
        if (AttackRateMultiplier > MAX_ATTACK_RATE_MULTIPLIER) AttackRateMultiplier = MAX_ATTACK_RATE_MULTIPLIER; 
        ReloadMultiplier = Level * ReloadMultiplier;
        DamageMultiplier = Level * DamageMultiplier;
        MaxHealth = Health = MaxHealth + (Level * 10);
    }

}
