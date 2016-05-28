using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BingImageSender.Models
{
    public class Subscriber
    {
        public string SubscriberId { get; set; } //email
        public bool isSubscribed { get; set; }
    }

    public class SubscribeContext : DbContext
    {
        public DbSet<Subscriber> Subscribers { get; set; }
        public SubscribeContext() : base("SubscribeDB")
        { }
    }
}