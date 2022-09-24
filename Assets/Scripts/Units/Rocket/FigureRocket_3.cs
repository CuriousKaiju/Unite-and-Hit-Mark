using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureRocket_3 : Figure
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

            yield return new WaitForSeconds(0.2f);

            var newBullet_2 = _pool.GetFreeElement();
            newBullet_2.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet_2.gameObject.SetActive(true);
            newBullet_2.TurnOnTrail();

            yield return new WaitForSeconds(0.2f);

            var newBullet_3 = _pool.GetFreeElement();
            newBullet_3.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet_3.gameObject.SetActive(true);
            newBullet_3.TurnOnTrail();

            yield return new WaitForSeconds(_attackDelay);
        }
    }
}
