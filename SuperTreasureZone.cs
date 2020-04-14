using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OSFEModding
{
    public class SuperTreasureZone : ICustomZone
    {
        public SuperTreasureZone()
        {
        }

        public void GenerateZone(SpawnCtrl spawnCtrl, ZoneType zoneType)
        {
            if (zoneType == (ZoneType) 16)
            {
                spawnCtrl.ti.mainBattleGrid.currentEnemies.Clear();
                spawnCtrl.idCtrl.MakeWorldBarAvailable(false);
                spawnCtrl.ctrl.GameState = GState.Idle;

                for (int i = 0; i < 10; i++)
                {
                    List<Tile> tileList = spawnCtrl.ti.mainBattleGrid.Get(new TileApp(Location.RandEnemyUnique, Shape.Default, Pattern.Unoccupied, 1, (Tile) null, Pattern.All), 0, (Being) null, (ItemObject) null, true);
                    int index = Random.Range(0, tileList.Count);
                    spawnCtrl.PlaceBeing(spawnCtrl.treasureChests[Random.Range(0, spawnCtrl.treasureChests.Count)].beingID, spawnCtrl.ti.mainBattleGrid.grid[tileList[index].x, tileList[index].y], 0, true, (BattleGrid) null).GetComponent<Cpu>();
                }
            }
        }

        public void SetupZone(WorldBar worldBar)
        {
            if(!worldBar.zoneSprites.ContainsKey("16"))
                worldBar.zoneSprites.Add("16", worldBar.zoneSprites["Treasure"]);
            
            foreach (ZoneDot currentZoneDot in worldBar.currentZoneDots)
            {
                if (Random.Range(0, 10) == 0 && currentZoneDot.type == ZoneType.Treasure)
                {
                    currentZoneDot.SetType((ZoneType) 16);
                }
            }
        }
    }
}