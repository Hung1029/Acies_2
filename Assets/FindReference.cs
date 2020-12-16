
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Boo.Lang;

public class FindReference
{
    /// <summary>
    /// 查询目标文件夹下资源的使用（.prefab/.unity/.mat/.asset）情况，标记出资源使用为0的
    /// </summary>
    [MenuItem("Tools/Find References", false, 10)]
    static private void Find()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;

        //在这里定义查找文件的类型和文件的路径
        string[] guids = AssetDatabase.FindAssets("", new string[] { "Assets/PrestreamingAssets/Scenes/City/Animations" });


        string[] allassetpaths = new string[guids.Length];
        int index = 0;
        foreach (var item in guids)//将全部GUID转换为AssetPath
        {
            allassetpaths[index] = AssetDatabase.GUIDToAssetPath(item);
            index++;
        }
        int totalGuid = index;
        Dictionary<string, int> refDic = new Dictionary<string, int>();

        List withoutExtensions = new List() { ".prefab", ".unity", ".mat", ".asset" };
        string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
            .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        Match(allassetpaths, index, files, refDic);
    }

    [MenuItem("Tools/Find References", true)]
    static private bool VFind()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }

    static private void Match(string[] allassetpaths, int index, string[] files, Dictionary<string, int> refDic)
    {
        if (index > 0 && !string.IsNullOrEmpty(allassetpaths[index - 1]))
        {
            string guid = AssetDatabase.AssetPathToGUID(allassetpaths[index - 1]);

            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                string file = files[startIndex];
                bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);
                if (Regex.IsMatch(File.ReadAllText(file), guid))
                {
                    if (!refDic.ContainsKey(guid))
                    {
                        refDic.Add(guid, 1);
                    }
                    else
                    {
                        refDic[guid] += 1;
                    }
                }
                else
                {
                    if (!refDic.ContainsKey(guid))
                    {
                        refDic.Add(guid, 0);
                    }
                }
                startIndex++;
                if (isCancel || startIndex >= files.Length)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("匹配结束");
                    if ((index - 1) == 0)
                    {
                        OutputUnuse(refDic);
                    }
                    else
                    {
                        Match(allassetpaths, index - 1, files, refDic);
                    }
                }
            };
        }
    }
    static private void OutputUnuse(Dictionary<string, int> refDic)
    {
        Debug.Log("资源文件数量 ： " + refDic.Count);
        foreach (var item in refDic)
        {
            if (item.Value == 0)
            {
                Debug.Log(AssetDatabase.GUIDToAssetPath(item.Key) + " ---> 此资源引用次数 ：" + item.Value);
            }
        }
    }
    static private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }
}