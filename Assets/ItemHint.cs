using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHint : MonoBehaviour
{
    [SerializeField] HintElement[] elements;

    [SerializeField] ParticleSystem particleSystem;

    GameObject currentElement;
    public void Hint(Item item)
    {
        if(currentElement){
            currentElement.SetActive(false);
            currentElement = null;
        }

        for(int i=0;i<elements.Length;i++){
            if( elements[i].item == item){
                currentElement = elements[i].element;
                break;
            }
        }

        if( currentElement ){
           currentElement.SetActive(true);
            particleSystem.Play();
        }
        else {
            particleSystem.Stop();
        }
    }


    [System.Serializable]
    public class HintElement {
        public Item item;
        public GameObject element;
    }
}
