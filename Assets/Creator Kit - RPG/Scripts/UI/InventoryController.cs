using System.Collections;
using System.Collections.Generic;
using RPGM.Core;
using RPGM.Gameplay;
using TMPro;
using UnityEngine;

namespace RPGM.UI
{
    public class InventoryController : MonoBehaviour
    {
        public Transform elementPrototype;
        public float stepSize = 1;
        Vector2 firstItem;
        GameModel model = Schedule.GetModel<GameModel>();
        SpriteUIElement sizer;
        bool visible = false;

        void Start()
        {
            firstItem = elementPrototype.localPosition;
            elementPrototype.gameObject.SetActive(false);
            sizer = GetComponent<SpriteUIElement>();
            sizer.Hide(); // Start with the inventory hidden
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (visible)
                {
                    sizer.Hide();
                    visible = false;
                }
                else
                {
                    Refresh();
                    sizer.Show();
                    visible = true;
                }
            }
        }

        public void Refresh()
        {
            var cursor = firstItem;
            for (var i = transform.childCount - 1; i >= 1; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            foreach (var i in model.InventoryItems)
            {
                var count = model.GetInventoryCount(i);
                if (count <= 0) continue;

                var e = Instantiate(elementPrototype, transform);
                e.localPosition = cursor;
                e.GetChild(0).GetComponent<SpriteRenderer>().sprite = model.GetInventorySprite(i);
                e.GetChild(1).GetComponent<TextMeshPro>().text = $"x {count}";
                e.gameObject.SetActive(true);
                cursor.y -= stepSize;
            }
        }
    }
}
