using UnityEngine;

namespace GPCommon
{
    public class PropertyStr
    {
        private readonly string _content;

        public PropertyStr(string name, object value)
        {
            _content = Build(name, value);
        }

        private PropertyStr(string content)
        {
            _content = content;
        }

        public PropertyStr And(string name, object value)
        {
            return new PropertyStr($"{_content}, {Build(name, value)}");
        }

        private string Build(string name, object value)
        {
            return $"{name} = {value}";
        }

        public override string ToString()
        {
            return _content;
        }

        public static implicit operator string(PropertyStr input)
        {
            return input.ToString();
        }
    }
    public enum PerformanceRating
    {
        High,
        Middle,
        Low,
    }

    public static class DeviceEnvironment
    {
        private static int _performanceScore;

        public static int PerformanceScore
        {
            get { return _performanceScore; }
            private set
            {
                _performanceScore = value;

                if (_performanceScore >= 6)
                {
                    PerformanceRating = _performanceScore >= 9 ? PerformanceRating.High : PerformanceRating.Middle;
                }
                else
                {
                    PerformanceRating = PerformanceRating.Low;
                }
            }
        }

        public static PerformanceRating PerformanceRating { get; private set; }

        public static string GetOpenGLVersion()
        {
            var str = SystemInfo.graphicsDeviceVersion;
            return str.Contains("OpenGL ES") ? str : "0";
        }

        public static string GetSummary()
        {
            return
                new PropertyStr("deviceModel", SystemInfo.deviceModel).And("deviceName", SystemInfo.deviceName)
                    .And("processorType", SystemInfo.processorType).And("processorCount", SystemInfo.processorCount)
                    .And("systemMemorySize", SystemInfo.systemMemorySize)
                    .And("graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor)
                    .And("graphicsDeviceName", SystemInfo.graphicsDeviceName)
                    .And("graphicsDeviceVersion", SystemInfo.graphicsDeviceVersion)
                    .And("graphicsMemorySize", SystemInfo.graphicsMemorySize)
                    .And("graphicsShaderLevel", SystemInfo.graphicsShaderLevel)
                    .And("maxTextureSize", SystemInfo.maxTextureSize).And("PerformanceScore", PerformanceScore)
                    .And("PerformanceRating", PerformanceRating)
                    .And("supportsImageEffects", SystemInfo.supportsImageEffects);
        }

        public static void CheckPerformance()
        {
#if UNITY_IPHONE
            // https://browser.geekbench.com/ios-benchmarks
            switch (UnityEngine.iOS.Device.generation)
            {
                case UnityEngine.iOS.DeviceGeneration.iPhone5S: // 1268 A7
                case UnityEngine.iOS.DeviceGeneration.iPadMini3Gen: // 1247 A7
                case UnityEngine.iOS.DeviceGeneration.iPadAir1: // 1328 A7
                    PerformanceScore = 5;
                    break;

                case UnityEngine.iOS.DeviceGeneration.iPhone6: // 1422 A8
                case UnityEngine.iOS.DeviceGeneration.iPhone6Plus: // 1460 A8
                case UnityEngine.iOS.DeviceGeneration.iPodTouch6Gen: // 1330 A8
                    PerformanceScore = 6;
                    break;
                case UnityEngine.iOS.DeviceGeneration.iPadMini4Gen: // Apple A8 @ 1.5 GHz 1658
                case UnityEngine.iOS.DeviceGeneration.iPadAir2: // Apple A8X @ 1.5 GHz 1795
                    PerformanceScore = 7;
                    break;

                default:
                    PerformanceScore = 10;
                    break;
            }

#elif UNITY_ANDROID
            var processorCount = SystemInfo.processorCount;
            var systemMemorySize = SystemInfo.systemMemorySize;

            if (processorCount >= 4 && systemMemorySize >= 1500)
            {
                if (processorCount >= 8 || systemMemorySize >= 5000)
                    PerformanceScore = 9;
                else
                    PerformanceScore = 6;
            }
            else
            {
                PerformanceScore = 4;
            }
#else
            PerformanceScore = 10;
#endif
        }
    }
}