using System;
using System.Linq;
using AnimeTask;
using Cysharp.Threading.Tasks;
using Shapes;
using UnityEditor;
using UnityEngine;
using Animator = AnimeTask.Animator;
using Random = UnityEngine.Random;

namespace ShapeTest
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] private bool playOnce = false;

        public void Start()
        {
            Wrap().Forget();
        }

        private async UniTask Wrap()
        {
            while (true)
            {
                Debug.Log("Main Start");

                await ExplosionTest();

                if (playOnce) break;
            }
            EditorApplication.isPlaying = false;
        }

        private static async UniTask Sample1()
        {
            using (var shapeObject = new DisposableGameObject(new Vector3(-5f, 0f, 0f)))
            {
                var shape = shapeObject.AddComponent<Rectangle>();
                shape.Type = Rectangle.RectangleType.RoundedHollow;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Thickness = 0.1f;

                var cube = shape;

                await Anime.Delay(0.3f);
                await Anime.Play(
                    Easing.Create<Linear>(new Vector3(-5f, 0f, 0f), new Vector3(5f, 0f, 0f), 2f),
                    TranslateTo.LocalPosition(cube)
                );
                await Anime.Delay(0.3f);
            }
        }

        private static async UniTask Sample2()
        {
            using (var shapeObject = new DisposableGameObject(new Vector3(1f, -2f, 0f)))
            {
                var shape = shapeObject.AddComponent<Rectangle>();
                shape.Type = Rectangle.RectangleType.RoundedHollow;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Thickness = 0.1f;

                var cube = shape;

                await Anime.Delay(0.3f);
                await Anime.PlayTo(
                    Easing.Create<Linear>(new Vector3(-5f, 3f, 0f), 2f),
                    TranslateTo.LocalPosition(cube)
                );
                await Anime.Delay(0.3f);
            }
        }

        private static async UniTask Sample3()
        {
            using (var shapeObject = new DisposableGameObject(new Vector3(1f, -2f, 0f)))
            {
                var shape = shapeObject.AddComponent<Rectangle>();
                shape.Type = Rectangle.RectangleType.RoundedHollow;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Thickness = 0.1f;

                var cube = shape;

                await Anime.Delay(0.3f);
                await Anime.PlayTo(
                    Easing.Create<InCubic>(new Vector3(-5f, 3f, 0f), 2f),
                    TranslateTo.LocalPosition(cube)
                );
                await Anime.Delay(0.3f);
            }
        }

        private static async UniTask Sample4()
        {
            using (var shapeObject = new DisposableGameObject(new Vector3(-3f, 0f, 0f)))
            {
                var shape = shapeObject.AddComponent<Rectangle>();
                shape.Type = Rectangle.RectangleType.RoundedHollow;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Thickness = 0.1f;

                var cube = shape;

                await Anime.Delay(0.3f);
                await UniTask.WhenAll(
                    Anime.PlayTo(
                        Moving.Linear(3f, 2f),
                        TranslateTo.LocalPositionX(cube)
                    ),
                    Anime.PlayTo(
                        Animator.Delay(1.8f, Easing.Create<Linear>(Vector3.zero, 0.2f)),
                        TranslateTo.LocalScale(cube)
                    )
                );
                await Anime.Delay(0.3f);
            }
        }

        private static async UniTask GravityTest()
        {
            while (true)
            {
                AnimationElement().Forget();
                await UniTask.Delay(100);
            }
        }

        private static async UniTask AnimationElement()
        {
            using (var shapeObjects = new DisposableGameObject(Vector3.zero))
            {
                var shape = shapeObjects.AddComponent<Disc>();
                shape.Type = Disc.DiscType.Ring;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Radius = 0.3f;
                shape.Thickness = 0.1f;

                const float xrange = 5f;
                const float yrangeMin = 5f;
                const float yrangeMax = 10f;
                await Anime.PlayTo(
                    Moving.Gravity(new Vector3(Random.Range(-xrange, xrange), Random.Range(yrangeMin, yrangeMax)), Vector3.down * 9.8f, 5f),
                    TranslateTo.LocalPosition(shape)
                );
            }
        }

        private static async UniTask ExplosionTest()
        {
            await UniTask.WhenAll(Enumerable.Range(0, 50).Select(x => ExplosionTestElement()));
        }

        private static async UniTask ExplosionTestElement()
        {
            using (var shapeObjects = new DisposableGameObject(Vector3.zero))
            {
                var shape = shapeObjects.AddComponent<Disc>();
                shape.Type = Disc.DiscType.Ring;
                shape.Color = new Color32(255, 242, 173, 255);
                shape.Radius = 0.3f;
                shape.Thickness = 0.1f;

                const float rangeMin = 1f;
                const float rangeMax = 3f;
                var v = Random.insideUnitCircle.normalized * Random.Range(rangeMin, rangeMax);

                const float duration = 1f;
                const float eraseDuration = 0.5f;
                await Anime.Delay(0.5f);
                await UniTask.WhenAll(
                    Anime.PlayTo(
                        Easing.Create<OutExpo>(
                            new Vector3(v.x, v.y, 0f),
                            duration
                        ),
                        TranslateTo.LocalPosition(shape)
                    ),
                    Anime.Play(
                        Animator.Delay(duration - eraseDuration - 0.3f, Easing.Create<Linear>(1f, 0f, eraseDuration)),
                        TranslateTo.Action<float>(x =>
                        {
                            var tmp = shape.Color;
                            tmp.a = x;
                            shape.Color = tmp;
                        })
                    )
                );
            }
        }
    }
}
