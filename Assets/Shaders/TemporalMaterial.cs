using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalMaterial : MonoBehaviour
{
    private List<Renderer> renderer_list = new List<Renderer>();
    private List<Material[]> original_materials_in_renderer = new List<Material[]>();
    private List<Material[]> substitute_materials_in_renderer = new List<Material[]>();
    private Dictionary<Material, Material> material_substitutes = new Dictionary<Material, Material>();

    /*public Material temp_material;

    void Start()
    {
        if (temp_material != null)
        {
            set_temporal_material(temp_material);
        }
    }*/

    public void set_temporal_material(Material temporal_material)
    {
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer_list.Add(renderer);

            Material[] original_materials = new Material[renderer.materials.Length];
            renderer.materials.CopyTo(original_materials, 0);

            original_materials_in_renderer.Add(original_materials);

            Material[] substitute_materials = new Material[renderer.materials.Length];

            for (int i = 0; i < renderer.materials.Length; ++i)
            {
                Material existing_substitute_material;
                if (material_substitutes.TryGetValue(renderer.materials[i], out existing_substitute_material))
                {
                    substitute_materials[i] = existing_substitute_material;
                }
                else
                {
                    Material substitute_material = new Material(temporal_material);
                    substitute_material.mainTexture = renderer.materials[i].mainTexture;

                    material_substitutes[substitute_material] = substitute_material;

                    substitute_materials[i] = substitute_material;
                }
            }

            substitute_materials_in_renderer.Add(substitute_materials);

            renderer.materials = substitute_materials;
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < renderer_list.Count; ++i)
        {
            renderer_list[i].materials = original_materials_in_renderer[i];
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < renderer_list.Count; ++i)
        {
            renderer_list[i].materials = substitute_materials_in_renderer[i];
        }
    }
}
