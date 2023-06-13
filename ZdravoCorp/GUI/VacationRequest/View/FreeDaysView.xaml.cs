using System.Windows;
using ZdravoCorp.GUI.VacationRequest.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for FreeDaysView.xaml
    /// </summary>
    public partial class FreeDaysView : Window
    {
        public FreeDaysView(Core.Domain.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new FreeDaysViewModel(doctor);
        }
    }
}
