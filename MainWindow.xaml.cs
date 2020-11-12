using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using AutoLotModel;
using System.Data.Entity;


namespace Simion_Sabina_Lab6
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
        AutoLotEntitiesModel itx = new AutoLotEntitiesModel();
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();

            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = itx.Inventories.Local;
            itx.Inventories.Load();
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            customerOrdersViewSource.Source = ctx.Orders.Local;

            ///System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            ctx.Orders.Load();
            itx.Inventories.Load();
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
            cmbCustomers.ItemsSource = ctx.Customers.Local;
            cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";
            BindDataGrid();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
           
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            custIdTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            custIdTextBox.Text = "";
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            Keyboard.Focus(custIdTextBox);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetValidationBinding();
            action = ActionState.Edit;
            string tempCustId = custIdTextBox.Text.ToString();
            string tempFirstName =firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            custIdTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            custIdTextBox.Text = tempCustId;
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            Keyboard.Focus(custIdTextBox);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tempCustId = custIdTextBox.Text.ToString();
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            custIdTextBox.Text = tempCustId; 
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SetValidationBinding();
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
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
                catch (DataException ex)
                {
                   
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
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
                
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                custIdTextBox.SetBinding(TextBox.TextProperty, custIdTextBox.Text.ToString());
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBox.Text.ToString());
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBox.Text.ToString());
            }
            else
if (action == ActionState.Delete)
            {
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
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                custIdTextBox.SetBinding(TextBox.TextProperty, custIdTextBox.Text.ToString());
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBox.Text.ToString());
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBox.Text.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            
            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
            custIdTextBox.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            custIdTextBox.SetBinding(TextBox.TextProperty, custIdTextBox.Text.ToString());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBox.Text.ToString());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBox.Text.ToString());
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnNewI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            carIdTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            carIdTextBox.Text = "";
            makeTextBox.Text = "";
            colorTextBox.Text = "";
            Keyboard.Focus(carIdTextBox);
        }

        private void btnEditI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempCarId = carIdTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            carIdTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            carIdTextBox.Text = tempCarId;
            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
            Keyboard.Focus(custIdTextBox);
        }

        private void btnDeleteI_Click(object sender, RoutedEventArgs e)
        {
            string tempCarId = carIdTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            carIdTextBox.Text = tempCarId;
            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
            
        }

        private void btnSaveI_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Make = firstNameTextBox.Text.Trim(),
                        Color = lastNameTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    itx.Inventories.Add(inventory);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    itx.SaveChanges();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                carIdTextBox.IsEnabled = false;
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
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(inventory);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                carIdTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
                carIdTextBox.SetBinding(TextBox.TextProperty, carIdTextBox.Text.ToString());
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBox.Text.ToString());
                colorTextBox.SetBinding(TextBox.TextProperty, colorTextBox.Text.ToString());
            }
            else
if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    itx.Inventories.Remove(inventory);
                    itx.SaveChanges();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                carIdTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
                carIdTextBox.SetBinding(TextBox.TextProperty, carIdTextBox.Text.ToString());
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBox.Text.ToString());
                colorTextBox.SetBinding(TextBox.TextProperty, colorTextBox.Text.ToString());
            }
        }

        private void btnNewO_Click(object sender, RoutedEventArgs e)
        {
             action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            cmbCustomers.IsEnabled = true;
            cmbInventory.IsEnabled = true;
            
            BindingOperations.ClearBinding(cmbCustomers, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbInventory, TextBox.TextProperty);

            cmbCustomers.Text = "";
            cmbInventory.Text = "";
            
            Keyboard.Focus(cmbCustomers);
        }

        private void btnEditO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempCustomers = cmbCustomers.Text.ToString();
            string tempInventory = cmbInventory.Text.ToString();
            
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            cmbCustomers.IsEnabled = true;
            cmbInventory.IsEnabled = true;
            
            BindingOperations.ClearBinding(cmbCustomers, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbInventory, TextBox.TextProperty);

            cmbCustomers.Text = tempCustomers;
            cmbInventory.Text = tempInventory;
            
            Keyboard.Focus(cmbCustomers);
        }

        private void btnDeleteO_Click(object sender, RoutedEventArgs e)
        {
            string tempCustomers = cmbCustomers.Text.ToString();
            string tempInventory = cmbInventory.Text.ToString();
            
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(cmbCustomers, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbInventory, TextBox.TextProperty);

            cmbCustomers.Text = tempCustomers;
            cmbInventory.Text = tempInventory;
            
        }

        private void btnSaveO_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory invent = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {
                        CustId = customer.CustId,
                        CarId = invent.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    itx.SaveChanges();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                carIdTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
            }
            else
if (action == ActionState.Edit)
            {
                dynamic selectedOrder = orderDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                    editedOrder.CarId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = orderDataGrid.SelectedItem;
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
            }
        }

        private void btnCancelO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;

            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
            cmbCustomers.IsEnabled = false;
            cmbInventory.IsEnabled = false;

            cmbCustomers.SetBinding(TextBox.TextProperty, cmbCustomers.Text.ToString());
            cmbInventory.SetBinding(TextBox.TextProperty, cmbInventory.Text.ToString());
            
        }

        private void btnPrevO_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNextO_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
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
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding); //setare binding nou
        }
    }
  
}
