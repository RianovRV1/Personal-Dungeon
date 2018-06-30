using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData = new CharacterData();
    public string characterName;
    public int level;

    public void StoreData()
    {
        //set save data to be equal to current data
        characterData.characterName = characterName;
        characterData.level = level;
    }

    public void ApplyData()
    {
        //add characterData to the container
        CharacterSave.AddCharacterData(characterData);
    }

    public void LoadData()
    {
        //set current data to be equal to save data
        characterName = characterData.characterName;
        level = characterData.level;
    }

    private void OnEnable()
    {
        //add load and save functions to deligates when object is enabled
        CharacterSave.OnLoaded += LoadData;
        CharacterSave.OnBeforeSave += StoreData;
        CharacterSave.OnBeforeSave += ApplyData;
    }
    private void OnDisable()
    {
        //remove functions from deligates when object is disabled
        CharacterSave.OnLoaded -= LoadData;
        CharacterSave.OnBeforeSave -= StoreData;
        CharacterSave.OnBeforeSave -= ApplyData;
    }
}

[Serializable]
public class CharacterData
{
    //info that will be saved for characters
    public string characterName;
    public int level;
}