using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace earnshark_sdk_dot_net
{
    public class Main
    {
        static HttpClient client = new HttpClient();

        static string baseURL = "https://app.earnshark.com/prod/";
        static string appDir = "http://app.earnshark.com/";

        public async Task<JObject> addNewSubscription(int product_id, string key, string name, string email, string accountID, string start_date, int license_id, string enableNotifications, string sendInvoiceNow)
        {

            JObject user = new JObject(
                 new JProperty("account",
                    new JObject(
                        new JProperty("name", name),
                        new JProperty("email", email),
                        new JProperty("accountID", accountID),
                        new JProperty("start_date", start_date)
                    )),
                 new JProperty("license_id", license_id),
                 new JProperty("enableNotifications", enableNotifications),
                 new JProperty("sendInvoiceNow", sendInvoiceNow)
            );

            JObject newSubscription = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(baseURL + "product/" + product_id.ToString() + "/addsubscriptionfromapi?key=" + key.ToString(), user);

            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                dynamic dynObj = JsonConvert.DeserializeObject(temp);
                newSubscription = JObject.Parse(dynObj);
            }

            return newSubscription;
        }

        public async Task<JArray> getAccountInformation(int product_id, string account_id, string key)
        {
            string path = baseURL + "product/" + product_id + "/subscriptioninfo/" + account_id + "?key=" + key;

            JArray account = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                account = JArray.Parse(temp);
            }
            return account;
        }

        public async Task<JObject> renewSubscription(int product_id, string key, int subscription_id, int new_license_id)
        {
            string path = baseURL + "product/" + product_id + "/subscription/" + subscription_id + "/apiRenewSubscription/" + new_license_id + "?key=" + key;

            JObject subscription = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                subscription = JObject.Parse(temp);
            }
            return subscription;
        }

        public async Task<JObject> getLicenseInformation(int product_id, string key, int license_id)
        {
            string path = baseURL + "product/" + product_id + "/license/" + license_id + "/getlicensefromapi?key=" + key;

            JObject license = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                license = JObject.Parse(temp);
            }
            return license;
        }

        public async Task<JArray> getAccountPayments(int product_id, string account_id, string key)
        {
            string path = baseURL + "product/" + product_id + "/account/" + account_id + "/transactions?key=" + key;

            JArray payments = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                payments = JArray.Parse(temp);
            }
            return payments;
        }

        public async Task<JArray> getAllLicensesOfProduct(int product_id, string key)
        {
            string path = baseURL + "product/" + product_id + "/license/all?key=" + key;

            JArray licenses = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string temp = await response.Content.ReadAsStringAsync();
                licenses = JArray.Parse(temp);
            }
            return licenses;
        }

        public async Task<string> getPaymentURL(int product_id, string key, string account_id, string redirect)
        {
            JObject body = new JObject
            {
                new JProperty("redirect", redirect),
                new JProperty("account_id", account_id),
                new JProperty("product_id", product_id),
                new JProperty("key", key)
            };

            string url = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(baseURL + "payments/getTransactionID", body);

            if (response.IsSuccessStatusCode)
            {
                url = await response.Content.ReadAsStringAsync();
            }
            return url.Trim('"');
        }
    }
}
