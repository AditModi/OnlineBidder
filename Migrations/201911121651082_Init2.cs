namespace practice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AuctionViewModels", "TypeTime");
            DropColumn("dbo.AuctionViewModels", "IncrementTimePerBid");
            DropColumn("dbo.AuctionViewModels", "BidsTotal");
            DropColumn("dbo.AuctionViewModels", "LastBid");
            DropColumn("dbo.AuctionViewModels", "EndTime");
            DropColumn("dbo.AuctionViewModels", "ValueLastBid");
            DropColumn("dbo.AuctionViewModels", "LastUserBid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuctionViewModels", "LastUserBid", c => c.String());
            AddColumn("dbo.AuctionViewModels", "ValueLastBid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AuctionViewModels", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuctionViewModels", "LastBid", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuctionViewModels", "BidsTotal", c => c.Int(nullable: false));
            AddColumn("dbo.AuctionViewModels", "IncrementTimePerBid", c => c.Int(nullable: false));
            AddColumn("dbo.AuctionViewModels", "TypeTime", c => c.String());
        }
    }
}
