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
            while (true)
            {
                AnimationElement().Forget();
                await UniTask.Delay(100);
            }
        }

        private async UniTask AnimationElement()
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
    }
}

