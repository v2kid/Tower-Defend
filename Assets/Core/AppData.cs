using System.Collections.Generic;
using UnityEngine;

public static class AppData
{
    public static Monster[] monsterData;
    public static List<int[]> Waves;

    static AppData()
    {
        monsterData = MockData.Monsters;
        Waves = MockData.Waves;
    }
}
public class MockData
{
    public static Monster[] Monsters = {
        new Monster { Health = 100, Speed = 1, Damage = 10, MonsterType = 0, AttackRange = 0.5f },
        new Monster { Health = 200, Speed = 1.5f, Damage = 20, MonsterType = 1 , AttackRange = 0.5f},
        new Monster { Health = 300, Speed = 0.1f, Damage = 30, MonsterType = 2 , AttackRange = 0.5f},
        new Monster { Health = 500, Speed = 0.5f, Damage = 50, MonsterType = 3 , AttackRange = 0.5f}
    };
    public static List<int[]> Waves = new List<int[]>
    {
        new int[] { 0, 1, 0 },
        new int[] { 5, 1, 1 },
        new int[] { 5, 1, 2 },
        new int[] { 15, 1, 3 }
    };


}
//wave mean time start wave, amount of monster, type of monster
