using System.Collections.Generic;
using UnityEngine;

namespace Palettes
{
    public class PaletteView : MonoBehaviour
    {
        public static string DefaultColor { get; set; }

        [SerializeField] private List<Palette> palettes;

        private static PaletteView _instance;

        public static string GetAiColorCode(string excludeCode)
        {
            return _instance.GetRandomCode(excludeCode);
        }


        private void Start()
        {
            _instance = this;

            for (int i = 0; i < palettes.Count; i++)
            {
                palettes[i].OnSelect += OnSelect;
            }

            DefaultColor = palettes[0].GetPaletteColor();
        }

        private void OnSelect(Palette currentPalette)
        {
            DefaultColor = currentPalette.GetPaletteColor();

            for (int i = 0; i < palettes.Count; i++)
            {
                if(currentPalette != palettes[i])
                    palettes[i].Reset();   
            }
        }

        private string GetRandomCode(string excludeCode)
        {
            string code = "";

            code = palettes[Random.Range(0, palettes.Count - 1)].GetPaletteColor();

            if(code == excludeCode)
                return GetRandomCode(excludeCode);
            else
                return code;
        }

    }
}