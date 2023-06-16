using System.Collections.Generic;
using UnityEngine;

//Author: Dan 'Thrash' Donato https://github.com/KaraThrash

namespace DonutDiner.UI.Menus
{
    public class Journal : MonoBehaviour
    {
        //indicate the active tab by moving that tab on their heirachy so it displays over the journal image
        public List<GameObject> pages; //the tabs at the top of the journal

        private int current_page;

        public void OnEnable()
        {
            SetPage(current_page);
        }

        public void ChangePage(int _change)
        {
            //disable the current page
            if (GetPages().Count > current_page && current_page >= 0)
            {
                GetPages()[current_page].SetActive(false);
            }

            current_page += _change;

            //wrap the value so the player can also scroll in either direction
            if (current_page < 0)
            {
                current_page = GetPages().Count - 1;
            }
            else if (current_page >= GetPages().Count)
            {
                current_page = 0;
            }

            if (GetPages()[current_page] != null)
            {
                GetPages()[current_page].SetActive(true);
            }
        }

        public void SetPage(int _page)
        {
            //disable the current page
            if (GetPages().Count > current_page && current_page >= 0)
            {
                if (GetPages()[current_page] != null)
                {
                    GetPages()[current_page].SetActive(false);
                }
            }

            current_page = _page;

            //set the page to the new value
            if (GetPages().Count > current_page && current_page >= 0)
            {
                if (GetPages()[current_page] != null)
                {
                    GetPages()[current_page].SetActive(true);
                }
            }
        }

        public List<GameObject> GetPages()
        {
            if (pages == null)
            {
                pages = new List<GameObject>();
            }

            return pages;
        }
    }
}