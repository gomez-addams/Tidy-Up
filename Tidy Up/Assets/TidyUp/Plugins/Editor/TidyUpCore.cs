//-----------------------------------------------------------------
// This script handles the TidyUp Core System.
// It hosts the main Extension functions for manipulating project in the editor.
// TODO: 
// [X] Initialize Project Folders			Main Function
// [X] Clean Up My Mess			            Main Function
// [X] Create My Own Style                  Main Function
// [ ] Import/Export Setting                New Function
// [ ] Reset Setting                        New Function
//
//-----------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
public class TidyUpCore
{
    public static FolderTemplate folderTemplate = new FolderTemplate();
    private static string pathToResource = "TidyUp/Plugins/Editor/Data.json";

    internal static void CreateFolders()
    {
        foreach (FolderStructure _folderName in Enum.GetValues(typeof(FolderStructure)))
        {
            bool isExists = AssetDatabase.IsValidFolder("Assets/" + _folderName.ToString());
            if (isExists) //Skip Creating folder if it's already Exist
                continue;

            //Otherwise Create Folder
            AssetDatabase.CreateFolder("Assets", _folderName.ToString());
        }
    }

    internal static void CleanRootDirectory()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
        FileInfo[] info = dir.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly); //get only files in root Directory
        foreach (FileInfo file in info) //Loop over root files
        {
            int extensionPos = file.ToString().IndexOf(".");
            string extension = file.ToString().Substring(extensionPos);

            if (extension == ".unity" || extension == ".unity.meta")    //if file is Scene
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure._Scenes.ToString() + "/" + file.Name);
            }
            else if (extension == ".anim" || extension == ".anim.meta")   //if file is Animation
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure.Animation.ToString() + "/" + file.Name);
            }
            else if (extension == ".ttf" || extension == ".ttf.meta")   //if file is Font
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure.Fonts.ToString() + "/" + file.Name);
            }
            else if (extension == ".mat" || extension == ".mat.meta")   //if file is Material
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure.Materials.ToString() + "/" + file.Name);
            }
            else if (extension == ".prefab" || extension == ".prefab.meta")   //if file is Prefab
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure.Prefabs.ToString() + "/" + file.Name);
            }
            else if (extension == ".cs" || extension == ".cs.meta")   //if file is Script
            {
                AssetDatabase.MoveAsset(
                    "Assets/" + file.Name,
                    "Assets/" + FolderStructure.Scripts.ToString() + "/" + file.Name);
            }
        }
    }

    internal static FolderTemplate LoadSetting()
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, pathToResource));

        folderTemplate = JsonUtility.FromJson<FolderTemplate>(json);    //retrieve JSON to object

        return folderTemplate;
    }
    internal static void StoreSetting(FolderTemplate template)
    {
        string json = JsonUtility.ToJson(template); //save as JSON

        File.WriteAllText(Application.dataPath + "/" + pathToResource, json); //store json to file
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // for testing purpose
    internal static void RestSetting()
    {
        foreach (var item in Enum.GetValues(typeof(FolderStructure))) //populate list with enum data
        {
            Folder FT = new Folder();
            FT.folderName = item.ToString();
            FT.folderPath = "/";

            folderTemplate.folderTemplateList.Add(FT);
        }

        string json = JsonUtility.ToJson(folderTemplate); //convert list to json string
        //Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/" + pathToResource, json); //store json to file

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}