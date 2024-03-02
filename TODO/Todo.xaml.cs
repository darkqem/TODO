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

namespace TODO
{
    /// <summary>
    /// Логика взаимодействия для Todo.xaml
    /// </summary>
    public partial class Todo : UserControl
    {

        public class TodoItem
        {
            public string Task { get; set; }
            public string Description { get; set; }
            public bool IsDone { get; set; }
            public string Expire { get; set; }
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
                
                Expire = todoArray[3].Split(": ")[1],
            };

            Task.Text = todoArray[0];
            Description.Text = todoArray[1];
            DueTime.Text = todoArray[3];




        }
    }
}
