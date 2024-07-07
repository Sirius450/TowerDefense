using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour
{
    //healt info
    [SerializeField] int maxHealt;
    int curentHealt;
    EnemyAI enemy;

    //healt bar
    [SerializeField] HealtBar healtBar;
    private Vector3 maxSizeHpBar;
    private Vector3 curentSize;


    private void Start()
    {
        enemy = GetComponent<EnemyAI>();
    }

    internal void OnTakeDamage(int damage)
    {
        curentHealt = Mathf.Clamp(curentHealt - damage,0,maxHealt);
        healtBar.SetHealtBar(curentHealt, maxHealt);

        if (curentHealt == 0)
        {
            enemy.IsDead();
        }
        
    }

    internal void OnTakeHealing(int healt)
    {
        curentHealt = Mathf.Clamp(curentHealt + healt, 0, maxHealt);
        healtBar.SetHealtBar(curentHealt, maxHealt);
    }
}
