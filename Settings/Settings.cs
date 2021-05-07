using UnityModManagerNet;

namespace AdofaiUtils.Settings
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("추가 키바인드 설정", Collapsible = true)]
        public KeyBindSettings KeyBindSettings = new KeyBindSettings();
        
        [Draw("기타 기능", Collapsible = true)]
        public MiscSettings MiscSettings = new MiscSettings();


        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry entry)
        {
            Save(this, entry);
        }
    }
}