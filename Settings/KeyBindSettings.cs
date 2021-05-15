using UnityEngine;
using UnityModManagerNet;

namespace AdofaiUtils.Settings
{
    public class KeyBindSettingsFields
    {
        
        [DrawFields(DrawFieldMask.Public)]
        public class CLSKeyBindSettings
        {
            [Horizontal]
            [Draw("리로드")]
            public KeyBindSetting Reload = new KeyBindSetting(KeyCode.R);
            [Horizontal]
            [Draw("창작마당 열기")]
            public KeyBindSetting Workshop = new KeyBindSetting(KeyCode.W);
            [Horizontal]
            [Draw("에디터 열기")]
            public KeyBindSetting Editor = new KeyBindSetting(KeyCode.E);
            [Horizontal]
            [Draw("맵 바로 입장")]
            public KeyBindSetting EnterMap = new KeyBindSetting(KeyCode.LeftArrow);
            [Horizontal]
            [Draw("맵 정보 보기")]
            public KeyBindSetting MapInfo = new KeyBindSetting(KeyCode.I);
        }
        
        [DrawFields(DrawFieldMask.Public)]
        public class EditorKeyBindSettings
        {
            [Horizontal]
            [Draw("에디터 나가기")]
            public KeyBindSetting Quit = new KeyBindSetting(KeyCode.Q);
        }
        
        public class CustomPlayKeyBindSettings
        {
            [Horizontal]
            [Draw("즉시 재시작")] public KeyBindSetting Restart = new KeyBindSetting(KeyCode.R);
        }
    }
    
    [DrawFields(DrawFieldMask.Public)]
    public class KeyBindSetting
    {
        public KeyBindSetting()
        {
            Key = new KeyBinding();
        }
        
        public KeyBindSetting(KeyCode keyCode)
        {
            Key = new KeyBinding {keyCode = keyCode};
        }
        
        public bool Enabled => Key.keyCode != KeyCode.None;

        public bool Down => Key.Down();

        [Draw("키 설정")] public KeyBinding Key;
    }
    
    [DrawFields(DrawFieldMask.Public)]
    public class KeyBindSettings
    {
        [Header("CLS")]
        [Draw("")] public KeyBindSettingsFields.CLSKeyBindSettings ClsKeyBindSettings = new KeyBindSettingsFields.CLSKeyBindSettings();
        [Header("에디터")]
        [Draw("")] public KeyBindSettingsFields.EditorKeyBindSettings EditorKeyBindSettings = new KeyBindSettingsFields.EditorKeyBindSettings();
        [Header("커스터 레벨 플레이")]
        [Draw("")] public KeyBindSettingsFields.CustomPlayKeyBindSettings CustomPlayKeyBindSettings = new KeyBindSettingsFields.CustomPlayKeyBindSettings();
    }
}