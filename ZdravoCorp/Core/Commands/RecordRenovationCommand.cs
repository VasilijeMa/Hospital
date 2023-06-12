using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class RecordRenovationCommand : BaseCommand
    {
        private RenovationViewModel viewModel;

        public RecordRenovationCommand(RenovationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (!viewModel.ShouldRunCommands)
            {
                return true;
            }
            string error = viewModel.ValidateConditions();
            if (error != null)
            {
                MessageBox.Show(error);
            }
            return error == null;
        }

        public override void Execute(object? parameter)
        {
            if (!viewModel.ShouldRunCommands)
            {
                return;
            }
            viewModel.SaveRenovation();
            MessageBox.Show("Renovation successfully recorded.");
        }
    }
}