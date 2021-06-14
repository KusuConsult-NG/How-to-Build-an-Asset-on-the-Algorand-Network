using System;
using Algorand.V2;
using Algorand;
using Account = Algorand.Account;
using Algorand.V2.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text;

namespace BuildingAssetAlgorand.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildAsset : ContentPage
    {
        public BuildAsset()
        {
            InitializeComponent();
        }
        private async void createAsset(object sender, EventArgs e)
        {
            string ALGOD_API_ADDR = "https://testnet-algorand.api.purestake.io/ps2"; //find in algod.net
            string ALGOD_API_TOKEN = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab"; //find in algod.token 
            AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);

            // Shown for demonstration purposes. NEVER reveal secret mnemonics in practice.
            // These two accounts are for testing purposes
            string account1_mnemonic = CreatorMnemonicKey.Text.ToString();
            string account2_mnemonic = ManagerMnemonicKey.Text.ToString(); 

            Account acct1 = new Account(account1_mnemonic);
            Account acct2 = new Account(account2_mnemonic);
            var transParams = algodApiInstance.TransactionParams();

            // The following parameters are asset specific
            // and will be re-used throughout the example. 

            // Create the Asset
            // Total number of this asset available for circulation            
            var ap = new AssetParams(creator: acct1.Address.ToString(), name: AssetName.Text.ToString(), unitName: UnitName.Text.ToString(), total: 10000,
                decimals: 0, url: Url.Text.ToString(), metadataHash: Encoding.ASCII.GetBytes("16efaa3924a6fd9d3a4880099a4ac65d"))
            {
                Manager = acct2.Address.ToString()
            };
            // Specified address can change reserve, freeze, clawback, and manager
            // you can leave as default, by default the sender will be manager/reserve/freeze/clawback
            // the following code only set the freeze to acct1
            var tx = Utils.GetCreateAssetTransaction(ap, transParams, "asset tx message");

            // Sign the Transaction by sender
            SignedTransaction signedTx = acct1.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            long? assetID = 0;
            try
            {
                var id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id);
                Console.WriteLine("Confirmed Round is: " +
                    Utils.WaitTransactionToComplete(algodApiInstance, id.TxId).ConfirmedRound);
                // Now that the transaction is confirmed we can get the assetID
                var ptx = algodApiInstance.PendingTransactionInformation(id.TxId);
                assetID = ptx.AssetIndex;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                return;
            }
            await DisplayAlert("AssetID", $"{assetID}", "OK");
            AssetId.IsEnabled = true;
            AssetId.Text = $"{assetID}";
            //Console.WriteLine("AssetID = " + assetID);
            // now the asset already created
        }
    }
}