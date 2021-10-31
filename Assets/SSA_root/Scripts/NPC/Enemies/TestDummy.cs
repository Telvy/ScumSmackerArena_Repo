using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{

    private int health = 10;

    

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        this.gameObject.SetActive(false); 
    }
}
