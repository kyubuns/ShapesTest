using System;
using AnimeTask;
using UnityEngine;

namespace ShapeTest
{
    public class DisposableGameObject : IDisposable
    {
        public GameObject GameObject { get; }

        public DisposableGameObject(Vector3 position)
        {
            GameObject = new GameObject();
            GameObject.transform.localPosition = position;
        }

        public T AddComponent<T>() where T : Component
        {
            return GameObject.AddComponent<T>();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(GameObject);
        }
    }

    public static class MyEasing
    {
        public static Vector3EasingAnimatorMid Create<T>(Vector3 start, Vector3 mid, Vector3 end, float duration) where T : IEasing, new()
        {
            return new Vector3EasingAnimatorMid(new T(), start, mid, end, duration);
        }
    }

    public class Vector3EasingAnimatorMid : IAnimator<Vector3>
    {
        private readonly IEasing easing;
        private readonly Vector3 start;
        private readonly Vector3 mid;
        private readonly Vector3 end;
        private readonly float duration;

        public Vector3EasingAnimatorMid(IEasing easing, Vector3 start, Vector3 mid, Vector3 end, float duration)
        {
            this.easing = easing;
            this.start = start;
            this.mid = mid;
            this.end = end;
            this.duration = duration;
        }

        public void Start()
        {
        }

        public Tuple<Vector3, bool> Update(float time)
        {
            var t = Mathf.Min(time / duration, 1f);
            if (t < 0.5f)
            {
                t = t * 2f;
                return Tuple.Create(Vector3.LerpUnclamped(start, mid, easing.Function(t)), time > (double) duration);
            }
            else
            {
                t = (t - 0.5f) * 2f;
                return Tuple.Create(Vector3.LerpUnclamped(mid, end, easing.Function(t)), time > (double) duration);
            }
        }
    }
}
