using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace SimpleContacts
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ContactViewModel();
        }
    }

    public class ContactViewModel
    {
        public Command SmsCommand { get; }
        public Command EmailCommand { get; }
        public Command PhoneCommand { get; }
        public Command NavigateCommand { get; }
        public Contact Contact { get; }
        public ContactViewModel(Contact contact)
        {
            Contact = contact;
        }
        public ContactViewModel()
        {
            Contact = new Contact
            {
                Name = "James Montemagno",
                Address = "Microsoft Building 18",
                City = "Redmond",
                State = "WA",
                ZipCode = "98052",
                Email = "motz@microsoft.com",
                PhoneNumber = "555-555-5555"
            };

            SmsCommand = new Command(async () => await ExecuteSmsCommand());
            EmailCommand = new Command(async () => await ExecuteEmailCommand());
            PhoneCommand = new Command(ExecutePhoneCommand);
            NavigateCommand = new Command(async () => await ExecuteNavigateCommand());
        }

        async Task ExecuteSmsCommand()
        {
            try
            {
                await Sms.ComposeAsync(new SmsMessage(string.Empty, Contact.PhoneNumber));
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        async Task ExecuteEmailCommand()
        {
            try
            {
                await Email.ComposeAsync(string.Empty, string.Empty, Contact.Email);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        void ExecutePhoneCommand()
        {
            try
            {
                PhoneDialer.Open(Contact.PhoneNumber);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        async Task ExecuteNavigateCommand()
        {
            try
            {
                await Map.OpenAsync(new Placemark
                {
                    Thoroughfare = Contact.Address,
                    Locality = Contact.City,
                    AdminArea = Contact.State,
                    PostalCode = Contact.ZipCode
                });
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        void ProcessException(Exception ex)
        {
            if (ex != null)
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
