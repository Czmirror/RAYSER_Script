namespace BGM.Volume
{
    /// <summary>
    /// 音量管理クラス
    /// </summary>
    public class VolumeData
    {
        private float Volume { get; set; } = 1f;

        public float GetVolume()
        {
            return Volume;
        }

        public void SetVolume(float volume)
        {
            Volume = volume;
        }
    }
}
