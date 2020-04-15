using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zeze.Domin.Models.Users;
using Zeze.Domin.Models.Vows;

namespace Zeze.Domin.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext()
        {

        }
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
        }
        public DbSet<Vow> Vows { get; set; }

        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 重写自定义Map配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VowMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// 许愿类
        /// </summary>
        public class VowMap : IEntityTypeConfiguration<Vow>
        {
            /// <summary>
            /// 实体属性配置
            /// </summary>
            /// <param name="builder"></param>
            public void Configure(EntityTypeBuilder<Vow> builder)
            {
                builder.HasKey(v => v.Id);

                builder.Property(v => v.Id)
                    .HasColumnName("Id");

                //builder.HasOne(v => v.User)
                //    .WithMany(u => u.Vows);

                builder.Property(v => v.VowerName)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);
            }
        }

        /// <summary>
        /// 用户类
        /// </summary>
        public class UserMap : IEntityTypeConfiguration<User>
        {
            /// <summary>
            /// 实体属性配置
            /// </summary>
            /// <param name="builder"></param>
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(u => u.Id);

                builder.Property(u => u.Id)
                    .HasColumnName("Id");

                builder.Property(u => u.Name)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);
            }
        }
        /// <summary>
        /// 重写连接数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            // define the database to use
            // 因为我使用的是txt文件，所以用的是 File.ReadAllText() ，如果你直接配置的是字符串，可以直接这么写：
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Zeze.Core;Integrated Security=True");
            //optionsBuilder.UseSqlServer(DbConfig.InitConn(config.GetConnectionString("DefaultConnection_file"), config.GetConnectionString("DefaultConnection")));
        }
    }
}
