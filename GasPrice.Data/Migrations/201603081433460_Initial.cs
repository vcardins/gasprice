namespace GasPrice.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(),
                        LongName = c.String(),
                        Description = c.String(),
                        Copyright = c.String(),
                        Keywords = c.String(),
                        Version = c.String(),
                        Email = c.String(),
                        EmailSignature = c.String(),
                        LongDateTimeFormat = c.String(),
                        ShortDateTimeFormat = c.String(),
                        TinyDateTimeFormat = c.String(),
                        CacheDurationInMinutes = c.Int(nullable: false),
                        GoogleAnalyticsKey = c.String(),
                        RequireAccountVerification = c.Boolean(nullable: false),
                        EmailIsUsername = c.Boolean(nullable: false),
                        MultiTenant = c.Boolean(nullable: false),
                        AllowAccountDeletion = c.Boolean(nullable: false),
                        PasswordHashingIterationCount = c.Int(nullable: false),
                        AccountLockoutFailedLoginAttempts = c.Int(nullable: false),
                        AccountLockoutDuration = c.Int(nullable: false),
                        PasswordResetFrequency = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExceptionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HResult = c.Int(nullable: false),
                        Message = c.String(nullable: false, maxLength: 800),
                        Source = c.String(maxLength: 400),
                        RequestUri = c.String(maxLength: 200),
                        Method = c.String(maxLength: 20),
                        StackTrace = c.String(),
                        UserId = c.Int(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccount", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        LastUpdated = c.DateTime(nullable: false),
                        IsAccountClosed = c.Boolean(nullable: false),
                        AccountClosed = c.DateTime(),
                        IsLoginAllowed = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastFailedLogin = c.DateTime(),
                        FailedLoginCount = c.Int(nullable: false),
                        PasswordChanged = c.DateTime(),
                        RequiresPasswordReset = c.Boolean(nullable: false),
                        Email = c.String(nullable: false, maxLength: 254),
                        IsAccountVerified = c.Boolean(nullable: false),
                        VerificationKey = c.String(maxLength: 100),
                        VerificationPurpose = c.Int(),
                        VerificationKeySent = c.DateTime(),
                        VerificationStorage = c.String(maxLength: 100),
                        HashedPassword = c.String(maxLength: 200),
                        Username = c.String(nullable: false, maxLength: 254),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true, name: "IX_USER_EMAIL")
                .Index(t => t.Username, unique: true, name: "IX_USERNAME");
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccount", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Action = c.Int(nullable: false),
                        Module = c.Int(),
                        Message = c.String(),
                        Source = c.String(),
                        ObjectId = c.Int(),
                        UserId = c.Int(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccount", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FuelPriceHistory",
                c => new
                    {
                        GasStationId = c.Int(nullable: false),
                        FuelServiceId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.GasStationId, t.FuelServiceId, t.UserId, t.DateTime })
                .ForeignKey("dbo.FuelServiceGasStation", t => new { t.GasStationId, t.FuelServiceId })
                .ForeignKey("dbo.UserAccount", t => t.UserId)
                .Index(t => new { t.GasStationId, t.FuelServiceId })
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FuelServiceGasStation",
                c => new
                    {
                        GasStationId = c.Int(nullable: false),
                        FuelServiceId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.GasStationId, t.FuelServiceId })
                .ForeignKey("dbo.GasStation", t => t.GasStationId)
                .ForeignKey("dbo.FuelService", t => t.FuelServiceId)
                .Index(t => t.GasStationId)
                .Index(t => t.FuelServiceId);
            
            CreateTable(
                "dbo.GasStation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FuelBrandId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(nullable: false, maxLength: 400),
                        GeoLocation = c.Double(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FuelBrand", t => t.FuelBrandId)
                .Index(t => t.FuelBrandId);
            
            CreateTable(
                "dbo.FuelBrand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FuelService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.UserAccount", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        Description = c.String(maxLength: 200),
                        BitMask = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExceptionLog", "UserId", "dbo.UserAccount");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.UserAccount");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.FuelPriceHistory", "UserId", "dbo.UserAccount");
            DropForeignKey("dbo.FuelPriceHistory", new[] { "GasStationId", "FuelServiceId" }, "dbo.FuelServiceGasStation");
            DropForeignKey("dbo.FuelServiceGasStation", "FuelServiceId", "dbo.FuelService");
            DropForeignKey("dbo.FuelServiceGasStation", "GasStationId", "dbo.GasStation");
            DropForeignKey("dbo.GasStation", "FuelBrandId", "dbo.FuelBrand");
            DropForeignKey("dbo.Log", "UserId", "dbo.UserAccount");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.UserAccount");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.GasStation", new[] { "FuelBrandId" });
            DropIndex("dbo.FuelServiceGasStation", new[] { "FuelServiceId" });
            DropIndex("dbo.FuelServiceGasStation", new[] { "GasStationId" });
            DropIndex("dbo.FuelPriceHistory", new[] { "UserId" });
            DropIndex("dbo.FuelPriceHistory", new[] { "GasStationId", "FuelServiceId" });
            DropIndex("dbo.Log", new[] { "UserId" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropIndex("dbo.UserAccount", "IX_USERNAME");
            DropIndex("dbo.UserAccount", "IX_USER_EMAIL");
            DropIndex("dbo.ExceptionLog", new[] { "UserId" });
            DropTable("dbo.Role");
            DropTable("dbo.UserRole");
            DropTable("dbo.FuelService");
            DropTable("dbo.FuelBrand");
            DropTable("dbo.GasStation");
            DropTable("dbo.FuelServiceGasStation");
            DropTable("dbo.FuelPriceHistory");
            DropTable("dbo.Log");
            DropTable("dbo.UserClaim");
            DropTable("dbo.UserAccount");
            DropTable("dbo.ExceptionLog");
            DropTable("dbo.AppSettings");
        }
    }
}
