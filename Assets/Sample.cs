using System;
using AnimeTask;
using Cysharp.Threading.Tasks;
using Shapes;
using UnityEditor;
using UnityEngine;

namespace ShapeTest
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] private bool playOnce = false;

        public void Start()
        {
            Animation().Forget();
        }

        private async UniTask Animation()
        {
            while (true)
            {
                Debug.Log("Main Start");

                await Anime.Delay(0.1f);
                var discObject = new GameObject("Disc1");
                var disc = discObject.AddComponent<Disc>();
                disc.Type = Disc.DiscType.Ring;

                const float duration = 3f;
                await UniTask.WhenAll(
                    Anime.Play(
                        Easing.Create<Linear>(-10f, 10f, duration),
                        TranslateTo.LocalPositionX(discObject)
                    ),
                    Anime.Play(
                        new Vector3EasingAnimatorMid(
                            new InOutCirc(),
                            new Vector3(0f, 0f, 0f),
                            new Vector3(2.5f, 2.5f, 2.5f),
                            new Vector3(0f, 0f, 0f),
                        duration),
                        TranslateTo.LocalScale(discObject)
                    )
                );

                Destroy(discObject);

                if (playOnce) break;
            }
            EditorApplication.isPlaying = false;
        }

        private static async UniTask Play(params Func<UniTask>[] tasks)
        {
            foreach (var task in tasks)
            {
                await task();
            }
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

