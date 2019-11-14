namespace practice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuctionViewModels",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductPrice = c.Double(nullable: false),
                        ImagePath = c.String(),
                        ProductInfo = c.String(),
                        BidPrice = c.Double(nullable: false),
                        TypeTime = c.String(),
                        IncrementTimePerBid = c.Int(nullable: false),
                        BidsTotal = c.Int(nullable: false),
                        LastBid = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ValueLastBid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastUserBid = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuctionViewModels");
        }
    }
}
