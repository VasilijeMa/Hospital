using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repository;
using ZdravoCorp.Core.PhysicalAssets.Service;
//using System.Windows.Forms;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for EquipmentUsedByDoctor.xaml
    /// </summary>
    public partial class EquipmentUsedByDoctor : Window
    {
        Appointment appointment { get; set; }
        public BindingList<AlteredEquipmentQuantity> AllQuantities { get; set; }

        public EquipmentUsedByDoctor(Appointment appointment)
        {
            DataContext = this;
            AllQuantities = new BindingList<AlteredEquipmentQuantity>();
            InitializeComponent();
            this.appointment = appointment;
            RefreshDataGrid();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            FunctionalItemRepository functionalItemRepository = new FunctionalItemRepository();
            List<FunctionalItem> functionalItems = functionalItemRepository.LoadAll();
            ItemCollection gridItems = TransferGrid.Items;
            foreach (AlteredEquipmentQuantity gridItem in gridItems)
            {
                foreach (FunctionalItem functionalItem in functionalItems)
                {
                    if (gridItem.GetName() == functionalItem.GetWhat() && appointment.IdRoom == functionalItem.GetWhere())
                    {
                        functionalItem.SetAmount(functionalItem.GetAmount() - gridItem.GetSelectedQuantity());
                        if (functionalItem.GetAmount() == 0)
                        {
                            functionalItems.Remove(functionalItem);
                        }
                        break;
                    }
                }
            }
            functionalItemRepository.SaveAll(functionalItems);
            MessageBox.Show("Equipment successfully removed from room.");
            this.Close();
        }

        private void RefreshDataGrid()
        {
            AllQuantities.Clear();
            Dictionary<string, EquipmentQuantity> equipmentOrganization = EquipmentRepository.LoadOnlyStaticOrDynamic(true);

            EquipmentRepository.LoadQuantitiesFromRoom(ref equipmentOrganization, appointment.IdRoom);

            foreach (EquipmentQuantity equipmentQuantity in equipmentOrganization.Values)
            {
                if (equipmentQuantity.Quantity > 0)
                {
                    AllQuantities.Add(new AlteredEquipmentQuantity(equipmentQuantity.GetName(), equipmentQuantity.GetQuantity()));
                }
            }
        }




    }
}
