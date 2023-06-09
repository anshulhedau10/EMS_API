﻿using Azure;
using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using EmsApi.Models;
using Newtonsoft.Json;
using System.Text;

namespace EmsApi
{
    public class Helper
    {
        public static async Task<bool> UploadBlob(
            IConfiguration config,
            Employee employee)
        {
            string blobConnString = config.GetConnectionString("StorAccConnString");
            BlobServiceClient client = new BlobServiceClient(blobConnString);
            string container = config.GetValue<string>("Container");
            var containerClient = client.GetBlobContainerClient(container);

            string fileName = "ems.employee." + Guid.NewGuid().ToString() + ".json";
            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            //memorystream
            using (var stream = new MemoryStream())
            {
                var serializer = JsonSerializer.Create(new JsonSerializerSettings());

                // Use the 'leave open' option to keep the memory stream open after the stream writer is disposed
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    // Serialize the job to the StreamWriter
                    serializer.Serialize(writer, employee);
                }

                // Rewind the stream to the beginning
                stream.Position = 0;

                // Upload the job via the stream
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            await PublishToEventGrid(config, employee);
            return true;
        }

        private static async Task PublishToEventGrid(
            IConfiguration config, Employee employee)
        {
            var endpoint = config.GetValue<string>("EventGridTopicEndpoint");
            var accessKey = config.GetValue<string>("EventGridAccessKey");

            EventGridPublisherClient client = new EventGridPublisherClient(
                new Uri(endpoint),
                new AzureKeyCredential(accessKey));

            var event1 = new EventGridEvent(
                    "PMS",
                    "PMS.EmployeeEvent",
                    "1.0",
                    JsonConvert.SerializeObject(employee));
            event1.Id = (new Guid()).ToString();
            event1.EventTime = DateTime.Now;
            //resource id
            //event1.Topic = "/subscriptions/73d972cd-c4c3-4ec5-9443-661a57525a5d/resourceGroups/rg-training/providers/Microsoft.EventGrid/topics/omsegt";
            event1.Topic = config.GetValue<string>("EventGridTopic");
            List<EventGridEvent> eventsList = new List<EventGridEvent>
            {
                event1
            };

            // Send the events
            await client.SendEventsAsync(eventsList);
        }
    }
}
