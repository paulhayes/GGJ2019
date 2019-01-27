using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineView : AbstractView
{
    static bool triggerTutorial = true;

	[SerializeField] ItemCollection collection;

	public Item currentItem;

    [SerializeField]
    DialogManager dialogManager;

    [SerializeField] Dialog introDialog;

    SearchView searchView;

    public void Awake(){
        searchView = GetComponent<SearchView>();
        dialogManager.OnFinished += OnDialogComplete;

        triggerTutorial = SofaView.triggerTutorial;

    }

    private void OnDialogComplete()
    {
        currentItem.dialog.Enqueues = null;
		collection.Complete (currentItem);

		searchView.Begin();
        //dialogManager.Play(introDialog);

        End();
    }

    public override void Begin()
    {
        if(!currentItem){
            searchView.Begin();
            End();
            return;
        }
        dialogManager.OnFinished += OnDialogComplete;

        currentItem.hasBeenFound = true;
        if (triggerTutorial)
        {
            currentItem.dialog.Enqueues = introDialog;
            triggerTutorial = false;
        } else
        {
            currentItem.dialog.Enqueues = null;
        }

        dialogManager.Play(currentItem.dialog);
        

    }

    public override void End()
    {
        dialogManager.OnFinished -= OnDialogComplete;
    }

}
