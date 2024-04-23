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
using System.Windows.Shapes;

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для EditTodoWindow.xaml
    /// </summary>
    public partial class EditTodoWindow : Window
    {
        private Todo todoControl;
        private string taskId;

        public EditTodoWindow(Todo todoControl, string taskId, string task, string description, string dueTime)
        {
            InitializeComponent();         
            this.todoControl = todoControl;
            this.taskId = taskId;
            TaskTextBox.Text = task;
            DescriptionTextBox.Text = description;
            
        }

        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newTask = TaskTextBox.Text;
            string newDescription = DescriptionTextBox.Text;
            
            DateTime? selectedDate = DueDatePicker.SelectedDate;
            string newDueTime = selectedDate.HasValue ? selectedDate.Value.ToString("dd.MM.yyyy") : null;
            todoControl.UpdateTaskInDatabase(taskId, newTask, newDescription, newDueTime);
            this.Close();
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
    }
}
