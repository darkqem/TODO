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
                }
            }
            


        }



        public User GetUserByNickname(string nickname)
        {

            var database = client.GetDatabase("Todo");
            var collection = database.GetCollection<User>("TOdo");
            var user = collection.Find(u => u.nickname == nickname).FirstOrDefault();
            return user;
        }


        static string ExtractAndRemoveBetweenSlashes(ref string input)
        {
            int firstSlashIndex = input.IndexOf('/');
            int lastSlashIndex = input.LastIndexOf('/');

            // Check if there are at least two slashes in the string
            if (firstSlashIndex >= 0 && lastSlashIndex > firstSlashIndex)
            {
                // Extract the substring between the first and last slash
                string extracted = input.Substring(firstSlashIndex + 1, lastSlashIndex - firstSlashIndex - 1);

                // Remove the extracted substring from the original string
                input = input.Remove(firstSlashIndex + 1, lastSlashIndex - firstSlashIndex - 1);

                return extracted;
            }
            else
            {
                // Return an empty string or handle the case as needed
                return string.Empty;
            }
        }

    }
}
