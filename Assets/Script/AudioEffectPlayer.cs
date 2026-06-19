using UnityEngine;

[RequireComponent(typeof(AudioSource))] // 确保挂载此脚本的物体上必须有 AudioSource 组件
public class AudioEffectPlayer : MonoBehaviour
{
    // 静态单例实例，供全局直接访问
    public static AudioEffectPlayer Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        // 确保场景中只有一个音效播放中心（单例初始化）
        if (Instance == null)
        {
            Instance = this;
            // 如果跨场景不需要销毁，可以取消注释下面这行
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 获取自身的 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        
        // 经典音效设置优化
        audioSource.playOnAwake = false; // 游戏启动时不自动播放
        audioSource.loop = false;        // 关闭循环播放
    }

    /// <summary>
    /// 全局公开方法：供外界任意脚本调用，播放一次指定的音效
    /// </summary>
    /// <param name="clip">音频片段 (AudioClip)</param>
    /// <param name="volume">音量大小 (0.0 到 1.0)</param>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("尝试播放音效，但传入的 AudioClip 为空！");
            return;
        }

        if (audioSource == null) return;

        // 使用 PlayOneShot 的绝对优势：
        // 即使前一个音效没播完，再次调用时也会重叠播放，不会掐断前一个声音
        audioSource.PlayOneShot(clip, volume);
    }
}