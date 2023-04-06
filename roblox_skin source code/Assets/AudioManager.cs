
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	#region Inspector Variables

	[SerializeField] private AudioSource	audioPlayer;
	[SerializeField] private AudioClip		buttonClickSound;

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	#endregion

	#region Unity Methods

	#endregion

	#region Public Methods

	public void PlayButtonClickSound()
    {
		audioPlayer.PlayOneShot(buttonClickSound);

	}

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	#endregion
}
