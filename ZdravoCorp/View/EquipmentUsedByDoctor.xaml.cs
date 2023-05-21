using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
using ZdravoCorp.Domain;
using ZdravoCorp.EquipmentGroup;
using ZdravoCorp.InfrastructureGroup;

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
            List<FunctionalItem> functionalItems = FunctionalItemRepository.LoadAll();
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
            FunctionalItemRepository.SaveAll(functionalItems);
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
