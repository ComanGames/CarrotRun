using System.Collections;
using UnityEngine;

namespace Assets.Script.ObjectPool.Demo.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public Explosion ExplosionPrefab;
        public float ShootDistance;
        public float ShootSpeed;

        public void OnEnable()
        {
            StartCoroutine(Shoot());
        }

        public void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator Shoot()
        {
            float travelledDistance = 0;
            while (travelledDistance < ShootDistance)
            {
                travelledDistance += ShootSpeed * Time.deltaTime;
                transform.position += transform.forward * (ShootSpeed * Time.deltaTime);
                yield return 0;
            }

            //Spawn a pooled explosion prefab
            Script.ObjectPool.Scripts.ObjectPoolExtensions.Spawn(ExplosionPrefab, transform.position);

            //Recycle this pooled bullet instance
            Script.ObjectPool.Scripts.ObjectPoolExtensions.Recycle(gameObject);
        }
    }
}
