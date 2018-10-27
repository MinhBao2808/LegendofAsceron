using System;

namespace JsonDataClasses
{
    [Serializable]
    public class ItemJson
    {
        public string id;
        public string name;
        public int lvlRequirement;
        public ItemType type;
        public string tooltips;
    }

    [Serializable]
    public class AccessoryJson : EquipmentJson
    {
        public AccessoryType aType;
    }

    [Serializable]
    public class ArmorJson : EquipmentJson
    {
        public ArmorType aType;
        public ArmorPiece aPiece;
    }

    [Serializable]
    public class WeaponJson : EquipmentJson
    {
        public WeaponType wType;
    }

    [Serializable]
    public class EquipmentJson : ItemJson
    {
        public EquipmentRarity rarity;
        public EquipmentModJson modification;
        public EquipmentStatJson stats;
        public int durability;
    }

    [Serializable]
    public class EquipmentStatJson
    {
        public float strength;
        public float dexterity;
        public float intelligence;
        public float vitality;
        public float endurance;
        public float wisdom;
        public float fireRes;
        public float lightningRes;
        public float iceRes;
        public float holyRes;
        public float darkRes;
    }

    [Serializable]
    public class EquipmentModJson
    {
        public float hp;
        public float mp;
        public float patk;
        public float matk;
        public float speed;
        public float evasion;
        public float crit;
        public float mdef;
        public float pdef;
    }
    
    public enum ArmorType
    {
        Heavy,
        Light,
        Clothing
    }

    public enum ArmorPiece
    {
        Head,
        Chest,
        Gauntlets,
        Boots
    }

    public enum WeaponType
    {
        Sword,
        Axe,
        Gun,
        Crossbow,
        Grimoire
    }

    public enum AccessoryType
    {
        Ring,
        Earring
    }

    public enum ItemType
    {
        Equipment,
        Usable,
        Recipe,
        Miscellaneous,
        Quest
    }

    public enum EquipmentRarity
    {
        Common,
        Uncommon,
        Rare,
        Mythical,
        Legendary,
        Relic
    }
}