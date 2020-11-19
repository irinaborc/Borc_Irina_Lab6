using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using static Borc_Irina_Lab6.Validation;

namespace Borc_Irina_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource customerOrdersViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource inventoryOrdersViewSource;
        Binding firstNameTextBoxBinding = new Binding();
        Binding lastNameTextBoxBinding = new Binding();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            firstNameTextBoxBinding.Path = new PropertyPath("FirstName");
            lastNameTextBoxBinding.Path = new PropertyPath("LastName");
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            customerOrdersViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            inventoryViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            inventoryOrdersViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryOrdersViewSource")));

            customerOrdersViewSource.Source = ctx.Orders.Local;
            
            inventoryOrdersViewSource.Source = ctx.Orders.Local;


            ctx.Customers.Load();
            ctx.Orders.Load();
            ctx.Inventories.Load();
            cmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            //cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";
            BindDataGrid();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    
                    //instantiem Customer entity
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
            }
            else
                if (action == ActionState.Edit)
            {
                try
                {
                    

                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

                    


            }
            else if (action == ActionState.Delete)
            {
                string tempFirstName = firstNameTextBox.Text.ToString();
                string tempLastName = lastNameTextBox.Text.ToString();
                BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
                firstNameTextBox.Text = tempFirstName;
                lastNameTextBox.Text = tempLastName;
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                    
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnSaveO_Click(object sender, RoutedEventArgs e)
        {


            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true; 
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;
            }


            if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                    editedOrder.CarId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        //salvam modificarile
                        customerOrdersViewSource.View.Refresh();
                        customerOrdersViewSource.View.MoveCurrentTo(order);

                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                customerViewSource.View.MoveCurrentTo(selectedOrder);

                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;
               
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerOrdersViewSource.View.Refresh();
                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;
               

            }

        }

        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.CarId
                 equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }
        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty,
           firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty,
           lastNameValidationBinding); //setare binding nou
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;

            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            SetValidationBinding();
          
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {

                    
                    inventory = new Inventory()
                    {
                        Make = makeTextBox.Text.Trim(),
                        Color = colorTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
            }
            else
                if (action == ActionState.Edit)
            {
                try
                {


                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Make = makeTextBox.Text.Trim();
                    inventory.Color = colorTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;




            }
            else if (action == ActionState.Delete)
            {
                string tempMake = makeTextBox.Text.ToString();
                string tempColor = colorTextBox.Text.ToString();
                BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
                makeTextBox.Text = tempMake;
                colorTextBox.Text = tempColor;
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;

            

        }
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;

            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevious1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            makeTextBox.Text = "";
            colorTextBox.Text = "";
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevious1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;

            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
            SetValidationBinding();
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();
            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevious1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;




        }

        private void btnNewO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;

            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
           

        }

        private void btnDeleteO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            
            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;
            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
            
        }

        private void btnEditO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
           

            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;
            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
            

            
            SetValidationBinding();
        }
    }
}
