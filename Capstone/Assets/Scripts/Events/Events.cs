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

// other events to add...

