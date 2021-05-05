using UnityModManagerNet;

namespace AdofaiUtils.Tweaks
{
    public class TextOverlay
    {
        public void Show()
        {
            
        }
    }

    public abstract class TextOverlayItem
    {
        public string ID;

        public TextOverlayItem(string id)
        {
            ID = id;
        }

        public string Render(UnityModManager.ModEntry modEntry)
        {
            return "";
        }
    }
}