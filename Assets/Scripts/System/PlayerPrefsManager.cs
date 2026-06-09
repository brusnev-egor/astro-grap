using System;
using UnityEngine;

public class PlayerPrefsManager
{
    private const string MUSIC_PREF_KEY = "MUSIC_PREF_KEY";
    private const string SOUND_PREF_KEY = "SOUND_PREF_KEY";
    private const string VIBRATION_PREF_KEY = "VIBRATION_PREF_KEY";

    public static bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MUSIC_PREF_KEY) == 1;
    }

    public static bool IsSoundEnabled()
    {
        Debug.Log("value: " + PlayerPrefs.GetInt(VIBRATION_PREF_KEY));
        return PlayerPrefs.GetInt(SOUND_PREF_KEY) == 1;
    }

    public static bool IsVibrationEnabled()
    {
        return PlayerPrefs.GetInt(VIBRATION_PREF_KEY) == 1;
    }

    public static void SetMusic(int value)
    {
        PlayerPrefs.SetInt(MUSIC_PREF_KEY, value);
        PlayerPrefs.Save();
        Debug.Log("Save");
    }

    public static void SetSound(int value)
    {
        PlayerPrefs.SetInt(SOUND_PREF_KEY, value);
        PlayerPrefs.Save();
    }

    public static void ToggleVibration(int value)
    {
        PlayerPrefs.SetInt(VIBRATION_PREF_KEY, value);
        PlayerPrefs.Save();
    }
}