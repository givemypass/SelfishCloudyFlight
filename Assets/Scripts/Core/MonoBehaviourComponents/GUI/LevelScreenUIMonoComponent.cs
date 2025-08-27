using UnityEngine;
using UnityEngine.UI;

namespace Components.MonoBehaviourComponents
{
    public class LevelScreenUIMonoComponent : MonoBehaviour
    {
        public Button Reset;
        public Sprite StartMarker;
        public Sprite EndMarker;
        public StartEndMarkerUIMonoComponent MarkerPrefab;
        public int MarkerMinDist = 10;
        public int MarkerMaxDist = 10;
        public ColorButtonMonoComponent[] ColorButtons;

        public void SetLevelColors(Color[] colors)
        {
            for (var i = 0; i < ColorButtons.Length; i++)
            {
                var color = colors[i];
                var colorButton = ColorButtons[i];
                colorButton.Image.color = color;
            } 
        }
    }
}