using Boss.save;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss.Map
{
    public class MapManager : MonoBehaviour, ISavable
    {
        List<MapInterractor> _maps = new List<MapInterractor>();

        List<BossLocalization> unlockedLocalization = new List<BossLocalization>();

        private void Awake()
        {
            _maps = GetComponentsInChildren<MapInterractor>().ToList();
            _maps.ForEach(m => m.onInteract.AddListener(map => Debug.Log(map.location)));
        }

        public void UnlockNewLocation(BossLocalization bossLocalization)
        {
            unlockedLocalization.Add(bossLocalization);
            _maps.Find(m => m.location == bossLocalization).Unlock();
        }

        public DTO GetData()
        {
            return new MapManager_DTO() { locations = unlockedLocalization };
        }

        public void LoadData(DTO dTO)
        {
            unlockedLocalization = (dTO as MapManager_DTO).locations;
            unlockedLocalization.ForEach(ul => _maps.Find(m => m.location == ul).Unlock());
        }
    }

    public class MapManager_DTO : DTO
    {
        public List<BossLocalization> locations = new List<BossLocalization>();
    }

    public enum BossLocalization
    {
        CaravanRex,
        Ubuntu
    }
}

