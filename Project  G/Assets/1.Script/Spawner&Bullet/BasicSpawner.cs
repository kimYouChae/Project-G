using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : NetSpawner
{
    public override void SettingBulletShootPosi()
    {
        SettingBulletShotPosi();
    }

    public override void SettingMoving()
    {
        // 플레이어 따라 이동
        SettingOwnerFollowMoving();
    }
}
