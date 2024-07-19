using System.Collections.Generic;
using UnityEngine;

namespace Palettes
{
    public class PaletteView : MonoBehaviour
    {
        [SerializeField] private List<Palette> palettes;

        private void Start()
        {
            for (int i = 0; i < palettes.Count; i++)
            {
                palettes[i].OnSelect += OnSelect;
            }
        }

        private void OnSelect(Palette currentPalette)
        {
            for (int i = 0; i < palettes.Count; i++)
            {
                if(currentPalette != palettes[i])
                    palettes[i].Reset();   
            }
        }

    }
}