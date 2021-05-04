using UnityModManagerNet;

namespace AdofaiUtils
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("추가 키바인드 설정", Collapsible = true)]
        public KeyBindSettings KeyBindSettings = new KeyBindSettings();

        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry entry)
        {
            Save(this, entry);
        }
    }
}