using Example_4.TheClient;
using Example_4.TheServer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;

namespace Example_4.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        

        public RelayCommand ClientBtnClicked { get; set; }
        public RelayCommand ServerBtnClicked { get; set; }
        public RelayCommand AddBtnClicked { get; set; }
        public ObservableCollection<ProductVM> ProductList { get; set; }
        public ObservableCollection<string> ComboBoxList { get; set; }

        public bool isConnected = false;

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; RaisePropertyChanged(); }
        }

        public bool  btnClicked = false;

        public bool BtnClicked
        {
            get { return btnClicked; }
            set { btnClicked = value; RaisePropertyChanged(); }
        }

        private string productId = "";

        public string ProductID
        {
            get { return productId; }
            set { productId = value; }
        }

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private string type = "";

        public string Type
        {
            get { return type; }
            set { type = value; }
        }





        //public string ProductID { get; set; }
        //public string Name { get; set; }
        //public double Price { get; set; }
        //public string Type { get; set; }

        private const int port = 10100;
        private const string ip = "127.0.0.1";
        private Server server;
        private Client client;

        public MainViewModel()
        {
            ClientBtnClicked = new RelayCommand(()=> 
            {
                client = new Client(ip, port, UpdateGuiWithNewMessage);
                isConnected = client.IsConnected();
                btnClicked = true;
            },
            ()=> { return !btnClicked; });

            ServerBtnClicked = new RelayCommand(() => 
            {
                server = new Server(port, ip, UpdateGuiWithNewMessage);
                btnClicked = true;
                isConnected = true;
            },
            ()=> { return !btnClicked; });

            AddBtnClicked = new RelayCommand(() => 
            {
                //ProductList.Add(new ProductVM(ProductID, Name, Price, Type));
                server.NewMessageReceived(ProductID + "," + Name + "," + Price + "," + Type);
            },
            ()=> { return CanShow(); });

            ComboBoxList = new ObservableCollection<string>();
            ProductList = new ObservableCollection<ProductVM>();

            BuildComboBox();
        }

        private bool CanShow()
        {
            if (isConnected == true && ProductID.Length > 0 && Name.Length > 0 && Type.Length > 0 && Price > 0)
            {
                return true;
            }
            return false;
        }

        private void UpdateGuiWithNewMessage(string message)
        {
            string[] receivedMessage = message.Split(',');

            App.Current.Dispatcher.Invoke(() => 
            {
            ProductList.Add(new ProductVM(receivedMessage[0], receivedMessage[1], Double.Parse(receivedMessage[2]), receivedMessage[3]));
            });
        }


        private void BuildComboBox()
        {
            ComboBoxList.Add("Engine");
            ComboBoxList.Add("Gears");
            ComboBoxList.Add("Tyre");
            ComboBoxList.Add("Body");
            ComboBoxList.Add("Chassi");
        }
    }
}