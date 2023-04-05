
using UnityEngine;
using System.Collections.Generic;

public class GirlSkin : MonoBehaviour
{
    #region Inspector Variables

    [Header("MeshRenderers")]
    [SerializeField] private GameObject         faceRenderer;
    [SerializeField] private GameObject         skirtRenderer;
    [SerializeField] private List<GameObject>   pantRenderer;

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

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
