using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5
using UnityEngine.Profiling;
#endif

public class MemoryStats : MonoBehaviour
{
    StringBuilder stringBuilder;
    
    float interval = 0.5f;
    float updateInterval = 1.0f;
    float lastInterval; // Last interval end time
    float frames = 0; // Frames over current interval
    float framesavtick = 0;
    float framesav = 0.0f;
    bool enabled = false;
    float timeNow;
    float fps;
    float ms;
    float fpsav;
    float TotalAllocatedMemory;
    float GetTotalReservedMemory;
    float GetTotalUnusedReservedMemory;
    float GetMonoHeapSize;
    float GetMonoUsedSize;
    float Cached_TotalAllocatedMemory;
    float Cached_GetTotalReservedMemory;
    float Cached_GetMonoHeapSize;
    float Cached_GetMonoUsedSize;

//ASSIGN YOUR UI TEXT OBJECTS HERE:

    public Text gui;
    public Text gui_cached;
    public Text gui_video;

    void OnEnable()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        framesav = 0;
        stringBuilder = new StringBuilder(255, 1024);

        gui_video.text = "VID MEM: " + SystemInfo.graphicsMemorySize.ToString() + " mb" + "\n" + "SYS MEM: " + SystemInfo.systemMemorySize.ToString() + " mb";

        enabled = true;
    }


    void OnDisable()
    {
        gui.text = "";
        gui_cached.text = "";
        enabled = false;
    }


    void Update()
    {
        if (enabled == true)
        {
            ++frames;
            
            timeNow = Time.realtimeSinceStartup;
            if (timeNow > lastInterval + updateInterval)
            {
                TotalAllocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1048576;
                GetTotalReservedMemory = (UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1048576);
                GetTotalUnusedReservedMemory = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1048576;
                fps = frames / (timeNow - lastInterval);
                ms = 1000.0f / Mathf.Max(fps, 0.00001f);
                ++framesavtick;
                framesav += fps;
                fpsav = framesav / framesavtick;
                stringBuilder.Length = 0;

                stringBuilder.AppendFormat("<b>{1} FPS</b> \n\n", Mathf.Round(ms), Mathf.Round(fps), Mathf.Round(fpsav), SystemInfo.graphicsMemorySize, SystemInfo.systemMemorySize)
                .AppendFormat("MEM ALLOC : {0} mb  /  {1} mb (-{2}mb)",
                TotalAllocatedMemory,
                GetTotalReservedMemory,
                GetTotalUnusedReservedMemory);

                frames = 0;
                lastInterval = timeNow;
            }

            gui.text = stringBuilder.ToString();
        }
    }


    string unityEditorOnly;
    public void CacheStats(string fromScene = "")
    {
        if (enabled == true)
        {
            Cached_TotalAllocatedMemory = TotalAllocatedMemory;
            Cached_GetTotalReservedMemory = GetTotalReservedMemory;
            Cached_GetMonoHeapSize = GetMonoHeapSize;
            Cached_GetMonoUsedSize = GetMonoUsedSize;
            
            if (fromScene != "")
            {
                fromScene = "LAST SCENE : " + fromScene;
            }

            if (enabled == true)
            {
                gui_cached.text = fromScene + " - MEM ALLOC : " + Cached_TotalAllocatedMemory.ToString() + " mb  /  " + Cached_GetTotalReservedMemory.ToString() + " mb";
            }
        }
    }
}
