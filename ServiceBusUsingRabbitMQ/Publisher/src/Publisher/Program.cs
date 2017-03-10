using EasyNetQ;
using Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class Program
    {
        //private readonly IConnection connection;

        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
            var connection = factory.CreateConnection();
           
            string Queuename = "Queue02202017";
            //string respQueueName = "Queue02202017_resp";
            string ExchangeName = "Exch02202017";
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true);
                channel.QueueDeclare(Queuename, false, false, false, null);
                channel.QueueBind(queue: Queuename,
                                  exchange: ExchangeName,
                                  routingKey: "");

                Console.WriteLine("Enter a message. 'Quit to quit.");
                string message = string.Empty;
                while ((message = Console.ReadLine()) != "Quit")
                {
                    var correlationId = Guid.NewGuid().ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = false;
                    properties.CorrelationId = correlationId;
                    channel.BasicPublish("", Queuename, properties, body);
                    Console.WriteLine("Published : {0} with CorrelationId of {1}", message,correlationId );
                    
                    consumeReply(message, correlationId);
                }
            }






            //using (var bus = RabbitHutch.CreateBus("host=myRabbitMQ"))
            //{
            //    var input = "";
            //    Console.WriteLine("Enter a message. 'Quit' to quit.");
            //    while ((input = Console.ReadLine()) != "Quit")
            //    {
            //        bus.Publish(new TextMessage
            //        {
            //            Text = input
            //        });
            //    }
            //}
        }


        private static void consumeReply(string message, string correlationId)
        {
            var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
            var connection = factory.CreateConnection();
            string respQueueName = string.Format("Queue02202017_resp_{0}",correlationId);
            string ExchangeName = "Exch02202017";
            //Console.WriteLine(string.Format("inside consumeReply with correlationID of {0}", correlationId));
            using (var channel = connection.CreateModel())
            {
                Thread.Sleep(20);
                ///read the response queue
                channel.QueueDeclare(respQueueName, false, false, false, null);
                channel.QueueBind(queue: respQueueName,
                                  exchange: ExchangeName,
                                  routingKey: "");
                var consumer = new EventingBasicConsumer(channel);
                var oldMessage = string.Empty;

                var messageresp = string.Empty;

                bool noAck = false;
                RabbitMQ.Client.BasicGetResult result = channel.BasicGet(respQueueName, noAck);
                while (result == null)
                {
                    // No message available at this time.
                    result = channel.BasicGet(respQueueName, noAck);
                }
                IBasicProperties props = result.BasicProperties;
                byte[] body = result.Body;
                messageresp = Encoding.UTF8.GetString(body);
                //Console.WriteLine(messageresp);
                if (correlationId == props.CorrelationId)
                {
                    
                    Console.WriteLine(string.Format("inside consumeReply with reply message {0}", messageresp));
                    channel.QueueDelete(respQueueName, false, false);
                }
                else
                {
                    Console.WriteLine(string.Format("no Replymessage received "));
                }

                    //    consumer.Received += (model, ea) =>
                    //{
                    //    var body = ea.Body;
                    //    if (correlationId == ea.BasicProperties.CorrelationId)
                    //    {
                    //        messageresp = Encoding.UTF8.GetString(body);
                    //        Console.WriteLine(string.Format("inside consumeReply with reply message {0}", messageresp));
                    //        if (!string.IsNullOrEmpty(messageresp))
                    //        {
                    //            if (messageresp != oldMessage)
                    //            {
                    //                Console.WriteLine("Received reply message: " + messageresp);
                    //            }
                    //            oldMessage = messageresp;
                    //        }
                    //    }
                    //};
                    //channel.BasicConsume(respQueueName, true, consumer);

                }
        }
        //private void SendTaskQueue()
        //{
        //    var factory = new ConnectionFactory { HostName = "myrabbitmq", UserName = "admin", Password = "admin" };
        //    var connection = factory.CreateConnection();

        //    string Queuename = "Queue_comp02202017"; //PayspanTask
        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.QueueDeclare(Queuename, true, false, false, null);

        //        var i = 0;
        //        while (i < 10)
        //        {
        //            var message = i.ToString(CultureInfo.InvariantCulture) + ". - Hello Jyoti K. Sinha!";
        //            var body = Encoding.UTF8.GetBytes(message);

        //            var properties = channel.CreateBasicProperties();
        //            properties.Persistent = true;

        //            channel.BasicPublish("", Queuename, properties, body);
        //            Console.WriteLine(" [x] Sent {0}", message);
        //            i++;
        //        }
        //        Console.WriteLine(i.ToString() +"messages posted.");
        //    }
        //}
    }
}
