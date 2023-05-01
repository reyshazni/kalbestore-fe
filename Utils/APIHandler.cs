using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using kalbestore_fe.Models;

namespace kalbestore_fe.Utils
{
    public static class APIHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string baseurl = "http://127.0.0.1:5000";

        public static async Task<List<Produk>> GetProduk()
        {
            string url = $"{baseurl}/produk/";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBodyString = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBodyString);
            var root = jsonDocument.RootElement;

            var produkList = new List<Produk>();
            foreach (var produkJson in root.GetProperty("data").EnumerateArray())
            {
                var produk = new Produk
                {
                    ProductID = produkJson.GetProperty("productID").GetInt32(),
                    ProductCode = produkJson.GetProperty("productCode").GetString(),
                    ProductName = produkJson.GetProperty("productName").GetString(),
                    Qty = produkJson.GetProperty("qty").GetInt32(),
                    Price = produkJson.GetProperty("price").GetDecimal(),
                    Inserted = produkJson.GetProperty("inserted").GetDateTime()
                };
                produkList.Add(produk);
            }

            return produkList;
        }

        public static async Task<List<Produk>> GetProdukByName(string productName)
        {
            string url = $"{baseurl}/produkNama?productName={productName}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBodyString = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBodyString);
            var root = jsonDocument.RootElement;

            var produkList = new List<Produk>();
            foreach (var produkJson in root.GetProperty("data").EnumerateArray())
            {
                var produk = new Produk
                {
                    ProductID = produkJson.GetProperty("productID").GetInt32(),
                    ProductCode = produkJson.GetProperty("productCode").GetString(),
                    ProductName = produkJson.GetProperty("productName").GetString(),
                    Qty = produkJson.GetProperty("qty").GetInt32(),
                    Price = produkJson.GetProperty("price").GetDecimal(),
                    Inserted = produkJson.GetProperty("inserted").GetDateTime()
                };
                produkList.Add(produk);
            }

            return produkList;
        }

        // Get Produk By Id
        public static async Task<Produk> GetProdukById(int productID)
        {
            string url = $"{baseurl}/product/{productID}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBodyString = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBodyString);
            var root = jsonDocument.RootElement;

            var produkJson = root.GetProperty("data");

            var produk = new Produk
            {
                ProductID = produkJson.GetProperty("productID").GetInt32(),
                ProductCode = produkJson.GetProperty("productCode").GetString(),
                ProductName = produkJson.GetProperty("productName").GetString(),
                Qty = produkJson.GetProperty("qty").GetInt32(),
                Price = produkJson.GetProperty("price").GetDecimal(),
                Inserted = produkJson.GetProperty("inserted").GetDateTime()
            };

            return produk;
        }

        // Create Order
        public static async Task<GetPenjualan> CreateOrder(AddCustomerRequest customer, int productId, int qty)
        {
            // Create Customer
            var customerJson = JsonSerializer.Serialize(customer);
            var customerContent = new StringContent(customerJson, Encoding.UTF8, "application/json");
            var customerResponse = await client.PostAsync($"{baseurl}/customer", customerContent);
            customerResponse.EnsureSuccessStatusCode();

            // get customer ID
            var customerResponseBodyString = await customerResponse.Content.ReadAsStringAsync();
            var customerJsonDocument = JsonDocument.Parse(customerResponseBodyString);
            var customerId = customerJsonDocument.RootElement.GetProperty("id").GetInt32();

            // create order
            var order = new AddPenjualanRequest
            {
                CustomerID = customerId,
                ProductID = productId,
                Qty = qty
            };
            var orderJson = JsonSerializer.Serialize(order);
            var orderContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var orderResponse = await client.PostAsync($"{baseurl}/salesorder", orderContent);
            orderResponse.EnsureSuccessStatusCode();

            var orderResponseBodyString = await orderResponse.Content.ReadAsStringAsync();
            var orderJsonDocument = JsonDocument.Parse(orderResponseBodyString);
            var orderJsonData = orderJsonDocument.RootElement.GetProperty("data");

            var penjualan = new GetPenjualan
            {
                SalesOrderID = orderJsonData.GetProperty("salesOrderID").GetInt32(),
                CustomerName = orderJsonData.GetProperty("customerName").GetString(),
                ProdukName = orderJsonData.GetProperty("produkName").GetString(),
                AlamatPengiriman = orderJsonData.GetProperty("alamatPengiriman").GetString(),
                Qty = orderJsonData.GetProperty("qty").GetDouble(),
                TotalHarga = orderJsonData.GetProperty("totalHarga").GetInt32()
            };

            return penjualan;
        }

        // Create Order Registered
        public static async Task<GetPenjualan> CreateOrderRegistered(int customerId, int productId, int qty)
        {
            // create order
            var order = new AddPenjualanRequest
            {
                CustomerID = customerId,
                ProductID = productId,
                Qty = qty
            };
            var orderJson = JsonSerializer.Serialize(order);
            var orderContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var orderResponse = await client.PostAsync($"{baseurl}/salesorder", orderContent);
            orderResponse.EnsureSuccessStatusCode();

            var orderResponseBodyString = await orderResponse.Content.ReadAsStringAsync();
            var orderJsonDocument = JsonDocument.Parse(orderResponseBodyString);
            var orderJsonData = orderJsonDocument.RootElement.GetProperty("data");

            var penjualan = new GetPenjualan
            {
                SalesOrderID = orderJsonData.GetProperty("salesOrderID").GetInt32(),
                CustomerName = orderJsonData.GetProperty("customerName").GetString(),
                ProdukName = orderJsonData.GetProperty("produkName").GetString(),
                AlamatPengiriman = orderJsonData.GetProperty("alamatPengiriman").GetString(),
                Qty = orderJsonData.GetProperty("qty").GetDouble(),
                TotalHarga = orderJsonData.GetProperty("totalHarga").GetInt32()
            };

            return penjualan;
        }

        public static async Task<int> GetCustomerIDByName(string customerName)
        {
            string url = $"{baseurl}/customer/{customerName}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBodyString = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBodyString);
            var root = jsonDocument.RootElement;

            var customerJson = root.GetProperty("data")[0];
            var customerID = customerJson.GetProperty("customerID").GetInt32();

            return customerID;
        }
    }
}
