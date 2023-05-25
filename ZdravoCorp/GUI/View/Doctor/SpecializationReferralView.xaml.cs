﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SpecializationReferralView.xaml
    /// </summary>
    public partial class SpecializationReferralView : Window
    {
        public SpecializationReferralView(Appointment appointment)
        {
            InitializeComponent();
            cmbSpecialization.Visibility = Visibility.Hidden;
            cmbDoctor.Visibility = Visibility.Hidden;
            DataContext = new SpecializationReferralViewModel(appointment);
        }

        private void rbSpecialization_Checked(object sender, RoutedEventArgs e)
        {
            cmbDoctor.Visibility = Visibility.Hidden;
            cmbSpecialization.Visibility = Visibility.Visible;
            cmbEmpty.Visibility = Visibility.Hidden;
        }

        private void rbDoctor_Checked(object sender, RoutedEventArgs e)
        {
            cmbDoctor.Visibility = Visibility.Visible;
            cmbSpecialization.Visibility = Visibility.Hidden;
            cmbEmpty.Visibility = Visibility.Hidden;
        }
    }
}
