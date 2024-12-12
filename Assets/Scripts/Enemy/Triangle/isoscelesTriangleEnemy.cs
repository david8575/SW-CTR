using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoscelesTriangleEnemy : EnemyBase
{
    public GameObject missilePrefab;

    protected override IEnumerator Attack()
    {
        if (distance < 15f)
            AudioManager.PlaySound(status.AttackSound);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject missile = Instantiate(missilePrefab, pos, Quaternion.Euler(0, 0, -45));
        yield return null;
    }

    protected override void ApproachPlayer()
    {
        
    }
}
