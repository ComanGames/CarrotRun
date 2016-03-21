using System.Collections;
using UnityEngine;

namespace Assets.Script.ObjectPool.Demo.Scripts
{
    public class Explosion : MonoBehaviour
    {
        public AnimationCurve AnimationCurve;
        public float Duration;

        public void OnEnable()
        {
            StartCoroutine(Shrink());
        }

        IEnumerator Shrink()
        {
            transform.localScale = Vector3.one;
            float elapsed = 0;
            while (elapsed < Duration)
            {
                float scale = 1 - AnimationCurve.Evaluate(elapsed / Duration);
                transform.localScale = new Vector3(scale, scale, scale);
                elapsed += Time.deltaTime;
                yield return 0;
            }

            //Recycle this pooled explosion instance
            Script.ObjectPool.Scripts.ObjectPoolExtensions.Recycle(gameObject);
        }
    }
}
