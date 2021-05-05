using UnityModManagerNet;

namespace AdofaiUtils
{
    [DrawFields(DrawFieldMask.Public)]
    public class KeyBindSettings
    {
        [Draw("CLS", Collapsible = true)] public CLSKeyBindSettings clsKeyBindSettings = new CLSKeyBindSettings();
        
        [DrawFields(DrawFieldMask.Public)]
        public class CLSKeyBindSettings
        {
            [Draw("리로드(R)")]
            public bool Reload = true;
            [Draw("창작마당 열기(W)")]
            public bool Workshop = true;
            [Draw("에디터 열기(E)")]
            public bool Editor = true;
            [Draw("맵 바로 입장(왼쪽 방향키)")]
            public bool EnterMap = true;
            [Draw("맵 정보 보기(I)")]
            public bool MapInfo = true;
        }
    }
}