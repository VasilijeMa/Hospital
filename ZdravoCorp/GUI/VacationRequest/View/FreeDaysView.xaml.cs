using System.Windows;
using ZdravoCorp.Core.VacationRequest.Services;
using ZdravoCorp.GUI.VacationRequest.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for FreeDaysView.xaml
    /// </summary>
    public partial class FreeDaysView : Window
    {
        public FreeDaysView(Core.Domain.Doctor doctor, FreeDaysService freeDaysService)
        {
            InitializeComponent();
            DataContext = new FreeDaysViewModel(doctor, freeDaysService);
        }
    }
}
