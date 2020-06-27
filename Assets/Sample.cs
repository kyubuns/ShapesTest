using AnimeTask;
using Cysharp.Threading.Tasks;
using Shapes;
using UnityEditor;
using UnityEngine;
using System.Linq;

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

                await Anime.Delay(0.1f);
                await Animation();

                if (playOnce) break;
            }
            EditorApplication.isPlaying = false;
        }

        private async UniTask Animation()
        {
            using (var shapeObjects = new DisposableGameObjectList(new []
            {
                new Vector3(-5f, 0f),
                new Vector3(-2.5f, 0f),
                new Vector3(0.0f, 0f),
                new Vector3(2.5f, 0f),
                new Vector3(5f, 0f),
            }))
            {
                var shapes = shapeObjects.GameObjects.Select(x =>
                {
                    var shape = x.AddComponent<Rectangle>();
                    shape.Type = Rectangle.RectangleType.RoundedHollow;
                    shape.Width = 1f;
                    shape.Height = 1f;
                    shape.Color = new Color32(255, 242, 173, 0);
                    shape.Thickness = 0f;
                    shape.CornerRadiusMode = Rectangle.RectangleCornerRadiusMode.PerCorner;
                    shape.CornerRadiii = Vector4.zero;
                    return shape;
                }).ToArray();

                async UniTask Anime1(Rectangle shape, int index)
                {
                    await Anime.Delay(index * 0.2f);

                    await UniTask.WhenAll(
                        Anime.Play(
                            Easing.Create<OutSine>(0f, 0.15f, 0.5f),
                            TranslateTo.Action<float>(x => shape.Thickness = x)
                        ),
                        Anime.Play(
                            Easing.Create<OutSine>(0f, 1f, 0.1f),
                            TranslateTo.Action<float>(x =>
                            {
                                var tmp = shape.Color;
                                tmp.a = x;
                                shape.Color = tmp;
                            })
                        )
                    );

                    await Anime.Delay(0.5f);

                    for (var i = 0; i < 4; ++i)
                    {
                        const float duration = 0.3f;
                        await Anime.Play(
                            Easing.Create<InOutCubic>(0f, 0.5f, duration),
                            TranslateTo.Action<float>(x =>
                            {
                                var tmp = shape.CornerRadiii;
                                tmp[(i + index) % 4] = x;
                                shape.CornerRadiii = tmp;
                            })
                        );

                        var tmp2 = shape.CornerRadiii;
                        tmp2[(i + index) % 4] = 100;
                        shape.CornerRadiii = tmp2;
                    }
                }

                async UniTask Anime2(Rectangle shape, int index)
                {
                    const float duration = 1f;
                    await UniTask.WhenAll(
                        Anime.PlayTo(
                            Easing.Create<InOutExpo>(0f, duration),
                            TranslateTo.LocalPositionX(shape.gameObject)
                        )
                    );

                    if (index != 0)
                    {
                        await UniTask.WhenAll(
                            Anime.Play(
                                Easing.Create<OutSine>(1f, 0f, 0.1f),
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

                async UniTask Anime3(Rectangle shape)
                {
                    await Anime.Play(
                        Easing.Create<InOutExpo>(0.5f, 0f, 0.5f),
                        TranslateTo.Action<float>(x => shape.CornerRadiii = Vector4.one * x)
                    );

                    await Anime.Play(
                        Easing.Create<InOutExpo>(0f, -20f, 0.5f),
                        TranslateTo.Action<float>(x => shape.transform.localRotation = Quaternion.Euler(0f, 0f, x))
                    );

                    await Anime.Play(
                        Easing.Create<InCubic>(0f, -7f, 0.5f),
                        TranslateTo.LocalPositionY(shape.gameObject)
                    );
                }

                await UniTask.WhenAll(shapes.Select((x, i) => Anime1(x, i)));
                await Anime.Delay(0.25f);
                await UniTask.WhenAll(shapes.Select((x, i) => Anime2(x, i)));
                await Anime3(shapes[0]);
                await Anime.Delay(0.5f);
            }
        }
    }
}

