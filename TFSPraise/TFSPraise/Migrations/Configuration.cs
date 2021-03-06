namespace TFSPraise.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Collections.Generic;
    using System.Linq;
    using TFSPraise.Entities;


    internal sealed class Configuration : DbMigrationsConfiguration<TFSPraise.Concrete.TFSPraiseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TFSPraise.Concrete.TFSPraiseContext context)
        {
            var userProfiles = new List<UserProfile>
            {
                new UserProfile { Identity = new Identity { Name = @"FAREAST\v-yaqyu", DispalyName = "Ketty Yu" }, Resign = false },
                new UserProfile { Identity = new Identity { Name = @"FAREAST\v-guoyun", DispalyName="Yun Guo" }, Resign = false },
            };
            userProfiles.ForEach(u => context.UserProfiles.Add(u));
            context.SaveChanges();

            List<Receiver> receivers = new List<Receiver>();
            userProfiles.ForEach(u => {
                Receiver receiver = new Receiver { Name = u.Identity.Name, ID = u.IdentityID, DisplayName = u.Identity.DispalyName };
                receivers.Add(receiver);
                context.Receivers.Add(receiver);
                });
            context.SaveChanges();

            var blogs = new List<Blog>
            {
                new Blog { Title ="DROP TABLE (Transact-SQL)", Content = "Removes one or more table definitions and all data, indexes, triggers, constraints, and permission specifications for those tables. Any view or stored procedure that references the dropped table must be explicitly dropped by using DROP VIEW or DROP PROCEDURE. To report the dependencies on a table, use sys.dm_sql_referencing_entities.",
                    Date = DateTime.Now.AddDays(-1),
                    Poster = userProfiles.FirstOrDefault()
                },
                new Blog { Title="How to: Send Data Using the WebRequest Class",
                    Content = "The following procedure describes the steps used to send data to a server. This procedure is commonly used to post data to a Web page.",
                    Date = DateTime.Now.AddDays(-3),
                    Poster = userProfiles.Skip(1).FirstOrDefault()
                }
            };
            blogs.ForEach(b => context.Blogs.Add(b));
            context.SaveChanges();

            var likes = new List<Like>
            {
                new Like { Content = "Praise content 1", SponsorID = 1, Date = DateTime.Now,
                    Receivers = receivers },
                new Like { Content = "Praise content 2", SponsorID = 2, Date = DateTime.Now,
                    Receivers = receivers.Take(2).ToList() },
                new Like { Content = "Praise content 3", SponsorID = 1, Date = DateTime.Now,
                    Receivers = receivers.Take(1).ToList()},
                new Like { Content = "Praise content 4", SponsorID = 2, Date = DateTime.Now,
                    Receivers = receivers.Skip(1).Take(1).ToList() },
            };
            likes.ForEach(p => context.Praises.Add(p));
            context.SaveChanges();
        }
    }
}
