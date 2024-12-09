using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{

    public List<Enemy> all_SpawnMonster;


    public void SendSound(Vector3 soundPos)
    {
        foreach (Enemy enemy in all_SpawnMonster)
        {
            if(enemy.GetComponent<GoSoundMonster>())
            {
                enemy.GetComponent<GoSoundMonster>().OnHearNoise(soundPos);
            }
        }
    }

}
