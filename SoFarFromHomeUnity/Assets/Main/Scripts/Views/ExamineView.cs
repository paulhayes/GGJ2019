using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineView : AbstractView
{
    static bool triggerTutorial;

	[SerializeField] ItemCollection collection;

	public Item currentItem;

    [HideInInspector]
    public AbstractView lastView;

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
        if (currentItem != null && currentItem.dialog != null)
            currentItem.dialog.Enqueues = null;

		collection.Complete (currentItem);

        //dialogManager.Play(introDialog);

        if (lastView)
            lastView.Begin();

        lastView = null;

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
        Item.unlocked++;
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
