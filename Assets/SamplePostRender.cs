using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShapeTest
{
    public class SamplePostRender : MonoBehaviour
    {
        private readonly List<Vector3> positions = new List<Vector3>();
        private readonly List<Vector2> sizes = new List<Vector2>();
        private readonly List<float> thicknesses = new List<float>();
        private readonly List<Color> colors = new List<Color>();

        public void Start()
        {
            Random.InitState(12345);
            for (var i = 0; i < 1000; ++i)
            {
                var width = Random.Range(0.2f, 1.5f);
                var height = Random.Range(0.2f, 1.5f);
                var color = Random.ColorHSV();
                var thickness = Random.Range(0.1f, 1.0f);
                var position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
                positions.Add(position);
                sizes.Add(new Vector2(width, height));
                thicknesses.Add(thickness);
                colors.Add(color);
            }
        }

        public void OnPostRender()
        {
            for (var i = 0; i < positions.Count; ++i)
            {
                Draw.RectangleBorder(positions[i], sizes[i], thicknesses[i], colors[i]);
            }
        }
    }
}
