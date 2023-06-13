using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.DoctorPatientSearch.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SearchDoctorWindow.xaml
    /// </summary>
    public partial class SearchDoctorWindow : Window
    {
        public SearchDoctorWindow(Patient patient)
        {
            InitializeComponent();
            DataContext = new SearchDoctorViewModel(patient);
        }
    }
}
