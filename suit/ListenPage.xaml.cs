using System;
using System.Threading.Tasks;
using Poz1.NFCForms.Abstract;
using NdefLibrary.Ndef;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace suit
{
    public partial class ListenPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        private INfcForms device;
        Boolean IsConnected  = false;
        Boolean IsNDEFSupported;
        Boolean IsWriteable;
        public ListenPage()
        {
            //device.TagConnected += device_TagConnected;
            //device.TagDisconnected += device_TagDisconnected;
            //var browser = new WebView();
            //browser.Source = "http://xamarin.com";
            //Content = browser;
            Console.WriteLine("listen page construct");
            IsConnected = false;
            InitializeComponent();
            device = DependencyService.Get<INfcForms>();
            device.NewTag += HandleNewTag;
            Console.WriteLine("mounted");
        }

        protected override void OnAppearing()
        {           
            
        }


        /*void device_TagDisconnected(object sender, NfcFormsTag e)
        {
            Console.WriteLine("tag disconnected");
            IsConnected = false;
        }

        void device_TagConnected(object sender, NfcFormsTag e)
        {
            Console.WriteLine("tag connected");
            IsConnected = true;
        }*/

        private ObservableCollection<string> readNDEFMEssage(NdefMessage message)
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            if (message == null)
            {
                return collection;
            }

            foreach (NdefRecord record in message)
            {
                // Go through each record, check if it's a Smart Poster
                if (record.CheckSpecializedType(false) == typeof(NdefSpRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefSpRecord(record);
                    collection.Add("Uri: " + spRecord.Uri);
                    collection.Add("Titles: " + spRecord.TitleCount());
                    collection.Add("1. Title: " + spRecord.Titles[0].Text);
                    collection.Add("Action set: " + spRecord.ActionInUse());
                }

                if (record.CheckSpecializedType(false) == typeof(NdefUriRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefUriRecord(record);
                    collection.Add("Uri: " + spRecord.Uri);
                }
                if (record.CheckSpecializedType(false) == typeof(NdefTextRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefTextRecord(record);
                    App.Current.Properties["locationID"] = spRecord.Text;
                    collection.Add("Text: " + spRecord.Text);
                }
            }
            return collection;
        }



        async void HandleNewTag(object sender, NfcFormsTag e)
        {
            Console.WriteLine("connected status" + IsConnected);

            if (!IsConnected)
            {

                IsConnected = true;

                System.Diagnostics.Trace.WriteLine("Tag detected");

                ObservableCollection<string> collection = new ObservableCollection<string>();

                IsWriteable = e.IsWriteable;
                IsNDEFSupported = e.IsNdefSupported;

                if (e.IsNdefSupported)
                {
                    readNDEFMEssage(e.NdefMessage);
                }

                Console.WriteLine("time "+ DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));

                await firebaseHelper.AddRead(DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), App.Current.Properties["locationID"].ToString(), App.Current.Properties["userid"].ToString());

                GetLocationParamaters();

                device = default(INfcForms);

            }
            
        }
      
        void GetUserData()
        {
            DateTime thisDay = DateTime.Today;
            ObservableCollection<string> userCollection = new ObservableCollection<string>();
            userCollection.Add("Fecha y hora: " + thisDay.ToString("g"));
            userCollection.Add("ID Usuario: " + Convert.ToString(App.Current.Properties["userid"]));
            userCollection.Add("Usuario: " + Convert.ToString(App.Current.Properties["name"]));
        }
        async void GetLocationParamaters()
        {
            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            App.Current.Properties["LocationAddress"] = location.Address;
            App.Current.Properties["LocationNeighborhood"] = location.Neighborhood;
            App.Current.Properties["LocationPDV"] = location.PDV;
            App.Current.Properties["LocationType"] = location.Type;            
            await Navigation.PushAsync(new InformationPage());
            Navigation.RemovePage(this);

        }
    }
}
    
