using Microsoft.Maui.Controls;
using d9.utl;

namespace d9.utl.maui;
public class RichConsole : View, IConsole
{
    public static readonly BindableProperty AutoScrollProperty
        = BindableProperty.Create(nameof(AutoScroll), typeof(bool), typeof(RichConsole), true);
    public bool AutoScroll
    {
        get => (bool)GetValue(AutoScrollProperty);
        set => SetValue(AutoScrollProperty, value);
    }
    public static readonly BindableProperty BufferSizeProperty
        = BindableProperty.Create(nameof(BufferSize), typeof(int), typeof(RichConsole), 8192);
    public int BufferSize
    {
        get => (int)GetValue(BufferSizeProperty);
        set => SetValue(BufferSizeProperty, value);
    }
    public void Write(object? obj) => throw new NotImplementedException();
    public void WriteLine(object? obj) => throw new NotImplementedException();
}