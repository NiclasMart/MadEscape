// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 22/11/23
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using Newtonsoft.Json;
using System.IO;

namespace Stats
{
    public static class LoadStats
    {
        private const string PLAYER_INPUT_CSV = "PlayerStats";
        private const string WEAPON_INPUT_CSV = "WeaponStats";
        private const string ENEMY_INPUT_CSV = "EnemyStats";

        public static List<StatRecord> LoadPlayerStats()
        {
            return LoadStatsFromFile(PLAYER_INPUT_CSV);
        }
        public static List<StatRecord> LoadWeaponStats()
        {
            return LoadStatsFromFile(WEAPON_INPUT_CSV);
        }
        public static List<StatRecord> LoadEnemyStats()
        {
            return LoadStatsFromFile(ENEMY_INPUT_CSV);
        }

        private static List<StatRecord> LoadStatsFromFile(String fileName)
        {
            List<StatRecord> statRecordList = new();

            //Read in CSV
            List<Dictionary<string, object>> data = CSVReader.Read(fileName);

            for (var i = 0; i < data.Count; i++)
            {
                int id = int.Parse(data[i]["id"].ToString(), System.Globalization.NumberStyles.Integer);
                data[i].Remove("id");
                string name = data[i]["name"].ToString();
                data[i].Remove("name");
                Dictionary<Stat, float> statDict = ConvertDictionary(data[i]);
                statRecordList.Add(MergeStatRecord(id, name, statDict));
            }
            return statRecordList;
        }

        private static Dictionary<Stat, float> ConvertDictionary(Dictionary<string, object> dict)
        {
            Dictionary<Stat, float> statDict = new();
            foreach (KeyValuePair<string, object> keyValuePair in dict)
            {
                if (Enum.TryParse(keyValuePair.Key, out Stat stat))
                {
                    //we need to disable warning CS0252 here, because keyValuePair.Value can not be cast to string nor can we use .ToString(), but it is ensured that this works because of how the csv is read...
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                    if (keyValuePair.Value != string.Empty)
                    {
                        statDict.Add(stat, (float)Convert.ToSingle(keyValuePair.Value));
                    }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
                }
            }
            return statDict;
        }

        private static StatRecord MergeStatRecord(int inputId, string inputName, Dictionary<Stat, float> inputStatDict)
        {
            return new StatRecord()
            {
                id = inputId,
                name = inputName,
                statDict = inputStatDict
            };
        }
    }
}