using System;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading;
using System.Text;

namespace MQTTSampleCode.Client
{
    class Program
    {
        const string TOPIC = "mqttsampleSV/topic";

        private static IMqttClient mqttClient;

        static void Main(string[] args)
        {
            // connect to MQTT Broker
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("MQTTSampleCode." + new Random().Next())
                .WithTcpServer("test.mosquitto.org")
                .WithCleanSession()
                .Build();

            var client = mqttClient.ConnectAsync(options).GetAwaiter().GetResult();

            /*
             * Subscribe
             */
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

            });

            mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(TOPIC).Build()).GetAwaiter().GetResult();

            /*
             * Publish
             */
            /*while (true)
            {
                var payload = "Hello World " + new Random().Next();
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(TOPIC)
                    .WithPayload(payload)
                    .WithExactlyOnceQoS()
                    .Build();

                mqttClient.PublishAsync(message, CancellationToken.None).GetAwaiter().GetResult();

                Console.WriteLine("Message published: " + payload);

                Thread.Sleep(1000);
            }*/

            Console.ReadLine();
            
        }

    }
}
