public class PlayerDamagedEvent
{
    public float Health { get; private set; }
    public int playerNum { get; private set; }

    public PlayerDamagedEvent(float amount, int playerNum) { this.Health = amount; this.playerNum = playerNum; }
}

public class PlayerHealedEvent
{
    public float Health { get; private set; }
    public int playerNum { get; private set; }

    public PlayerHealedEvent(float amount, int playerNum) { this.Health = amount; this.playerNum = playerNum; }
}

public class OnEnemyKilledEvent
{
    public OnEnemyKilledEvent() { }
}

public class OnPlayerWeaponChangedEvent
{
    public Weapon Weapon { get; private set; }
    public int playerNum { get; private set; }
    public Ammunition currentAmmunition { get; private set; }

    public OnPlayerWeaponChangedEvent(int playerNum, Weapon Weapon, Ammunition currentAmmunition) {
        this.playerNum = playerNum;
        this.Weapon = Weapon;
        this.currentAmmunition = currentAmmunition;
    }
}

public class OnPlayerDeathEvent
{
    public int playerNum { get; private set; }

    public OnPlayerDeathEvent(int playerNum) {this.playerNum = playerNum; }
}

public class OnWeaponReloadEvent
{
    public Weapon Weapon { get; private set; }
    public int playerNum { get; private set; }
    public OnWeaponReloadEvent(int playerNum, Weapon Weapon) { this.playerNum = playerNum; this.Weapon = Weapon; }
}

public class OnWeaponReloadCancelEvent
{
    public bool cancel { get; private set; }
    public int playerNum { get; private set; }
    public OnWeaponReloadCancelEvent(int playerNum, bool cancel) { this.playerNum = playerNum;  this.cancel = cancel; }
}

public class OnWeaponAmmoChangedEvent
{
    public int playerNum { get; private set; }
    public int currentAmmoClip { get; private set; }
    public OnWeaponAmmoChangedEvent(int playerNum, int currentAmmoClip) {
        this.playerNum = playerNum;
        this.currentAmmoClip = currentAmmoClip;
    }

}

public class OnPlayerAmmoChangedEvent
{
    public int playerNum { get; private set; }
    public Ammunition currentAmmunition { get; private set; }

    public OnPlayerAmmoChangedEvent(int playerNum, Ammunition currentAmmunition)
    {
        this.playerNum = playerNum;
        this.currentAmmunition = currentAmmunition;
    }
}

public class OnQuestItemPickUpEvent
{
    public QuestItem QuestItem { get; private set; }
    public OnQuestItemPickUpEvent(QuestItem item) { QuestItem = item; }
}

public class OnQuestItemDroppedEvent
{
    public QuestItem QuestItem { get; private set; }
    public OnQuestItemDroppedEvent(QuestItem item) { QuestItem = item; }
}

// other events to add...

