
using UnityEngine;
using System.Collections.Generic;

public class GirlSkin : Singleton<GirlSkin>
{
    #region Inspector Variables

    [Header("MeshRenderers")]
    [SerializeField] private GameObject         faceRenderer;

    [Header("Reference Materials")]
    [SerializeField] private Material           pantMaterial;
    [SerializeField] private Material           shirtMaterial;

    [Header("Gameobjects")]
    [SerializeField] private List<GameObject>   hatCollection;
    [SerializeField] private List<GameObject>   hairCollection;
    [SerializeField] private List<GameObject>   glassesCollection;

    #endregion

    #region Member Variables

    private Animator animator;

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #endregion

    #region Public Methods

    public void ChangeSkin(ItemType type, int index, Material material, Texture2D texture)
    {
        switch (type)
        {
            case ItemType.Face:
                ChangeFace(material);
                break;

            case ItemType.Shirt:
                ChangeShirt(texture);
                break;

            case ItemType.Hair:
                ChangeHair(index);
                break;

            case ItemType.Pant:
                ChangePant(texture);
                break;

            case ItemType.Glasses:
                ChangeGlasses(index);
                break;

            case ItemType.Hat:
                ChangeHat(index);
                break;
        }
    }

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    /// <summary>
    ///             Change skin for face
    /// </summary>
    /// <param name="material"></param>
    private void ChangeFace(Material material)
    {
        faceRenderer.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    /// <summary>
    ///             Change skin for shirt 
    /// </summary>
    /// <param name="texture"></param>
    private void ChangeShirt(Texture2D texture)
    {
        shirtMaterial.SetTexture("_MainTex", texture);
    }

    /// <summary>
    ///             Change skin for pant 
    /// </summary>
    /// <param name="texture"></param>
    private void ChangePant(Texture2D texture)
    {
        pantMaterial.SetTexture("_MainTex", texture);
    }

    /// <summary>
    ///             Change skin for hat 
    /// </summary>
    /// <param name="index"></param>
    private void ChangeHat(int index)
    {
        for (int i = 0; i < hatCollection.Count; i++)
        {
            hatCollection[i].SetActive(i == index);
        }
    }

    /// <summary>
    ///             Change skin for hair 
    /// </summary>
    /// <param name="index"></param>
    private void ChangeHair(int index)
    {
        for (int i = 0; i < hairCollection.Count; i++)
        {
            hairCollection[i].SetActive(i == index);
        }
    }

    /// <summary>
    ///             Change skin for glasses
    /// </summary>
    /// <param name="index"></param>
    private void ChangeGlasses(int index)
    {
        for (int i = 0; i < glassesCollection.Count; i++)
        {
            glassesCollection[i].SetActive(i == index);
        }
    }

    #endregion
}
