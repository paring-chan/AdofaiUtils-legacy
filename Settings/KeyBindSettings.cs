using UnityModManagerNet;

namespace AdofaiUtils
{
    [DrawFields(DrawFieldMask.Public)]
    public class KeyBindSettings
    {
        [Draw("리로드(R)")]
        public bool Reload = true;
        [Draw("창작마당 열기(W)")]
        public bool Workshop = true;
        [Draw("맵 바로 입장(왼쪽 방향키)")]
        public bool EnterMap = true;
    }
}