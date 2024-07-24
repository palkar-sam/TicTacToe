using Board;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Palettes
{
    public class Palette : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PaletteColor color;

        public event Action<Palette> OnSelect;

        public string GetPaletteColor()
        {
            return color.Code;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            transform.localScale = new Vector2(1.2f, 1.2f);
            CardBoard.SelectedCode = GetPaletteColor();
            OnSelect?.Invoke(this);
        }

        public void Reset()
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    public enum PaletteType
    {
        RED,
        YELLOW
    }

    [Serializable]
    public class PaletteColor
    {
        [SerializeField] private PaletteType type;
        [SerializeField] private string code;

        public string Code => code;
    }
}
