using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTest : MonoBehaviour
{
    private Texture2D texture;

    [Range(0, 200)]
    public int scale;

    private int _scale;

    [Range(0, 10)]
    public float timeScale;

    // Start is called before the first frame update
    void Start()
    {

        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

        // set the pixel values
        //texture.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        //texture.SetPixel(1, 0, Color.clear);
        //texture.SetPixel(0, 1, Color.white);
        //texture.SetPixel(1, 1, Color.black);


       

        var renderer = GetComponent<Renderer>();
        // connect texture to material of GameObject this script is attached to
        renderer.material.mainTexture = texture;
    }

    Color32[] textPlaceholder = new Color32[1024 * 1024];

    // Update is called once per frame
    void Update()
    {
        //if (scale != _scale)
        //{
            var center = new Vector2(512, 512);
            // texture.get
            for (var x = 0; x < 1024; x++)
            for (var y = 0; y < 1024; y++)
            {
                var m = (new Vector2(x, y) - center).magnitude;

                var c = Mathf.Cos((m / scale) + Time.fixedTime * timeScale);

                //texture.SetPixel(x, y, new Color(c, c, c));
                textPlaceholder[x + y * 1024] = new Color(c, c, c);
            }

        texture.SetPixels32(textPlaceholder);

            // Apply all SetPixel calls
        texture.Apply();

            _scale = scale;
        //}
    }
}
