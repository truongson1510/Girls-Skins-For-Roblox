
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BacSonStudio
{
    [RequireComponent(typeof(Button))]
    public class ButtonAnimation : MonoBehaviour
    {
		#region Inspector Variables

		#endregion

		#region Member Variables

		private Vector3 originalScale		= Vector3.zero;
		private Button	uiButton			= null;
		private float	duration			= 0.2f;
		private float	scaleMultiplier		= 0.2f;

		#endregion

		#region Properties

		#endregion

		#region Unity Methods

		private void Awake()
		{
			originalScale = transform.localScale;

			uiButton = GetComponent<Button>();
			uiButton.onClick.AddListener(PunchScale);
		}

		#endregion

		#region Public Methods

		#endregion

		#region Protected Methods

		#endregion

		#region Private Methods

		private void PunchScale()
        {
			// Play sound
			AudioManager.Instance.PlayButtonClickSound();

			// Kill any currently running tweens on this transform
			transform.DOKill();

			// Reset scale to its default
			transform.localScale = originalScale;

			// Punch the scale of the button
			transform.DOPunchScale(originalScale * scaleMultiplier, duration, 1, 0).SetEase(Ease.InQuad);
		}

		#endregion
	}
}
