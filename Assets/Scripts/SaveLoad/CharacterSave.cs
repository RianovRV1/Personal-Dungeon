using System.Collections;
using UnityEngine;
using System.IO;

public class CharacterSave
{
    public static CharacterContainer characterContainer = new CharacterContainer();
    public delegate void serializeAction();
    public static event serializeAction OnLoaded;
    public static event serializeAction OnBeforeSave;

    public static void Load(string path)
    {
        //called from button for now
        
        //populate container list then create characters
        characterContainer = LoadCharacters(path);

        foreach (CharacterData characterData in characterContainer.characterData)
        {
            SaveLoadManager.CreateCharacter(characterData, SaveLoadManager.prefab, FillRoster.trans);
        }

        OnLoaded();
        ClearCharacterList();
    }

    public static void Save(string path, CharacterContainer characters)
    {
        //called from button for now
        OnBeforeSave();
        SaveCharacters(path, characters);
        ClearCharacterList();
    }

    public static void AddCharacterData (CharacterData characterData)
    {
        //add charData to the container
        characterContainer.characterData.Add(characterData);
    }

     public static void ClearCharacterList()
    {
        //clear container
        characterContainer.characterData.Clear();
    }

    private static CharacterContainer LoadCharacters(string path)
    {
        //read chardata from json file
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<CharacterContainer>(json);
    }

    private static void SaveCharacters(string path, CharacterContainer characters)
    {
        //write chardata to json file
        string json = JsonUtility.ToJson(characters, true);

        StreamWriter sw = File.CreateText(path);
        sw.Close();

        File.WriteAllText(path, json);
    }
}
