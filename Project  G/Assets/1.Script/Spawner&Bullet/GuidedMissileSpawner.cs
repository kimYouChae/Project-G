using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileSpawner : NetSpawner
{
    public override void SettingBulletShootPosi()
    {
        SettingBulletShotPosi(); 
    }

    public override void SettingMoving()
    {
        // 움직임 X
    }
}
