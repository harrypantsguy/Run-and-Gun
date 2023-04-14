using _Project.Codebase.Gameplay;
using DanonFramework.Runtime.Core.ServiceLayer;
using UnityEngine;

namespace _Project.Codebase.Services
{
    public class BuildingService : IService
    {
        public Building Building { get; private set; }

        public void SetBuilding(GameObject buildingObj)
        {
            Building = buildingObj.GetComponent<Building>();
        }
    }
}