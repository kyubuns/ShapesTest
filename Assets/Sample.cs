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
            const float startPi = -Mathf.PI / 2f;

            using (var discObject1 = new DisposableGameObject(new Vector3(-4.5f, 0f)))
            using (var discObject2 = new DisposableGameObject(new Vector3(-1.5f, 0f)))
            using (var discObject3 = new DisposableGameObject(new Vector3(1.5f, 0f)))
            using (var discObject4 = new DisposableGameObject(new Vector3(4.5f, 0f)))
            {
                Disc CreateDisc(DisposableGameObject parent)
                {
                    var disc = parent.AddComponent<Disc>();
                    disc.Type = Disc.DiscType.Arc;
                    disc.Radius = 0.7f;
                    disc.Thickness = 0.3f;
                    disc.AngRadiansStart = startPi;
                    disc.AngRadiansEnd = startPi;
                    disc.Color = new Color32(255, 242, 173, 255);
                    return disc;
                }

                var disc1 = CreateDisc(discObject1);
                var disc2 = CreateDisc(discObject2);
                var disc3 = CreateDisc(discObject3);
                var disc4 = CreateDisc(discObject4);

                async UniTask Anime1(Disc disc, float delay)
                {
                    await Anime.Delay(delay);

                    await Anime.Play(
                        Easing.Create<OutSine>(startPi, startPi - Mathf.PI * 2f, 0.5f),
                        TranslateTo.Action<float>(x => disc.AngRadiansStart = x)
                    );

                    await Anime.Delay(1f);

                    await Anime.Play(
                        Easing.Create<InCubic>(0f, -7f, 0.5f),
                        TranslateTo.LocalPositionY(disc.gameObject)
                    );
                }

                await UniTask.WhenAll(
                    Anime1(disc1, 0.0f),
                    Anime1(disc2, 0.25f),
                    Anime1(disc3, 0.50f),
                    Anime1(disc4, 0.75f)
                );

                await Anime.Delay(0.5f);
            }
        }
    }
}

