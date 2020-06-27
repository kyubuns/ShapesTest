using AnimeTask;
using Cysharp.Threading.Tasks;
using Shapes;
using UnityEngine;

namespace ShapeTest
{
    public class Sample : MonoBehaviour
    {
        public void Start()
        {
            Animation().Forget();
        }

        private async UniTask Animation()
        {
            Debug.Log("Main Start");
            var discObject = new GameObject("Disc1");
            var disc = discObject.AddComponent<Disc>();
            disc.Type = Disc.DiscType.Disc;

            await Anime.Delay(1.0f);

        }
    }
}

