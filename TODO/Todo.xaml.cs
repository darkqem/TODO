using MongoDB.Bson;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using static TODO.MainWindow;
using static TODO.Todo;
using Microsoft.VisualBasic;

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для Todo.xaml
    /// </summary>
    public partial class Todo : UserControl
    {
        private object todoItem;
        private List<List<string>> todoItem1;



        public int Count { get; }
        public MongoClient Client { get; }
        public string UserName { get; set; }
        public string Id { get; set; }
        private TodoList todoList;

        public class TodoItem
        {
            public string Task { get; set; }
            public string Description { get; set; }
            public string IsDone { get; set; }
            public string Expire { get; set; }
            public string TaskId { get; set; }

        }



        public Todo(object todoItem, string name, MongoClient client, TodoList todoList)
        {
            InitializeComponent();
            UserName = name;
            this.Client = client;
            this.todoList = todoList;
            var collection = todoItem.ToJson();

            var todoArray = JsonConvert.DeserializeObject<List<string>>(collection);

            var todoItems = new TodoItem
            {
                Task = todoArray[0].Split(": ")[1],
                Description = todoArray[1].Split(": ")[1],
                IsDone = todoArray[2].Split(": ")[1],
                Expire = todoArray[3].Split(": ")[1],
                TaskId = todoArray[4]
            };

            var task = todoArray[0].Replace("Task : ", "");
            var descption = todoArray[1].Replace("Description : ", "");
            var done = todoArray[2].Replace("Done : ", "");
            var duetime = todoArray[3].Replace("Expire : ", "");
            var taskid = todoArray[4].Replace("TaskID : ", "");

            Id = todoArray[4];


            if (done == "true")
            {
                IsDone.IsChecked = true;
            }
            else
            {
                IsDone.IsChecked = false;
            }
            Task.Text = task;
            Description.Text = descption;
            DueTime.Text = duetime;
            IsDone.Checked += IsDone_CheckedChanged;
            IsDone.Unchecked += IsDone_CheckedChanged;
        }
        private void IsDone_CheckedChanged(object sender, RoutedEventArgs e)
        {
            // Получение нового статуса задачи
            string newDoneStatus = IsDone.IsChecked.Value ? "Done : true" : "Done : false";

            // Обновление статуса задачи в базе данных
            UpdateTaskDoneStatusInDatabase(Id, newDoneStatus);
        }

        public void UpdateTaskDoneStatusInDatabase(string taskId, string newDoneStatus)
        {
            var database = Client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            User user = GetUserByNickname(UserName);

            int taskIndex = FindTaskById(user.TODO, taskId);
            if (taskIndex != -1)
            {
                string task = "Task : " + Task.Text;
                string descption = "Description : " + Description.Text;
                string dueDate = "Expire : " + DueTime.Text;
                // Обновление задачи в списке TODO пользователя
                user.TODO[taskIndex] = new List<object> { task, descption, newDoneStatus, dueDate, taskId };

                // Обновление пользователя в базе данных
                var update = Builders<User>.Update.Set(u => u.TODO, user.TODO);
                collection.UpdateOne(u => u.nickname == UserName, update);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {       
            string taskId = Id; 
            DeleteTaskFromDatabase(taskId);
        }
        public User GetUserByNickname(string nickname)
        {

            var database = Client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            var user = collection.Find(u => u.nickname == nickname).FirstOrDefault();
            return user;
        }

        private void DeleteTaskFromDatabase(string taskId)
        {
            var database = Client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            string userNickname = UserName;
            User user = GetUserByNickname(userNickname);
          
            int taskIndex = FindTaskById(user.TODO, taskId);

            if (taskIndex != -1)
            {
                // Удаление задачи из списка TODO пользователя
                user.TODO.RemoveAt(taskIndex);

                // Обновление пользователя в базе данных
                var update = Builders<User>.Update.Set(u => u.TODO, user.TODO);
                collection.UpdateOne(u => u.nickname == userNickname, update);
                Todo todoControlToRemove = FindTodoControlByTaskId(taskId);
                if (todoControlToRemove != null)
                {
                    // Удалить элемент интерфейса
                    todoList.Listtodo.Children.Remove(todoControlToRemove);
                }

            }
        }

        private Todo FindTodoControlByTaskId(string taskId)
        {
            foreach (UIElement child in todoList.Listtodo.Children)
            {
                if (child is Todo todoControl && todoControl.Id == taskId)
                {
                    return todoControl;
                }
            }
            return null;
        }

        private int FindTaskById(object todoItem, string taskId)
        {

            var collection = todoItem.ToJson();

            var todoList = JsonConvert.DeserializeObject<List<List<string>>>(collection);


            if (todoList != null)
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    var task = todoList[i];
                    if (task.Contains(taskId))
                    {
                        return i;
                    }
                }
            }
            else
            {
                return -1;
            }

            return -1;

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditTodoWindow editTodoWindow = new EditTodoWindow(this, Id, Task.Text, Description.Text, DueTime.Text);
            editTodoWindow.ShowDialog();
        }

        public void UpdateTaskInDatabase(string taskId, string newTask, string newDescription, string newDueTime)
        {
            var database = Client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            User user = GetUserByNickname(UserName);

            int taskIndex = FindTaskById(user.TODO, taskId);
            if (taskIndex != -1)
            {
                string task = "Task : " + newTask;
                string descption = "Description : " + newDescription;
                string dueDate = "Expire : " + newDueTime;
                // Обновление задачи в списке TODO пользователя
                user.TODO[taskIndex] = new List<object> { task, descption, "Done : false", dueDate, taskId };

                // Обновление пользователя в базе данных
                var update = Builders<User>.Update.Set(u => u.TODO, user.TODO);
                collection.UpdateOne(u => u.nickname == UserName, update);

                // Обновление интерфейса пользователя
                Task.Text = newTask;
                Description.Text = newDescription;
                DueTime.Text = newDueTime;
            }
        }

    }
}
