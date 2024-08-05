using System.Collections.Generic;
using UnityEngine;

namespace Palettes
{
    public class PaletteView : MonoBehaviour
    {
        public static string DefaultColor { get; set; }

        [SerializeField] private List<Palette> palettes;

        private static PaletteView _instance;

        public static int UserCellColorIndex = 1;

        public string SelectedColor { get; private set; } = "FF0000";

        public static string GetAiColorCode(string excludeCode)
        {
            return _instance.GetRandomCode(excludeCode);
        }

        public static string GetColorCodeAtIndex(int index)
        {
            return _instance == null ? "FF0000": _instance.palettes[index].GetPaletteColor();
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
            SelectedColor = DefaultColor = currentPalette.GetPaletteColor();

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