using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для AddTodo.xaml
    /// </summary>
    public partial class AddTodo : Window
    {
        public AddTodo()
        {
            InitializeComponent();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранной даты
            DateTime? selectedDate = (sender as DatePicker).SelectedDate;

            // Проверка, была ли дата выбрана
            if (selectedDate.HasValue)
            {
                // Обработка выбранной даты
                // Например, вывод выбранной даты в консоль
                string duedate = selectedDate.Value.ToString("dd.MM.yyyy");
            }
        }

        public delegate void SaveEventHandler(string task, string description, string dueDate);
        
        public event SaveEventHandler OnSave;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string task = "Task : " + Task.Text;
            string descption = "Description : " + Descption.Text;
            DateTime? selectedDate = DueDatePicker.SelectedDate;
            string dueDate = selectedDate.HasValue ? selectedDate.Value.ToString("dd.MM.yyyy") : null;
            dueDate = "Expire : " + dueDate;

            if (task !=null & descption != null & dueDate != null)
            {
                OnSave?.Invoke(task, descption, dueDate);
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }
    }
}
