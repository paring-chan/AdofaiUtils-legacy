using UnityModManagerNet;

namespace AdofaiUtils
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Test")] public bool test = true;

        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry entry)
        {
            Save(this, entry);
        }
    }
}