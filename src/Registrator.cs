using UnityEngine;
using LethalLib.Modules;

namespace CustomAlkari
{
    public class Registrator
    {
        private AssetBundle bundle;

        public Registrator(AssetBundle bundle)
        {
            this.bundle = bundle;
        }

        private void YaroslavInit()
        {
            Item Yaroslav = bundle.LoadAsset<Item>("Assets/Yaroslav/YaroslavItem.asset");
            Utilities.FixMixerGroups(Yaroslav.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(Yaroslav.spawnPrefab);

            Items.RegisterScrap(Yaroslav, 25, Levels.LevelTypes.DineLevel | Levels.LevelTypes.RendLevel | Levels.LevelTypes.TitanLevel | Levels.LevelTypes.ExperimentationLevel);
        }

        private void ValeraInit()
        {
            EnemyType Valera = bundle.LoadAsset<EnemyType>("Assets/Valera/ValeraEnemy.asset");
            TerminalNode ValeraTerminal = bundle.LoadAsset<TerminalNode>("Assets/Valera/ValeraTerminal.asset");
            TerminalKeyword ValeraKeyword = bundle.LoadAsset<TerminalKeyword>("Assets/Valera/ValeraKeyword.asset");
            NetworkPrefabs.RegisterNetworkPrefab(Valera.enemyPrefab);

            Enemies.RegisterEnemy(Valera, 15, Levels.LevelTypes.All, ValeraTerminal, ValeraKeyword);
        }

        private void YuriInit()
        {
            Item Yuri = bundle.LoadAsset<Item>("Assets/Yuri/YuriItem.asset");
            Utilities.FixMixerGroups(Yuri.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(Yuri.spawnPrefab);

            Items.RegisterScrap(Yuri, 30, Levels.LevelTypes.All);
        }

        private void TsoiInit()
        {
            Item Tsoi = bundle.LoadAsset<Item>("Assets/Tsoi/TsoiItem.asset");
            Utilities.FixMixerGroups(Tsoi.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(Tsoi.spawnPrefab);
            
            Items.RegisterScrap(Tsoi, 10, Levels.LevelTypes.All);
        }

        private void BratanInit()
        {
            Item Bratan = bundle.LoadAsset<Item>("Assets/Bratan/BratanItem.asset");
            Utilities.FixMixerGroups(Bratan.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(Bratan.spawnPrefab);

            Items.RegisterScrap(Bratan, 30, Levels.LevelTypes.All);
        }

        private void BratanEnemyInit()
        {
            EnemyType Bratan = bundle.LoadAsset<EnemyType>("Assets/BratanEnemy/BratanEnemy.asset");
            /*TerminalNode ValeraTerminal = bundle.LoadAsset<TerminalNode>("Assets/Valera/ValeraTerminal.asset");
            TerminalKeyword ValeraKeyword = bundle.LoadAsset<TerminalKeyword>("Assets/Valera/ValeraKeyword.asset");*/
            NetworkPrefabs.RegisterNetworkPrefab(Bratan.enemyPrefab);

            Enemies.RegisterEnemy(Bratan, 30, Levels.LevelTypes.All, null, null);
        }

        private void AccordionmanInit()
        {
            EnemyType Accordionman = bundle.LoadAsset<EnemyType>("Assets/Accordionman/AccordionmanEnemy.asset");
            NetworkPrefabs.RegisterNetworkPrefab(Accordionman.enemyPrefab);

            Enemies.RegisterEnemy(Accordionman, 10, Levels.LevelTypes.All, null, null);
        }

        private void GuitarInit()
        {
            Item Guitar = bundle.LoadAsset<Item>("Assets/Guitar/GuitarItem.asset");
            Utilities.FixMixerGroups(Guitar.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(Guitar.spawnPrefab);

            Items.RegisterScrap(Guitar, 20, Levels.LevelTypes.All);
        }

        public void RegisterEnemies()
        {
            ValeraInit();
            //BratanEnemyInit();
            AccordionmanInit();
        }

        public void RegisterItems()
        {
            YaroslavInit();
            YuriInit();
            BratanInit();
            TsoiInit();
            GuitarInit();
        }
    }
}