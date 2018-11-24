using JsonDataClasses;
using System;

[Serializable]
public class EnemyUnit : EnemyJson {
    public EnemyUnit(EnemyJson json) {
        id = json.id;
        name = json.name;
        description = json.description;
        race = json.race;
        type = json.type;
        stats = json.stats;
        skills = json.skills;
        totalSkills = json.totalSkills;
        growthStat = json.growthStat;
        baseReward = json.baseReward;
        growthReward = json.growthReward;
    }
}
