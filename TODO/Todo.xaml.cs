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

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для Todo.xaml
    /// </summary>
    public partial class Todo : UserControl
    {
        private object todoItem;
        

        

        public int Count { get; }
        public MongoClient Client { get; }
        

        public class TodoItem
        {
            public string Task { get; set; }
            public string Description { get; set; }
            public string IsDone { get; set; }
            public string Expire { get; set; }
            public string TaskId { get; set; }
        }

        

        public Todo(object todoItem)
        {
            InitializeComponent();

            var collection = todoItem.ToJson();

            var todoArray = JsonConvert.DeserializeObject<List<string>>(collection);

            var todoItems = new TodoItem
            {
                Task = todoArray[0].Split(": ")[1],
                Description = todoArray[1].Split(": ")[1],
                IsDone = todoArray[2].Split(": ")[1],
                Expire = todoArray[3].Split(": ")[1],
                TaskId = todoArray[4].Split(": ")[1]
            };

            var task = todoArray[0].Replace("Task : ", "");
            var descption = todoArray[1].Replace("Description : ", "");
            var done = todoArray[2].Replace("Done : ", "");
            var duetime = todoArray[3].Replace("Expire : ", "");
            var taskid = todoArray[4].Replace("TaskID : ", "");


            if (done=="true")
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
        }


        
    }
}
