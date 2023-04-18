
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
    private ItemData data;

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

    public void ChangeSkin(ItemType type, int index, ItemData data)
    {
        if(this.data != data)
        {
            UIManager.Instance.OpenRating();
        }
        this.data = data;

        switch (type)
        {
            case ItemType.Face:
                ChangeFace(data);
                break;

            case ItemType.Shirt:
                ChangeShirt(data);
                break;

            case ItemType.Hair:
                ChangeHair(index);
                break;

            case ItemType.Pant:
                ChangePant(data);
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
    private void ChangeFace(ItemData data)
    {
        faceRenderer.GetComponent<SkinnedMeshRenderer>().material = data.itemMaterial;
    }

    /// <summary>
    ///             Change skin for shirt 
    /// </summary>
    /// <param name="texture"></param>
    private void ChangeShirt(ItemData data)
    {
        shirtMaterial.SetTexture("_MainTex", data.ingameTexture);
        GameManager.Instance.shirtData = data;
    }

    /// <summary>
    ///             Change skin for pant 
    /// </summary>
    /// <param name="texture"></param>
    private void ChangePant(ItemData data)
    {
        pantMaterial.SetTexture("_MainTex", data.ingameTexture);
        GameManager.Instance.pantData = data;
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
