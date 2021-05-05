using System;
using System.Linq;
using UnityModManagerNet;

namespace AdofaiUtils.Tweaks
{
    public class TextOverlay
    {
        public TextOverlayItem[] Items = { };
        
        public TextOverlay()
        {
        }

        public void Show()
        {
            
        }
    }

    public class ProgressOverlay : TextOverlayItem
    {
        public ProgressOverlay() : base("progress", "진행도")
        {
        }
        
        public override string Render(UnityModManager.ModEntry modEntry)
        {
            return "TEST";
        }
    }

    public  abstract class TextOverlayItem
    {
        public string ID;
        public string ConfigLabel;

        public TextOverlayItem(string id, string configLabel)
        {
            ID = id;
            ConfigLabel = configLabel;
        }

        public virtual string Render(UnityModManager.ModEntry modEntry)
        {
            return "";
        }
    }
}