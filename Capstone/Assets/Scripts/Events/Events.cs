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
    public int PlayerNumber { get; private set; } // the one who killed the enemy
    public int EXP { get; private set; } 
    public OnEnemyKilledEvent(int playerNum, int EXP)
    {
        this.PlayerNumber = playerNum;
        this.EXP = EXP;
    }
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

public class OnLootBagChangedEvent
{
    public LootBag LootBag { get; private set; }
    public OnLootBagChangedEvent(LootBag lootBag)
    {
        LootBag = lootBag;
    }
}

public class OnMainInvChangedEvent
{
    public int playerNumber { get; private set; }
    public int slotNum { get; private set; }
    public Item item { get; private set; }
    public int Quantity { get; private set; }
    public OnMainInvChangedEvent(int playerNum, int index, Item item, int quantity)
    {
        playerNumber = playerNum;
        slotNum = index;
        this.item = item;
        Quantity = quantity;
    }
}

public class OnWeaponInvChangedEvent
{
    public int playerNumber { get; private set; }
    public int slotNum { get; private set; }
    public Item item { get; private set; }
    public int Quantity { get; private set; }
    public bool Equipped { get; private set; }
    public OnWeaponInvChangedEvent(int playerNum, int index, Item item, int quantity, bool equipped)
    {
        playerNumber = playerNum;
        slotNum = index;
        this.item = item;
        Quantity = quantity;
        Equipped = equipped;
    }
}

public class OnLevelUpEvent
{
    public int PlayerNumber { get; private set; }
    public Stats Stats { get; private set; }

    public OnLevelUpEvent(int playerNum, Stats stat)
    {
        this.PlayerNumber = playerNum;
        this.Stats = stat;
    }
}

// other events to add...

