using UnityEngine;

namespace Assets.Script.ObjectPool.Demo.Scripts
{
    public class Turret : MonoBehaviour
    {
        public Bullet BulletPrefab;
        public Transform Gun;

        public void Update()
        {
            var plane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hit;
            if (plane.Raycast(ray, out hit))
            {
                var aimDirection = Vector3.Normalize(ray.GetPoint(hit) - transform.position);
                var targetRotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);

                if (Input.GetMouseButtonDown(0))
                    Script.ObjectPool.Scripts.ObjectPoolExtensions.Spawn(BulletPrefab, Gun.position, Gun.rotation);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Script.ObjectPool.Scripts.ObjectPoolExtensions.DestroyPooled(BulletPrefab);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Script.ObjectPool.Scripts.ObjectPoolExtensions.DestroyAll(BulletPrefab);
            }
        }
    }
}
