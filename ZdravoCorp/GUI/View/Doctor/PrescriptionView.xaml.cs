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
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for PrescriptionView.xaml
    /// </summary>
    public partial class PrescriptionView : Window
    {
        public PrescriptionView(Appointment appointment)
        {
            InitializeComponent();
            DataContext = new PrescriptionViewModel(appointment);
        }
    }
}