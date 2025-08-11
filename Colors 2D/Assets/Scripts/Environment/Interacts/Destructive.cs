using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructive : MonoBehaviour , IDamageable
{
    public void TakeDmg(int dmg)
    {
        Destroy(gameObject);
    }
}
