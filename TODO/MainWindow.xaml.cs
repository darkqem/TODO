using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Driver.Core.Operations;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson.Serialization;


namespace TODO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMongoDatabase _database;
        private MongoClient client;

        public class User
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            public string nickname { get; set; }
            public string password { get; set; }
            public List<object> TODO { get; set; } = new List<object>();

        }



        public MainWindow()
        {
            InitializeComponent();
            client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("Todo");
            
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            

            var collection = _database.GetCollection<User>("TOdo");
            var user = collection.Find(u => u.nickname == txtUsername.Text && u.password == txtPassword.Password).FirstOrDefault();
            
           

            if (user != null)
            {
                
                TodoList todoList = new TodoList(user.nickname, client);
                todoList.Show();
                
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            var user = collection.Find(u => u.nickname == txtUsername.Text).FirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    nickname = txtUsername.Text,
                    password = txtPassword.Password
                };
                collection.InsertOne(user);
                MessageBox.Show("Регистрация успешна!");
            }
            else
            {
                MessageBox.Show("Этот никнейм уже используется.");
            }
                                       
        }

    }
}