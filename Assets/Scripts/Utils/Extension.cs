using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Utils.GetOrAddComponent<T>(go);
	}

	/*public static void BindEvent(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_Base.BindEvent(go, action, type);
	}*/
	
	public static void BindEvent(this GameObject go, Action action, String clipName = "Button1", Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_Base.BindEvent(go, action, clipName, type);
	}

	static System.Random _rand = new System.Random();

	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = _rand.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static T GetRandom<T>(this IList<T> list)
	{
		int index = _rand.Next(list.Count);
		return list[index];
	}

	public static void ResetVertical(this ScrollRect scrollRect)
	{
		scrollRect.verticalNormalizedPosition = 1;
	}

	public static void ResetHorizontal(this ScrollRect scrollRect)
	{
		scrollRect.horizontalNormalizedPosition = 1;
	}
}