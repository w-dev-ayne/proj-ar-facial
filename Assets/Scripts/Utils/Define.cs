using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{

	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

	public enum Scene
	{
		Unknown,
		Loading,
		Main,
		AR,
		SenderScene
	}

	public enum Sound
	{
		Bgm,
		Effect,
		Speech,
		Effect2,
		Effect3,
		Max,
	}

	public enum FaceTrackingStatus
	{
		None,
		Tracking,
		Out
	}

	public enum AppearDirection
	{
		left,
		right,
		top,
		bottom
	}

	public enum AnimationType
	{
		A,
		B,
		C
	}
}
