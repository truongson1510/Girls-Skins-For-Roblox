
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonNavigation : MonoBehaviour
{
	#region Inspector Variables

	private Button button;

    #endregion

    #region Member Variables

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
