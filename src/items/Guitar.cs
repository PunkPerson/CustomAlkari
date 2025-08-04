using UnityEngine;

namespace CustomAlkari
{
    public class Guitar : PhysicsProp
    {
        [SerializeField] private SkinnedMeshRenderer mesh;
        [SerializeField] private AudioSource audio;
        [SerializeField] private Transform model;
        [SerializeField] private ScanNodeProperties scanNode;

        private int isGuitarBroken = 0;
        private bool isGuitarEquipped = false;

        private int guitarStage = 0;

        private float time = 0;
        
        public override int GetItemDataToSave()
        {
            base.GetItemDataToSave();
            return isGuitarBroken;
        }

        public override void LoadItemSaveData(int saveData)
        {
            base.LoadItemSaveData(saveData);
            isGuitarBroken = saveData;

            if (isGuitarBroken == 1) {
                mesh.SetBlendShapeWeight(0, 100.0f);
            }
        }

        public override void Start()
        {
            base.Start();

            model.localPosition = new Vector3(0, 0.1f, 0);
            model.localRotation = Quaternion.identity;
        }

        public override void Update()
        {
            base.Update();

            if (isGuitarBroken == 0 && playerHeldBy != null) time += Time.deltaTime;
        }

        public override void EquipItem()
        {
            base.EquipItem();

            time = 0;

            if (playerHeldBy != null) {
                playerHeldBy.equippedUsableItemQE = true;
                model.localPosition = new Vector3(-0.3418f, 0, 0);
                model.localRotation = Quaternion.Euler(0, 270.0f, 0);
            }
        }

        public override void DiscardItem()
        {
            if (playerHeldBy != null) {
                playerHeldBy.equippedUsableItemQE = false;
            }

            isGuitarEquipped = false;
            model.localPosition = new Vector3(0, 0.1f, 0);
            model.localRotation = Quaternion.identity;

            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);

            if (isGuitarBroken == 0) {
                if (isGuitarEquipped) {
                    if (time < 2) {
                        if (guitarStage < 10) {
                            guitarStage += 1;
                            time = 0;
                        }
                        else {
                            isGuitarBroken = 1;
                            playerHeldBy.DamagePlayer(10);
                            mesh.SetBlendShapeWeight(0, 100.0f);
                            scrapValue = (int) (scrapValue * 0.7);
                            scanNode.scrapValue = scrapValue;
                        }
                    }

                    time = 0;
                    audio.Play();
                }
            }
        }

        public override void ItemInteractLeftRight(bool right)
        {
            base.ItemInteractLeftRight(right);

            // Equip
            if (!right)
            {
                if (!isGuitarEquipped) {
                    isGuitarEquipped = true;
                    model.localPosition = Vector3.zero;
                    model.localRotation = Quaternion.identity;
                }

                else {
                    isGuitarEquipped = false;
                    model.localPosition = new Vector3(-0.3418f, 0, 0);
                    model.localRotation = Quaternion.Euler(0, 270.0f, 0);
                }
            }
        }
    }
}