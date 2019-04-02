using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessLayer;

namespace Aquapark
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Help_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                System.Diagnostics.Process.Start("help.chm");
            }
        }

        private void AvailableServicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_price.Text = TicketService.getPriceTicket(AvailableServicesListBox.SelectedValue.ToString()).First().price.ToString() + " zł";
            tb_order.Text = AvailableServicesListBox.SelectedValue.ToString();
        }

        private void AvailableServicesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in TicketService.getEntryTickets()) list.Add(i.entry);
            AvailableServicesListBox.ItemsSource = list;
        }

        private void Bt_buyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TicketService.addVisit(Convert.ToInt32(cb_saleIdRfid.SelectedValue), TicketService.getIdTicket(AvailableServicesListBox.SelectedValue.ToString()));
                MessageBox.Show("Dodano nową wizytę");
            }
            catch(System.NullReferenceException)
            {
                MessageBox.Show("Nie wybrano biletu");
            }
            catch(System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Nie wybrano zegarka");
            }
        }

        private void Bt_addClient_Click(object sender, RoutedEventArgs e)
        {
            ClientService.addClient(tb_orderName.Text, tb_orderSurname.Text, tb_clientPesel.Text);
            tb_orderName.Text = "";
            tb_orderSurname.Text = "";
            tb_clientPesel.Text = "";
            MessageBox.Show("Dodano Klienta");
        }

        private void Bt_orderPass_Click(object sender, RoutedEventArgs e)
        {
            if (ClientService.checkPass(tb_clientPesel.Text)) MessageBox.Show("Klient ma aktywny karnet");
            else
            {
                ClientService.addPass(tb_clientPesel.Text);
                MessageBox.Show("Dodano karnet na 30 dni");
                bt_enterPoolPass.Visibility = Visibility.Visible;
                label_idRFIDPass.Visibility = Visibility.Visible;
                cb_RFIDPass.Visibility = Visibility.Visible;
            }
        }
    
        private void Tb_clientPesel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tb_clientPesel.Text.Length < 11)
            {
                label_checkPesel.Content = "PESEL za krótki";
                label_passName.Visibility = Visibility.Hidden;
                label_passSurname.Visibility = Visibility.Hidden;
                tb_orderName.Visibility = Visibility.Hidden;
                tb_orderSurname.Visibility = Visibility.Hidden;
                bt_addClient.Visibility = Visibility.Hidden;
                bt_orderPass.Visibility = Visibility.Hidden;
                bt_enterPoolPass.Visibility = Visibility.Hidden;
                label_idRFIDPass.Visibility = Visibility.Hidden;
                cb_RFIDPass.Visibility = Visibility.Hidden;
            }
            else
            {
                if (ClientService.checkClient(tb_clientPesel.Text))
                {
                    label_checkPesel.Content = "Klient istnieje";
                    label_passName.Visibility = Visibility.Hidden;
                    label_passSurname.Visibility = Visibility.Hidden;
                    tb_orderName.Visibility = Visibility.Hidden;
                    tb_orderSurname.Visibility = Visibility.Hidden;
                    bt_addClient.Visibility = Visibility.Hidden;
                    bt_orderPass.Visibility = Visibility.Visible;

                    if (ClientService.checkPass(tb_clientPesel.Text))
                    {
                        bt_enterPoolPass.Visibility = Visibility.Visible;
                        label_idRFIDPass.Visibility = Visibility.Visible;
                        cb_RFIDPass.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        bt_enterPoolPass.Visibility = Visibility.Hidden;
                        label_idRFIDPass.Visibility = Visibility.Hidden;
                        cb_RFIDPass.Visibility = Visibility.Hidden;
                    } 
                }
                else
                {
                    label_checkPesel.Content = "Klient nie istnieje";
                    label_passName.Visibility = Visibility.Visible;
                    label_passSurname.Visibility = Visibility.Visible;
                    tb_orderName.Visibility = Visibility.Visible;
                    tb_orderSurname.Visibility = Visibility.Visible;
                    bt_addClient.Visibility = Visibility.Visible;
                    bt_orderPass.Visibility = Visibility.Hidden;
                    bt_enterPoolPass.Visibility = Visibility.Hidden;
                    label_idRFIDPass.Visibility = Visibility.Hidden;
                    cb_RFIDPass.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Bt_reportGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportService.makeChoice(lb_reports.SelectedValue, lb_reports, cal_reports.SelectedDates.First(), cal_reports.SelectedDates.Last());
                MessageBox.Show("Raport został wygenerowany");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("nie wybrano zakresu lub nie można wygenerować");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nie wybrano raportu");
            }
        }

        private void Bt_changePrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ModifyService.updateTicketPrice(lb_priceList.SelectedValue.ToString(), double.Parse(tb_prmodifyNewPrice.Text), cal_prmodifyWhenStart.SelectedDates.First(), cal_prmodifyWhenStart.SelectedDates.Last()))
                {
                    MessageBox.Show("Zaaktualizowano cenę");
                    tb_prmodifyNewPrice.Text = "";
                    tb_prmodifyOldPrice.Text = ModifyService.getPrice(lb_priceList.SelectedValue.ToString()).ToString();
                    gd_planned.ItemsSource = ModifyService.getTicketTimePeriods(lb_priceList.SelectedValue.ToString());
                }
                else MessageBox.Show("Błędna cena, data już zaplanowana lub z przeszłości"); 
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Nie wybrano zakresu obowiązywania biletu");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nie wybrano zakresu obowiązywania lub biletu");
            }
            catch (FormatException)
            {
                MessageBox.Show("Nie wybrano nowej ceny");
            }     
        }

        private void Bt_finalSummary_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zapłacono " + tb_finalPrice.Text);
            tb_finalPrice.Text = "";
            List<string> empty = new List<string>();
            lb_finalServices.ItemsSource = empty;
            cb_finalRFID.ItemsSource = empty;
        } 

        private void Gd_RFID_Loaded(object sender, RoutedEventArgs e)
        {
            gd_RFID.ItemsSource = RFIDService.getAllRFIDWatch();
        }

        private void Cb_saleIdRfid_DropDownOpened(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in TicketService.getAllRFIDWatchID()) list.Add(i.id.ToString());
            cb_saleIdRfid.ItemsSource = list;
        }

        private void Lb_priceList_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in ModifyService.getEntry()) list.Add(i.entry);
            lb_priceList.ItemsSource = list;
        }

        private void Lb_priceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bt_changePrice.Visibility = Visibility.Visible;
            bt_changePriceAttraction.Visibility = Visibility.Hidden;
            if (lb_priceList.SelectedValue != null)
            tb_prmodifyOldPrice.Text = ModifyService.getPrice(lb_priceList.SelectedValue.ToString()).ToString();
            gd_planned.ItemsSource = ModifyService.getTicketTimePeriods(lb_priceList.SelectedValue.ToString());
        }

        private void Bt_changeStatusRFID_Click(object sender, RoutedEventArgs e)
        {
            IList<object> selected = new List<object>();
            foreach(var i in gd_RFID.SelectedItems)
            {
                selected.Add(i);
            }

            foreach(var i in selected)
            {
                RFIDService.changeStatus(Convert.ToInt32(i.GetType().GetProperty("id").GetValue(i, null)));
            }

            Gd_RFID_Loaded(sender, e);
        }

        private void Bt_addRFID_Click(object sender, RoutedEventArgs e)
        {
            RFIDService.insRFID();
            Gd_RFID_Loaded(sender, e);
        }

        private void Lb_priceAttraction_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in ModifyService.getAttractionName()) list.Add(i.name);
            lb_priceAttraction.ItemsSource = list;
        }

        private void Lb_priceAttraction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bt_changePrice.Visibility = Visibility.Hidden;
            bt_changePriceAttraction.Visibility = Visibility.Visible;
            if (lb_priceAttraction.SelectedValue != null)
            tb_prmodifyOldPrice.Text = ModifyService.getPriceAttraction(lb_priceAttraction.SelectedValue.ToString()).ToString();
            gd_planned.ItemsSource = ModifyService.getAttractionTimePeriods(lb_priceAttraction.SelectedValue.ToString());
        }

        private void Bt_changePriceAttraction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ModifyService.updatePriceAttraction(lb_priceAttraction.SelectedValue.ToString(), double.Parse(tb_prmodifyNewPrice.Text), cal_prmodifyWhenStart.SelectedDates.First(), cal_prmodifyWhenStart.SelectedDates.Last()))
                {
                    MessageBox.Show("Zaaktualizowano cenę atrakcji");
                    tb_prmodifyNewPrice.Text = "";
                    tb_prmodifyOldPrice.Text = ModifyService.getPriceAttraction(lb_priceAttraction.SelectedValue.ToString()).ToString();
                    gd_planned.ItemsSource = ModifyService.getAttractionTimePeriods(lb_priceAttraction.SelectedValue.ToString());
                }
                else MessageBox.Show("Błędna cena, data już zaplanowana lub z przeszłości");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Nie wybrano zakresu obowiązywania");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nie wybrano zakresu obowiązywania lub atrakcji");
            }
            catch (FormatException)
            {
                MessageBox.Show("Nie wybrano nowej ceny");
            }
        }

        private void Bt_enterPoolPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientService.addVisitPass(Convert.ToInt32(cb_RFIDPass.SelectedValue), ClientService.getPassID(tb_clientPesel.Text));
                MessageBox.Show("Posiadacz karnetu wszedł do aquaparku");
            }          
            catch(System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Nie wybrano zegarka");
            }
        }

        private void Cb_RFIDPass_DropDownOpened(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in ClientService.getAllRFIDWatchIDPass()) list.Add(i.id.ToString());
            cb_RFIDPass.ItemsSource = list;
        }

        private void Cb_gateRFID_DropDownOpened(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in VisitService.getInsideRFID()) list.Add(i.idWatch.ToString());
            cb_gateRFID.ItemsSource = list;
        }

        private void Cb_gateID_DropDownOpened(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in VisitService.getGatesID()) list.Add(i.id.ToString());
            cb_gateID.ItemsSource = list;
        }

        private void Bt_gate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VisitService.insertGateEntering(Convert.ToInt32(cb_gateID.SelectedValue), Convert.ToInt32(cb_gateRFID.SelectedValue));
                MessageBox.Show("Przejście przez bramkę " + cb_gateID.SelectedValue.ToString());
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Nie wybrano RFID");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Nie wybrano ID bramki");
            }
        }

        private void Cb_finalRFID_DropDownOpened(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var i in SettlementService.getInsideRFID()) list.Add(i.idWatch.ToString());
            cb_finalRFID.ItemsSource = list;
        }

        private void Bt_finalScan_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            List<string> list = new List<string>();
            foreach (var i in SettlementService.calculateCost(Convert.ToInt32(cb_finalRFID.SelectedValue), now).Item1) list.Add(i.ToString());
            lb_finalServices.ItemsSource = list;
            tb_finalPrice.Text = SettlementService.calculateCost(Convert.ToInt32(cb_finalRFID.SelectedValue), now).Item2.ToString();
            SettlementService.exitAquapark(Convert.ToInt32(cb_finalRFID.SelectedValue), now);
            if (tb_finalPrice.Text == "-1")
            {
                tb_finalPrice.Text = "";
                MessageBox.Show("Błąd w wyborze RFID");
            }
        }
    }
}
