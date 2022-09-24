using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureRocket_2 : Figure
{
    public override IEnumerator StartAttackAction()
    {
        while (true)
        {
            _spawnParticlesAnimations.SetActive(true);
            var newBullet = _pool.GetFreeElement();
            newBullet.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet.gameObject.SetActive(true);
            newBullet.TurnOnTrail();

            yield return new WaitForSeconds(0.2f);

            var newBullet_1 = _pool.GetFreeElement();
            newBullet_1.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet_1.gameObject.SetActive(true);
            newBullet_1.TurnOnTrail();
            yield return new WaitForSeconds(_attackDelay);
        }
    }
}
