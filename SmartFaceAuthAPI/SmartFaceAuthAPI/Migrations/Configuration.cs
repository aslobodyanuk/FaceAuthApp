namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartFaceAuthAPI.Context.ApiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SmartFaceAuthAPI.Context.ApiContext";
        }

        protected override void Seed(SmartFaceAuthAPI.Context.ApiContext context)
        {
            //context.Groups.RemoveRange(
            //context.Groups.Where(x => !x.Id.Equals(6)));
            //context.SaveChanges();
            //Models.Group group = new Models.Group();
            //group.GroupId = Guid.NewGuid();
            //group.Email = "K1@gmail.com";
            //Models.ImageData img1 = new Models.ImageData(ImageToByteArray(@"C:\Users\Слободянюк\source\repos\SmartFaceAuth\SmartFaceAuth\UploadedFiles\7dc93e63-74ef-479b-a345-52c008c6858b.jpeg"));
            //Models.ImageData img2 = new Models.ImageData(ImageToByteArray(@"C:\Users\Слободянюк\source\repos\SmartFaceAuth\SmartFaceAuth\UploadedFiles\6ab2f98d-3c52-40dc-b98a-cfee52a92313.jpeg"));
            //Models.ImageData img3 = new Models.ImageData(ImageToByteArray(@"C:\Users\Слободянюк\source\repos\SmartFaceAuth\SmartFaceAuth\UploadedFiles\d6c30f7e-30da-41e9-b717-171a640077b3.jpeg"));
            //Models.ImageData img4 = new Models.ImageData(ImageToByteArray(@"C:\Users\Слободянюк\source\repos\SmartFaceAuth\SmartFaceAuth\UploadedFiles\473a8be7-8273-4de1-89dd-daa056722f46.jpeg"));
            //Models.ImageData img5 = new Models.ImageData(ImageToByteArray(@"C:\Users\Слободянюк\source\repos\SmartFaceAuth\SmartFaceAuth\UploadedFiles\dd2995dd-9d38-42c0-8855-e342921902fb.jpeg"));
            //group.PersonsList = new System.Collections.Generic.List<Models.Person>();
            //group.PersonsList.Add(new Models.Person("Ivan", Guid.NewGuid(), img1));
            //group.PersonsList.Add(new Models.Person("Anna", Guid.NewGuid(), img2));
            //group.PersonsList.Add(new Models.Person("Bill", Guid.NewGuid(), img3));
            //group.PersonsList.Add(new Models.Person("Vasya", Guid.NewGuid(), img4));
            //group.PersonsList.Add(new Models.Person("Inna", Guid.NewGuid(), img5));
            //context.Groups.Add(group);
            //context.Logs.SingleOrDefault(x => x.Id == 1).AuthImage = new Models.ImageData(new byte[] { 1, 2, 3, 4, 5 });

            //context.Logs.Add(new Models.Log(DateTime.Now, "MessageFromSeed"));
            //context.Logs.Add(new Models.Log(DateTime.Now, "MessageFromSeed1"));
            //context.Logs.Add(new Models.Log(DateTime.Now, "MessageFromSeed2"));
            //context.Logs.Add(new Models.Log(DateTime.Now, "MessageFromSeed3"));
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }

        //public byte[] ImageToByteArray(string filePath)
        //{
        //    byte[] imgData = File.ReadAllBytes(filePath);
        //    return imgData;
        //}
    }
}
