using System.Collections.Generic;
using UnityEngine;

public static class AppData
{
    public static Monster[] monsterData;
    public static List<int[]> Waves;
    public static Soilder[] SoidlerData;

    static AppData()
    {
        monsterData = MockData.Monsters;
        Waves = MockData.Waves;
        SoidlerData = MockData.Soidlers;
    }
}
public class MockData
{
    public static Monster[] Monsters = {
        new Monster { Health = 100, Speed = 1, Damage = 10, MonsterType = 0, AttackRange = 0.2f },
        new Monster { Health = 200, Speed = 1.5f, Damage = 20, MonsterType = 1 , AttackRange = 0.2f},
        new Monster { Health = 300, Speed = 0.1f, Damage = 30, MonsterType = 2 , AttackRange = 0.2f},
        new Monster { Health = 500, Speed = 0.5f, Damage = 50, MonsterType = 3 , AttackRange = 0.2f}
    };
    public static Soilder[] Soidlers = {
        new Soilder { Health = 1000, Speed = 1, Damage = 100, AttackRange = 0.5f, DetectionRange = 1f, SoilderType = SoilderType.Melee  },
    };
    public static List<int[]> Waves = new List<int[]>
    {
        new int[] { 0, 10, 0 },
        new int[] { 5, 10, 1 },
        new int[] { 5, 10, 2 },
        new int[] { 15, 10, 3 }
    };


}
//wave mean time start wave, amount of monster, type of monster
