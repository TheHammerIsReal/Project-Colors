using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : IAbility
{
    public void Ability(Player player)
    {
        var bullet = SpawneableFactory.instance.GetObjFromPool(SpawnType.Bullet);
        bullet.SetValues(player.spawnPointShoot.position, player.dir);
        player.ColorWaste();
    }

}
