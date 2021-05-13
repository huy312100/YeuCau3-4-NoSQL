using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Search_NoSQL
{
    public partial class Form1 : Form
    {
        public class Product
        {
            //[BsonId]
            //public ObjectId ID { get; set; }

            public string name { get; set; }
            public int price { get; set; }
            public string type { get; set; }
            public string decription { get; set; }

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("a");
            MongoClient dbClient = new MongoClient("mongodb://localhost");

            var database = dbClient.GetDatabase("Tiki");
            var collection = database.GetCollection<BsonDocument>("Product");
            

            //collection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Text.(x => )));
            List<Product> Product = new List<Product>();
            
            //var data = collection.Find(Builders<BsonDocument>.Filter.Text("Iphone")).ForEachAsync(x => System.Diagnostics.Debug.WriteLine(x));

            //collection.Find(new BsonDocument()).ForEachAsync(x => System.Diagnostics.Debug.WriteLine(x));
            var data = collection.Find(Builders<BsonDocument>.Filter.Text(searchTextBox.Text.ToString())).ToList();
            if (data.Count != 0)
            {
                dataGridViewTxtSearch.Visible = true;
                foreach (var item in data)
                {
                    Product dataProduct = new Product();
                    //dataProduct.ID = item[0].AsObjectId;
                    dataProduct.name = item[1].ToString();
                    dataProduct.price = item[2].ToInt32();
                    dataProduct.type = item[3].ToString();
                    dataProduct.decription = item[4].ToString();
                    Product.Add(dataProduct);
                }
                dataGridViewTxtSearch.DataSource = Product;
            }
            else
            {
                data = collection.Find(Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(searchTextBox.Text.ToString(), "i"))).ToList();
                if (data.Count != 0)
                {
                    dataGridViewTxtSearch.Visible = true;
                    foreach (var item in data)
                    {
                        Product dataProduct = new Product();
                        //dataProduct.ID = item[0].AsObjectId;
                        dataProduct.name = item[1].ToString();
                        dataProduct.price = item[2].ToInt32();
                        dataProduct.type = item[3].ToString();
                        dataProduct.decription = item[4].ToString();
                        Product.Add(dataProduct);
                    }
                    dataGridViewTxtSearch.DataSource = Product;
                }
            }
           
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            MongoClient dbClient = new MongoClient("mongodb://localhost");

            var database = dbClient.GetDatabase("Tiki");
            var collection = database.GetCollection<BsonDocument>("Product");

            List<Product> Product = new List<Product>();

            //var data = collection.Find(Builders<BsonDocument>.Filter.Text("Iphone")).ForEachAsync(x => System.Diagnostics.Debug.WriteLine(x));

            var data = collection.Find(Builders<BsonDocument>.Filter.Eq("type",comboBox1.SelectedItem.ToString())).ToList();
            if (data.Count != 0)
            {
                dataGridViewTxtSearch.Visible = true;
                foreach (var item in data)
                {
                    Product dataProduct = new Product();
                    //dataProduct.ID = item[0].AsObjectId;
                    dataProduct.name = item[1].ToString();
                    dataProduct.price = item[2].ToInt32();
                    dataProduct.type = item[3].ToString();
                    dataProduct.decription = item[4].ToString();
                    Product.Add(dataProduct);
                }
                dataGridViewTxtSearch.DataSource = Product;
            }
            else
            {
                MessageBox.Show("Không thấy kết qủa tìm kiếm");
            }
        }
    }
}
