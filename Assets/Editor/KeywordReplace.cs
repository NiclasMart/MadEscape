using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class KeywordReplace : AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index < 0)
        {
            return;
        }
        string file = path.Substring(index);

        if (file != ".cs" && file != ".js" && file != ".boo")
        {
            return;
        }
        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;

        if (!File.Exists(path))
        {
            return;
        }

        file = File.ReadAllText(path);
        file = file.Replace("#CREATIONDATE#", DateTime.Today.ToString("dd/MM/yy") + "");
        file = file.Replace("#PROJECTNAME#", CloudProjectSettings.projectName);
        file = file.Replace("#DEVELOPER#", CloudProjectSettings.userName);
        file = file.Replace("#NAMESPACE#", GetNamespaceFromPath(path));

        File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }

    private static string GetNamespaceFromPath(string path)
    {
        DirectoryInfo parentDir = Directory.GetParent(path);
        string returnPath = parentDir.Name;
        string[] notNamespaces = { "Assets", "Code", "Scripts" };
        if (notNamespaces.Contains(returnPath))
        {
            returnPath = "yourNamespaceName //TODO: Please set your Namespace here";
        }
        return returnPath;
    }
}