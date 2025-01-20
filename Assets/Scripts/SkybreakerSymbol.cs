using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkybreakerSymbol : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float FlickerSpeed = 1f;
    [SerializeField]
    private AnimationCurve BrightnessCurve;
    private Renderer Renderer;
    private List<Material> Materials = new();
    private List<Color> InitialColors = new ();

    void Awake()
    {
        Renderer = GetComponent<Renderer>();
        BrightnessCurve.postWrapMode = WrapMode.Loop;

        foreach (Material material in Renderer.materials)
        {
            Debug.Log(material.name);
            if (Renderer.material.HasColor("_Color"))
            {
                Materials.Add(material);
                InitialColors.Add(material.GetColor("_Color"));
            }
            else
            {
                Debug.LogWarning($"{material.name} is not configured to be emissive." +
                $"so SkybreakerSymbol on {name} cannot animate this material");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Renderer.isVisible)
        {
            float scaledTime = Time.time * FlickerSpeed;

            for (int i = 0; i < Materials.Count; i++)
            {
                Color color = InitialColors[i];

                float brightness = BrightnessCurve.Evaluate(scaledTime);
                color = new Color(
                    color.r * Mathf.Pow(2, brightness),
                    color.g * Mathf.Pow(2, brightness),
                    color.b * Mathf.Pow(2, brightness),
                    color.a
                );

                Materials[i].SetColor("_Color", color);
            }
        }
    }
}
