using System.Windows;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.GUI.CommunicationSystem.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for ChatsView.xaml
    /// </summary>
    public partial class ChatsView : Window
    {
        public ChatsView(User user, ChatService chatService)
        {
            InitializeComponent();
            DataContext = new ChatsViewModel(user, chatService);
        }
    }
}
