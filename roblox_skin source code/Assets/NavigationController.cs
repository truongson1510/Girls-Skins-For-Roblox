
using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
	#region Inspector Variables

	[SerializeField] private List<ButtonNavigation> listButton;

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	public int			SelectedButtonIndex { get; set; }
	//public Action<int>	OnButtonSelected	{ get; set; }

    #endregion

    #region Unity Methods

    private void Start()
    {
		Setup(0);
	}

    #endregion

    #region Public Methods

    public void Setup(int selectedButtonIndex)
	{
		for (int i = 0; i < listButton.Count; i++)
		{
			listButton[i].SetSelected(i == selectedButtonIndex);

			listButton[i].Index = i;
			listButton[i].OnListItemClicked = OnButtonClick;
		}

		SelectedButtonIndex = selectedButtonIndex;
	}

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	private void OnButtonClick(int index, object data)
	{
		if (index != SelectedButtonIndex)
		{
			// Set the current selected ColorListItem to un-selected and select the new one
			listButton[SelectedButtonIndex].SetSelected(false);
			listButton[index].SetSelected(true);

			SelectedButtonIndex = index;
			//OnButtonSelected(index);
		}
	}

	#endregion
}
