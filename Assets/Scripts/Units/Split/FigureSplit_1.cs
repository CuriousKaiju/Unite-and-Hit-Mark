using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSplit_1 : Figure
{
    public override IEnumerator StartAttackAction()
    {
        while (true)
        {
            _spawnParticlesAnimations.SetActive(true);
            var newBullet = _pool.GetFreeElement();
            newBullet.gameObject.transform.position = transform.position + new Vector3(0.1f, 0.6f, 0);
            newBullet.gameObject.transform.rotation = Quaternion.Euler(0, 7, 0);
            newBullet.gameObject.SetActive(true);
            newBullet.TurnOnTrail();


            var newBullet_1 = _pool.GetFreeElement();
            newBullet_1.gameObject.transform.position = transform.position + new Vector3(-0.1f, 0.6f, 0);
            newBullet_1.gameObject.transform.rotation = Quaternion.Euler(0, -7, 0);
            newBullet_1.gameObject.SetActive(true);
            newBullet_1.TurnOnTrail();

            var newBullet_2 = _pool.GetFreeElement();
            newBullet_2.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet_2.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            newBullet_2.gameObject.SetActive(true);
            newBullet_2.TurnOnTrail();
            yield return new WaitForSeconds(_attackDelay);
        }
    }
}
