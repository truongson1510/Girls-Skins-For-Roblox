
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RawImage))]
public class RawImageGirl : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    #region Inspector Variables

    [SerializeField] private Transform girlTransform;

    #endregion

    #region Member Variables

    private RawImage    rawImage;
    private bool        isClickOnImage;

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Awake()
	{
		rawImage = GetComponent<RawImage>();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        isClickOnImage = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerMove(PointerEventData eventData)
    {
        if (isClickOnImage)
        {
            float rotation = Input.GetAxis("Horizontal") * -0.25f;
            girlTransform.Rotate(0f, rotation, 0f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        isClickOnImage = false;
    }

    #endregion

    #region Public Methods

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
