using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.CommunicationSystem.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for ChatsView.xaml
    /// </summary>
    public partial class ChatsView : Window
    {
        public ChatsView(User user)
        {
            InitializeComponent();
            DataContext = new ChatsViewModel(user);
        }
    }
}
