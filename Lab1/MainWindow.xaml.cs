using Amazon.S3;
using Amazon.S3.Model;
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

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IAmazonS3 client;
        public MainWindow()
        {
            client = new AmazonS3Client();
            //Uri resourceUri = new Uri("cat.jpg");

            //catImage.Source = new BitmapImage(resourceUri);
            InitializeComponent();
           
        }

        private void LoadBucketButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> bucketlist = new List<String>();
            ListBucketsResponse response = client.ListBuckets();
            foreach (S3Bucket bucket in response.Buckets)
            {
                bucketlist.Add(bucket.BucketName);
                Console.WriteLine("You own Bucket with name: {0}", bucket.BucketName);
            }

            bucketListBox.ItemsSource = bucketlist;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            String name = bucketListBox.SelectedItem.ToString();
            uploadText.Content = "Upload Successfully";
            UploadImage(name).Wait();
            
        }

        static async Task UploadImage(String bucketName)
        {
            client = new AmazonS3Client();
            string filePath = @"C:\\Users\\SONY\\Downloads\\cat.jpg";
            string keyName = "cat.jpg";
          
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = filePath
                };

                putRequest.Metadata.Add("cat", "cat");
                PutObjectResponse response = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
    }
}
