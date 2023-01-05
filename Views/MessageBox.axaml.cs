using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace RLCClient.Views
{
    public partial class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo    
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static Task<MessageBoxResult> Show(Window parent, string message, MessageBoxButtons buttons = MessageBoxButtons.OkCancel, string title = "Message Box")
        {
            MessageBoxResult res = MessageBoxResult.Cancel;

            var msgbox = new MessageBox()
            {
                Title = title
            };

            msgbox.FindControl<TextBlock>("Title_TextBox").Text = title;
            msgbox.FindControl<TextBlock>("Message").Text = message;

            if(buttons == MessageBoxButtons.Ok)
            {
                Grid.SetColumn(msgbox.FindControl<Button>("One_Button"), 2);
                msgbox.FindControl<Button>("One_Button").IsVisible = true;
                msgbox.FindControl<Button>("One_Button").Content = "Ok";
            }
            else if(buttons == MessageBoxButtons.OkCancel)
            {
                msgbox.FindControl<Button>("One_Button").IsVisible = true;
                msgbox.FindControl<Button>("Two_Button").IsVisible = true;
                msgbox.FindControl<Button>("One_Button").Content = "Ok";
                msgbox.FindControl<Button>("Two_Button").Content = "Cancel";

            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                msgbox.FindControl<Button>("One_Button").IsVisible = true;
                msgbox.FindControl<Button>("Two_Button").IsVisible = true;
                msgbox.FindControl<Button>("One_Button").Content = "Yes";
                msgbox.FindControl<Button>("Two_Button").Content = "No";

            }

            msgbox.FindControl<Button>("One_Button").Click += (_, __) =>
            {
                if(buttons == MessageBoxButtons.OkCancel)
                {
                    res = MessageBoxResult.Ok;
                }
                else if(buttons == MessageBoxButtons.YesNo)
                {
                    res = MessageBoxResult.Yes;
                }
                msgbox.Close();
            };
            msgbox.FindControl<Button>("Two_Button").Click += (_, __) =>
            {
                if (buttons == MessageBoxButtons.OkCancel)
                {
                    res = MessageBoxResult.Cancel;

                }
                else if(buttons == MessageBoxButtons.YesNo)
                {
                    res = MessageBoxResult.No;
                }
                msgbox.Close();
            };
            msgbox.FindControl<Button>("ButtonClose").Click += (_, __) =>
            {
                if (buttons == MessageBoxButtons.OkCancel)
                {
                    res = MessageBoxResult.Cancel;
                }
                else if (buttons == MessageBoxButtons.YesNo) 
                {
                    res = MessageBoxResult.No;
                }
                msgbox.Close();
            };

            var tcs = new TaskCompletionSource<MessageBoxResult>();
            msgbox.Closed += delegate { tcs.TrySetResult(res); };

            msgbox.ShowDialog(parent);

            return tcs.Task;
        }

        #region TitleLabel
        public void BeginMoveDrag(object? sender, PointerPressedEventArgs e)
        {
            PlatformImpl?.BeginMoveDrag(e);
        }
        #endregion
    }
}
