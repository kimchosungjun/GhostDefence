using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SEnemyData", menuName ="Scriptable/SEnemyData", order =int.MaxValue)]
public class SEnemyData : ScriptableObject
{
    public int EnemyID;
    public float Speed;
    public float HP;
    public string EnemyName;
    public int Damage;

    public SEnemyData(int _id, float _speed, float _hp, string _name, int _damage)
    {
        EnemyID = _id;
        Speed = _speed;
        HP = _hp;
        EnemyName = _name;
        Damage = _damage;
    }
}
