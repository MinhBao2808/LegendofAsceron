using System;

namespace JsonDataClasses
{
    [Serializable]
    public class UnitJson
    {
        public string id;
        public string name;
        public string description;
        public UnitRace race;
        public UnitType type;
        public UnitStatJson baseStat;
        public string[] skills;
        public SkillJson[] totalSkills;
    }

    public enum UnitType
    {
        Melee, Range
    }

    public enum UnitRace
    {
        Undead, Humanoid
    }
}
