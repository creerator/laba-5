using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KALKYLATOP
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<OperationRecord> Operations { get; } = new ObservableCollection<OperationRecord>();

        public MainWindow()
        {
            InitializeComponent();
            LogListView.ItemsSource = Operations;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessCommand(InputTextBox.Text);
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessCommand(InputTextBox.Text);
            }
        }

        private void ProcessCommand(string command)
        {
            object resultObj = null;
            bool success = true;
            string result = string.Empty;

            try
            {
                command = command.Replace(" ", string.Empty);
                var dataTable = new DataTable();
                resultObj = dataTable.Compute(command, "");

                if (resultObj is double || resultObj is int || resultObj is long ||
                    resultObj is decimal || resultObj is float || resultObj is short ||
                    resultObj is byte || resultObj is sbyte)
                {
                    result = Convert.ToString(resultObj);
                }
                else
                {
                    throw new InvalidOperationException("Invalid operation or data type.");
                }
            }
            catch (Exception ex)
            {
                success = false;
                result = ex.Message;
            }

            LogOperation(command, result, success);
            InputTextBox.Clear();
            InputTextBox.Focus();
        }

        private void LogListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is OperationRecord selectedItem && selectedItem.Success)
            {
                InputTextBox.Text = selectedItem.Result;
                InputTextBox.Focus();
                InputTextBox.SelectAll();
            }
        }



        private void LogOperation(string operation, string result, bool success)
        {
            Operations.Add(new OperationRecord
            {
                Operation = operation,
                Result = result,
                Success = success
            });
        }

        public double CurrentTotal { get; set; } = 0;

        public class OperationRecord
        {
            public string Operation { get; set; }
            public string Result { get; set; }
            public bool Success { get; set; }
        }
    }
}