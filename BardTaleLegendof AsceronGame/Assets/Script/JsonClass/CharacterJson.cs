using System;

namespace JsonDataClasses
{
    [Serializable]
    public class CharacterJson : UnitJson
    {
        public UnitStatJson growthStat;
        public WeaponJson weapon;
        public ArmorJson[] armors;
        public AccessoryJson[] accessories;
    }
}