using Boss.inventory;
using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.Map
{
    public class MapManager : MonoBehaviour, ISavable
    {
        public UnityEvent<string> onGoToBossFight = new UnityEvent<string>(){};

        [SerializeField] GameObject mapParent;
        List<MapInterractor> _maps = new List<MapInterractor>();

        List<BossLocalization> unlockedLocalization = new List<BossLocalization>();

        private void Awake()
        {
            if (mapParent != null)
            {
                _maps = mapParent.GetComponentsInChildren<MapInterractor>().ToList();
                _maps.ForEach(m => m.onInteract.AddListener(map => {
                    onGoToBossFight?.Invoke(DADDY.Instance.GetBossFight(map.location));
                }));
            }
        }

        public void UnlockNewLocation(BossSacrificeable sacrifice)
        {
            unlockedLocalization.Add(sacrifice.Location);
            _maps.Find(m => m.location == sacrifice.Location).Unlock();
        }

        public DTO GetData()
        {
            return new MapManager_DTO() { locations = unlockedLocalization };
        }

        public void LoadData(DTO dTO)
        {
            unlockedLocalization = (dTO as MapManager_DTO).locations;
            unlockedLocalization.ForEach(ul => _maps.Find(m => m.location == ul)?.Unlock());
        }

        internal bool UnlockRandom()
        {
            MapInterractor map = _maps.Find(m => !unlockedLocalization.Contains(m.location));
            if (map == null)
            {
                return false;
            }
            else
            {
                map.Unlock();
                return true;
            }
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

