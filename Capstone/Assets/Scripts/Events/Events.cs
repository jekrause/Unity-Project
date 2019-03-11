public class PlayerDamagedEvent
{
    public float Health { get; set; }
    public int playerNum { get; set; }

    public PlayerDamagedEvent(float amount, int playerNum) { this.Health = amount; this.playerNum = playerNum; }
}

public class PlayerHealedEvent
{
    public float Health { get; set; }
    public int playerNum { get; set; }

    public PlayerHealedEvent(float amount, int playerNum) { this.Health = amount; this.playerNum = playerNum; }
}

public class OnEnemeyNearDeathEvent
{
    // on this event called, we can have other nearby enemies come and aid the 
    // enemey that is near death
}

public class OnWeaponEquipEvent
{
    public Weapon Weapon;
    public int playerNum { get; set; }

    public OnWeaponEquipEvent(int playerNum, Weapon Weapon) { this.playerNum = playerNum; this.Weapon = Weapon; }
}

public class OnPlayerDeathEvent
{
    public int playerNum { get; set; }

    public OnPlayerDeathEvent(int playerNum) {this.playerNum = playerNum; }
}

// other events to add...

