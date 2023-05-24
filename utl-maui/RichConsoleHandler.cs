#if IOS || MACCATALYST
using PlatformView = VideoDemos.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = VideoDemos.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = VideoDemos.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.maui
{
    public partial class RichConsoleHandler
    {
        public static IPropertyMapper<RichConsole, RichConsoleHandler> PropertyMapper = new(ViewHandler.ViewMapper)
        {
            [nameof(RichConsole.AutoScroll)] = MapAutoScroll,
            [nameof(RichConsole.BufferSize)] = MapBufferSize
        };
    }
}
