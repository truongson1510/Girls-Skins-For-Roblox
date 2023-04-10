
using UnityEngine;
using BacSonStudio;
using UnityEngine.UI;

public class ButtonNavigation : ClickableListItem
{
    #region Inspector Variables

    [SerializeField] private GameObject selectIcon;
    
    #endregion

    #region Member Variables

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    #endregion

    #region Public Methods

    public void SetSelected(bool state)
    {
        selectIcon.SetActive(state);
    }

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
