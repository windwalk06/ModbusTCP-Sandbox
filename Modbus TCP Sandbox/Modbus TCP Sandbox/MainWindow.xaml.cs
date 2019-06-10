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
using EasyModbus;

namespace Modbus_TCP_Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ModbusClient modbusClient;
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool ConnectToSlave(string IP)
        {
            bool ToReturn = false;
            try
            {
                modbusClient = new ModbusClient(IP, 502);    //Ip-Address and Port of Modbus-TCP-Server
                modbusClient.Connect();
                ToReturn = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return ToReturn;
        }

        //private void btn_Read_Click(object sender, RoutedEventArgs e)
        //{
            
        //}

        private void btn_WriteRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteRegister(int.Parse(tb_Address.Text), int.Parse(tb_Value.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteRegister(int address, int value)
        {
            if (ConnectToSlave(tb_IPAddress.Text))
            {
                address = address - 1;
                modbusClient.WriteSingleRegister(address, value);
                modbusClient.Disconnect();
            }
        }

        private void btn_Read_Click_1(object sender, RoutedEventArgs e)
        {
            if (ConnectToSlave(tb_IPAddress.Text))
            {
                int Top = 0;
                int Bottom = 0;
                int startingReg = int.Parse(tb_StartingReg.Text);
                int endingReg = int.Parse(tb_EndingReg.Text);
                int regCount = endingReg - startingReg;
                lbResults.Items.Clear();
                int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(startingReg, regCount);
                //int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(40005, 2);
                // bool[] readCoils = modbusClient.ReadCoils(6, 1);
                for (int i = 0; i < readHoldingRegisters.Count(); i++)
                {
                    lbResults.Items.Add((startingReg + i + 1).ToString() + " - " + readHoldingRegisters[i].ToString());//the old off by one game....
                    if (startingReg + i + 1 == 40007)
                    {
                        lbl_temptest.Content = readHoldingRegisters[i].ToString();
                    }
                }

                modbusClient.Disconnect();
                //0101110000101001 0100001010010001
            }
            else
            {
                lbResults.Items.Clear();
                lbResults.Items.Add("No Connection, Please Hang Up And Try Your Call Again...");
            }
        }

        float combinedIntsToFloat(int upper, int lower)
        {
            Int32 top = upper << 16;
            Int32 bottom = lower;
            return (float)(top | bottom);
        }

        /*
         while (true)
            {
                if (ConnectToSlave("192.168.0.157"))
                {
                    int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(40000, 20);
                    //int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(40005, 2);
                    // bool[] readCoils = modbusClient.ReadCoils(6, 1);
                    lbl_Response.Content = "Register1: " + readHoldingRegisters[0].ToString() + "\t" + "register2: " + readHoldingRegisters[1].ToString();
                    
                    modbusClient.Disconnect();
                }
                else
                {
                    lbl_Response.Content = "Failed to connect";
                    Thread.Sleep(2000);
                }
                Thread.Sleep(500);
            }

        public delegate void UpdateTextCallback(string message);

        lbl_status.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                                   new Action(delegate ()
                                   {
                                       lbl_status.Content = toupdate; 
                                   }));
         */
    }
}
