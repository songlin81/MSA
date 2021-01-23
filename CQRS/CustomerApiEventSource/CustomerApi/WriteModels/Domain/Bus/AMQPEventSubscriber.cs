using CQRSlite.Events;
using CustomerApi.WriteModels.Events;
using CustomerApi.WriteModels.Events.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.WriteModels.Domain.Bus
{
    public class AMQPEventSubscriber
    {
        private readonly IBusEventHandler[] _handlers;
        private Dictionary<Type, MethodInfo> lookups = new Dictionary<Type, MethodInfo>();

        public AMQPEventSubscriber(IHostingEnvironment env, IBusEventHandler[] handlers)
        {
            _handlers = handlers;

            foreach (var handler in _handlers)
            {
                var meth = (from m in handler.GetType()
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            let prms = m.GetParameters()
                            where prms.Count() == 1 && m.Name.Contains("Handle")
                            select new
                            {
                                EventType = handler.HandlerType,
                                Method = m
                            }).FirstOrDefault();
                if (meth != null)
                {
                    lookups.Add(meth.EventType, meth.Method);
                }
            }

            new Thread(() =>
            {
                Start(env.ContentRootPath);
            }).Start();
        }
        public void Start(string contentRootPath)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
            builder.Build().GetSection("amqp").Bind(connectionFactory);
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);

            using (IConnection conn = connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    DeclareQueues(channel);
                    var subscriptionCreated = new Subscription(channel, Constants.QUEUE_CUSTOMER_CREATED, false);
                    var subscriptionUpdated = new Subscription(channel, Constants.QUEUE_CUSTOMER_UPDATED, false);
                    var subscriptionDeleted = new Subscription(channel, Constants.QUEUE_CUSTOMER_DELETED, false);
                    while (true)
                    {
                        // Sleeps for 5 sec before trying again
                        Thread.Sleep(5000);
                        new Thread(() =>
                        {
                            ListerCreated(subscriptionCreated);
                        }).Start();
                        new Thread(() =>
                        {
                            ListenUpdated(subscriptionUpdated);
                        }).Start();
                        new Thread(() =>
                        {
                            ListenDeleted(subscriptionDeleted);
                        }).Start();
                    }
                }
            }
        }
        private void ListenDeleted(Subscription subscriptionDeleted)
        {
            BasicDeliverEventArgs eventArgsDeleted = subscriptionDeleted.Next();
            if (eventArgsDeleted != null)
            {
                string messageContent = Encoding.UTF8.GetString(eventArgsDeleted.Body);
                HandleEvent(JsonConvert.DeserializeObject<CustomerDeletedEvent>(messageContent));
                subscriptionDeleted.Ack(eventArgsDeleted);
            }
        }

        private void ListenUpdated(Subscription subscriptionUpdated)
        {
            BasicDeliverEventArgs eventArgsUpdated = subscriptionUpdated.Next();
            if (eventArgsUpdated != null)
            {
                string messageContent = Encoding.UTF8.GetString(eventArgsUpdated.Body);
                HandleEvent(JsonConvert.DeserializeObject<CustomerUpdatedEvent>(messageContent));
                subscriptionUpdated.Ack(eventArgsUpdated);
            }
        }
        private void ListerCreated(Subscription subscriptionCreated)
        {
            BasicDeliverEventArgs eventArgsCreated = subscriptionCreated.Next();
            if (eventArgsCreated != null)
            {
                string messageContent = Encoding.UTF8.GetString(eventArgsCreated.Body);
                HandleEvent(JsonConvert.DeserializeObject<CustomerCreatedEvent>(messageContent));
                subscriptionCreated.Ack(eventArgsCreated);
            }
        }

        private void HandleEvent(IEvent @event)
        {
            var theHandler = _handlers.SingleOrDefault(x => x.HandlerType == @event.GetType());
            Task.Run(() =>
            {
                foreach (KeyValuePair<Type, MethodInfo> entry in lookups)
                {
                    if (entry.Key == @event.GetType())
                    {
                        entry.Value.Invoke(theHandler, new[] { (object)@event });
                    }
                }
            }).Wait();
        }

        private static void DeclareQueues(IModel channel)
        {
            channel.QueueDeclare(
                queue: Constants.QUEUE_CUSTOMER_CREATED,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueDeclare(
                queue: Constants.QUEUE_CUSTOMER_UPDATED,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueDeclare(
                queue: Constants.QUEUE_CUSTOMER_DELETED,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
    }
}