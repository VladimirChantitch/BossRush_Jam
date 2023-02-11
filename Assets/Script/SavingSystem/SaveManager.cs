using player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Mono.Cecil.Cil;
using System;
using System.IO;
using Boss.Map;

namespace Boss.save
{
    public class SaveManager : MonoBehaviour
    {
        List<DTO> dTOs = new List<DTO>();

        public void LoadGame()
        {
            dTOs = new List<DTO>();
            dTOs = GetDTOFromJsonString();

            if (dTOs.Count == 0)
            {
                return;
            }

            List<UnityEngine.Object> savables = FindObjectsOfType<UnityEngine.Object>().Where(o => o is ISavable).ToList();
            
            foreach (DTO dto in dTOs)
            {
                switch (dto)
                {
                    case Player_DTO player_DTO:
                        (savables.Where(s => s is PlayerManager).First() as ISavable).LoadData(player_DTO);
                        break;
                    case MapManager_DTO mapManager_DTO:
                        (savables.Where(s => s is MapManager).First() as ISavable).LoadData(mapManager_DTO);
                        break;
                }
            }
        }

        public void SaveGame()
        {
            dTOs = new List<DTO>();

            List<UnityEngine.Object> savables = FindObjectsOfType<UnityEngine.Object>().Where(o => o is ISavable).ToList();
            savables.ForEach(s => dTOs.Add((s as ISavable).GetData()));

            GenerateJsonString();
        }

        private void GenerateJsonString()
        {
            string jsonStrng = "";
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };

            try
            {
                string path = Directory.GetCurrentDirectory();
                Directory.CreateDirectory(path + @"\Saves");

                string save_file_path = @$"{path}\Saves\SaveFile.json";
                Debug.Log(save_file_path);
                File.WriteAllText(save_file_path, JsonConvert.SerializeObject(dTOs, settings));

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        private List<DTO> GetDTOFromJsonString()
        {
            List<DTO> saved_dtos = new List<DTO>();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };

            string path = Directory.GetCurrentDirectory();

            try
            {
                path = Directory.GetDirectories(path).ToList().Where(dir_path => dir_path.Contains("Saves")).ToList().First();
            }
            catch(Exception e)
            {
                Debug.Log("<color=red> No save files Yet </color>");
                return saved_dtos;
            }


            if (path == null || !path.Contains("Saves"))
            {
                Debug.Log("<color=red> No save files Yet </color>");
                return saved_dtos;
            }
            string save_path = Directory.GetFiles(path)[0];


            try
            {
                saved_dtos = JsonConvert.DeserializeObject<List<DTO>>(File.ReadAllText(save_path), settings);
                return saved_dtos;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message); 
                return saved_dtos;
            }
        }

        public void DestroySaveFile()
        {
            string path = Directory.GetCurrentDirectory();
            try
            {
                path = Directory.GetDirectories(path)?.ToList()?.Where(dir_path => dir_path.Contains("Saves"))?.ToList()?.First();
                Directory.Delete(path, true);
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }

        }
    }

    public interface ISavable
    {
        public abstract DTO GetData();
        public abstract void LoadData(DTO dTO);
    }

    public class DTO
    {

    }
}
