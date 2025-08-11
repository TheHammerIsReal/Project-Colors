using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartShoot : IAbility
{
    Spawneable smartBullet;
    int _dmg = 5;
    
    public void Ability(Player player)
    {
        if (!player.smartBulletThrowed)
        {
            smartBullet = SpawneableFactory.instance.GetObjFromPool(SpawnType.SmartBullet);
            smartBullet.SetValues(player.spawnPointShoot.position, player.dir);
            //cambiar direccion del disparo a parabola o que la bala vaya cayendo
            //implementar disparo con linea estilo Prince of Persia
            player.smartBulletThrowed = true;
            return;
        }


        if (player.smartBulletThrowed)
        {
            var initialPos = player.transform.position;
            player.transform.position = smartBullet.transform.position;
            Line(initialPos, player.transform.position);
            //agregar en view metodo con el feedback
            player.smartBulletThrowed = false;
            SpawneableFactory.instance.ReturnToPool(smartBullet, SpawnType.SmartBullet);
            player.ColorWaste();
        }

    }


    void Line(Vector3 initialPos, Vector3 finalPos)
    {
        //Spawnear el objeto y pasarle las dos posiciones
        //Line renderer en view
        var hits = Physics2D.RaycastAll(initialPos, finalPos - initialPos);

        if (hits.Length>0)
        {
            foreach (var item in hits)
            {
               var dmg = item.collider.GetComponent<IDamageable>();

                if (dmg is Player) continue;

                if (dmg != null) dmg.TakeDmg(_dmg);
            }
        }
    }
}
