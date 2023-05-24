using Microsoft.Maui.Handlers;

namespace utl_maui
{
    // All the code in this file is only included on Windows.
    public partial class RichConsoleHandler : ViewHandler<RichConsoleHandler, RichConsole_Windows>
    {
        protected override RichConsole_Windows CreatePlatformView()
        {
            throw new NotImplementedException();
        }
        protected override void ConnectHandler(RichConsole_Windows platformView)
        {
            base.ConnectHandler(platformView);
        }
        protected override void DisconnectHandler(RichConsole_Windows platformView)
        {
            base.DisconnectHandler(platformView);
        }
    }
    public class RichConsole_Windows { }
}