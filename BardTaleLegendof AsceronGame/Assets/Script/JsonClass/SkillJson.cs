using System;

namespace JsonDataClasses
{
    [Serializable]
    public class SkillJson
    {
        public string id;
        public string tooltips;
        public bool isPassive;
        public int mpCost;
        public int lvlRequirement;
        public SkillTarget[] target;
        public SkillType[] type;
        public int power;
        public int damageDuration;
        public int buffDuration;
        public int debuffDuration;
    }

    public enum SkillTarget
    {
        Self,
        Ally,
        Allies,
        Enemy,
        Enemies
    }

   public enum SkillType
   {
        Damage,
        DoT,
        Heal,
        Debuff,
        Buff
   }
}
