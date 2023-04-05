using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
	#region Inspector Variables

	[SerializeField] private List<ButtonNavigation> listButton;
	[SerializeField] private List<GameObject>		listItemHolder;

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	public int SelectedButtonIndex { get; set; }

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
			// Set the current selected button to un-selected and select the new one
			listButton[SelectedButtonIndex].SetSelected(false);
			listItemHolder[SelectedButtonIndex].SetActive(false);

			listButton[index].SetSelected(true);
			listItemHolder[index].SetActive(true);

			SelectedButtonIndex = index;
		}
	}

	#endregion
}
