using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.Audio;
#endif

using UnityEngine.UIElements;

public class SoundManager
{
	public AudioSource[] _audioSources = new AudioSource[(int) Define.Sound.Max];

	public List<AudioSource> audioSourcePull = new List<AudioSource>();

	public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
	
	public AudioClip buttonClip;
	public AudioClip[] defaultClips;
	
	public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

	private GameObject _soundRoot = null;

	public float bgmVolume = 0.5f;
	public float speechVolume = 1.0f;
	

	public void Init()
	{
		if (_soundRoot == null)
		{
			_soundRoot = GameObject.Find("@SoundRoot");
			if (_soundRoot == null)
			{
				_soundRoot = new GameObject {name = "@SoundRoot"};
				Object.DontDestroyOnLoad(_soundRoot);

				string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
				for (int count = 0; count < soundTypeNames.Length - 1; count++)
				{
					GameObject go = new GameObject {name = soundTypeNames[count]};
					_audioSources[count] = go.AddComponent<AudioSource>();
					go.transform.parent = _soundRoot.transform;
				}

				_audioSources[(int) Define.Sound.Bgm].loop = true;
				//_audioSources[(int) Define.Sound.Bgm].volume = 0.1f;
			}
		}

		defaultClips = Resources.LoadAll<AudioClip>("DefaultSound/");

		foreach (AudioClip clip in defaultClips)
		{
			clips.Add(clip.name, clip);
			Debug.Log($"Load Default Clip {clip.name}");
		}

	}

	public void Clear()
	{
		foreach (AudioSource audioSource in _audioSources)
			audioSource.Stop();
		_audioClips.Clear();
	}

	public void SetPitch(Define.Sound type, float pitch = 1.0f)
	{
		AudioSource audioSource = _audioSources[(int) type];
		if (audioSource == null)
			return;

		audioSource.pitch = pitch;
	}

	public bool Play(Define.Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
	{
		if (string.IsNullOrEmpty(path))
			return false;

		AudioSource audioSource = _audioSources[(int) type];
		if (path.Contains("Sound/") == false)
			path = string.Format("Sound/{0}", path);

		audioSource.volume = volume;

		if (type == Define.Sound.Bgm)
		{
			AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
			if (audioClip == null)
				return false;

			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.Play();
			return true;
		}
		else if (type == Define.Sound.Effect)
		{
			AudioClip audioClip = GetAudioClip(path);
			if (audioClip == null)
				return false;

			audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
			return true;
		}
		else if (type == Define.Sound.Speech)
		{
			AudioClip audioClip = GetAudioClip(path);
			if (audioClip == null)
				return false;

			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.Play();
			return true;
		}

		return false;
	}

	private void SetVolume()
	{
		_audioSources[(int)Define.Sound.Bgm].volume = bgmVolume;
		_audioSources[(int)Define.Sound.Speech].volume = speechVolume;
		_audioSources[(int)Define.Sound.Effect].volume = speechVolume;
		_audioSources[(int)Define.Sound.Effect2].volume = speechVolume;
		_audioSources[(int)Define.Sound.Effect3].volume = speechVolume;
	}

	public void ModifyVolume(bool isBGM, float value)
	{
		if (isBGM)
		{
			bgmVolume = value;
			
			_audioSources[(int)Define.Sound.Bgm].volume = bgmVolume;
		}
		else
		{
			speechVolume = value;
			
			_audioSources[(int)Define.Sound.Speech].volume = speechVolume;
			_audioSources[(int)Define.Sound.Effect].volume = speechVolume;
			_audioSources[(int)Define.Sound.Effect2].volume = speechVolume;
			_audioSources[(int)Define.Sound.Effect3].volume = speechVolume;
		}
	}

	public void PlayAudioClip(Define.Sound type, AudioClip _clip, float volume = 1.0f, float pitch = 1.0f)
	{
		AudioSource audioSource = _audioSources[(int) type];
		
		//audioSource.volume = volume;

		if (type == Define.Sound.Bgm)
		{
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = _clip;
			audioSource.pitch = pitch;
			audioSource.Play();
		}
		else if (type == Define.Sound.Effect)
		{
			if (audioSource.isPlaying)
			{
				PlayAudioClip(Define.Sound.Effect2, _clip, volume, pitch);
				return;
			}
			
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(_clip);
		}
		else if (type == Define.Sound.Speech)
		{
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = _clip;
			audioSource.pitch = pitch;
			audioSource.Play();
		}
		else if (type == Define.Sound.Effect2)
		{
			if (audioSource.isPlaying)
			{
				PlayAudioClip(Define.Sound.Effect3, _clip, volume, pitch);
				return;
			}
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(_clip);
		}
		else if (type == Define.Sound.Effect3)
		{
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(_clip);
		}
	}

	/*public void PlayAudioClip(Define.Sound type, string clipName)
	{
		Addressables.LoadAssetAsync<AudioClip>(clipName).Completed +=
			handle =>
			{
				Managers.Addressable.handles.Add(handle);
				
			};
	}*/

	public void Stop(Define.Sound type)
	{
		AudioSource audioSource = _audioSources[(int) type];
		audioSource.Stop();
	}

	public float GetAudioClipLength(string path)
	{
		AudioClip audioClip = GetAudioClip(path);
		if (audioClip == null)
			return 0.0f;
		return audioClip.length;
	}

	private AudioClip GetAudioClip(string path)
	{
		AudioClip audioClip = null;
		if (_audioClips.TryGetValue(path, out audioClip))
			return audioClip;

		audioClip = Managers.Resource.Load<AudioClip>(path);
		_audioClips.Add(path, audioClip);
		return audioClip;
	}
	

	
}

        
