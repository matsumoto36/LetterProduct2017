﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MeleeEffectPlayableAsset : PlayableAsset
{

	public ExposedReference<PKFxFX> meleeEffect;

	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
		var behaviour = new MeleeEffectPlayableBehaviour();
		behaviour.meleeEffect = meleeEffect.Resolve(graph.GetResolver());
		return ScriptPlayable<MeleeEffectPlayableBehaviour>.Create(graph, behaviour);
	}
}
