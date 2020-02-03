using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SoundCollection:ScriptableObject {
	public AudioClip[] clips;

	int index;

	public AudioClip GetRandom(){
		index = ( Random.Range (1, clips.Length - 1) + index ) % clips.Length;
		return clips[ index ];
	}

	public AudioClip GetNext(){

		int i = index;
		index++;
		if( index >= clips.Length )
			index = 0;
		return clips[i];
	}
}


