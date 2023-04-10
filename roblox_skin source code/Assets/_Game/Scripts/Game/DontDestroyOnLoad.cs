
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    #region Inspector Variables

    #endregion

    #region Member Variables

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region Public Methods

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
