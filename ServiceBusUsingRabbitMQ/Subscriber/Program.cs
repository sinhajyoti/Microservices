using EasyNetQ;
using Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
            
            string Queuename = "Queue02202017";
            string ExchangeName = "Exch02202017";
            var connection = factory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true);
                channel.QueueDeclare(Queuename, false, false, false, null);

                channel.QueueBind(queue: Queuename,
                   exchange: ExchangeName,
                   routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                var oldCorrelationId = string.Empty;
                //int ctr = 0;
                while (!Console.KeyAvailable)
                {

                    var message = string.Empty;
                    
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var correlationId = ea.BasicProperties.CorrelationId;
                        message = Encoding.UTF8.GetString(body);
                        if (!string.IsNullOrEmpty(message))
                        {
                            if (correlationId != oldCorrelationId)
                            {
                                Console.WriteLine("Received message: " + message);
                                publishReply(message + correlationId, correlationId);
                                message = string.Empty;
                            }
                            oldCorrelationId = correlationId;
                        }
                    };
                    channel.BasicConsume(Queuename, true, consumer);
                    //ctr++;
                    //System.Diagnostics.Debug.Write(ctr.to)
                    
                    Thread.Sleep(10);
                }
            }

            //using (var bus = RabbitHutch.CreateBus("host=myRabbitMQ"))
            //{
            //    bus.Subscribe<TextMessage>("test", HandleTextMessage);

            //    Console.WriteLine("Listening for messages. Hit <return> to quit.");
            //    Console.ReadLine();
            //}

        }

        private static void publishReply(string message, string correlationId)
        {
            
            var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
            var connection = factory.CreateConnection();
            string respQueueName = string.Format("Queue02202017_resp_{0}", correlationId); 
            string ExchangeName = "Exch02202017";
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true);
                channel.QueueDeclare(respQueueName, false, false, false, null);
                channel.QueueBind(queue: respQueueName,
                                  exchange: ExchangeName,
                                  routingKey: "");

                if (message.Length > 0)
                {
                    
                    var body = Encoding.UTF8.GetBytes(message);
                    Console.WriteLine("inside publishreply:"+ message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = false;
                    properties.CorrelationId = correlationId;
                    channel.BasicPublish("", respQueueName, properties, body);
                }
            }
            connection.Close();


            //static void HandleTextMessage(TextMessage textMessage)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Got message: {0}", textMessage.Text);
            //    Console.ResetColor();
            //}

            //private void ReceiveMessage()
            //{
            //    var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
            //    var connection = factory.CreateConnection();

            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("Queue_comp02202017", true, false, false, null);

            //        var consumer = new EventingBasicConsumer(channel);
            //        consumer.Received += (model, ea) =>
            //        {
            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);
            //            Console.WriteLine("Received message"+ message);
            //        };
            //        channel.BasicConsume("Queue_comp02202017", true, consumer);
            //    }
            //}
        }
    }
}
