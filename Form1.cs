using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace Gurme2
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();

        //public static object ReturnTypeStatus { get; private set; }

        public Form1()
        {
            InitializeComponent();

            //HTTP client'i burada yapılandırabilirsiniz
            httpClient.BaseAddress = new Uri ("https://cdu-test.arcelik.com/CardDataApi");
            //Diğer başlangıç yapılandırmaları
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            //string token = "";
            string barcode = textBox1.Text;

            await DeactivateCardAsync(barcode);

        }
        private static async Task DeactivateCardAsync (string barcode)
        {
           // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(barcode);

            var model = new CardDeactivationModel
            {
                Fullbarcode = barcode,
                Description = "Tamam"
            };

            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            try
            {
                var response = await httpClient.PostAsync("api/CardData/deactivecard", httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Server returned an error: { response.StatusCode}\n{ responseContent}");
                }

                var result = JsonConvert.DeserializeObject<ReturnModel<string>>(responseContent);
                if (result.Status == ReturnTypeStatus.Success)
                {
                    MessageBox.Show("Deactivation card succesfully");
                }
                else if(result.Status == ReturnTypeStatus.Error)
                {
                    MessageBox.Show("ERROR: " + result.Message);
                }



            }
            catch (JsonReaderException jsonEx)
            {
                MessageBox.Show("JSON parsing error:" + jsonEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }
        public enum ReturnTypeStatus
        {
            Success,
            Error
        }

    }

    
}
