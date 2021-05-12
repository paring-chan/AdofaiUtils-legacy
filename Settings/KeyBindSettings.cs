using UnityEngine;
using UnityModManagerNet;

namespace AdofaiUtils.Settings
{
    public class KeyBindSettingsFields
    {
        
        [DrawFields(DrawFieldMask.Public)]
        public class CLSKeyBindSettings
        {
            [Draw("리로드")]
            public KeyBindSetting Reload = new KeyBindSetting(KeyCode.R);
            [Draw("창작마당 열기")]
            public KeyBindSetting Workshop = new KeyBindSetting(KeyCode.W);
            [Draw("에디터 열기")]
            public KeyBindSetting Editor = new KeyBindSetting(KeyCode.E);
            [Draw("맵 바로 입장")]
            public KeyBindSetting EnterMap = new KeyBindSetting(KeyCode.LeftArrow);
            [Draw("맵 정보 보기")]
            public KeyBindSetting MapInfo = new KeyBindSetting(KeyCode.I);
        }
        
        [DrawFields(DrawFieldMask.Public)]
        public class EditorKeyBindSettings
        {
            [Draw("에디터 나가기")]
            public KeyBindSetting Quit = new KeyBindSetting(KeyCode.Q);
        }
        
        public class CustomPlayKeyBindSettings
        {
            [Draw("즉시 재시작(R)")] public bool Restart = true;
        }
    }

    public class KeyBindSetting
    {
        private KeyCode _keyCode;
        
        public KeyBindSetting(KeyCode keyCode, bool defaultEnabled = true)
        {
            _keyCode = keyCode;
            Key = new KeyBinding {keyCode = keyCode};
            Enabled = defaultEnabled;
        }
        
        [Draw("활성화")] public bool Enabled;

        [Draw("키 설정")] public KeyBinding Key;
    }
    
    [DrawFields(DrawFieldMask.Public)]
    public class KeyBindSettings
    {
        [Draw("CLS", Collapsible = true)] public KeyBindSettingsFields.CLSKeyBindSettings ClsKeyBindSettings = new KeyBindSettingsFields.CLSKeyBindSettings();
        [Draw("에디터", Collapsible = true)] public KeyBindSettingsFields.EditorKeyBindSettings EditorKeyBindSettings = new KeyBindSettingsFields.EditorKeyBindSettings();
        [Draw("커스텀 레벨 플레이", Collapsible = true)] public KeyBindSettingsFields.CustomPlayKeyBindSettings CustomPlayKeyBindSettings = new KeyBindSettingsFields.CustomPlayKeyBindSettings();
    }
}