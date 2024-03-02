using MongoDB.Bson;
using MongoDB.Driver;
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
using static TODO.MainWindow;

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для TodoList.xaml
    /// </summary>
    public partial class TodoList : Window
    {
        private string userNickname;
        private readonly MongoClient client;


        public TodoList(string nickname, MongoClient client)
        {
            InitializeComponent();
            userNickname = nickname;
            this.client = client;
            User user = GetUserByNickname(userNickname);

            if (user != null && user.TODO != null)
            {
                foreach (var todoItem in user.TODO)
                {
                    


                   



                    
                    
                    
                    

                    Todo todoControl = new Todo(todoItem);
                    Listtodo.Children.Add(todoControl);
                    Rectangle blackStripe = new Rectangle
                    {
                        Fill = Brushes.Black,
                        Height = 1,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                    Listtodo.Children.Add(blackStripe);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTodo addtodo = new AddTodo();
            addtodo.OnSave += AddTodo_OnSave;
            addtodo.Show();
        }

        private void AddTodo_OnSave(string task, string description, string dueDate)
        {
            AddTaskToUserTodo(userNickname, task, description, dueDate);
        }

        private void AddTaskToUserTodo(string nickname, string task, string description, string dueDate)
        {
            var database = client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            User user = GetUserByNickname(userNickname);
            var taskId = "TaskID : " + user.TODO.Count;
            var update = Builders<User>.Update.Push(u => u.TODO, new List<object> { task, description, "Done : false", dueDate, taskId });
            collection.UpdateOne(u => u.nickname == nickname, update);
            user = GetUserByNickname(userNickname);
            var lastTodoItem = user.TODO[user.TODO.Count - 1];
            Todo todoControl = new Todo(lastTodoItem);
            Listtodo.Children.Add(todoControl);


            Rectangle blackStripe = new Rectangle
            {
                Fill = Brushes.Black,
                Height = 1,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Listtodo.Children.Add(blackStripe);
        }



        public User GetUserByNickname(string nickname)
        {

            var database = client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            var user = collection.Find(u => u.nickname == nickname).FirstOrDefault();
            return user;
        }




    }
}
